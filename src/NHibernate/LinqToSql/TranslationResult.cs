using NHibernate.SqlCommand;
using NHibernate.SqlTypes;

namespace NHibernate.LinqToSql
{
	public class TranslationResult
	{
		public SqlString Sql { get; set; }
		public SqlType[] ParameterTypes { get; set; }
	}
}
