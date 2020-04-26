namespace BoolExpressions.DisjunctiveNormalForm
{
    using System.Collections.Generic;
    using BoolExpressions.DisjunctiveNormalForm.Operation;

    public class DfnAnd<T>
    {
        public DfnAnd(
            IReadOnlyCollection<IDnfOperation<T>> elementList)
        {
            this.ElementList = elementList;
        }

        public IReadOnlyCollection<IDnfOperation<T>> ElementList { get; }
    }
}