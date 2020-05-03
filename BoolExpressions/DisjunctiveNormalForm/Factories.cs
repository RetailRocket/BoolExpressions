using System.Collections.Generic;
using BoolExpressions.DisjunctiveNormalForm.Operation;

namespace BoolExpressions.DisjunctiveNormalForm
{
    public class Factories {
        public static DnfAnd<T> DnfAndOf<T>(
            params IDnfOperation<T>[] operationList) where T : class
        {
            return new DnfAnd<T>(new HashSet<IDnfOperation<T>>(operationList));
        }
    }
}
