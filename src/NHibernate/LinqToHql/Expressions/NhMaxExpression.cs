using System.Linq.Expressions;

namespace NHibernate.LinqToHql.Expressions
{
	public class NhMaxExpression : NhAggregatedExpression
	{
		public NhMaxExpression(Expression expression)
			: base(expression, NhExpressionType.Max)
		{
		}

		public override Expression CreateNew(Expression expression)
		{
			return new NhMaxExpression(expression);
		}
	}
}