using NHibernate.LinqToHql;

namespace NHibernate.Test.LinqToHql
{
    static class ExtensionMethods
    {
        [LinqExtensionMethod("Replace")]
        public static string ReplaceExtension(this string subject, string search, string replaceWith)
        {
            return null;
        }

        [LinqExtensionMethod]
        public static string Replace(this string subject, string search, string replaceWith)
        {
            return null;
        }
    }
}
