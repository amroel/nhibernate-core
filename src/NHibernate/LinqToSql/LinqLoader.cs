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

		public LinqLoader(IDataReader dataReader)
		{
			_enumerator = new Enumerator(dataReader);
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
            private T _current;
			private int[] _fieldLookup;
			private readonly PropertyInfo[] _fields;

			public Enumerator(IDataReader dataReader)
			{
				_dataReader = dataReader;
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
					if (_fieldLookup == null)
					{
						InitFieldLookup();
					}

					T instance = new T();

					for (int i = 0, n = _fields.Length; i < n; i++)
					{
						int index = _fieldLookup[i];

						if (index >= 0)
						{
							var fi = _fields[i];
							if (_dataReader.IsDBNull(index))
							{
								fi.SetValue(instance, null, null);
							}
							else
							{
								fi.SetValue(instance, _dataReader.GetValue(index), null);
							}
						}
					}

					_current = instance;
					return true;
				}

				return false;
			}

			public void Reset()
			{				
			}

			#endregion

			private void InitFieldLookup()
			{
				var map = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
				for (int i = 0, n = _dataReader.FieldCount; i < n; i++)
				{
					map.Add(_dataReader.GetName(i), i);
				}
				_fieldLookup = new int[_fields.Length];
				for (int i = 0; i < _fields.Length; i++)
				{
					int index;
					if (map.TryGetValue(_fields[i].Name, out index))
					{
						_fieldLookup[i] = index;
					}
					else
					{
						_fieldLookup[i] = -1;
					}
				}
			}
		}
	}
}
