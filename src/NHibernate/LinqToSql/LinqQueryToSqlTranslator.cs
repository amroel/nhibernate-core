using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;

namespace NHibernate.LinqToSql
{
	public class LinqQueryToSqlTranslator : ExpressionVisitor, ILinqToSqlTranslator
	{
		private StringBuilder _sqlString;
		private readonly IMetaDataRepository _metaDataRepository;
		private readonly List<ILoadable> _loadables = new List<ILoadable>();

		public LinqQueryToSqlTranslator(IMetaDataRepository metaDataRepository)
		{
			_metaDataRepository = metaDataRepository;
		}

		public TranslationResult Translate(Expression expression)
		{
			_sqlString = new StringBuilder();
			Visit(expression);

			return new TranslationResult
			{
				Sql = new SqlString(_sqlString.ToString()),
				ParameterTypes = new SqlTypes.SqlType[] { },
				LoadableEntities = _loadables.ToArray()
			};
		}
		protected override Expression VisitConstant(ConstantExpression c)
		{
			System.Linq.IQueryable q = c.Value as System.Linq.IQueryable;
			if (q != null)
			{
				_sqlString.Append("SELECT * FROM ");
				var queryable = _metaDataRepository.LoadingInfoFor(q.ElementType);
				string tableName = queryable.TableName;
				_sqlString.Append(tableName);
				_loadables.Add(queryable);
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
	}
}
