using System;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.LinqToSql;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Test.LinqToSql.Units
{
	[TestFixture]
	public class TranslatingLinqExpression
	{
		[Test]
		public void MappedEntity_ConstantQueryableExpression_ReturnsCorrectTranslationResult()
		{
			var metaDataRepository = new FakeMetaDataRepository();
			metaDataRepository.AddTableMapFor<SimpleEntity>("simple_entities");

			var translator = new LinqQueryToSqlTranslator(metaDataRepository);
			var queryable = new SimpleEntity[] { }.AsQueryable();
			var result = translator.Translate(Expression.Constant(queryable));

			result.ParameterTypes.Should().Be.Empty();
			result.Sql.ToString().Should().Be.EqualTo("SELECT * FROM simple_entities");
		}

		[Test]
		public void MappedEntity_ConstantNonQueryableExpression_Throws()
		{
			var metaDataRepository = new FakeMetaDataRepository();
			metaDataRepository.AddTableMapFor<SimpleEntity>("simple_entities");

			var translator = new LinqQueryToSqlTranslator(metaDataRepository);

			Assert.Throws<NotSupportedException>(() => translator.Translate(Expression.Constant(new SimpleEntity())));
		}
	}
}
