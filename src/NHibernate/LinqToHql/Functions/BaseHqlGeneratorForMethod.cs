using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Hql.Ast;
using NHibernate.LinqToHql.Visitors;

namespace NHibernate.LinqToHql.Functions
{
    public abstract class BaseHqlGeneratorForMethod : IHqlGeneratorForMethod
    {
        public IEnumerable<MethodInfo> SupportedMethods { get; protected set; }

        public abstract HqlTreeNode BuildHql(MethodInfo method, Expression targetObject, ReadOnlyCollection<Expression> arguments, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor);
    }
}