using System.Linq.Expressions;

namespace NHibernate.Linq_NEW
{
	/// <summary>
	/// Finds the first sub-expression that is of a specified type
	/// </summary>
	public class TypedSubtreeFinder : ExpressionVisitor
	{
		private Expression _root;
		private readonly System.Type _type;

		private TypedSubtreeFinder(System.Type type)
		{
			_type = type;
		}

		public static Expression Find(Expression expression, System.Type type)
		{
			TypedSubtreeFinder finder = new TypedSubtreeFinder(type);
			finder.Visit(expression);
			return finder._root;
		}

		public override Expression Visit(Expression exp)
		{
			Expression result = base.Visit(exp);

			// remember the first sub-expression that produces an IQueryable
			if (_root == null && result != null)
			{
				if (_type.IsAssignableFrom(result.Type))
					_root = result;
			}

			return result;
		}
	}
}
