using Remotion.Linq.Clauses.ResultOperators;

namespace NHibernate.LinqToHql.ResultOperators
{
	public class NonAggregatingGroupBy : ClientSideTransformOperator
	{
		public NonAggregatingGroupBy(GroupResultOperator groupBy)
		{
			GroupBy = groupBy;
		}

		public GroupResultOperator GroupBy { get; private set; }
	}
}