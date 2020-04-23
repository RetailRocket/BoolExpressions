namespace BoolExpressions.DisjunctiveNormalForm.DnfAndBlockElement
{
    public class DnfVariable<T>
        : IDnfBlockElement<T>
    {
        public DnfVariable(
            T value)
        {
            this.Value = value;
        }

        public T Value { get; set; }
    }
}