
namespace NHibernate.Test.LinqToSql.Units
{
	public class FakeQueryable : NHibernate.Persister.Entity.IQueryable
	{
		#region IQueryable Members

		public bool IsExplicitPolymorphism
		{
			get { throw new System.NotImplementedException(); }
		}

		public string MappedSuperclass
		{
			get { throw new System.NotImplementedException(); }
		}

		public string DiscriminatorSQLValue
		{
			get { throw new System.NotImplementedException(); }
		}

		public object DiscriminatorValue
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool IsMultiTable
		{
			get { throw new System.NotImplementedException(); }
		}

		public string[] ConstraintOrderedTableNameClosure
		{
			get { throw new System.NotImplementedException(); }
		}

		public string[][] ContraintOrderedTableKeyColumnClosure
		{
			get { throw new System.NotImplementedException(); }
		}

		public string TemporaryIdTableName
		{
			get { throw new System.NotImplementedException(); }
		}

		public string TemporaryIdTableDDL
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool VersionPropertyInsertable
		{
			get { throw new System.NotImplementedException(); }
		}

		public string IdentifierSelectFragment(string name, string suffix)
		{
			throw new System.NotImplementedException();
		}

		public string PropertySelectFragment(string alias, string suffix, bool allProperties)
		{
			throw new System.NotImplementedException();
		}

		public int GetSubclassPropertyTableNumber(string propertyPath)
		{
			throw new System.NotImplementedException();
		}

		public Persister.Entity.Declarer GetSubclassPropertyDeclarer(string propertyPath)
		{
			throw new System.NotImplementedException();
		}

		public string GetSubclassTableName(int number)
		{
			throw new System.NotImplementedException();
		}

		public string GenerateFilterConditionAlias(string rootAlias)
		{
			throw new System.NotImplementedException();
		}

		#endregion

		#region ILoadable Members

		public Type.IType DiscriminatorType
		{
			get { throw new System.NotImplementedException(); }
		}

		public string[] IdentifierColumnNames
		{
			get { throw new System.NotImplementedException(); }
		}

		public string DiscriminatorColumnName
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool IsAbstract
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool HasSubclasses
		{
			get { throw new System.NotImplementedException(); }
		}

		public string GetSubclassForDiscriminatorValue(object value)
		{
			throw new System.NotImplementedException();
		}

		public string[] GetIdentifierAliases(string suffix)
		{
			throw new System.NotImplementedException();
		}

		public string[] GetPropertyAliases(string suffix, int i)
		{
			throw new System.NotImplementedException();
		}

		public string[] GetPropertyColumnNames(int i)
		{
			throw new System.NotImplementedException();
		}

		public string GetDiscriminatorAlias(string suffix)
		{
			throw new System.NotImplementedException();
		}

		public bool HasRowId
		{
			get { throw new System.NotImplementedException(); }
		}

		public object[] Hydrate(System.Data.IDataReader rs, object id, object obj, Persister.Entity.ILoadable rootLoadable, string[][] suffixedPropertyColumns, bool allProperties, Engine.ISessionImplementor session)
		{
			throw new System.NotImplementedException();
		}

		#endregion

		#region IEntityPersister Members

		public Engine.ISessionFactoryImplementor Factory
		{
			get { throw new System.NotImplementedException(); }
		}

		public string RootEntityName
		{
			get { throw new System.NotImplementedException(); }
		}

		public string EntityName
		{
			get { throw new System.NotImplementedException(); }
		}

		public Tuple.Entity.EntityMetamodel EntityMetamodel
		{
			get { throw new System.NotImplementedException(); }
		}

		public string[] PropertySpaces
		{
			get { throw new System.NotImplementedException(); }
		}

		public string[] QuerySpaces
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool IsMutable
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool IsInherited
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool IsIdentifierAssignedByInsert
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool IsVersioned
		{
			get { throw new System.NotImplementedException(); }
		}

		public Type.IVersionType VersionType
		{
			get { throw new System.NotImplementedException(); }
		}

		public int VersionProperty
		{
			get { throw new System.NotImplementedException(); }
		}

		public int[] NaturalIdentifierProperties
		{
			get { throw new System.NotImplementedException(); }
		}

		public Id.IIdentifierGenerator IdentifierGenerator
		{
			get { throw new System.NotImplementedException(); }
		}

		public Type.IType[] PropertyTypes
		{
			get { throw new System.NotImplementedException(); }
		}

		public string[] PropertyNames
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool[] PropertyInsertability
		{
			get { throw new System.NotImplementedException(); }
		}

		public Engine.ValueInclusion[] PropertyInsertGenerationInclusions
		{
			get { throw new System.NotImplementedException(); }
		}

		public Engine.ValueInclusion[] PropertyUpdateGenerationInclusions
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool[] PropertyCheckability
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool[] PropertyNullability
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool[] PropertyVersionability
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool[] PropertyLaziness
		{
			get { throw new System.NotImplementedException(); }
		}

		public Engine.CascadeStyle[] PropertyCascadeStyles
		{
			get { throw new System.NotImplementedException(); }
		}

		public Type.IType IdentifierType
		{
			get { throw new System.NotImplementedException(); }
		}

		public string IdentifierPropertyName
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool IsCacheInvalidationRequired
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool IsLazyPropertiesCacheable
		{
			get { throw new System.NotImplementedException(); }
		}

		public Cache.ICacheConcurrencyStrategy Cache
		{
			get { throw new System.NotImplementedException(); }
		}

		public Cache.Entry.ICacheEntryStructure CacheEntryStructure
		{
			get { throw new System.NotImplementedException(); }
		}

		public Metadata.IClassMetadata ClassMetadata
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool IsBatchLoadable
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool IsSelectBeforeUpdateRequired
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool IsVersionPropertyGenerated
		{
			get { throw new System.NotImplementedException(); }
		}

		public void PostInstantiate()
		{
			throw new System.NotImplementedException();
		}

		public bool IsSubclassEntityName(string entityName)
		{
			throw new System.NotImplementedException();
		}

		public bool HasProxy
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool HasCollections
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool HasMutableProperties
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool HasSubselectLoadableCollections
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool HasCascades
		{
			get { throw new System.NotImplementedException(); }
		}

		public Type.IType GetPropertyType(string propertyName)
		{
			throw new System.NotImplementedException();
		}

		public int[] FindDirty(object[] currentState, object[] previousState, object entity, Engine.ISessionImplementor session)
		{
			throw new System.NotImplementedException();
		}

		public int[] FindModified(object[] old, object[] current, object entity, Engine.ISessionImplementor session)
		{
			throw new System.NotImplementedException();
		}

		public bool HasIdentifierProperty
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool CanExtractIdOutOfEntity
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool HasNaturalIdentifier
		{
			get { throw new System.NotImplementedException(); }
		}

		public object[] GetNaturalIdentifierSnapshot(object id, Engine.ISessionImplementor session)
		{
			throw new System.NotImplementedException();
		}

		public bool HasLazyProperties
		{
			get { throw new System.NotImplementedException(); }
		}

		public object Load(object id, object optionalObject, LockMode lockMode, Engine.ISessionImplementor session)
		{
			throw new System.NotImplementedException();
		}

		public void Lock(object id, object version, object obj, LockMode lockMode, Engine.ISessionImplementor session)
		{
			throw new System.NotImplementedException();
		}

		public void Insert(object id, object[] fields, object obj, Engine.ISessionImplementor session)
		{
			throw new System.NotImplementedException();
		}

		public object Insert(object[] fields, object obj, Engine.ISessionImplementor session)
		{
			throw new System.NotImplementedException();
		}

		public void Delete(object id, object version, object obj, Engine.ISessionImplementor session)
		{
			throw new System.NotImplementedException();
		}

		public void Update(object id, object[] fields, int[] dirtyFields, bool hasDirtyCollection, object[] oldFields, object oldVersion, object obj, object rowId, Engine.ISessionImplementor session)
		{
			throw new System.NotImplementedException();
		}

		public bool[] PropertyUpdateability
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool HasCache
		{
			get { throw new System.NotImplementedException(); }
		}

		public object[] GetDatabaseSnapshot(object id, Engine.ISessionImplementor session)
		{
			throw new System.NotImplementedException();
		}

		public object GetCurrentVersion(object id, Engine.ISessionImplementor session)
		{
			throw new System.NotImplementedException();
		}

		public object ForceVersionIncrement(object id, object currentVersion, Engine.ISessionImplementor session)
		{
			throw new System.NotImplementedException();
		}

		public EntityMode? GuessEntityMode(object obj)
		{
			throw new System.NotImplementedException();
		}

		public bool IsInstrumented(EntityMode entityMode)
		{
			throw new System.NotImplementedException();
		}

		public bool HasInsertGeneratedProperties
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool HasUpdateGeneratedProperties
		{
			get { throw new System.NotImplementedException(); }
		}

		public void AfterInitialize(object entity, bool lazyPropertiesAreUnfetched, Engine.ISessionImplementor session)
		{
			throw new System.NotImplementedException();
		}

		public void AfterReassociate(object entity, Engine.ISessionImplementor session)
		{
			throw new System.NotImplementedException();
		}

		public object CreateProxy(object id, Engine.ISessionImplementor session)
		{
			throw new System.NotImplementedException();
		}

		public bool? IsTransient(object obj, Engine.ISessionImplementor session)
		{
			throw new System.NotImplementedException();
		}

		public object[] GetPropertyValuesToInsert(object obj, System.Collections.IDictionary mergeMap, Engine.ISessionImplementor session)
		{
			throw new System.NotImplementedException();
		}

		public void ProcessInsertGeneratedProperties(object id, object entity, object[] state, Engine.ISessionImplementor session)
		{
			throw new System.NotImplementedException();
		}

		public void ProcessUpdateGeneratedProperties(object id, object entity, object[] state, Engine.ISessionImplementor session)
		{
			throw new System.NotImplementedException();
		}

		public System.Type GetMappedClass(EntityMode entityMode)
		{
			throw new System.NotImplementedException();
		}

		public bool ImplementsLifecycle(EntityMode entityMode)
		{
			throw new System.NotImplementedException();
		}

		public bool ImplementsValidatable(EntityMode entityMode)
		{
			throw new System.NotImplementedException();
		}

		public System.Type GetConcreteProxyClass(EntityMode entityMode)
		{
			throw new System.NotImplementedException();
		}

		public void SetPropertyValues(object obj, object[] values, EntityMode entityMode)
		{
			throw new System.NotImplementedException();
		}

		public void SetPropertyValue(object obj, int i, object value, EntityMode entityMode)
		{
			throw new System.NotImplementedException();
		}

		public object[] GetPropertyValues(object obj, EntityMode entityMode)
		{
			throw new System.NotImplementedException();
		}

		public object GetPropertyValue(object obj, int i, EntityMode entityMode)
		{
			throw new System.NotImplementedException();
		}

		public object GetPropertyValue(object obj, string name, EntityMode entityMode)
		{
			throw new System.NotImplementedException();
		}

		public object GetIdentifier(object obj, EntityMode entityMode)
		{
			throw new System.NotImplementedException();
		}

		public void SetIdentifier(object obj, object id, EntityMode entityMode)
		{
			throw new System.NotImplementedException();
		}

		public object GetVersion(object obj, EntityMode entityMode)
		{
			throw new System.NotImplementedException();
		}

		public object Instantiate(object id, EntityMode entityMode)
		{
			throw new System.NotImplementedException();
		}

		public bool IsInstance(object entity, EntityMode entityMode)
		{
			throw new System.NotImplementedException();
		}

		public bool HasUninitializedLazyProperties(object obj, EntityMode entityMode)
		{
			throw new System.NotImplementedException();
		}

		public void ResetIdentifier(object entity, object currentId, object currentVersion, EntityMode entityMode)
		{
			throw new System.NotImplementedException();
		}

		public Persister.Entity.IEntityPersister GetSubclassEntityPersister(object instance, Engine.ISessionFactoryImplementor factory, EntityMode entityMode)
		{
			throw new System.NotImplementedException();
		}

		public bool? IsUnsavedVersion(object version)
		{
			throw new System.NotImplementedException();
		}

		#endregion

		#region IOptimisticCacheSource Members


		public System.Collections.IComparer VersionComparator
		{
			get { throw new System.NotImplementedException(); }
		}

		#endregion

		#region IPropertyMapping Members

		public Type.IType Type
		{
			get { throw new System.NotImplementedException(); }
		}

		public Type.IType ToType(string propertyName)
		{
			throw new System.NotImplementedException();
		}

		public bool TryToType(string propertyName, out Type.IType type)
		{
			throw new System.NotImplementedException();
		}

		public string[] ToColumns(string alias, string propertyName)
		{
			throw new System.NotImplementedException();
		}

		public string[] ToColumns(string propertyName)
		{
			throw new System.NotImplementedException();
		}

		#endregion

		#region IJoinable Members

		public string Name
		{
			get { throw new System.NotImplementedException(); }
		}

		public string[] KeyColumnNames
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool IsCollection
		{
			get { throw new System.NotImplementedException(); }
		}

		public string TableName { get; set; }

		public string SelectFragment(Persister.Entity.IJoinable rhs, string rhsAlias, string lhsAlias, string currentEntitySuffix, string currentCollectionSuffix, bool includeCollectionColumns)
		{
			throw new System.NotImplementedException();
		}

		public SqlCommand.SqlString WhereJoinFragment(string alias, bool innerJoin, bool includeSubclasses)
		{
			throw new System.NotImplementedException();
		}

		public SqlCommand.SqlString FromJoinFragment(string alias, bool innerJoin, bool includeSubclasses)
		{
			throw new System.NotImplementedException();
		}

		public string FilterFragment(string alias, System.Collections.Generic.IDictionary<string, IFilter> enabledFilters)
		{
			throw new System.NotImplementedException();
		}

		public string OneToManyFilterFragment(string alias)
		{
			throw new System.NotImplementedException();
		}

		public bool ConsumesEntityAlias()
		{
			throw new System.NotImplementedException();
		}

		public bool ConsumesCollectionAlias()
		{
			throw new System.NotImplementedException();
		}

		#endregion
	}
}
