using NHibernate.SqlCommand;
using NHibernate.SqlCommand.Parser;
using NUnit.Framework;
using System.Linq;

namespace NHibernate.Test.SqlCommandTest
{
	public class TestParsingFirebirdSelect
	{
		[Test]
		public void SelectWithoutParams()
		{
			var sql = SqlString.Parse("select one, two, three from table");
			var parser = new FirebirdSelectParser(sql);

			parser.Parse();

			Assert.That(parser.ParamsToTypeCast, Is.Empty);
		}

		[Test]
		public void SelectWithSimpleParamInSelectClause()
		{
			var sql = SqlString.Parse("select one, ?, three from table");
			var parser = new FirebirdSelectParser(sql);

			parser.Parse();

			Assert.That(parser.ParamsToTypeCast.Any(x => x.ParamPos == 0), Is.True);
		}

		[Test]
		public void SelectWithParamInWhereClause()
		{
			var sql = SqlString.Parse("select one, two from table where x = ?");
			var parser = new FirebirdSelectParser(sql);

			parser.Parse();

			Assert.That(parser.ParamsToTypeCast, Is.Empty);
		}

		[Test]
		public void SelectFirstParamWithoutParamInSelectClause()
		{
			var sql = SqlString.Parse("select first ? two, three from table");
			var parser = new FirebirdSelectParser(sql);

			parser.Parse();

			Assert.That(parser.ParamsToTypeCast, Is.Empty);
		}

		[Test]
		public void SelectFirstAndSkipParamsWithoutParamInSelectClause()
		{
			var sql = SqlString.Parse("select first ? skip ? two, three from table");
			var parser = new FirebirdSelectParser(sql);

			parser.Parse();

			Assert.That(parser.ParamsToTypeCast, Is.Empty);
		}

		[Test]
		public void SelectFirstParamWithParamInWhereClause()
		{
			var sql = SqlString.Parse("select first ? two, ? from table");
			var parser = new FirebirdSelectParser(sql);

			parser.Parse();

			Assert.That(parser.ParamsToTypeCast.Any(x => x.ParamPos == 1), Is.True);
		}

		[Test]
		public void SelectCaseWhenWithParams()
		{
			var sql = SqlString.Parse("select one, two, three from table order by (case when b.id = ? then ? else ? end)");
			var parser = new FirebirdSelectParser(sql);

			parser.Parse();

			Assert.That(parser.ParamsToTypeCast.Count, Is.EqualTo(1));
			Assert.That(parser.ParamsToTypeCast.SingleOrDefault(x => x.ParamPos == 1), Is.Not.Null);
		}

		[Test]
		public void SelectWhereParamIn()
		{
			var sql = SqlString.Parse("select one, two, three from table where ? in (select one from table)");
			var parser = new FirebirdSelectParser(sql);

			parser.Parse();

			Assert.That(parser.ParamsToTypeCast.Count, Is.EqualTo(1));
			Assert.That(parser.ParamsToTypeCast.SingleOrDefault(x => x.ParamPos == 0), Is.Not.Null);
		}
	}
}
