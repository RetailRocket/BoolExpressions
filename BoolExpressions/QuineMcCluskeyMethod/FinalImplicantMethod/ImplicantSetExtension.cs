using System;
using System.Collections.Generic;
using System.Linq;
using BoolExpressions.QuineMcCluskeyMethod.Term;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("UnitTests")]

namespace BoolExpressions.QuineMcCluskeyMethod.FinalImplicantMethod
{    
    internal static class ImplicantSetExtension
    {
        private static void ProcessCurrentLevelImplicantSet<T>(
            HashSet<Implicant<T>> currentWightImplicantSet,
            HashSet<Implicant<T>> nextWeightImplicantSet,
            out HashSet<Implicant<T>> currentLevelProcessedImplicantSet,
            out HashSet<Implicant<T>> nextLevelImplicantSet)
        {
            currentLevelProcessedImplicantSet = new HashSet<Implicant<T>>();
            nextLevelImplicantSet = new HashSet<Implicant<T>>();

            foreach (var currentWightImplicant in currentWightImplicantSet)
            {
                foreach (var nextWeightImplicant in nextWeightImplicantSet)
                {
                    var implicantsDistance = currentWightImplicant.GetCombinedValueDistance(nextWeightImplicant);
                    if (implicantsDistance != 0) continue;
                    var nextLevelImplicantCandidate = currentWightImplicant.CombineImplicants(nextWeightImplicant);
                    var nextLevelImplicantCandidateDistance = currentWightImplicant.GetCombinedValueDistance(nextLevelImplicantCandidate);
                    if (nextLevelImplicantCandidateDistance != 1) continue;

                    nextLevelImplicantSet.Add(nextLevelImplicantCandidate);
                    currentLevelProcessedImplicantSet.Add(currentWightImplicant);
                    currentLevelProcessedImplicantSet.Add(nextWeightImplicant);
                }
            }
        }

        internal static HashSet<Implicant<T>> GetFinalImplicantSet<T>(
            this HashSet<Implicant<T>> implicantSet)
        {
            var finalImplicantSet = new HashSet<Implicant<T>>();
            var currentLevelImplicantSet = implicantSet;

            while(currentLevelImplicantSet.Count() > 0) {
                var implicantWeightImplicantMap = currentLevelImplicantSet
                    .GroupBy(implicant => implicant.GetPositiveWeight())
                    .ToDictionary(
                        keySelector: group => group.Key,
                        elementSelector: group => group.ToHashSet());

                var processedImplicantSet = new HashSet<Implicant<T>>();                
                var nextLevelImplicantSet = new HashSet<Implicant<T>>();

                var weights = implicantWeightImplicantMap
                    .Keys
                    .OrderBy(weight => weight)
                    .ToList();

                foreach(var (currentWeight, nextWeight) in weights.Zip(weights.Skip(1), Tuple.Create))
                {
                    var currentLevelProcessedImplicantSet = new HashSet<Implicant<T>>();
                    HashSet<Implicant<T>> currentWeightAndNextLevelImplicantSet;

                    ProcessCurrentLevelImplicantSet(
                        currentWightImplicantSet: implicantWeightImplicantMap[currentWeight],
                        nextWeightImplicantSet: implicantWeightImplicantMap[nextWeight],
                        currentLevelProcessedImplicantSet: out currentLevelProcessedImplicantSet,
                        nextLevelImplicantSet: out currentWeightAndNextLevelImplicantSet);

                    processedImplicantSet.UnionWith(currentLevelProcessedImplicantSet);
                    nextLevelImplicantSet.UnionWith(currentWeightAndNextLevelImplicantSet);
                }

                finalImplicantSet.UnionWith(currentLevelImplicantSet.Except(processedImplicantSet));
                currentLevelImplicantSet = nextLevelImplicantSet;
            }

            return finalImplicantSet;
        }
    }
}
