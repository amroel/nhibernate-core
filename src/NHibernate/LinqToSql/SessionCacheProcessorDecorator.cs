using System;
using NHibernate.Engine;

namespace NHibernate.LinqToSql
{
	public class SessionCacheProcessorDecorator : IMaterializationProcessor
	{
		private readonly TwoPhaseLoadProcessorDecorator _innerProcessor;
		private readonly ISessionImplementor _session;

		public SessionCacheProcessorDecorator(ISessionImplementor session, TwoPhaseLoadProcessorDecorator innerProcessor)
		{
			_session = session;
			_innerProcessor = innerProcessor;			
		}

		#region IMaterializationProcessor Members

		public void Process(MaterializationProcess process)
		{
			_innerProcessor.Process(process);
		}

		#endregion
	}
}
