using Remotion.Linq.EagerFetching;

namespace NHibernate.LinqToHql.Visitors.ResultOperatorProcessors
{
    public class ProcessFetchOne : ProcessFetch, IResultOperatorProcessor<FetchOneRequest>
    {
        public void Process(FetchOneRequest resultOperator, QueryModelVisitor queryModelVisitor, IntermediateHqlTree tree)
        {
            base.Process(resultOperator, queryModelVisitor, tree);
        }
    }
}