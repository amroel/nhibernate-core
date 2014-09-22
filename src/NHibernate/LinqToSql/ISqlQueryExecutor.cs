using System.Data;

namespace NHibernate.LinqToSql
{
	public interface ISqlQueryExecutor : IDataReader
	{
		void Run(TranslationResult translationResult);
	}
}
