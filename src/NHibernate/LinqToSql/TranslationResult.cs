using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;

namespace NHibernate.LinqToSql
{
	public class TranslationResult
	{
		public SqlString Sql { get; set; }
		public SqlType[] ParameterTypes { get; set; }
		public ILoadable[] LoadableEntities { get; set; }
	}
}
