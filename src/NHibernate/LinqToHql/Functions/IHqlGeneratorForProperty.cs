using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Hql.Ast;
using NHibernate.LinqToHql.Visitors;

namespace NHibernate.LinqToHql.Functions
{
    public interface IHqlGeneratorForProperty
    {
        IEnumerable<MemberInfo> SupportedProperties { get; }
        HqlTreeNode BuildHql(MemberInfo member, Expression expression, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor);
    }
}