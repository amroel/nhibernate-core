using System.Data;

namespace NHibernate.LinqToSql
{
	public interface ISqlQueryExecutor : IDataReader
	{
		IMaterializer Run(TranslationResult translationResult);
	}
}
