﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Linq;
using NUnit.Framework;
using NHibernate.Linq;

namespace NHibernate.Test.NHSpecificTest.GH2549
{
	using System.Threading.Tasks;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		protected override void OnSetUp()
		{
			using (var s = OpenSession())
			using (var t = s.BeginTransaction())
			{
				s.Save(new Person {Id = 1, Name = "Name"});
				s.Save(new Customer {Deleted = false, Name = "Name", Id = 1});
				s.Save(new Customer {Deleted = true, Name = "Name", Id = 2});

				t.Commit();
			}
		}

		protected override void OnTearDown()
		{
			using (var s = OpenSession())
			using (var t = s.BeginTransaction())
			{
				s.CreateQuery("delete from System.Object").ExecuteUpdate();
				t.Commit();
			}
		}

		[Test]
		public async Task EntityJoinFilterLinqAsync()
		{
			using (var s = OpenSession())
			{
				var list = await ((from p in s.Query<Person>()
							join c in s.Query<Customer>() on p.Name equals c.Name
							select p).ToListAsync());

				s.EnableFilter("DeletedCustomer").SetParameter("deleted", false);

				var filteredList = await ((from p in s.Query<Person>()
									join c in s.Query<Customer>() on p.Name equals c.Name
									select p).ToListAsync());

				Assert.That(list, Has.Count.EqualTo(2));
				Assert.That(filteredList, Has.Count.EqualTo(1));
			}
		}

		[Test]
		public async Task EntityJoinFilterQueryOverAsync()
		{
			using (var s = OpenSession())
			{
				Customer c = null;
				Person p = null;
				var list = await (s.QueryOver(() => p).JoinEntityAlias(() => c, () => c.Name == p.Name).ListAsync());

				s.EnableFilter("DeletedCustomer").SetParameter("deleted", false);

				var filteredList = await (s.QueryOver(() => p).JoinEntityAlias(() => c, () => c.Name == p.Name).ListAsync());

				Assert.That(list, Has.Count.EqualTo(2));
				Assert.That(filteredList, Has.Count.EqualTo(1));
			}
		}
	}
}