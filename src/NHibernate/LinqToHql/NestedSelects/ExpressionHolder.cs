using System.Linq.Expressions;

namespace NHibernate.LinqToHql.NestedSelects
{
	class ExpressionHolder
	{
		public int Tuple { get; set; }
		public Expression Expression { get; set; }
	}
}