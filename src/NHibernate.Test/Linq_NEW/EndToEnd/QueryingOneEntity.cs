using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
			var actual = from user in db.Users
						 select user;

			actual.ToList().Should().Have.Count.EqualTo(3);
		}
	}
}
