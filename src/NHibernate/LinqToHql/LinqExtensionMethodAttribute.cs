using System;

namespace NHibernate.LinqToHql
{
	public class LinqExtensionMethodAttribute: Attribute
	{
		public LinqExtensionMethodAttribute()
		{
		}

		public LinqExtensionMethodAttribute(string name)
		{
			Name = name;
		}

		public string Name { get; private set; }
	}
}