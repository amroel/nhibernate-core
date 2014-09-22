using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NHibernate.LinqToSql
{
	public class LinqQuery<T> : IOrderedQueryable<T>
	{
		private readonly QueryProvider _provider;
		private readonly Expression _expression;

		public LinqQuery(QueryProvider provider)
			: this(provider, (System.Type)null)
		{
		}

		public LinqQuery(QueryProvider provider, System.Type staticType)
		{
			if (provider == null)
				throw new ArgumentNullException("queryProvider");

			_provider = provider;
			_expression = staticType != null
				? Expression.Constant(this, staticType)
				: Expression.Constant(this);
		}

		public LinqQuery(QueryProvider queryProvider, Expression expression)
		{
			if (queryProvider == null)
				throw new ArgumentNullException("queryProvider");
			if (expression == null)
				throw new ArgumentNullException("expression");

			if (!typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
				throw new ArgumentOutOfRangeException("expression");

			_provider = queryProvider;
			_expression = expression;
		}

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator()
		{
			return ((IEnumerable<T>)_provider.Execute(_expression)).GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)_provider.Execute(_expression)).GetEnumerator();
		}

		#endregion

		#region IQueryable Members

		public System.Type ElementType
		{
			get { return typeof(T); }
		}

		public Expression Expression
		{
			get { return _expression; }
		}

		public IQueryProvider Provider
		{
			get { return _provider; }
		}

		#endregion
	}
}
