namespace BoolExpressions.DisjunctiveNormalForm
{
    using System.Collections.Generic;
    using BoolExpressions.DisjunctiveNormalForm.DnfAndBlockElement;

    public class DfnAndBlock<T>
    {
        public DfnAndBlock(
            IReadOnlyCollection<IDnfBlockElement<T>> elementList)
        {
            this.ElementList = elementList;
        }

        public IReadOnlyCollection<IDnfBlockElement<T>> ElementList { get; }
    }
}