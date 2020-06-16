namespace BoolExpressions.DisjunctiveNormalForm.Operation
{
    public class Factory {
        public static DnfNotVariable<T> DnfNotVariableOf<T>(
            T value)
        {
            return new DnfNotVariable<T>(value);
        }

        public static DnfVariable<T> DnfVariableOf<T>(
            T value)
        {
            return new DnfVariable<T>(value);
        }
    }
}
