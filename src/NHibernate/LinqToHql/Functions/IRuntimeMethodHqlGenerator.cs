using System.Reflection;

namespace NHibernate.LinqToHql.Functions
{
	public interface IRuntimeMethodHqlGenerator
	{
		bool SupportsMethod(MethodInfo method);
		IHqlGeneratorForMethod GetMethodGenerator(MethodInfo method);
	}
}