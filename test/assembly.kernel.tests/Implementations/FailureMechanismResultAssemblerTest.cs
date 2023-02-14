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

using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FailureMechanismSections;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Implementations
{
    [TestFixture]
    public class FailureMechanismResultAssemblerTest
    {
        [Test]
        public void Constructor_ExpectedValues()
        {
            // Call
            var assembler = new FailureMechanismResultAssembler();

            // Assert
            Assert.IsInstanceOf<IFailureMechanismResultAssembler>(assembler);
        }

        #region CalculateFailureMechanismFailureProbabilityBoi1A1

        [Test]
        public void CalculateFailureMechanismFailureProbabilityBoi1A1_FailureMechanismSectionAssemblyResultsNull_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(1.0, null, false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismSectionAssemblyResults", EAssemblyErrors.ValueMayNotBeNull)
            });
        }

        [Test]
        public void CalculateFailureMechanismFailureProbabilityBoi1A1_FailureMechanismSectionAssemblyResultsEmpty_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(1.0, Enumerable.Empty<Probability>(), false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismSectionAssemblyResults", EAssemblyErrors.EmptyResultsList)
            });
        }

        [Test]
        public void CalculateFailureMechanismFailureProbabilityBoi1A1_LengthEffectFactorInvalid_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(0.0, new[]
            {
                new Probability(0.0)
            }, false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("lengthEffectFactor", EAssemblyErrors.LengthEffectFactorOutOfRange)
            });
        }

        [Test]
        public void CalculateFailureMechanismFailureProbabilityBoi1A1_PartialAssemblyFalseAndAssemblyResultsWithUndefinedProbabilities_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(1.0, new[]
            {
                new Probability(0.0),
                Probability.Undefined,
                new Probability(0.0)
            }, false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismSectionAssemblyResult", EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult)
            });
        }

        #endregion

        #region CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2

        [Test]
        public void CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2_FailureMechanismSectionAssemblyResultsNull_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(1.0, null, false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismSectionAssemblyResults", EAssemblyErrors.ValueMayNotBeNull)
            });
        }

        [Test]
        public void CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2_FailureMechanismSectionAssemblyResultsEmpty_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(
                1.0, Enumerable.Empty<ResultWithProfileAndSectionProbabilities>(), false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismSectionAssemblyResults", EAssemblyErrors.EmptyResultsList)
            });
        }

        [Test]
        public void CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2_LengthEffectFactorInvalid_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(0.0, new[]
            {
                new ResultWithProfileAndSectionProbabilities(new Probability(0.0), new Probability(0.0))
            }, false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("lengthEffectFactor", EAssemblyErrors.LengthEffectFactorOutOfRange)
            });
        }

        [Test]
        public void CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2_PartialAssemblyFalseAndAssemblyResultsWithUndefinedProbabilities_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(1.0, new[]
            {
                new ResultWithProfileAndSectionProbabilities(new Probability(0.0), new Probability(0.0)),
                new ResultWithProfileAndSectionProbabilities(Probability.Undefined, Probability.Undefined),
                new ResultWithProfileAndSectionProbabilities(new Probability(0.0), new Probability(0.0))
            }, false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismSectionAssemblyResult", EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult)
            });
        }

        #endregion
    }
}