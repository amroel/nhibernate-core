using System;
using System.Data;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace NHibernate.Test.LinqToSql
{
	public class SimpleUserTypeEntity
	{
		public virtual int Id { get; set; }
		public virtual MySillyType SillyType { get; set; }
	}

	public struct MySillyType
	{
		public int MyValue { get; set; }
	}

	public class MySillyUserType : IUserType
	{
		#region IUserType Members

		public SqlType[] SqlTypes
		{
			get { return new SqlType[] { NHibernateUtil.Int32.SqlType }; }
		}

		public System.Type ReturnedType
		{
			get { return typeof(MySillyType); }
		}

		public bool Equals(object x, object y)
		{
			return object.Equals(x, y);
		}

		public int GetHashCode(object x)
		{
			return x.GetHashCode();
		}

		public object NullSafeGet(IDataReader rs, string[] names, object owner)
		{
			int index0 = rs.GetOrdinal(names[0]);
			if (rs.IsDBNull(index0))
			{
				return null;
			}
			int value = rs.GetInt32(index0);
			return new MySillyType { MyValue = value };
		}

		public void NullSafeSet(IDbCommand cmd, object value, int index)
		{
			if (value == null)
			{
				((IDbDataParameter)cmd.Parameters[index]).Value = DBNull.Value;
			}
			else
			{
				MySillyType myType = (MySillyType)value;
				((IDbDataParameter)cmd.Parameters[index]).Value = myType.MyValue;
			}
		}

		public object DeepCopy(object value)
		{
			return value;
		}

		public bool IsMutable
		{
			get { return false; }
		}

		public object Replace(object original, object target, object owner)
		{
			return original;
		}

		public object Assemble(object cached, object owner)
		{
			return cached;
		}

		public object Disassemble(object value)
		{
			return value;
		}

		#endregion
	}

}
