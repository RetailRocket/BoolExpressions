namespace BoolExpressions.DisjunctiveNormalForm.Operation
{
    public class Factories {
        public static DnfNot<T> DnfNotOf<T>(
            DnfVariable<T> variable) where T : class
        {
            return new DnfNot<T>(variable);
        }

        public static DnfVariable<T> DnfVariableOf<T>(
            T value) where T : class
        {
            return new DnfVariable<T>(value);
        }
    }
}
