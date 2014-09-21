using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq_NEW;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Test.Linq_NEW.EndToEnd
{
	class SimpleEntity
	{
		public virtual int Id { get; set; }
		public virtual int Simple { get; set; }
	}

	[TestFixture]
	public class QueryingSingleSimpleEntity : TestCaseMappingByCode
	{
		[Test]
		public void SimplestPossible()
		{
			var entity = new SimpleEntity { Id = 1, Simple = 1 };
			Persist(entity);
			try
			{
				using (var session = sessions.OpenSession())
				{
					var qry = session.NewQuery<SimpleEntity>();
					qry.ToList().Should().Have.Count.EqualTo(1);
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
			mapper.Class<SimpleEntity>(map =>
			{
				map.Id(simple => simple.Id, idMap => idMap.Generator(Generators.Assigned));
				map.Property(entity => entity.Simple);
			});

			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		private void Persist(SimpleEntity entity)
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

		private void Delete(SimpleEntity entity)
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
