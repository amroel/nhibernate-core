using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NHibernate.Engine;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.SqlCommand;

namespace NHibernate.Linq_NEW
{
	public class LinqQueryToSqlTranslator : ExpressionVisitor
	{
		private readonly ISessionImplementor _session;
		private StringBuilder _sqlString;

		public LinqQueryToSqlTranslator(ISessionImplementor session)
		{
			_session = session;
		}

		public TranslationResult Translate(Expression expression)
		{
			_sqlString = new StringBuilder();
			Visit(expression);

			return new TranslationResult
			{
				Sql = new SqlString(_sqlString.ToString()),
				ParameterTypes = new SqlTypes.SqlType[] { }
			};
		}

		//protected override Expression VisitMethodCall(MethodCallExpression m)
		//{
		//	if (m.Method.DeclaringType == typeof(Queryable) && m.Method.Name == "Where")
		//	{
		//		_sqlString.Append("SELECT * FROM (");
		//		Visit(m.Arguments[0]);
		//		_sqlString.Append(") AS T WHERE ");

		//		LambdaExpression lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);
		//		Visit(lambda.Body);
		//		return m;
		//	}

		//	throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));
		//}

		//private static Expression StripQuotes(Expression e)
		//{
		//	while (e.NodeType == ExpressionType.Quote)
		//	{
		//		e = ((UnaryExpression)e).Operand;
		//	}
		//	return e;
		//}

		//protected override Expression VisitUnary(UnaryExpression u)
		//{
		//	switch (u.NodeType)
		//	{
		//		case ExpressionType.Not:
		//			_sqlString.Append(" NOT ");
		//			Visit(u.Operand);
		//			break;
		//		default:
		//			throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", u.NodeType));
		//	}
		//	return u;
		//}

		//protected override Expression VisitBinary(BinaryExpression b)
		//{
		//	_sqlString.Append("(");
		//	Visit(b.Left);
		//	switch (b.NodeType)
		//	{
		//		case ExpressionType.And:
		//			_sqlString.Append(" AND ");
		//			break;
		//		case ExpressionType.Or:
		//			_sqlString.Append(" OR");
		//			break;
		//		case ExpressionType.Equal:
		//			_sqlString.Append(" = ");
		//			break;
		//		case ExpressionType.NotEqual:
		//			_sqlString.Append(" <> ");
		//			break;
		//		case ExpressionType.LessThan:
		//			_sqlString.Append(" < ");
		//			break;
		//		case ExpressionType.LessThanOrEqual:
		//			_sqlString.Append(" <= ");
		//			break;
		//		case ExpressionType.GreaterThan:
		//			_sqlString.Append(" > ");
		//			break;
		//		case ExpressionType.GreaterThanOrEqual:
		//			_sqlString.Append(" >= ");
		//			break;
		//		default:
		//			throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));
		//	}

		//	Visit(b.Right);
		//	_sqlString.Append(")");
		//	return b;
		//}

		protected override Expression VisitConstant(ConstantExpression c)
		{
			IQueryable q = c.Value as IQueryable;
			if (q != null)
			{
				// assume constant nodes w/ IQueryables are table references
				_sqlString.Append("SELECT * FROM ");
				var helper = new SessionFactoryHelperExtensions(_session.Factory);
				var persistor = helper.RequireClassPersister(q.ElementType.Name) as NHibernate.Persister.Entity.IQueryable;
				_sqlString.Append(persistor.TableName);
			}
			else if (c.Value == null)
			{
				_sqlString.Append("NULL");
			}
			else
			{
				switch (System.Type.GetTypeCode(c.Value.GetType()))
				{
					case TypeCode.Boolean:
						_sqlString.Append(((bool)c.Value) ? 1 : 0);
						break;
					case TypeCode.String:
						_sqlString.Append("'");
						_sqlString.Append(c.Value);
						_sqlString.Append("'");
						break;
					case TypeCode.Object:
						throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", c.Value));
					default:
						_sqlString.Append(c.Value);
						break;
				}
			}
			return c;
		}

		//protected override Expression VisitMember(MemberExpression m)
		//{
		//	if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter)
		//	{
		//		_sqlString.Append(m.Member.Name);
		//		return m;
		//	}

		//	throw new NotSupportedException(string.Format("The member '{0}' is not supported", m.Member.Name));
		//}
	}
}
