using System.Linq;
using Assembly.Kernel.Exceptions;
using NUnit.Framework;

namespace Assembly.Kernel.Tests
{
    public class TestHelper
    {
        public static void AssertExpectedErrorMessage(TestDelegate testDelegate, EAssemblyErrors expectedError)
        {
            AssertExpectedErrorMessage(testDelegate, new[] { expectedError });
        }

        public static void AssertExpectedErrorMessage(TestDelegate testDelegate, EAssemblyErrors[] expectedErrors)
        {
            var errors = Assert.Throws<AssemblyException>(testDelegate).Errors.ToArray();

            Assert.AreEqual(expectedErrors.Length, errors.Length);

            for (int i = 0; i < expectedErrors.Length; i++)
            {
                var message = errors.ElementAt(i);
                Assert.NotNull(message);
                Assert.AreEqual(expectedErrors.ElementAt(i), message.ErrorCode);
            }
        }

    }
}
