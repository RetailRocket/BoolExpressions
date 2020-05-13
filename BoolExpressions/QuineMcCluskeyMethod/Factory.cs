using System;
using System.Collections.Generic;
using System.Linq;
using BoolExpressions.DisjunctiveNormalForm;
using BoolExpressions.DisjunctiveNormalForm.Operation;
using BoolExpressions.QuineMcCluskeyMethod.Term;
using static BoolExpressions.QuineMcCluskeyMethod.Term.Factory;
using static BoolExpressions.DisjunctiveNormalForm.Operation.Factory;

namespace BoolExpressions.QuineMcCluskeyMethod
{
    internal class Factory {
        public static Implicant<T> ImplicantOf<T>(
            params Term<T>[] termSet) where T : class
        {
            return new Implicant<T>(new HashSet<Term<T>>(termSet));
        }

        public static Implicant<T> ImplicantOf<T>(
            IEnumerable<Term<T>> termSet) where T : class
        {
            return new Implicant<T>(new HashSet<Term<T>>(termSet));
        }

        public static Implicant<T> ImplicantOf<T>(
            DnfAnd<T> minterm) where T : class
        {
            return ImplicantOf(
                minterm
                    .ElementSet
                    .Select(operation =>
                    {
                        Term<T> term = operation switch
                        {
                            DnfVariable<T> variable => (Term<T>)PositiveTermOf(variable.Value),
                            DnfNotVariable<T> notOperation => (Term<T>)NegativeTermOf(notOperation.Value),
                            _ => throw new ArgumentException(
                                message: "pattern matching in C# is sucks",
                                paramName: nameof(operation))
                        };
                        return term;
                    }));
        }

        public static DnfAnd<T> mintermOf<T>(
             Implicant<T> implicant) where T : class
        {
            var elementSet = implicant.TermSet
                .SelectMany(term =>
                {
                    return term switch
                    {
                        PositiveTerm<T> _ => new List<IDnfVariable<T>> { DnfVariableOf(term.Value) },
                        NegativeTerm<T> _ => new List<IDnfVariable<T>> { DnfNotVariableOf(term.Value) },
                        _ => new List<IDnfVariable<T>>()
                    };
                })
                .ToHashSet();
            return new DnfAnd<T>(elementSet);
        }
    }
}
