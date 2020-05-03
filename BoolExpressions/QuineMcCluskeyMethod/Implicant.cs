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
            TermSet = minterm;
        }

        public HashSet<Term<T>> TermSet { get; }

        public override bool Equals(
            object obj)
        {
            return Equals(obj as Implicant<T>);
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

        override public string ToString()
        {
            var mintermString = String.Join(", ", TermSet.Select(term => term.ToString()));
            return $"Implicant({mintermString})";
        }
    }
}
