// Copyright (C) Rijkswaterstaat 2022. All rights reserved.
//
// This file is part of the Assembly kernel.
//
// Assembly kernel is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
//
// All names, logos, and references to "Rijkswaterstaat" are registered trademarks of
// Rijkswaterstaat and remain full property of Rijkswaterstaat at all times.
// All rights reserved.

using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;
using NUnit.Framework;

namespace Assembly.Kernel.Tests
{
    /// <summary>
    /// Class containing helper functions which can be used for unit tests.
    /// </summary>
    public static class TestHelper
    {
        public static void AssertExpectedErrorMessage(TestDelegate testDelegate, params EAssemblyErrors[] expectedErrors)
        {
            AssemblyErrorMessage[] errors = Assert.Throws<AssemblyException>(testDelegate).Errors.ToArray();

            Assert.AreEqual(expectedErrors.Length, errors.Length);

            for (int i = 0; i < expectedErrors.Length; i++)
            {
                var message = errors.ElementAt(i);
                Assert.NotNull(message);
                Assert.AreEqual(expectedErrors.ElementAt(i), message.ErrorCode);
            }
        }

        /// <summary>
        /// Asserts that the <see cref="AssemblyException"/> is thrown and that it contains the <paramref name="expectedErrorMessages"/>.
        /// </summary>
        /// <param name="call">The call to execute.</param>
        /// <param name="expectedErrorMessages">The expected error messages.</param>
        /// <exception cref="AssertionException">Thrown when:
        /// <list type="bullet">
        /// <item>no <see cref="AssemblyException"/> is thrown;</item>
        /// <item>The number of messages are not equal;</item>
        /// <item>The <see cref="AssemblyErrorMessage.EntityId"/> of the elements are not equal;</item>
        /// <item>The <see cref="AssemblyErrorMessage.ErrorCode"/> of the elements are not equal.</item>
        /// </list></exception>
        public static void AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(
            TestDelegate call,
            IEnumerable<AssemblyErrorMessage> expectedErrorMessages)
        {
            var exception = Assert.Throws<AssemblyException>(call);
            IEnumerable<AssemblyErrorMessage> actualErrorMessages = exception.Errors; 
            
            Assert.AreEqual(expectedErrorMessages.Count(), actualErrorMessages.Count());

            for (var i = 0; i < expectedErrorMessages.Count(); i++)
            {
                AssemblyErrorMessage expectedErrorMessage = expectedErrorMessages.ElementAt(i);
                AssemblyErrorMessage actualErrorMessage = actualErrorMessages.ElementAt(i);
                
                Assert.AreEqual(expectedErrorMessage.EntityId, actualErrorMessage.EntityId);
                Assert.AreEqual(expectedErrorMessage.ErrorCode, actualErrorMessage.ErrorCode);
            }
        }

    }
}
