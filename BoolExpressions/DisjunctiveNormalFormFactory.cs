namespace BoolExpressions
{
    using System.Collections.Generic;
    using System.Linq;
    using BoolExpressions.NonCanonicalForm;
    using ConditionTree.BoolExpression.DisjunctiveNormalForm;
    using ConditionTree.BoolExpression.DisjunctiveNormalForm.DnfAndBlockElement;

    public static class DisjunctiveNormalFormFactory
    {
        public static DnfExpression<T> Build<T>(
            INcfExpression<T> ncfExpression)
        {
            var allPossibleVarCombinationOf = NonCanonicalBoolExpressionHelpers.AllPossibleVarCombinationFor(
                ncfExpression: ncfExpression);

            var trueCombinationList = allPossibleVarCombinationOf.Where(
                line => NonCanonicalBoolExpressionHelpers.Execute(
                    ncfExpression,
                    line));

            var dnfExpression = new List<DfnAndBlock<T>>();

            foreach (var trueCombination in trueCombinationList)
            {
                var elementList = new List<IDnfBlockElement<T>>();

                foreach (var keyValue in trueCombination)
                {
                    var dnfVar = new DnfVariable<T>(
                        value: keyValue.Key);

                    elementList.Add(
                        item: keyValue.Value ?
                            (IDnfBlockElement<T>)keyValue.Key :
                            new DfnNot<T>(variable: dnfVar));
                }

                dnfExpression.Add(new DfnAndBlock<T>(
                    elementList: elementList));
            }

            return new DnfExpression<T>(
                andBlockList: dnfExpression);
        }
    }
}