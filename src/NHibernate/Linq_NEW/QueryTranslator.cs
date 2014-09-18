using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace NHibernate.Linq_NEW
{
	public class QueryTranslator
	{
		public Expression BuildExecutionPlan(Expression provider)
		{
			var translation = Translate(provider);
			return ExecutionBuilder.Build(this.translator.Linguist, this.policy, query, provider);
		}

		public Expression Translate(Expression expression)
		{
			throw new NotImplementedException();
		}
	}
}
