using System.Linq.Expressions;
using NHibernate.LinqToSql;

namespace NHibernate.Test.LinqToSql.Units
{
	public class LinqToSqlTranslatorStub : ILinqToSqlTranslator
	{
		#region ILinqToSqlTranslator Members

		public TranslationResult Translate(Expression expression)
		{
			return new TranslationResult
			{
				ParameterTypes = ParameterTypes,
				Sql = Sql
			};
		}

		#endregion

		public NHibernate.SqlTypes.SqlType[] ParameterTypes { get; set; }
		public NHibernate.SqlCommand.SqlString Sql { get; set; }
	}
}
