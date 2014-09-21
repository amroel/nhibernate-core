using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;

namespace NHibernate.Linq_NEW
{
	public abstract class QueryProvider : IQueryProvider
	{
		#region IQueryProvider Members

		public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
		{
			return new LinqQuery<TElement>(this, expression);
		}

		public IQueryable CreateQuery(Expression expression)
		{
			var elementType = expression.Type.DetermineCollectionElementType() ?? expression.Type;
			try
			{
				return (IQueryable)Activator.CreateInstance(typeof(LinqQuery<>).MakeGenericType(elementType), new object[] { this, expression });
			}
			catch (TargetInvocationException e)
			{
				throw e.InnerException;
			}
		}

		public TResult Execute<TResult>(Expression expression)
		{
			return (TResult)Execute(expression);
		}

		public abstract object Execute(Expression expression);

		#endregion
	}
}
