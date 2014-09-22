using System.Linq.Expressions;

namespace NHibernate.LinqToHql.Expressions
{
	public class NhSumExpression : NhAggregatedExpression
	{
		public NhSumExpression(Expression expression)
			: base(expression, NhExpressionType.Sum)
		{
		}

		public override Expression CreateNew(Expression expression)
		{
			return new NhSumExpression(expression);
		}
	}
}