using System;
using System.Data;
using System.Linq.Expressions;
using NHibernate.Engine;
using NHibernate.Mapping.ByCode;

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
			var translationResult = Translate(expression);
			var command = _session.Batcher.PrepareQueryCommand(CommandType.Text, translationResult.Sql, translationResult.ParameterTypes);
			var dataReader = _session.Batcher.ExecuteReader(command);
			var elementType = expression.Type.DetermineCollectionElementType() ?? expression.Type;
			var result = Activator.CreateInstance(typeof(LinqLoader<>).MakeGenericType(elementType), new object[] { dataReader, (System.Action)(() => _session.Batcher.CloseCommand(command, dataReader)) });
			return result;
		}

		private TranslationResult Translate(Expression expression)
		{
			return new QueryTranslator(_session).Translate(expression);
		}
	}
}
