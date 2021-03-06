namespace BoolExpressions.NonCanonicalForm
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class NonCanonicalBoolExpressionHelpers
    {
        public static bool Execute<T>(
            INcfExpression<T> ncfExpression,
            Dictionary<T, bool> values)
        {
            switch (ncfExpression)
            {
                case NcfVariable<T> v:
                    return values[v.Value];
                case NcfAndBlock<T> and:
                    return and.TermList.All(t => Execute(t, values));
                case NcfOrBlock<T> and:
                    return and.TermList.Any(t => Execute(t, values));
                case NcfNot<T> not:
                    return !Execute(not.NcfExpression, values);
                default:
                    throw new ArgumentException(
                        message: "pattern matching in C# is sucks",
                        paramName: nameof(ncfExpression));
            }
        }

        public static HashSet<T> GetVariables<T>(
            INcfExpression<T> ncfExpression)
        {
            switch (ncfExpression)
            {
                case NcfVariable<T> v:
                    return new HashSet<T> {v.Value};
                case INcfBlock<T> blockOf:
                    return new HashSet<T>(
                        collection: blockOf.TermList.SelectMany(GetVariables));
                case NcfNot<T> not:
                    return GetVariables(ncfExpression: not.NcfExpression);
                default:
                    throw new ArgumentException(
                        message: "pattern matching in C# is sucks",
                        paramName: nameof(ncfExpression));
            }
        }

        public static List<Dictionary<T, bool>> AllPossibleVarCombinationFor<T>(
            INcfExpression<T> ncfExpression)
        {
            var listOfVar = GetVariables(ncfExpression: ncfExpression)
                .ToList();

            var numberOfAllPossibleCombination = (ulong)Math.Pow(2, listOfVar.Count);
            var tableOfAllPossibleCombination = new List<Dictionary<T, bool>>();
            var numberOfVars = listOfVar.Count;
            uint binaryMaskForLine = 0;

            for (var i = 0u; i < numberOfAllPossibleCombination; ++i)
            {
                var lineOfCombination = new Dictionary<T, bool>(
                    capacity: numberOfVars);

                uint digit = 1;
                for (var j = 0; j < numberOfVars; j++)
                {
                    lineOfCombination.Add(
                        key: listOfVar[j],
                        value: (binaryMaskForLine & digit) > 0);

                    digit <<= 1;
                }

                binaryMaskForLine += 1;

                tableOfAllPossibleCombination.Add(lineOfCombination);
            }

            return tableOfAllPossibleCombination;
        }
    }
}