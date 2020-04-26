using System;
using System.Collections.Generic;
using System.Linq;
using BoolExpressions.QuineMcCluskeyMethod.Term;
using static BoolExpressions.QuineMcCluskeyMethod.Term.Factories;

namespace BoolExpressions.QuineMcCluskeyMethod
{
    public class ImplicantHelpers
    {
        public static int GetCombinedVariableDistance<T>(
            Implicant<T> a,
            Implicant<T> b) where T : class
        {
            Func<Implicant<T>, HashSet<T>> getCombinedVariables = (Implicant<T> implicant) =>
            {
                return implicant
                    .Minterm
                    .Where(term =>
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

        public static Implicant<T> CombineImplicants<T>(
            Implicant<T> implicantA,
            Implicant<T> implicantB) where T : class
        {
            var variableTermMapB = implicantB.Minterm.ToDictionary(term => term.Variable, term => term);
            var combinedMinterm = implicantA
              .Minterm
              .Select(termA =>
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

        public static List<Implicant<T>> CombineImplicantLists<T>(
            List<Implicant<T>> implicantListA,
            List<Implicant<T>> implicantListB) where T : class
        {
            var nextLevelImplicants = new List<Implicant<T>>();

            foreach (var implicantA in implicantListA)
            {
                foreach (var implicantB in implicantListB)
                {
                    var implicantsDistance = ImplicantHelpers.GetCombinedVariableDistance(implicantA, implicantB);
                    if (implicantsDistance != 0) continue;
                    var nextLevelImplicant = CombineImplicants(implicantA, implicantB);
                    var nextLevelImplicantDistance = ImplicantHelpers.GetCombinedVariableDistance(implicantA, nextLevelImplicant);
                    if (nextLevelImplicantDistance != 1) continue;
                    nextLevelImplicants.Add(nextLevelImplicant);
                }
            }

            return nextLevelImplicants;
        }
    }
}
