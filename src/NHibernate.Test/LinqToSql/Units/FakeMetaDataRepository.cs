using System.Collections.Generic;
using NHibernate.LinqToSql;
using NHibernate.Persister.Entity;
using System;

namespace NHibernate.Test.LinqToSql.Units
{
	public class FakeMetaDataRepository : IMetaDataRepository
	{
		private readonly Dictionary<System.Type, string> _tableMap = new Dictionary<System.Type, string>();

		public void AddTableMapFor<TEntity>(string tableName)
		{
			_tableMap.Add(typeof(TEntity), tableName);
		}

		#region IMetaDataRepository Members

		public IQueryable LoadingInfoFor(System.Type entityType)
		{
			return new FakeQueryable { TableName = _tableMap[entityType] };
		}

		#endregion
	}
}
