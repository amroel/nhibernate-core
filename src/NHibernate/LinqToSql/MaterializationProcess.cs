using System.Data;
using NHibernate.Engine;

namespace NHibernate.LinqToSql
{
	public class MaterializationProcess
	{
		public MaterializationProcess(TranslationResult translationResult)
		{
			TranslationResult = translationResult;			
		}

		public TranslationResult TranslationResult { get; private set; }
		public IDataReader DataReader { get; set; }
		public EntityKey[] EntityKeys { get; set; }
		public object MaterializationResult { get; set; }
	}
}
