using System.Collections;
using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Test.Exceptions
{
    /// <summary>
    /// Comparer for <see cref="AssemblyErrorMessage"/>.
    /// </summary>
    internal class AssemblyErrorMessageComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return x is AssemblyErrorMessage assemblyErrorMessageX
                   && y is AssemblyErrorMessage assemblyErrorMessageY
                   && assemblyErrorMessageX.ErrorCode == assemblyErrorMessageY.ErrorCode
                   && assemblyErrorMessageX.EntityId == assemblyErrorMessageY.EntityId
                       ? 0
                       : 1;
        }
    }
}