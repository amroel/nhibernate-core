using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.LinqToSql;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Test.LinqToSql.EndToEnd
{
	[TestFixture]
	public class QueryingSingleEntityWithMappedUserType : TestCaseMappingByCode
	{
		[Test]
		public void WithUserTypeProperties()
		{
			var entity = new SimpleUserTypeEntity { Id = 1, SillyType = new MySillyType { MyValue = 1 } };
			Persist(entity);
			try
			{
				using (var session = sessions.OpenSession())
				{
					var qry = session.Query<SimpleUserTypeEntity>();
					var result = qry.ToList();

					result.Should().Have.Count.EqualTo(1);
					result[0].Id.Should().Be(1);
					result[0].SillyType.Should().Be(new MySillyType { MyValue = 1 });
				}
			}
			finally
			{
				Delete(entity);
			}
		}

		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<SimpleUserTypeEntity>(map =>
			{
				map.Id(simple => simple.Id, idMap => idMap.Generator(Generators.Assigned));
				map.Property(entity => entity.SillyType, m => m.Type<MySillyUserType>());
			});

			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		private void Persist(SimpleUserTypeEntity entity)
		{
			using (var session = sessions.OpenSession())
			{
				using (var txn = session.BeginTransaction())
				{
					session.Persist(entity);
					txn.Commit();
				}

			}
		}

		private void Delete(SimpleUserTypeEntity entity)
		{
			using (var session = sessions.OpenSession())
			{
				using (var txn = session.BeginTransaction())
				{
					session.Delete(entity);
					txn.Commit();
				}
			}
		}
	}
}
