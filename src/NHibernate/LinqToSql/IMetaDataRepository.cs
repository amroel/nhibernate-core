using NHibernate.Persister.Entity;

namespace NHibernate.LinqToSql
{
	public interface IMetaDataRepository
	{
		IQueryable LoadingInfoFor(System.Type entityType);		
	}
}
