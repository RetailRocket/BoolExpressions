using System;
using System.Collections.Generic;
using System.Linq;
using BoolExpressions.QuineMcCluskeyMethod.Term;

namespace BoolExpressions.QuineMcCluskeyMethod
{
    internal class Implicant<T>
    {
        public Implicant(
            HashSet<Term<T>> termSet)
        {
            TermSet = termSet;
        }

        public HashSet<Term<T>> TermSet { get; }

        public override bool Equals(
            object obj)
        {
            return obj is Implicant<T> that && this.Equals(that);
        }

        public bool Equals(
            Implicant<T> that)
        {
            return that != null && HashSet<Term<T>>.CreateSetComparer().Equals(this.TermSet, that.TermSet);
        }

        public override int GetHashCode()
        {
            return HashSet<Term<T>>.CreateSetComparer().GetHashCode(TermSet);
        }
    }
}
