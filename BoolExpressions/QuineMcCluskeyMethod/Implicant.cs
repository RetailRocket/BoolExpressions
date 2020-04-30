using System;
using System.Collections.Generic;
using System.Linq;
using BoolExpressions.QuineMcCluskeyMethod.Term;

namespace BoolExpressions.QuineMcCluskeyMethod
{
    public class Implicant<T> where T : class
    {
        public Implicant(
            HashSet<Term<T>> minterm)
        {
            Minterm = minterm;
        }

        public HashSet<Term<T>> Minterm { get; }

        public override bool Equals(
            object obj)
        {
            return Equals(obj as Implicant<T>);
        }

        public bool Equals(
            Implicant<T> that)
        {
            return that != null && HashSet<Term<T>>.CreateSetComparer().Equals(this.Minterm, that.Minterm);
        }

        public override int GetHashCode()
        {
            return HashSet<Term<T>>.CreateSetComparer().GetHashCode(Minterm);
        }
    }
}
