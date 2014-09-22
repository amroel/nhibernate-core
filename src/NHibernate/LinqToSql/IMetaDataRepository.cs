
namespace NHibernate.LinqToSql
{
	public interface IMetaDataRepository
	{
		string TableNameFor(System.Type entityType);		
	}
}
