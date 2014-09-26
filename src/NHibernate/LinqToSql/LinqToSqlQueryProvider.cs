using System;
using System.Linq.Expressions;
using NHibernate.Mapping.ByCode;

namespace NHibernate.LinqToSql
{
	public class LinqToSqlQueryProvider : QueryProvider
	{
		private readonly ISqlQueryExecutor _executor;
		private readonly ILinqToSqlTranslator _translator;

		public LinqToSqlQueryProvider(ILinqToSqlTranslator translator, ISqlQueryExecutor executor)
		{
			_translator = translator;
			_executor = executor;
		}

		public override object Execute(Expression expression)
		{
			var translationResult = _translator.Translate(expression);
			var materializer = _executor.Run(translationResult);
			var elementType = expression.Type.DetermineCollectionElementType() ?? expression.Type;
			var result = Activator.CreateInstance(typeof(LinqLoader<>).MakeGenericType(elementType), new object[] { _executor, materializer });
			return result;
		}
	}
}
