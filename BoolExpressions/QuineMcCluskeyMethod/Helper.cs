using System;
using System.Collections.Generic;
using System.Linq;
using BoolExpressions.DisjunctiveNormalForm;
using BoolExpressions.DisjunctiveNormalForm.Operation;
using BoolExpressions.QuineMcCluskeyMethod.Term;
using static BoolExpressions.QuineMcCluskeyMethod.Factory;
using System.Runtime.CompilerServices;
using BoolExpressions.QuineMcCluskeyMethod.FinalImplicantMethod;

[assembly:InternalsVisibleTo("UnitTests")]

namespace BoolExpressions.QuineMcCluskeyMethod
{
    public static class Helper
    {
        public static DnfExpression<T> ProcessDnf<T>(
            in DnfExpression<T> dnfExpression) where T : class
        {
            var mintermSet = dnfExpression.AndBlockSet.ToHashSet();

            var implicantSet = mintermSet
                .Select(andBlock => ImplicantOf(andBlock))
                .ToHashSet();

            var finalImplicantSet = implicantSet.GetFinalImplicantSet();

            HashSet<DnfAnd<T>> finalMintermSet;
            HashSet<Implicant<T>> primaryImplicantSet;

            PrimaryImplicantMethod.Helper.GetPrimaryImplicantSet(
                mintermSet: mintermSet,
                finalImplicantSet: finalImplicantSet,
                finalMintermSet: out finalMintermSet,
                primaryImplicantSet: out primaryImplicantSet);

            var minimalImplicantSet = PetrickMethod.Helper.GetMinimalImplicantSet(
                mintermSet: finalMintermSet,
                implicantSet: implicantSet.Except(primaryImplicantSet).ToHashSet());

            var primaryAndMinimalMintermSet = primaryImplicantSet
                .Union(minimalImplicantSet)
                .Select(mintermOf)
                .ToHashSet();

            return new DnfExpression<T>(primaryAndMinimalMintermSet);
        }
    }
}
