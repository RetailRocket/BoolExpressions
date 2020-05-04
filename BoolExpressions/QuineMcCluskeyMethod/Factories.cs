using System;
using System.Collections.Generic;
using System.Linq;
using BoolExpressions.QuineMcCluskeyMethod.Term;

namespace BoolExpressions.QuineMcCluskeyMethod
{
    public class Factories {
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
    }
}
