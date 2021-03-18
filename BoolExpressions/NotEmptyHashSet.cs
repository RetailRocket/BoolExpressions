namespace BoolExpressions
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class NotEmptyHashSet<T> : IEnumerable<T>
    {
        private readonly HashSet<T> root;

        public NotEmptyHashSet(
            T head,
            IEnumerable<T> tail,
            IEqualityComparer<T> comparer)
        {
            this.root = new HashSet<T>(
                tail.Append(head),
                comparer);
        }

        public NotEmptyHashSet(
            T head)
            : this(
                head: head,
                tail: Enumerable.Empty<T>(),
                comparer: EqualityComparer<T>.Default)
        {
        }

        public NotEmptyHashSet(
            T head,
            IEnumerable<T> tail)
            : this(
                head: head,
                tail: tail,
                comparer: EqualityComparer<T>.Default)
        {
        }

        public void Add(
            T item) => this.root.Add(item);

        public bool SetEquals(
            IEnumerable<T> other) => this.root.SetEquals(other);

        public bool Contains(
            T item) => this.root.Contains(item);

        public int Count => this.root.Count;

        public IEnumerator<T> GetEnumerator() => this.root.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override int GetHashCode()
        {
            return this.root
                .GetHashCode();
        }
    }
}