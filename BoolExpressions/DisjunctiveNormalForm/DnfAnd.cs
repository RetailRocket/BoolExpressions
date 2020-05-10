namespace BoolExpressions.DisjunctiveNormalForm
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BoolExpressions.DisjunctiveNormalForm.Operation;

    public class DnfAnd<T> where T : class
    {
        public DnfAnd(
            HashSet<IDnfVariable<T>> elementSet)
        {
            this.ElementSet = elementSet;
        }

        public HashSet<IDnfVariable<T>> ElementSet { get; }

        override public bool Equals(
            object that)
        {
            return Equals(that as DnfAnd<T>);
        }

        public bool Equals(
            DnfAnd<T> that)
        {
            return that != null && HashSet<IDnfVariable<T>>.CreateSetComparer().Equals(this.ElementSet, that.ElementSet);
        }

        public override int GetHashCode()
        {
            return HashSet<IDnfVariable<T>>.CreateSetComparer().GetHashCode(ElementSet);
        }
    }
}