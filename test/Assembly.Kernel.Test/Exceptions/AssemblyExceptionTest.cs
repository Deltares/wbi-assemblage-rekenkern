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
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Assembly.Kernel.Exceptions;
using NUnit.Framework;

namespace Assembly.Kernel.Test.Exceptions
{
    [TestFixture]
    public class AssemblyExceptionTest
    {
        [Test]
        public void ConstructorWithSingleError_Always_ExpectedValues()
        {
            // Setup
            const string id = "TestId";
            const EAssemblyErrors error = EAssemblyErrors.FailureMechanismSectionLengthInvalid;

            // Call
            var exception = new AssemblyException(id, error);

            // Assert
            Assert.AreEqual(1, exception.Errors.Count());

            AssemblyErrorMessage errorMessage = exception.Errors.First();
            Assert.AreEqual(id, errorMessage.EntityId);
            Assert.AreEqual(error, errorMessage.ErrorCode);
        }

        [Test]
        public void ConstructorWithMultipleErrors_WithErrors_ExpectedValues()
        {
            // Setup
            var expectedMessages = new List<AssemblyErrorMessage>
            {
                new AssemblyErrorMessage("TestId1", EAssemblyErrors.FailureMechanismSectionLengthInvalid),
                new AssemblyErrorMessage("TestId2", EAssemblyErrors.LengthEffectFactorOutOfRange)
            };

            // Call
            var exception = new AssemblyException(expectedMessages);

            // Assert
            Assert.AreEqual(expectedMessages.Count, exception.Errors.Count());
            Assert.AreEqual(expectedMessages, exception.Errors);

            for (var i = 0; i < expectedMessages.Count; i++)
            {
                AssemblyErrorMessage expectedErrorMessage = expectedMessages.ElementAt(i);
                AssemblyErrorMessage actualErrorMessage = exception.Errors.ElementAt(i);

                Assert.AreEqual(expectedErrorMessage.EntityId, actualErrorMessage.EntityId);
                Assert.AreEqual(expectedErrorMessage.ErrorCode, actualErrorMessage.ErrorCode);
            }
        }

        [Test]
        public void ConstructorWithMultipleErrors_ErrorMessagesNull_ThrowsArgumentNullException()
        {
            // Call
            void Call() => new AssemblyException(null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Call);
            Assert.AreEqual("errorMessages", exception.ParamName);
        }

        [Test]
        public void Constructor_SerializationRoundTrip_ExceptionProperlyInitialized()
        {
            // Setup
            var originalException = new AssemblyException("test", EAssemblyErrors.UndefinedProbability);

            // Call
            AssemblyException persistedException = SerializeAndDeserializeException(originalException);

            // Assert
            Assert.IsNull(persistedException.Errors);
        }

        [Test]
        public void Message_ExceptionWithSingleError_ReturnsExpectedMessage()
        {
            // Setup
            var exception = new AssemblyException("Test", EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult);

            // Call
            string message = exception.Message;

            // Assert
            string expectedMessage = "One or more errors occured during the assembly process:"
                                     + Environment.NewLine
                                     + EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult;
            Assert.AreEqual(expectedMessage, message);
        }

        [Test]
        public void Message_ExceptionWithMultipleErros_ReturnsExpectedMessage()
        {
            // Setup
            var exception = new AssemblyException(new[]
            {
                new AssemblyErrorMessage("Test1", EAssemblyErrors.InvalidCategoryValue),
                new AssemblyErrorMessage("Test2", EAssemblyErrors.LengthEffectFactorOutOfRange)
            });

            // Call
            string message = exception.Message;

            // Assert
            string expectedMessage = "One or more errors occured during the assembly process:"
                                     + Environment.NewLine
                                     + EAssemblyErrors.InvalidCategoryValue
                                     + Environment.NewLine
                                     + EAssemblyErrors.LengthEffectFactorOutOfRange;
            Assert.AreEqual(expectedMessage, message);
        }

        private static AssemblyException SerializeAndDeserializeException(AssemblyException original)
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, original);
                stream.Seek(0, SeekOrigin.Begin);
                return (AssemblyException) formatter.Deserialize(stream);
            }
        }
    }
}