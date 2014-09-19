using System.Linq;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Test.Linq_NEW.EndToEnd
{
	[TestFixture]
	public class QueryingOneEntity : LinqTestCase
	{
		[Test]
		public void SimplestPossible()
		{
			db.Users.ToList().Should().Have.Count.EqualTo(3);
		}
	}
}
