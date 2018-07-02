#region Copyright (c) 2018 Technolution BV. All Rights Reserved. 

// // Copyright (C) Technolution BV. 2018. All rights reserved.
// //
// // This file is part of the Assembly kernel.
// //
// // Ringtoets is free software: you can redistribute it and/or modify
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
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.AssessmentResultTypes;
using Assembly.Kernel.Model.FmSectionTypes;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Implementations
{
    [TestFixture]
    public class AssessmentResultsTranslatorAssessmentResultTests
    {
        [SetUp]
        public void Init()
        {
            translator = new AssessmentResultsTranslator();
        }

        private IAssessmentResultsTranslator translator;

        private sealed class AssessmentResultTestCases
        {
            public static IEnumerable Wbi0E1
            {
                get
                {
                    yield return
                        new TestCaseData(EAssessmentResultTypeE1.Nvt).Returns(EFmSectionCategory.NotApplicable);
                    yield return new TestCaseData(EAssessmentResultTypeE1.Fv).Returns(EFmSectionCategory.Iv);
                    yield return new TestCaseData(EAssessmentResultTypeE1.Vb).Returns(EFmSectionCategory.VIIv);
                    yield return new TestCaseData(EAssessmentResultTypeE1.Gr).Returns(EFmSectionCategory.Gr);
                }
            }

            public static IEnumerable Wbi0E3
            {
                get
                {
                    yield return
                        new TestCaseData(EAssessmentResultTypeE2.Nvt).Returns(EFmSectionCategory.NotApplicable);
                    yield return new TestCaseData(EAssessmentResultTypeE2.Wvt).Returns(EFmSectionCategory.VIIv);
                    yield return new TestCaseData(EAssessmentResultTypeE2.Gr).Returns(EFmSectionCategory.Gr);
                }
            }

            public static IEnumerable Wbi0G1
            {
                get
                {
                    yield return new TestCaseData(EAssessmentResultTypeG1.V).Returns(EFmSectionCategory.IIv);
                    yield return new TestCaseData(EAssessmentResultTypeG1.Vn).Returns(EFmSectionCategory.Vv);
                    yield return new TestCaseData(EAssessmentResultTypeG1.Ngo).Returns(EFmSectionCategory.VIIv);
                    yield return new TestCaseData(EAssessmentResultTypeG1.Gr).Returns(EFmSectionCategory.Gr);
                }
            }

            public static IEnumerable Wbi0T1
            {
                get
                {
                    yield return new TestCaseData(EAssessmentResultTypeT1.V).Returns(EFmSectionCategory.IIv);
                    yield return new TestCaseData(EAssessmentResultTypeT1.Vn).Returns(EFmSectionCategory.Vv);
                    yield return new TestCaseData(EAssessmentResultTypeT1.Ngo).Returns(EFmSectionCategory.VIIv);
                    yield return new TestCaseData(EAssessmentResultTypeT1.Fv).Returns(EFmSectionCategory.Iv);
                    yield return new TestCaseData(EAssessmentResultTypeT1.Gr).Returns(EFmSectionCategory.Gr);
                }
            }

            public static IEnumerable Wbi0E2
            {
                get
                {
                    yield return new TestCaseData(EAssessmentResultTypeE1.Nvt).Returns(EIndirectAssessmentResult.Nvt);
                    yield return new TestCaseData(EAssessmentResultTypeE1.Fv).Returns(EIndirectAssessmentResult.FvEt);
                    yield return new TestCaseData(EAssessmentResultTypeE1.Vb).Returns(EIndirectAssessmentResult.Ngo);
                    yield return new TestCaseData(EAssessmentResultTypeE1.Gr).Returns(EIndirectAssessmentResult.Gr);
                }
            }

            public static IEnumerable Wbi0E4
            {
                get
                {
                    yield return new TestCaseData(EAssessmentResultTypeE2.Nvt).Returns(EIndirectAssessmentResult.Nvt);
                    yield return new TestCaseData(EAssessmentResultTypeE2.Wvt).Returns(EIndirectAssessmentResult.Ngo);
                    yield return new TestCaseData(EAssessmentResultTypeE2.Gr).Returns(EIndirectAssessmentResult.Gr);
                }
            }

            public static IEnumerable Wbi0G2
            {
                get
                {
                    yield return new TestCaseData(EAssessmentResultTypeG1.V).Returns(EIndirectAssessmentResult.FvGt);
                    yield return new TestCaseData(EAssessmentResultTypeG1.Vn).Returns(EIndirectAssessmentResult.Ngo);
                    yield return new TestCaseData(EAssessmentResultTypeG1.Ngo).Returns(EIndirectAssessmentResult.Ngo);
                    yield return new TestCaseData(EAssessmentResultTypeG1.Gr).Returns(EIndirectAssessmentResult.Gr);
                }
            }

            public static IEnumerable Wbi0T2
            {
                get
                {
                    yield return new TestCaseData(EAssessmentResultTypeT2.V).Returns(EIndirectAssessmentResult.FvTom);
                    yield return new TestCaseData(EAssessmentResultTypeT2.Vn).Returns(EIndirectAssessmentResult.Ngo);
                    yield return new TestCaseData(EAssessmentResultTypeT2.Ngo).Returns(EIndirectAssessmentResult.Ngo);
                    yield return new TestCaseData(EAssessmentResultTypeT2.Fv).Returns(EIndirectAssessmentResult.FvTom);
                    yield return new TestCaseData(EAssessmentResultTypeT2.Verd).Returns(EIndirectAssessmentResult
                        .FactoredInOtherFailureMechanism);
                }
            }
        }

        [Test, TestCaseSource(
             typeof(AssessmentResultTestCases),
             nameof(AssessmentResultTestCases.Wbi0E1))]
        public EFmSectionCategory Wbi0E1Test(EAssessmentResultTypeE1 assessmentResult)
        {
            FmSectionAssemblyDirectResultWithProbability result =
                translator.TranslateAssessmentResultWbi0E1(assessmentResult);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<FmSectionAssemblyDirectResultWithProbability>(result);

            if (result.Result == EFmSectionCategory.Iv || result.Result == EFmSectionCategory.NotApplicable)
            {
                Assert.AreEqual(0.0, result.FailureProbability);
            }
            else
            {
                Assert.IsNaN(result.FailureProbability);
            }

            return result.Result;
        }

        [Test, TestCaseSource(
             typeof(AssessmentResultTestCases),
             nameof(AssessmentResultTestCases.Wbi0E2))]
        public EIndirectAssessmentResult Wbi0E2Test(EAssessmentResultTypeE1 assessmentResult)
        {
            FmSectionAssemblyIndirectResult result = translator.TranslateAssessmentResultWbi0E2(assessmentResult);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<FmSectionAssemblyIndirectResult>(result);

            return result.Result;
        }

        [Test, TestCaseSource(
             typeof(AssessmentResultTestCases),
             nameof(AssessmentResultTestCases.Wbi0E3))]
        public EFmSectionCategory Wbi0E3Test(EAssessmentResultTypeE2 assessmentResult)
        {
            FmSectionAssemblyDirectResultWithProbability result =
                translator.TranslateAssessmentResultWbi0E3(assessmentResult);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<FmSectionAssemblyDirectResultWithProbability>(result);
            if (result.Result == EFmSectionCategory.Iv || result.Result == EFmSectionCategory.NotApplicable)
            {
                Assert.AreEqual(0.0, result.FailureProbability);
            }
            else
            {
                Assert.IsNaN(result.FailureProbability);
            }

            return result.Result;
        }

        [Test, TestCaseSource(
             typeof(AssessmentResultTestCases),
             nameof(AssessmentResultTestCases.Wbi0E4))]
        public EIndirectAssessmentResult Wbi0E4Test(EAssessmentResultTypeE2 assessmentResult)
        {
            var result = translator.TranslateAssessmentResultWbi0E4(assessmentResult);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<FmSectionAssemblyIndirectResult>(result);

            return result.Result;
        }

        [Test, TestCaseSource(
             typeof(AssessmentResultTestCases),
             nameof(AssessmentResultTestCases.Wbi0G1))]
        public EFmSectionCategory Wbi0G1Test(EAssessmentResultTypeG1 assessmentResult)
        {
            var result = translator.TranslateAssessmentResultWbi0G1(assessmentResult);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<FmSectionAssemblyDirectResult>(result);

            return result.Result;
        }

        [Test, TestCaseSource(
             typeof(AssessmentResultTestCases),
             nameof(AssessmentResultTestCases.Wbi0G2))]
        public EIndirectAssessmentResult Wbi0G2Test(EAssessmentResultTypeG1 assessmentResult)
        {
            var result = translator.TranslateAssessmentResultWbi0G2(assessmentResult);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<FmSectionAssemblyIndirectResult>(result);

            return result.Result;
        }

        [Test, TestCaseSource(
             typeof(AssessmentResultTestCases),
             nameof(AssessmentResultTestCases.Wbi0T1))]
        public EFmSectionCategory Wbi0T1Test(EAssessmentResultTypeT1 assessmentResult)
        {
            FmSectionAssemblyDirectResult result = translator.TranslateAssessmentResultWbi0T1(assessmentResult);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<FmSectionAssemblyDirectResult>(result);

            return result.Result;
        }

        [Test, TestCaseSource(
             typeof(AssessmentResultTestCases),
             nameof(AssessmentResultTestCases.Wbi0T2))]
        public EIndirectAssessmentResult Wbi0T2Test(EAssessmentResultTypeT2 assessmentResult)
        {
            var result = translator.TranslateAssessmentResultWbi0T2(assessmentResult);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<FmSectionAssemblyIndirectResult>(result);

            return result.Result;
        }
    }
}