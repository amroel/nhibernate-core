using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Hql.Ast;
using NHibernate.LinqToHql.Visitors;

namespace NHibernate.LinqToHql.Functions
{
    public interface IHqlGeneratorForMethod
    {
        IEnumerable<MethodInfo> SupportedMethods { get; }
        HqlTreeNode BuildHql(MethodInfo method, Expression targetObject, ReadOnlyCollection<Expression> arguments, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor);
    }
}