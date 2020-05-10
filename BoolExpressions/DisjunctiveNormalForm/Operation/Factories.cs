namespace BoolExpressions.DisjunctiveNormalForm.Operation
{
    public class Factories {
        public static DnfNotVariable<T> DnfNotVariableOf<T>(
            T value) where T : class
        {
            return new DnfNotVariable<T>(value);
        }

        public static DnfVariable<T> DnfVariableOf<T>(
            T value) where T : class
        {
            return new DnfVariable<T>(value);
        }
    }
}
