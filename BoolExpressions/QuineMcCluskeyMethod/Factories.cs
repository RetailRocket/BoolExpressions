using System;
using System.Collections.Generic;
using System.Linq;
using BoolExpressions.DisjunctiveNormalForm;
using BoolExpressions.DisjunctiveNormalForm.Operation;
using BoolExpressions.QuineMcCluskeyMethod.Term;
using static BoolExpressions.QuineMcCluskeyMethod.Term.Factories;

namespace BoolExpressions.QuineMcCluskeyMethod
{
    internal class Factories {
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
                            DnfVariable<T> variable => PositiveTermOf(variable.Value),
                            DnfNot<T> notOperation => NegativeTermOf(notOperation.Variable.Value),
                            _ => throw new ArgumentException(
                                message: "pattern matching in C# is sucks",
                                paramName: nameof(operation))
                        };
                        return term;
                    }));
        }
    }
}
