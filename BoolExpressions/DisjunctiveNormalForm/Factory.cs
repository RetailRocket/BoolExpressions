using System.Collections.Generic;
using BoolExpressions.DisjunctiveNormalForm.Operation;

namespace BoolExpressions.DisjunctiveNormalForm
{
    public class Factory {
        public static DnfAnd<T> DnfAndOf<T>(
            params IDnfVariable<T>[] operationList)
        {
            return new DnfAnd<T>(new HashSet<IDnfVariable<T>>(operationList));
        }
    }
}
