using System;
using System.Collections.Generic;
using System.Linq;
using BoolExpressions.QuineMcCluskeyMethod.Term;
using static BoolExpressions.QuineMcCluskeyMethod.Term.Factories;

namespace BoolExpressions.QuineMcCluskeyMethod
{
    public class ImplicantHelpers
    {
        public static int getCombinedVariableDistance<T>(
            Implicant<T> a,
            Implicant<T> b) where T : class
        {
            Func<Implicant<T>, HashSet<T>> getCombinedVariables = (Implicant<T> implicant) =>
            {
                return implicant
                    .Minterm
                    .Where((term) =>
                    {
                        return term switch
                        {
                            CombinedTerm<T> _ => true,
                            _ => false
                        };
                    })
                    .Select(term => term.Variable)
                    .ToHashSet();
            };

            var combinedVariablesA = getCombinedVariables(a);
            var combinedVariablesB = getCombinedVariables(b);

            var difference = combinedVariablesA
                .Union(combinedVariablesB)
                .Except(combinedVariablesA
                    .Intersect(combinedVariablesB));

            return difference.Count();
        }

        public static Implicant<T> combineImplicants<T>(
            Implicant<T> implicantA,
            Implicant<T> implicantB) where T : class
        {
            var variableTermMapB = implicantB.Minterm.ToDictionary(term => term.Variable, term => term);
            var combinedMinterm = implicantA
              .Minterm
              .Select((termA) =>
              {
                  var variable = termA.Variable;
                  var termB = variableTermMapB[variable];
                  return (termA, termB) switch
                  {
                      (PositiveTerm<T> _, NegativeTerm<T> _) => new CombinedTerm<T>(variable),
                      (NegativeTerm<T> _, PositiveTerm<T> _) => new CombinedTerm<T>(variable),
                      _ => termA
                  };
              })
              .ToHashSet();
            return new Implicant<T>(combinedMinterm);
        }
    }
}
