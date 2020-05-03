namespace BoolExpressions.DisjunctiveNormalForm
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BoolExpressions.DisjunctiveNormalForm.Operation;

    public class DnfAnd<T> where T : class
    {
        public DnfAnd(
            HashSet<IDnfOperation<T>> elementSet)
        {
            this.ElementSet = elementSet;
        }

        public HashSet<IDnfOperation<T>> ElementSet { get; }

        override public bool Equals(
            object that)
        {
            return Equals(that as DnfAnd<T>);
        }

        public bool Equals(
            DnfAnd<T> that)
        {
            return that != null && HashSet<IDnfOperation<T>>.CreateSetComparer().Equals(this.ElementSet, that.ElementSet);
        }

        public override int GetHashCode()
        {
            return HashSet<IDnfOperation<T>>.CreateSetComparer().GetHashCode(ElementSet);
        }

        override public string ToString()
        {
            var elementString = String.Join(", ", ElementSet.Select(term => term.ToString()));
            return $"And({elementString})";
        }
    }
}