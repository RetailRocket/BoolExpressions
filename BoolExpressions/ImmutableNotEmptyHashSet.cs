namespace BoolExpressions
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class ImmutableNotEmptyHashSet<T> : IEnumerable<T>
    {
        private readonly NotEmptyHashSet<T> root;

        private readonly IEqualityComparer<T> comparer;

        public ImmutableNotEmptyHashSet(
            T head,
            IEnumerable<T> tail,
            IEqualityComparer<T> comparer)
        {
            this.comparer = comparer;
            this.root = new NotEmptyHashSet<T>(
                head: head,
                tail: tail,
                comparer: comparer);
        }

        public ImmutableNotEmptyHashSet(
            T head)
            : this(
                head: head,
                tail: Enumerable.Empty<T>(),
                comparer: EqualityComparer<T>.Default)
        {
        }

        public ImmutableNotEmptyHashSet(
            T head,
            IEqualityComparer<T> comparer)
            : this(
                head: head,
                tail: Enumerable.Empty<T>(),
                comparer: comparer)
        {
        }

        public ImmutableNotEmptyHashSet(
            ImmutableNotEmptyHashSet<T> first,
            IEnumerable<T> right,
            IEqualityComparer<T> comparer)
            : this(
                head: first.First(),
                tail: first.Skip(1).Concat(right),
                comparer: comparer)
        {
        }

        public ImmutableNotEmptyHashSet(
            ImmutableNotEmptyHashSet<T> first,
            IEnumerable<T> second)
            : this(
                head: first.First(),
                tail: first.Skip(1).Concat(second),
                comparer: EqualityComparer<T>.Default)
        {
        }


        public ImmutableNotEmptyHashSet(
            T head,
            IEnumerable<T> tail)
            : this(
                head: head,
                tail: tail,
                comparer: EqualityComparer<T>.Default)
        {
        }

        public bool SetEquals(
            IEnumerable<T> other) => this.root.SetEquals(other);

        public bool Contains(
            T item) => this.root.Contains(item);

        public IEnumerator<T> GetEnumerator() => this.root.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override int GetHashCode()
        {
            return this.root
                .GetHashCode();
        }
    }
}