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

using System;
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Exceptions
{
    [TestFixture]
    public class AssemblyExceptionTest
    {
        [Test]
        public void ConstructorSingleExceptionPassesArguments()
        {
            var id = "TestId";
            var error = EAssemblyErrors.FailureMechanismSectionLengthInvalid;

            var exception = new AssemblyException(id,error);

            Assert.AreEqual(1, exception.Errors.Count());

            var firstError = exception.Errors.First();
            Assert.AreEqual(id, firstError.EntityId);
            Assert.AreEqual(error, firstError.ErrorCode);
        }

        [Test]
        public void ConstructorMultipleExceptionPassesArguments()
        {
            var expectedMessages = new List<AssemblyErrorMessage>
            {
                new AssemblyErrorMessage("TestId1", EAssemblyErrors.FailureMechanismSectionLengthInvalid),
                new AssemblyErrorMessage("TestId2", EAssemblyErrors.LengthEffectFactorOutOfRange)
            };

            var exception = new AssemblyException(expectedMessages);

            Assert.AreEqual(expectedMessages.Count, exception.Errors.Count());
            Assert.AreEqual(expectedMessages, exception.Errors);

            for (int i = 0; i < expectedMessages.Count; i++)
            {
                var message = exception.Errors.ElementAt(i);
                var expectedErrorMessage = expectedMessages.ElementAt(i);
                Assert.AreEqual(expectedErrorMessage.EntityId, message.EntityId);
                Assert.AreEqual(expectedErrorMessage.ErrorCode, message.ErrorCode);
            }
        }

        [Test]
        public void ConstructorConsumesNullMessage()
        {
            var exception = new AssemblyException(null);

            Assert.AreEqual(1, exception.Errors.Count());
            Assert.AreEqual(EAssemblyErrors.ErrorConstructingErrorMessage, exception.Errors.First().ErrorCode);
        }

        [Test]
        public void ToStringWorks()
        {
            var exception = new AssemblyException("Test",EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult);

            Assert.AreEqual("One or more errors occured during the assembly process:" + Environment.NewLine + EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult + Environment.NewLine ,
                exception.Message);
        }
    }
}
