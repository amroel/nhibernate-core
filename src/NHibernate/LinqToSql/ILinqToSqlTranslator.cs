using System.Linq.Expressions;

namespace NHibernate.LinqToSql
{
	public interface ILinqToSqlTranslator
	{
		TranslationResult Translate(Expression expression);
	}
}
