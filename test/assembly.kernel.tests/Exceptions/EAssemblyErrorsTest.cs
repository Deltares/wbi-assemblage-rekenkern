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
using Assembly.Kernel.Exceptions;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Exceptions
{
    [TestFixture]
    public class EAssemblyErrorsTest
    {
        [Test]
        public void EAssemblyErrors_Always_ExpectedValues()
        {
            // Call
            Array assemblyErrors = Enum.GetValues(typeof(EAssemblyErrors));
            
            // Assert
            Assert.AreEqual(27, assemblyErrors.Length);
            Assert.AreEqual(0, (int) EAssemblyErrors.LengthEffectFactorOutOfRange);
            Assert.AreEqual(1, (int) EAssemblyErrors.SectionLengthOutOfRange);
            Assert.AreEqual(2, (int) EAssemblyErrors.SignalFloodingProbabilityAboveMaximumAllowableFloodingProbability);
            Assert.AreEqual(3, (int) EAssemblyErrors.LowerLimitIsAboveUpperLimit);
            Assert.AreEqual(4, (int) EAssemblyErrors.ValueMayNotBeNull);
            Assert.AreEqual(5, (int) EAssemblyErrors.FailureMechanismSectionLengthInvalid);
            Assert.AreEqual(6, (int) EAssemblyErrors.FailureMechanismSectionSectionStartEndInvalid);
            Assert.AreEqual(7, (int) EAssemblyErrors.FailureProbabilityOutOfRange);
            Assert.AreEqual(8, (int) EAssemblyErrors.InputNotTheSameType);
            Assert.AreEqual(9, (int) EAssemblyErrors.EmptyResultsList);
            Assert.AreEqual(10, (int) EAssemblyErrors.CommonFailureMechanismSectionsInvalid);
            Assert.AreEqual(11, (int) EAssemblyErrors.CommonFailureMechanismSectionsDoNotHaveEqualSections);
            Assert.AreEqual(12, (int) EAssemblyErrors.CommonFailureMechanismSectionsNotConsecutive);
            Assert.AreEqual(13, (int) EAssemblyErrors.RequestedPointOutOfRange);
            Assert.AreEqual(14, (int) EAssemblyErrors.InvalidCategoryLimits);
            Assert.AreEqual(15, (int) EAssemblyErrors.SectionsWithoutCategory);
            Assert.AreEqual(16, (int) EAssemblyErrors.ProfileProbabilityGreaterThanSectionProbability);
            Assert.AreEqual(17, (int) EAssemblyErrors.UndefinedProbability);
            Assert.AreEqual(18, (int) EAssemblyErrors.ErrorConstructingErrorMessage);
            Assert.AreEqual(19, (int) EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult);
            Assert.AreEqual(20, (int) EAssemblyErrors.NonMatchingProbabilityValues);
            Assert.AreEqual(21, (int) EAssemblyErrors.InvalidCategoryValue);
            Assert.AreEqual(22, (int) EAssemblyErrors.ProbabilitiesNotBothDefinedOrUndefined);
            Assert.AreEqual(23, (int) EAssemblyErrors.InvalidEnumValue);
            Assert.AreEqual(24, (int) EAssemblyErrors.UnequalCommonFailureMechanismSectionLists);
            Assert.AreEqual(25, (int) EAssemblyErrors.CommonSectionsWithoutCategoryValues);
            Assert.AreEqual(26, (int) EAssemblyErrors.InvalidArgumentType);
        }
    }
}