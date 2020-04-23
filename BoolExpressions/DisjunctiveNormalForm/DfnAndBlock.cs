namespace ConditionTree.BoolExpression.DisjunctiveNormalForm
{
    using System.Collections.Generic;
    using ConditionTree.BoolExpression.DisjunctiveNormalForm.DnfAndBlockElement;

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