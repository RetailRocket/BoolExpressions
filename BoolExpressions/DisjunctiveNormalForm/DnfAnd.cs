namespace BoolExpressions.DisjunctiveNormalForm
{
    using System.Collections.Generic;
    using BoolExpressions.DisjunctiveNormalForm.Operation;

    public class DnfAnd<T>
    {
        public DnfAnd(
            IReadOnlyCollection<IDnfOperation<T>> elementList)
        {
            this.ElementList = elementList;
        }

        public IReadOnlyCollection<IDnfOperation<T>> ElementList { get; }
    }
}