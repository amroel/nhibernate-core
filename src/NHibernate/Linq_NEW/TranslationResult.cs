using NHibernate.SqlCommand;
using NHibernate.SqlTypes;

namespace NHibernate.Linq_NEW
{
	public class TranslationResult
	{
		public SqlString Sql { get; set; }
		public SqlType[] ParameterTypes { get; set; }
	}
}
