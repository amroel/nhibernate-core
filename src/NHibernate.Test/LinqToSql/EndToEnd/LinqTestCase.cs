using System;
using System.Collections.Generic;
using System.Linq;
using SharpTestsEx;

namespace NHibernate.Test.LinqToSql.EndToEnd
{
	public abstract class LinqTestCase : TestCaseMappingByCode
	{
		private Northwind _northwind;
		private ISession _session;

		protected Northwind db
		{
			get { return _northwind; }
		}

		protected ISession session
		{
			get { return _session; }
		}

		protected override void OnSetUp()
		{
			base.OnSetUp();

			_session = OpenSession();
			_northwind = new Northwind(_session);
		}

		protected override void OnTearDown()
		{
			if (_session.IsOpen)
			{
				_session.Close();
			}
		}

		public void AssertByIds<TEntity, TId>(IEnumerable<TEntity> entities, TId[] expectedIds, Converter<TEntity, TId> entityIdGetter)
		{
			entities.Select(x => entityIdGetter(x)).Should().Have.SameValuesAs(expectedIds);
		}
	}
}