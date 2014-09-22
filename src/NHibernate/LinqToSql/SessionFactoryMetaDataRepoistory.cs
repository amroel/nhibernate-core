using NHibernate.Hql.Ast.ANTLR;

namespace NHibernate.LinqToSql
{
	public class SessionFactoryMetaDataRepoistory : IMetaDataRepository
	{
		private readonly SessionFactoryHelperExtensions _helper;

		public SessionFactoryMetaDataRepoistory(NHibernate.Engine.ISessionFactoryImplementor factory)
		{
			_helper = new SessionFactoryHelperExtensions(factory);
		}

		#region IMetaDataRepository Members

		public string TableNameFor(System.Type entityType)
		{
			var persistor = _helper.RequireClassPersister(entityType.Name) as NHibernate.Persister.Entity.IQueryable;
			return persistor.TableName;
		}

		#endregion
	}
}
