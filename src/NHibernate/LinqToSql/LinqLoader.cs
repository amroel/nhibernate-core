using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace NHibernate.LinqToSql
{
	public class LinqLoader<T> : IEnumerable<T> where T : class, new()
	{
		private Enumerator _enumerator;

		public LinqLoader(IDataReader dataReader, IMaterializer materializer)
		{
			_enumerator = new Enumerator(dataReader, materializer);
		}

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator()
		{
			Enumerator enumerator = _enumerator;
			if (enumerator == null)
				throw new InvalidOperationException("Cannot enumerate more than once");

			_enumerator = null;
			return enumerator;
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		private class Enumerator : IEnumerator<T>
		{
			private readonly IDataReader _dataReader;
			private readonly PropertyInfo[] _fields;
			private readonly IMaterializer _materializer;
            private T _current;

			public Enumerator(IDataReader dataReader, IMaterializer materializer)
			{
				_dataReader = dataReader;
				_materializer = materializer;
				_fields = typeof(T).GetProperties();
			}

			#region IEnumerator<T> Members

			public T Current
			{
				get { return _current; }
			}

			#endregion

			#region IDisposable Members

			public void Dispose()
			{
				_dataReader.Dispose();
			}

			#endregion

			#region IEnumerator Members

			object IEnumerator.Current
			{
				get { return _current; }
			}

			public bool MoveNext()
			{
				if (_dataReader.Read())
				{
					T instance = _materializer.Materialize<T>(_dataReader);
					_current = instance;
					return true;
				}

				return false;
			}

			public void Reset()
			{				
			}

			#endregion

		}
	}
}
