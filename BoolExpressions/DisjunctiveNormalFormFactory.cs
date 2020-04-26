namespace BoolExpressions
{
    using System.Collections.Generic;
    using System.Linq;
    using BoolExpressions.DisjunctiveNormalForm;
    using BoolExpressions.DisjunctiveNormalForm.Operation;
    using BoolExpressions.NonCanonicalForm;

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

            var dnfExpression = new List<DnfAnd<T>>();

            foreach (var trueCombination in trueCombinationList)
            {
                var elementList = new List<IDnfOperation<T>>();

                foreach (var keyValue in trueCombination)
                {
                    var dnfVar = new DnfVariable<T>(
                        value: keyValue.Key);

                    elementList.Add(
                        item: keyValue.Value ?
                            (IDnfOperation<T>)dnfVar :
                            new DnfNot<T>(variable: dnfVar));
                }

                dnfExpression.Add(new DnfAnd<T>(
                    elementList: elementList));
            }

            return new DnfExpression<T>(
                andBlockList: dnfExpression);
        }
    }
}