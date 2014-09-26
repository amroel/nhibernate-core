using System.Data;

namespace NHibernate.LinqToSql
{
	public interface IMaterializer
	{
		T Materialize<T>(IDataReader dataReader) where T : class;
	}
}
