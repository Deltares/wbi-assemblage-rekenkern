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
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.AssessmentResultTypes;
using Assembly.Kernel.Model.FmSectionTypes;
using NUnit.Framework;

// ReSharper disable UnusedMember.Local

namespace Assembly.Kernel.Tests.Implementations
{
    public class AssessmentResultsTranslatorCategorySuppliedTests
    {
        private IAssessmentResultsTranslator translator;

        [SetUp]
        public void Init()
        {
            translator = new AssessmentResultsTranslator();
        }

        [Test, TestCaseSource(
             typeof(CategorySuppliedTestCases),
             nameof(CategorySuppliedTestCases.Wbi0Gt4ResultSupplied))]
        public EFmSectionCategory Wbi0G4ResultSuppliedTest(EFmSectionCategory category)
        {
            var result = translator.TranslateAssessmentResultWbi0G4(EAssessmentResultTypeG2.ResultSpecified, category);
            Assert.IsAssignableFrom<FmSectionAssemblyDirectResult>(result);
            Assert.IsNaN(result.FailureProbability);

            return result.Result;
        }

        [Test, TestCaseSource(
             typeof(CategorySuppliedTestCases),
             nameof(CategorySuppliedTestCases.Wbi0G4AssessmentResult))]
        public EFmSectionCategory Wbi0G4AssessmentResultTest(
            EAssessmentResultTypeG2 assessmentResult)
        {
            var result = translator.TranslateAssessmentResultWbi0G4(assessmentResult, null);
            Assert.IsAssignableFrom<FmSectionAssemblyDirectResult>(result);
            Assert.IsNaN(result.FailureProbability);

            return result.Result;
        }

        [Test, TestCaseSource(
             typeof(CategorySuppliedTestCases),
             nameof(CategorySuppliedTestCases.WbiG4Exceptions))]
        public EAssemblyErrors? Wbi0G4ExceptionTest(EAssessmentResultTypeG2 assessment,
            EFmSectionCategory? category)
        {
            try
            {
                translator.TranslateAssessmentResultWbi0G4(assessment, category);
            }
            catch (AssemblyException e)
            {
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                return message.ErrorCode;
            }

            Assert.Fail("Expected exception not thrown.");
            return null;
        }

        [Test, TestCaseSource(
             typeof(CategorySuppliedTestCases),
             nameof(CategorySuppliedTestCases.Wbi0Gt4ResultSupplied))]
        public EFmSectionCategory Wbi0T4ResultSuppliedTest(EFmSectionCategory category)
        {
            var result = translator.TranslateAssessmentResultWbi0T4(EAssessmentResultTypeT3.ResultSpecified, category);
            Assert.IsAssignableFrom<FmSectionAssemblyDirectResult>(result);
            Assert.IsNaN(result.FailureProbability);

            return result.Result;
        }

        [Test, TestCaseSource(
             typeof(CategorySuppliedTestCases),
             nameof(CategorySuppliedTestCases.Wbi0T4AssessmentResult))]
        public EFmSectionCategory Wbi0T4AssessmentResultTest(
            EAssessmentResultTypeT3 assessmentResult)
        {
            var result = translator.TranslateAssessmentResultWbi0T4(assessmentResult, null);
            Assert.IsAssignableFrom<FmSectionAssemblyDirectResult>(result);
            Assert.IsNaN(result.FailureProbability);

            return result.Result;
        }

        [Test, TestCaseSource(
             typeof(CategorySuppliedTestCases),
             nameof(CategorySuppliedTestCases.WbiT4Exceptions))]
        public EAssemblyErrors? Wbi0T4ExceptionTest(EAssessmentResultTypeT3 assessment,
            EFmSectionCategory? category)
        {
            try
            {
                translator.TranslateAssessmentResultWbi0T4(assessment, category);
            }
            catch (AssemblyException e)
            {
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                return message.ErrorCode;
            }

            Assert.Fail("Expected exception not thrown.");
            return null;
        }

        private sealed class CategorySuppliedTestCases
        {
            public static IEnumerable Wbi0Gt4ResultSupplied
            {
                get
                {
                    yield return new TestCaseData(EFmSectionCategory.Iv).Returns(EFmSectionCategory.Iv);
                    yield return new TestCaseData(EFmSectionCategory.IIv).Returns(EFmSectionCategory.IIv);
                    yield return new TestCaseData(EFmSectionCategory.IIIv).Returns(EFmSectionCategory.IIIv);
                    yield return new TestCaseData(EFmSectionCategory.IVv).Returns(EFmSectionCategory.IVv);
                    yield return new TestCaseData(EFmSectionCategory.Vv).Returns(EFmSectionCategory.Vv);
                    yield return new TestCaseData(EFmSectionCategory.VIv).Returns(EFmSectionCategory.VIv);
                }
            }

            public static IEnumerable Wbi0G4AssessmentResult
            {
                get
                {
                    yield return new TestCaseData(EAssessmentResultTypeG2.Ngo).Returns(EFmSectionCategory.VIIv);
                    yield return new TestCaseData(EAssessmentResultTypeG2.Gr).Returns(EFmSectionCategory.Gr);
                }
            }

            public static IEnumerable WbiG4Exceptions
            {
                get
                {
                    yield return new TestCaseData(EAssessmentResultTypeG2.ResultSpecified,
                            EFmSectionCategory.NotApplicable)
                        .Returns(EAssemblyErrors.TranslateAssessmentInvalidInput);
                    yield return new TestCaseData(EAssessmentResultTypeG2.ResultSpecified, null)
                        .Returns(EAssemblyErrors.ValueMayNotBeNull);
                }
            }

            public static IEnumerable Wbi0T4AssessmentResult
            {
                get
                {
                    yield return new TestCaseData(EAssessmentResultTypeT3.Ngo).Returns(EFmSectionCategory.VIIv);
                    yield return new TestCaseData(EAssessmentResultTypeT3.Fv).Returns(EFmSectionCategory.Iv);
                    yield return new TestCaseData(EAssessmentResultTypeT3.Gr).Returns(EFmSectionCategory.Gr);
                }
            }

            public static IEnumerable WbiT4Exceptions
            {
                get
                {
                    yield return new TestCaseData(EAssessmentResultTypeT3.ResultSpecified,
                            EFmSectionCategory.NotApplicable)
                        .Returns(EAssemblyErrors.TranslateAssessmentInvalidInput);
                    yield return new TestCaseData(EAssessmentResultTypeT3.ResultSpecified, null)
                        .Returns(EAssemblyErrors.ValueMayNotBeNull);
                }
            }
        }
    }
}