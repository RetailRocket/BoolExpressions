namespace BoolExpressions.DisjunctiveNormalForm.Operation
{
    public class DnfNot<T>
        : IDnfOperation<T>
    {
        public DnfNot(
            DnfVariable<T> variable)
        {
            this.Variable = variable;
        }

        public DnfVariable<T> Variable { get; set; }
    }
}