using System;
using System.Collections.Generic;
using System.Linq;
using BoolExpressions.QuineMcCluskeyMethod.Term;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("UnitTests")]

namespace BoolExpressions.QuineMcCluskeyMethod.FinalImplicantMethod
{
    internal static class ImplicantExtension
    {
        internal static Implicant<T> CombineImplicants<T>(
            this Implicant<T> implicantA,
            Implicant<T> implicantB) where T : class
        {
            var variableTermMapB = implicantB.TermSet.ToDictionary(term => term.Value, term => term);
            var combinedMinterm = implicantA
              .TermSet
              .Select(termA =>
              {
                  var variable = termA.Value;
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

        internal static HashSet<T> GetCombinedValueSet<T>(
            this Implicant<T> implicant) where T : class
        {
            return implicant
                    .TermSet
                    .Where(term => term is CombinedTerm<T>)
                    .Select(term => term.Value)
                    .ToHashSet();
        }

        internal static int GetCombinedValueDistance<T>(
            this Implicant<T> implicantA,
            Implicant<T> implicantB) where T : class
        {
            var combinedValuesA = GetCombinedValueSet(implicantA);
            var combinedValuesB = GetCombinedValueSet(implicantB);

            var difference = combinedValuesA
                .Union(combinedValuesB)
                .Except(combinedValuesA
                    .Intersect(combinedValuesB));

            return difference.Count();
        }
    }
}
