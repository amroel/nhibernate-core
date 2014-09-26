using NHibernate.Engine;
using NHibernate.Persister.Entity;

namespace NHibernate.LinqToSql
{
	public class SessionFactoryMetaDataRepoistory : IMetaDataRepository
	{
		private readonly ISessionFactoryImplementor _factory;

		public SessionFactoryMetaDataRepoistory(ISessionFactoryImplementor factory)
		{
			_factory = factory;
		}

		#region IMetaDataRepository Members

		public IQueryable LoadingInfoFor(System.Type entityType)
		{
			return (_factory.GetEntityPersister(entityType.FullName) as IQueryable);
		}

		#endregion
	}
}
