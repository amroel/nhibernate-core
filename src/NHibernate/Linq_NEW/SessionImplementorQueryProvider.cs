using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using NHibernate.Engine;
using System.Collections.ObjectModel;

namespace NHibernate.Linq_NEW
{
	public class SessionImplementorQueryProvider : QueryProvider
	{
		private readonly ISessionImplementor _session;

		public SessionImplementorQueryProvider(ISessionImplementor session)
		{
			_session = session;
		}

		public override object Execute(Expression expression)
		{
			LambdaExpression lambda = expression as LambdaExpression;

			//if (lambda == null && _cache != null && expression.NodeType != ExpressionType.Constant)
			//{
			//	return _cache.Execute(expression);
			//}

			Expression plan = GetExecutionPlan(expression);

			if (lambda != null)
			{
				// compile & return the execution plan so it can be used multiple times
				LambdaExpression fn = Expression.Lambda(lambda.Type, plan, lambda.Parameters);
				return fn.Compile();
			}
			else
			{
				// compile the execution plan and invoke it
				Expression<Func<object>> efn = Expression.Lambda<Func<object>>(Expression.Convert(plan, typeof(object)));
				Func<object> fn = efn.Compile();
				return fn();
			}
		}

		private Expression GetExecutionPlan(Expression expression)
		{
			// strip off lambda for now
			LambdaExpression lambda = expression as LambdaExpression;
			if (lambda != null)
				expression = lambda.Body;

			var parameters = lambda != null ? lambda.Parameters : null;
			Expression provider = Find(expression, parameters, typeof(SessionImplementorQueryProvider));
			if (provider == null)
			{
				Expression rootQueryable = Find(expression, parameters, typeof(IQueryable));
				provider = Expression.Property(rootQueryable, typeof(IQueryable).GetProperty("Provider"));
			}

			// translate query into client & server parts
			var translator = new QueryTranslator();
			return translator.BuildExecutionPlan(provider);
			Expression translation = translator.Translate(expression);
			return translator.Police.BuildExecutionPlan(translation, provider);
		}

		private Expression Find(Expression expression, IEnumerable<ParameterExpression> parameters, System.Type type)
		{
			if (parameters != null)
			{
				Expression found = parameters.FirstOrDefault(p => type.IsAssignableFrom(p.Type));
				if (found != null)
					return found;
			}

			return TypedSubtreeFinder.Find(expression, type);
		}
	}
}
