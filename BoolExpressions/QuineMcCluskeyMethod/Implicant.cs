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
            Implicant<T> obj)
        {
            return obj != null && obj.Minterm.SetEquals(this.Minterm);
        }

        public override int GetHashCode()
        {
            return Minterm.GetHashCode();
        }
    }
}
