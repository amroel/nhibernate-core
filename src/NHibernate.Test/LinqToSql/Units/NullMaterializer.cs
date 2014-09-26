using System.Data;
using NHibernate.LinqToSql;

namespace NHibernate.Test.LinqToSql.Units
{
	public class NullMaterializer : IMaterializer
	{
		#region IMaterializer Members

		public T Materialize<T>(IDataReader dataReader) where T : class
		{
			return null;
		}

		#endregion
	}
}
