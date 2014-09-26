using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.Type;

namespace NHibernate.LinqToSql
{
	public class EntityKeyProcessor : IMaterializationProcessor
	{
		private readonly ISessionImplementor _session;
		private MaterializationProcess _process;

		public EntityKeyProcessor(ISessionImplementor session)
		{
			_session = session;
		}

		#region IMaterializationProcessor Members

		public void Process(MaterializationProcess process)
		{
			_process = process;
			ILoadable[] persisters = _process.TranslationResult.LoadableEntities;
			int entitySpan = persisters.Length;
			_process.EntityKeys = new EntityKey[entitySpan];

			for (int i = 0; i < entitySpan; i++)
			{
				process.EntityKeys[i] = GetKeyFromResultSet(persisters[i]);
			}
		}

		#endregion

		/// <summary>
		/// Read a row of <c>EntityKey</c>s from the <c>IDataReader</c> into the given array.
		/// </summary>
		/// <remarks>
		/// Warning: this method is side-effecty. If an <c>id</c> is given, don't bother going
		/// to the <c>IDataReader</c>
		private EntityKey GetKeyFromResultSet(ILoadable persister)
		{
			object resultId;

			IType idType = persister.IdentifierType;
			var keyNames = persister.IdentifierColumnNames;
			//var keyNames = persister.GetIdentifierAliases("");
			resultId = idType.NullSafeGet(_process.DataReader, keyNames, _session, null);

			return resultId == null ? null : _session.GenerateEntityKey(resultId, persister);
		}
	}
}
