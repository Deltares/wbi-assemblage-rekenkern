#region Copyright (c) 2018 Technolution BV. All Rights Reserved. 
// // Copyright (C) Technolution BV. 2018. All rights reserved.
// //
// // This file is part of the Assembly kernel.
// //
// // Assembly kernel is free software: you can redistribute it and/or modify
// // it under the terms of the GNU Lesser General Public License as published by
// // the Free Software Foundation, either version 3 of the License, or
// // (at your option) any later version.
// // 
// // This program is distributed in the hope that it will be useful,
// // but WITHOUT ANY WARRANTY; without even the implied warranty of
// // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// // GNU Lesser General Public License for more details.
// //
// // You should have received a copy of the GNU Lesser General Public License
// // along with this program. If not, see <http://www.gnu.org/licenses/>.
// //
// // All names, logos, and references to "Technolution BV" are registered trademarks of
// // Technolution BV and remain full property of Technolution BV at all times.
// // All rights reserved.
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations.Validators;

namespace Assembly.Kernel.Tests.Implementations.Validators {
    [TestFixture]
    public class FailureMechanismValidatorTests {

        [Test, TestCaseSource(typeof(FailureMechanismValidatorTestData), nameof(FailureMechanismValidatorTestData.TestCases))]
        public List<EAssemblyErrors> FailureMechanismCheckTest(double lengthEffectFactor, 
            double failureProbabilityMarginFactor) {

            try {
                FailureMechanismValidator.CheckFailureMechanismInput(lengthEffectFactor,
                    failureProbabilityMarginFactor);
            } catch (AssemblyException e) {
                Assert.NotNull(e.Errors);
                return e.Errors.Select(message => message.ErrorCode).ToList();
            }

            return null;
        }
    }

    public class FailureMechanismValidatorTestData {
        
        public static IEnumerable TestCases {
            get {
                yield return new TestCaseData(1, 0).Returns(null);
                yield return new TestCaseData(10, 0.5).Returns(null);
                yield return new TestCaseData(0, 0.1).Returns(
                    new List<EAssemblyErrors>() {
                        EAssemblyErrors.LengthEffectFactorOutOfRange
                    });
                yield return new TestCaseData(100, -1).Returns(
                    new List<EAssemblyErrors>() {
                        EAssemblyErrors.FailurePropbabilityMarginOutOfRange
                    });
                yield return new TestCaseData(-2, 2).Returns(
                    new List<EAssemblyErrors>() {
                        EAssemblyErrors.FailurePropbabilityMarginOutOfRange,
                        EAssemblyErrors.LengthEffectFactorOutOfRange
                    });
            }
        }
    }
}