using System.Data;
using System.Linq;

namespace NHibernate.LinqToSql
{
	public class MaterializerProcessorMaterializer : IMaterializer
	{
		private readonly MaterializationProcess _materializationProcess;
		private readonly IMaterializationProcessor _materializationProcessor;

		public MaterializerProcessorMaterializer(IMaterializationProcessor materializationProcessor, MaterializationProcess materializationProcess)
		{
			_materializationProcessor = materializationProcessor;
			_materializationProcess = materializationProcess;			
		}

		#region IMaterializer Members

		public T Materialize<T>(IDataReader dataReader) where T : class
		{
			_materializationProcess.DataReader = dataReader;
			_materializationProcessor.Process(_materializationProcess);
			var result = _materializationProcess.MaterializationResult;
			if (result == null)
				return null;

			if (result is T)
				return result as T;
			return ((object[])result).FirstOrDefault(x => x.GetType() == typeof(T)) as T;
		}

		#endregion
	}
}
