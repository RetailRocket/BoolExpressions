namespace BoolExpressions.DisjunctiveNormalForm.DnfAndBlockElement
{
    public class DfnNot<T>
        : IDnfBlockElement<T>
    {
        public DfnNot(
            DnfVariable<T> variable)
        {
            this.Variable = variable;
        }

        public DnfVariable<T> Variable { get; set; }
    }
}