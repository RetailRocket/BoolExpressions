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
            INcfExpression<T> ncfExpression) where T : class
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
                var elementSet = new HashSet<IDnfOperation<T>>();

                foreach (var keyValue in trueCombination)
                {
                    var dnfVar = new DnfVariable<T>(
                        value: keyValue.Key);

                    elementSet.Add(
                        item: keyValue.Value ?
                            (IDnfOperation<T>)dnfVar :
                            new DnfNot<T>(variable: dnfVar));
                }

                dnfExpression.Add(new DnfAnd<T>(
                    elementSet: elementSet));
            }

            return new DnfExpression<T>(
                andBlockSet: dnfExpression);
        }
    }
}