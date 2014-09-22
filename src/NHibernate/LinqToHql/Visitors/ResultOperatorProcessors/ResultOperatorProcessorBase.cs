using Remotion.Linq.Clauses;

namespace NHibernate.LinqToHql.Visitors.ResultOperatorProcessors
{
    public abstract class ResultOperatorProcessorBase
    {
        public abstract void Process(ResultOperatorBase resultOperator, QueryModelVisitor queryModel, IntermediateHqlTree tree);
    }
}