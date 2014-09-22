using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.LinqToSql;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Test.LinqToSql.Units
{
	[TestFixture]
	public class QueryProviderExecution
	{
		[Test]
		public void ReturnsEnumerableResult()
		{
			ILinqToSqlTranslator translatorStub = new LinqToSqlTranslatorStub
			{
				ParameterTypes = new SqlTypes.SqlType[] { },
				Sql = new SqlCommand.SqlString("select * from silly")
			};
			ISqlQueryExecutor queryExecutorStub = new SqlQueryExecutorStub();

			var provider = new LinqToSqlQueryProvider(translatorStub, queryExecutorStub);
			var queryable = new SimpleEntity[] { }.AsQueryable();
			var result = provider.Execute(Expression.Constant(queryable));

			result.Should().Be.InstanceOf<IEnumerable<SimpleEntity>>();
		}
	}
}
