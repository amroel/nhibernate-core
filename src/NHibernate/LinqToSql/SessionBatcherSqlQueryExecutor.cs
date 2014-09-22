using System;
using System.Data;
using NHibernate.Engine;

namespace NHibernate.LinqToSql
{
	public class SessionBatcherSqlQueryExecutor : ISqlQueryExecutor
	{
		private readonly IBatcher _batcher;
		private IDbCommand _command;
		private IDataReader _innerReader;

		public SessionBatcherSqlQueryExecutor(IBatcher batcher)
		{
			_batcher = batcher;
		}

		#region ISqlQueryExecutor Members

		public void Run(TranslationResult translationResult)
		{
			_command = _batcher.PrepareQueryCommand(CommandType.Text, translationResult.Sql, translationResult.ParameterTypes);
			_innerReader = _batcher.ExecuteReader(_command);
		}

		#endregion

		#region IDataReader Members

		public void Close()
		{
			_batcher.CloseCommand(_command, _innerReader);
		}

		public int Depth
		{
			get { return _innerReader.Depth; }
		}

		public DataTable GetSchemaTable()
		{
			return _innerReader.GetSchemaTable();
		}

		public bool IsClosed
		{
			get { return _innerReader.IsClosed; }
		}

		public bool NextResult()
		{
			return _innerReader.NextResult();
		}

		public bool Read()
		{
			return _innerReader.Read();
		}

		public int RecordsAffected
		{
			get { return _innerReader.RecordsAffected; }
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			_batcher.CloseCommand(_command, _innerReader);
		}

		#endregion

		#region IDataRecord Members

		public int FieldCount
		{
			get { return _innerReader.FieldCount; }
		}

		public bool GetBoolean(int i)
		{
			return _innerReader.GetBoolean(i);
		}

		public byte GetByte(int i)
		{
			return _innerReader.GetByte(i);
		}

		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			return _innerReader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
		}

		public char GetChar(int i)
		{
			return _innerReader.GetChar(i);
		}

		public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			return _innerReader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
		}

		public IDataReader GetData(int i)
		{
			return _innerReader.GetData(i);
		}

		public string GetDataTypeName(int i)
		{
			return _innerReader.GetDataTypeName(i);
		}

		public DateTime GetDateTime(int i)
		{
			return _innerReader.GetDateTime(i);
		}

		public decimal GetDecimal(int i)
		{
			return _innerReader.GetDecimal(i);
		}

		public double GetDouble(int i)
		{
			return _innerReader.GetDouble(i);
		}

		public System.Type GetFieldType(int i)
		{
			return _innerReader.GetFieldType(i);
		}

		public float GetFloat(int i)
		{
			return _innerReader.GetFloat(i);
		}

		public Guid GetGuid(int i)
		{
			return _innerReader.GetGuid(i);
		}

		public short GetInt16(int i)
		{
			return _innerReader.GetInt16(i);
		}

		public int GetInt32(int i)
		{
			return _innerReader.GetInt32(i);
		}

		public long GetInt64(int i)
		{
			return _innerReader.GetInt64(i);
		}

		public string GetName(int i)
		{
			return _innerReader.GetName(i);
		}

		public int GetOrdinal(string name)
		{
			return _innerReader.GetOrdinal(name);
		}

		public string GetString(int i)
		{
			return _innerReader.GetString(i);
		}

		public object GetValue(int i)
		{
			return _innerReader.GetValue(i);
		}

		public int GetValues(object[] values)
		{
			return _innerReader.GetValues(values);
		}

		public bool IsDBNull(int i)
		{
			return _innerReader.IsDBNull(i);
		}

		public object this[string name]
		{
			get { return _innerReader[name]; }
		}

		public object this[int i]
		{
			get { return _innerReader[i]; }
		}

		#endregion
	}
}
