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

namespace Assembly.Kernel.Tests.Implementations.Validators
{
    [TestFixture]
    public class AssessmentSectionValidatorTests {

        private sealed class AssesmentSectionTestData {
            
            public static IEnumerable TestCases {
                // ReSharper disable once UnusedMember.Local
                get {
                    yield return new TestCaseData(10000, 0, 0).Returns(null);
                    yield return new TestCaseData(1000, 1, 1).Returns(null);
                    yield return new TestCaseData(0, 0, -0.5).Returns(
                        new List<EAssemblyErrors>() {
                            EAssemblyErrors.SectionLengthOutOfRange,
                            EAssemblyErrors.LowerLimitOutOfRange,
                            EAssemblyErrors.SignallingLimitAboveLowerLimit
                        });
                    yield return new TestCaseData(10000, -0.9, 1).Returns(
                        new List<EAssemblyErrors>() {
                            EAssemblyErrors.SignallingLimitOutOfRange
                        });
                    yield return new TestCaseData(10000, -2, -0.5).Returns(
                        new List<EAssemblyErrors>() {
                            EAssemblyErrors.LowerLimitOutOfRange,
                            EAssemblyErrors.SignallingLimitOutOfRange
                        });
                    yield return new TestCaseData(10000, 1.5, 2).Returns(
                        new List<EAssemblyErrors>() {
                            EAssemblyErrors.LowerLimitOutOfRange,
                            EAssemblyErrors.SignallingLimitOutOfRange
                        });
                    yield return new TestCaseData(10000, 0.2, 0.1).Returns(
                        new List<EAssemblyErrors>() {
                            EAssemblyErrors.SignallingLimitAboveLowerLimit
                        });
                }
            }
        }

        [Test, TestCaseSource(typeof(AssesmentSectionTestData), nameof(AssesmentSectionTestData.TestCases))]
        public List<EAssemblyErrors> AssessmentSectionValidatorTest(double length, 
            double signallingLimit, double lowerLimit) {

            try {
                AssessmentSectionValidator.CheckAssessmentSectionInput(length, 
                    signallingLimit, lowerLimit);
            } catch (AssemblyException e) {
                Assert.NotNull(e.Errors);
                return e.Errors.Select(message => message.ErrorCode).ToList();
            }

            return null;
        }
    }
}