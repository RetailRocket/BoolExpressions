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

            var dnfExpression = new HashSet<DnfAnd<T>>();

            foreach (var trueCombination in trueCombinationList)
            {
                var elementSet = new HashSet<IDnfVariable<T>>();

                foreach (var keyValue in trueCombination)
                {
                    var variable = keyValue.Value ?
                        (IDnfVariable<T>)new DnfVariable<T>(value: keyValue.Key) :
                        (IDnfVariable<T>)new DnfNotVariable<T>(value: keyValue.Key);

                    elementSet.Add(
                        item: variable);
                }

                dnfExpression.Add(new DnfAnd<T>(
                    elementSet: elementSet));
            }

            return new DnfExpression<T>(
                andBlockSet: dnfExpression);
        }
    }
}