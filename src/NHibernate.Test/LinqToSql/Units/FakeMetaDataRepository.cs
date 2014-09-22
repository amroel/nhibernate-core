using System.Collections.Generic;
using NHibernate.LinqToSql;

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

		public string TableNameFor(System.Type entityType)
		{
			return _tableMap[entityType];
		}

		#endregion
	}
}
