using System;
using System.Collections.Generic;
using System.Data;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Impl;
using NHibernate.Loader;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using NHibernate.Util;

namespace NHibernate.LinqToSql
{
	public class TwoPhaseLoadProcessorDecorator : IMaterializationProcessor
	{
		private static readonly IInternalLogger Log = LoggerProvider.LoggerFor(typeof(TwoPhaseLoadProcessorDecorator));
		private readonly IMaterializationProcessor _innerProcessor;
		private readonly ISessionImplementor _session;

		private MaterializationProcess _process;
		private List<object> _hydratedObjects;

		public TwoPhaseLoadProcessorDecorator(ISessionImplementor session, IMaterializationProcessor innerProcessor)
		{
			_session = session;
			_innerProcessor = innerProcessor;
		}

		#region IMaterializationProcessor Members

		public void Process(MaterializationProcess process)
		{
			_innerProcessor.Process(process);
			_process = process;

			IPersistenceContext persistenceContext = _session.PersistenceContext;

			persistenceContext.BeforeLoad();
			try
			{
				_process.MaterializationResult = CreateResult();
			}
			finally
			{
				persistenceContext.AfterLoad();
			}

			persistenceContext.InitializeNonLazyCollections();
		}

		#endregion

		private object CreateResult()
		{
			using (new SessionIdLoggingContext(_session.SessionId))
			{
				_hydratedObjects = new List<object>();
				object result = null;

				try
				{
					if (Log.IsDebugEnabled)
					{
						Log.Debug("processing result set");
					}

					result = GetRowFromResultSet(new LockMode[] { LockMode.None });
				}
				catch (Exception e)
				{
					e.Data["actual-sql-query"] = _process.TranslationResult.Sql.ToString();
					throw;
				}

				InitializeEntitiesAndCollections();

				return result;
			}
		}

		internal object GetRowFromResultSet(LockMode[] lockModeArray)
		{
			ILoadable[] persisters = _process.TranslationResult.LoadableEntities;

			// this call is side-effecty
			object[] row = GetRow(persisters, lockModeArray);

			//ReadCollectionElements(row, resultSet, session);

			return row;
		}

		/// <summary>
		/// Resolve any ids for currently loaded objects, duplications within the <c>IDataReader</c>,
		/// etc. Instanciate empty objects to be initialized from the <c>IDataReader</c>. Return an
		/// array of objects (a row of results) and an array of booleans (by side-effect) that determine
		/// wheter the corresponding object should be initialized
		/// </summary>
		private object[] GetRow(ILoadable[] persisters, LockMode[] lockModes)
		{
			int cols = persisters.Length;
			//IEntityAliases[] descriptors = EntityAliases;

			if (Log.IsDebugEnabled)
			{
				Log.Debug("result row: " + StringHelper.ToString(_process.EntityKeys));
			}

			object[] rowResults = new object[cols];

			for (int i = 0; i < cols; i++)
			{
				object obj = null;
				EntityKey key = _process.EntityKeys[i];

				if (key == null)
				{
					// do nothing
					/* TODO NH-1001 : if (persisters[i]...EntityType) is an OneToMany or a ManyToOne and
					 * the keys.length > 1 and the relation IsIgnoreNotFound probably we are in presence of
					 * an load with "outer join" the relation can be considerer loaded even if the key is null (mean not found)
					*/
				}
				else
				{
					//If the object is already loaded, return the loaded one
					obj = _session.GetEntityUsingInterceptor(key);
					if (obj != null)
					{
						//its already loaded so dont need to hydrate it
						InstanceAlreadyLoaded(i, persisters[i], key, obj, lockModes[i]);
					}
					else
					{
						obj = InstanceNotYetLoaded(i, persisters[i], key, lockModes[i], "");
					}
				}

				rowResults[i] = obj;
			}
			return rowResults;
		}

		private void InstanceAlreadyLoaded(int i, ILoadable persister, EntityKey key, object obj, LockMode lockMode)
		{
			if (!persister.IsInstance(obj, _session.EntityMode))
			{
				string errorMsg = string.Format("loading object was of wrong class [{0}]", obj.GetType().FullName);
				throw new WrongClassException(errorMsg, key.Identifier, persister.EntityName);
			}

			if (LockMode.None != lockMode)
			{
				EntityEntry entry = _session.PersistenceContext.GetEntry(obj);
				bool isVersionCheckNeeded = persister.IsVersioned && entry.LockMode.LessThan(lockMode);

				// we don't need to worry about existing version being uninitialized
				// because this block isn't called by a re-entrant load (re-entrant
				// load _always_ have lock mode NONE
				if (isVersionCheckNeeded)
				{
					// we only check the version when _upgrading_ lock modes
					CheckVersion(persister, key.Identifier, obj);
					// we need to upgrade the lock mode to the mode requested
					entry.LockMode = lockMode;
				}
			}
		}

		private object InstanceNotYetLoaded(int i, ILoadable persister, EntityKey key, LockMode lockMode, string rowIdAlias)
		{
			object obj;

			string instanceClass = GetInstanceClass(persister, key.Identifier);

			obj = _session.Instantiate(instanceClass, key.Identifier);

			// need to hydrate it

			// grab its state from the DataReader and keep it in the Session
			// (but don't yet initialize the object itself)
			// note that we acquired LockMode.READ even if it was not requested
			LockMode acquiredLockMode = lockMode == LockMode.None ? LockMode.Read : lockMode;
			LoadFromResultSet(obj, instanceClass, key, rowIdAlias, acquiredLockMode, persister);

			// materialize associations (and initialize the object) later
			_hydratedObjects.Add(obj);

			return obj;
		}

		/// <summary>
		/// Hydrate the state of an object from the SQL <c>IDataReader</c>, into
		/// an array of "hydrated" values (do not resolve associations yet),
		/// and pass the hydrated state to the session.
		/// </summary>
		private void LoadFromResultSet(object obj, string instanceClass, EntityKey key,
									   string rowIdAlias, LockMode lockMode, ILoadable rootPersister)
		{
			object id = key.Identifier;

			// Get the persister for the _subclass_
			ILoadable persister = (ILoadable)_session.Factory.GetEntityPersister(instanceClass);

			if (Log.IsDebugEnabled)
			{
				Log.Debug("Initializing object from DataReader: " + MessageHelper.InfoString(persister, id));
			}

			//bool eagerPropertyFetch = IsEagerPropertyFetchEnabled(i);
			bool eagerPropertyFetch = false;

			// add temp entry so that the next step is circular-reference
			// safe - only needed because some types don't take proper
			// advantage of two-phase-load (esp. components)
			TwoPhaseLoad.AddUninitializedEntity(key, obj, persister, lockMode, !eagerPropertyFetch, _session);
			//var entityAliases = new DefaultEntityAliases(persister, "");
			var propertiesLength = persister.PropertyNames.Length;
			string[][] propertyColumnNames = new string[propertiesLength][];
			for (int j = 0; j < propertiesLength; j++)
			{
				propertyColumnNames[j] = new string[] { persister.PropertyNames[j] };
			}

			// This is not very nice (and quite slow):
			//string[][] cols = persister == rootPersister
			//					? entityAliases.SuffixedPropertyAliases
			//					: entityAliases.GetSuffixedPropertyAliases(persister);

			object[] values = persister.Hydrate(_process.DataReader, id, obj, rootPersister, propertyColumnNames, eagerPropertyFetch, _session);

			object rowId = persister.HasRowId ? _process.DataReader[rowIdAlias] : null;

			//IAssociationType[] ownerAssociationTypes = OwnerAssociationTypes;
			//if (ownerAssociationTypes != null && ownerAssociationTypes[i] != null)
			//{
			//	string ukName = ownerAssociationTypes[i].RHSUniqueKeyPropertyName;
			//	if (ukName != null)
			//	{
			//		int index = ((IUniqueKeyLoadable)persister).GetPropertyIndex(ukName);
			//		IType type = persister.PropertyTypes[index];

			//		// polymorphism not really handled completely correctly,
			//		// perhaps...well, actually its ok, assuming that the
			//		// entity name used in the lookup is the same as the
			//		// the one used here, which it will be

			//		EntityUniqueKey euk =
			//			new EntityUniqueKey(rootPersister.EntityName, ukName, type.SemiResolve(values[index], _session, obj), type,
			//								_session.EntityMode, _session.Factory);
			//		_session.PersistenceContext.AddEntity(euk, obj);
			//	}
			//}

			TwoPhaseLoad.PostHydrate(persister, id, values, rowId, obj, lockMode, !eagerPropertyFetch, _session);
		}

		private void InitializeEntitiesAndCollections()
		{
			//ICollectionPersister[] collectionPersisters = CollectionPersisters;
			//if (collectionPersisters != null)
			//{
			//	for (int i = 0; i < collectionPersisters.Length; i++)
			//	{
			//		if (collectionPersisters[i].IsArray)
			//		{
			//			//for arrays, we should end the collection load before resolving
			//			//the entities, since the actual array instances are not instantiated
			//			//during loading
			//			//TODO: or we could do this polymorphically, and have two
			//			//      different operations implemented differently for arrays
			//			EndCollectionLoad(collectionPersisters[i]);
			//		}
			//	}
			//}
			//important: reuse the same event instances for performance!
			PreLoadEvent pre;
			PostLoadEvent post;
			if (_session.IsEventSource)
			{
				var eventSourceSession = (IEventSource)_session;
				pre = new PreLoadEvent(eventSourceSession);
				post = new PostLoadEvent(eventSourceSession);
			}
			else
			{
				pre = null;
				post = null;
			}

			if (_hydratedObjects != null)
			{
				int hydratedObjectsSize = _hydratedObjects.Count;

				if (Log.IsDebugEnabled)
				{
					Log.Debug(string.Format("total objects hydrated: {0}", hydratedObjectsSize));
				}

				for (int i = 0; i < hydratedObjectsSize; i++)
				{
					TwoPhaseLoad.InitializeEntity(_hydratedObjects[i], false, _session, pre, post);
				}
			}

			//if (collectionPersisters != null)
			//{
			//	for (int i = 0; i < collectionPersisters.Length; i++)
			//	{
			//		if (!collectionPersisters[i].IsArray)
			//		{
			//			//for sets, we should end the collection load after resolving
			//			//the entities, since we might call hashCode() on the elements
			//			//TODO: or we could do this polymorphically, and have two
			//			//      different operations implemented differently for arrays
			//			EndCollectionLoad(collectionPersisters[i]);
			//		}
			//	}
			//}
		}

		private string GetInstanceClass(ILoadable persister, object id)
		{
			if (persister.HasSubclasses)
			{
				var entityAliases = new DefaultEntityAliases(persister, "");
				// code to handle subclasses of topClass
				object discriminatorValue =
					persister.DiscriminatorType.NullSafeGet(_process.DataReader, entityAliases.SuffixedDiscriminatorAlias, _session, null);

				string result = persister.GetSubclassForDiscriminatorValue(discriminatorValue);

				if (result == null)
				{
					// woops we got an instance of another class hierarchy branch.
					throw new WrongClassException(string.Format("Discriminator was: '{0}'", discriminatorValue), id,
												  persister.EntityName);
				}

				return result;
			}
			return persister.EntityName;
		}

		private void EndCollectionLoad(ICollectionPersister collectionPersister)
		{
			//this is a query and we are loading multiple instances of the same collection role
			_session.PersistenceContext.LoadContexts.GetCollectionLoadContext(_process.DataReader).EndLoadingCollections(collectionPersister);
		}

		private void CheckVersion(ILoadable persister, object id, object entity)
		{
			var entityAliases = new DefaultEntityAliases(persister, "");
			object version = _session.PersistenceContext.GetEntry(entity).Version;

			// null version means the object is in the process of being loaded somewhere else in the ResultSet
			if (version != null)
			{
				IVersionType versionType = persister.VersionType;
				object currentVersion = versionType.NullSafeGet(_process.DataReader, entityAliases.SuffixedVersionAliases, _session, null);
				if (!versionType.IsEqual(version, currentVersion))
				{
					if (_session.Factory.Statistics.IsStatisticsEnabled)
					{
						_session.Factory.StatisticsImplementor.OptimisticFailure(persister.EntityName);
					}

					throw new StaleObjectStateException(persister.EntityName, id);
				}
			}
		}
	}
}
