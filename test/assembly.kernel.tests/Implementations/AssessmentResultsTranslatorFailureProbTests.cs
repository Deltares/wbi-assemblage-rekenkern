#region Copyright (C) Rijkswaterstaat 2019. All rights reserved
// Copyright (C) Rijkswaterstaat 2019. All rights reserved.
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
#endregion

using System.Collections;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model.AssessmentResultTypes;
using Assembly.Kernel.Model.CategoryLimits;
using Assembly.Kernel.Model.FmSectionTypes;
using NUnit.Framework;

// ReSharper disable UnusedMember.Local

namespace Assembly.Kernel.Tests.Implementations
{
    public class AssessmentResultsTranslatorFailureProbTests
    {
        private const double FailureMechanismSectionLengthEffectFactor = 2;

        private static readonly CategoriesList<FmSectionCategory> AssessmentSectionAmelandDefaultCategories =
            new CategoriesList<FmSectionCategory>(new[]
            {
                new FmSectionCategory(EFmSectionCategory.Iv, 0, 1 / 30.0 * 1 / 1000.0 * 0.04 / 14.4),
                new FmSectionCategory(EFmSectionCategory.IIv, 1 / 30.0 * 1 / 1000.0 * 0.04 / 14.4,
                                      1 / 1000.0 * 0.04 / 14.4),
                new FmSectionCategory(EFmSectionCategory.IIIv, 1 / 1000.0 * 0.04 / 14.4, 1 / 300.0 * 0.04 / 14.4),
                new FmSectionCategory(EFmSectionCategory.IVv, 1 / 300.0 * 0.04 / 14.4, 1 / 300.0),
                new FmSectionCategory(EFmSectionCategory.Vv, 1 / 300.0, 30.0 * 1 / 300.0),
                new FmSectionCategory(EFmSectionCategory.VIv, 30.0 * 1 / 300.0, 1.0)
            });

        private IAssessmentResultsTranslator translator;

        [SetUp]
        public void Init()
        {
            translator = new AssessmentResultsTranslator();
        }

        [Test, TestCaseSource(
             typeof(FailureProbabilityTestCases),
             nameof(FailureProbabilityTestCases.Wbi0Gt3FailureProbability))]
        public EFmSectionCategory Wbi0T3FailureProbabilityTest(double failureProbability)
        {
            var result = translator.TranslateAssessmentResultWbi0T3(EAssessmentResultTypeT3.ResultSpecified,
                                                                    failureProbability,
                                                                    AssessmentSectionAmelandDefaultCategories);

            Assert.NotNull(result);
            Assert.AreEqual(failureProbability, result.FailureProbability);

            return result.Result;
        }

        [Test, TestCaseSource(
             typeof(FailureProbabilityTestCases),
             nameof(FailureProbabilityTestCases.Wbi0T35AssessmentResult))]
        public EFmSectionCategory Wbi0T3AssessmentResultTest(EAssessmentResultTypeT3 assessmentResult)
        {
            var result = translator.TranslateAssessmentResultWbi0T3(assessmentResult,
                                                                    double.NaN, AssessmentSectionAmelandDefaultCategories);

            Assert.NotNull(result);
            if (assessmentResult == EAssessmentResultTypeT3.Fv)
            {
                Assert.AreEqual(0.0, result.FailureProbability);
            }
            else
            {
                Assert.IsNaN(result.FailureProbability);
            }

            return result.Result;
        }

        [Test]
        public void Wbi0T3NullTest()
        {
            try
            {
                translator.TranslateAssessmentResultWbi0T3(EAssessmentResultTypeT3.ResultSpecified,
                                                           double.NaN, AssessmentSectionAmelandDefaultCategories);
            }
            catch (AssemblyException e)
            {
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNull, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("No expected exception not thrown");
        }

        [Test, TestCaseSource(
             typeof(FailureProbabilityTestCases),
             nameof(FailureProbabilityTestCases.Wbi0Gt5FailureProbability))]
        public EFmSectionCategory Wbi0T5FailureProbabilityTest(double failureProbability)
        {
            var result = translator.TranslateAssessmentResultWbi0T5(FailureMechanismSectionLengthEffectFactor,
                                                                    EAssessmentResultTypeT3.ResultSpecified,
                                                                    failureProbability,
                                                                    AssessmentSectionAmelandDefaultCategories);

            Assert.NotNull(result);
            Assert.AreEqual(failureProbability * FailureMechanismSectionLengthEffectFactor, result.FailureProbability);

            return result.Result;
        }

        [Test, TestCaseSource(
             typeof(FailureProbabilityTestCases),
             nameof(FailureProbabilityTestCases.Wbi0T35AssessmentResult))]
        public EFmSectionCategory Wbi0T5AssessmentResultTest(EAssessmentResultTypeT3 assessmentResult)
        {
            var result = translator.TranslateAssessmentResultWbi0T5(FailureMechanismSectionLengthEffectFactor,
                                                                    assessmentResult,
                                                                    double.NaN, AssessmentSectionAmelandDefaultCategories);

            Assert.NotNull(result);
            if (assessmentResult == EAssessmentResultTypeT3.Fv)
            {
                Assert.AreEqual(0.0, result.FailureProbability);
            }
            else
            {
                Assert.IsNaN(result.FailureProbability);
            }

            return result.Result;
        }

        [Test]
        public void Wbi0T5NullTest()
        {
            try
            {
                translator.TranslateAssessmentResultWbi0T5(FailureMechanismSectionLengthEffectFactor,
                                                           EAssessmentResultTypeT3.ResultSpecified,
                                                           double.NaN, AssessmentSectionAmelandDefaultCategories);
            }
            catch (AssemblyException e)
            {
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNull, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("No expected exception not thrown");
        }

        [Test]
        public void Wbi0T5ResultingFailureProbAboveOneTest()
        {
            var result = translator.TranslateAssessmentResultWbi0T5(FailureMechanismSectionLengthEffectFactor,
                                                                    EAssessmentResultTypeT3.ResultSpecified,
                                                                    0.6, AssessmentSectionAmelandDefaultCategories);
            Assert.AreEqual(1.0, result.FailureProbability);
        }

        [Test, TestCaseSource(
             typeof(FailureProbabilityTestCases),
             nameof(FailureProbabilityTestCases.Wbi0T7FailureProbability))]
        public EFmSectionCategory Wbi0T7FailureProbabilityTest(double failureProbability, double categoryBoundary)
        {
            var result = translator.TranslateAssessmentResultWbi0T7(
                EAssessmentResultTypeT4.ResultSpecified,
                failureProbability,
                new CategoriesList<FmSectionCategory>(new[]
                {
                    new FmSectionCategory(EFmSectionCategory.IIv, 0.0, categoryBoundary),
                    new FmSectionCategory(EFmSectionCategory.Vv, categoryBoundary, 1.0)
                }));

            Assert.NotNull(result);
            Assert.IsAssignableFrom<FmSectionAssemblyDirectResult>(result);

            return result.Result;
        }

        [Test, TestCaseSource(
             typeof(FailureProbabilityTestCases),
             nameof(FailureProbabilityTestCases.Wbi0T7AssessmentResult))]
        public EFmSectionCategory Wbi0T7AssessmentResultTest(EAssessmentResultTypeT4 assessmentResult)
        {
            var result = translator.TranslateAssessmentResultWbi0T7(
                assessmentResult,
                double.NaN,
                new CategoriesList<FmSectionCategory>(new[]
                {
                    new FmSectionCategory(EFmSectionCategory.IIv, 0.0, 0.5),
                    new FmSectionCategory(EFmSectionCategory.Vv, 0.5, 1.0)
                }));

            Assert.NotNull(result);

            return result.Result;
        }

        [Test]
        public void Wbi0T7ExceptionTest()
        {
            try
            {
                translator.TranslateAssessmentResultWbi0T7(
                    EAssessmentResultTypeT4.ResultSpecified,
                    double.NaN,
                    new CategoriesList<FmSectionCategory>(new[]
                    {
                        new FmSectionCategory(EFmSectionCategory.IIv, 0.0, 0.5),
                        new FmSectionCategory(EFmSectionCategory.Vv, 0.5, 1.0)
                    }));
            }
            catch (AssemblyException e)
            {
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNull, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("No expected exception not thrown");
        }

        private sealed class FailureProbabilityTestCases
        {
            public static IEnumerable Wbi0G35AssessmentResult
            {
                get
                {
                    yield return new TestCaseData(EAssessmentResultTypeG2.Ngo).Returns(EFmSectionCategory.VIIv);
                    yield return new TestCaseData(EAssessmentResultTypeG2.Gr).Returns(EFmSectionCategory.Gr);
                }
            }

            public static IEnumerable Wbi0Gt3FailureProbability
            {
                get
                {
                    /*
                    * input data used is from assessment section 2-1 Ameland (2) and failure mechanism STBI
                    */
                    yield return new TestCaseData(0.0).Returns(EFmSectionCategory.Iv);
                    yield return new TestCaseData(1.5E-6).Returns(EFmSectionCategory.IIv);
                    yield return new TestCaseData(9.259E-6).Returns(EFmSectionCategory.IIIv);
                    yield return new TestCaseData(1.5E-4).Returns(EFmSectionCategory.IVv);
                    yield return new TestCaseData(5.0E-3).Returns(EFmSectionCategory.Vv);
                    yield return new TestCaseData(0.6).Returns(EFmSectionCategory.VIv);
                }
            }

            public static IEnumerable Wbi0Gt5FailureProbability
            {
                get
                {
                    /*
                    * input data used is from assessment section 2-1 Ameland (2) and failure mechanism STBI
                    */
                    yield return new TestCaseData(0.0).Returns(EFmSectionCategory.Iv);
                    yield return new TestCaseData(1.5E-6).Returns(EFmSectionCategory.IIv);
                    yield return new TestCaseData(9.259E-6).Returns(EFmSectionCategory.IIIv);
                    yield return new TestCaseData(1.5E-4).Returns(EFmSectionCategory.IVv);
                    yield return new TestCaseData(5.0E-3).Returns(EFmSectionCategory.Vv);
                    yield return new TestCaseData(0.2).Returns(EFmSectionCategory.VIv);
                }
            }

            public static IEnumerable Wbi0T7FailureProbability
            {
                get
                {
                    /*
                    * input data used is from assessment section 2-1 Ameland (2) and failure mechanism STBI
                    */
                    yield return new TestCaseData(0.0, 0.5).Returns(EFmSectionCategory.IIv);
                    yield return new TestCaseData(1.5E-6, 2.0e-4).Returns(EFmSectionCategory.IIv);
                    yield return new TestCaseData(9.259E-5, 9.259E-5).Returns(EFmSectionCategory.IIv);
                    yield return new TestCaseData(1.5E-4, 1.0e-6).Returns(EFmSectionCategory.Vv);
                    yield return new TestCaseData(5.0E-3, 1.0e-14).Returns(EFmSectionCategory.Vv);
                    yield return new TestCaseData(0.6, 0.1).Returns(EFmSectionCategory.Vv);
                }
            }

            public static IEnumerable Wbi0T35AssessmentResult
            {
                get
                {
                    yield return new TestCaseData(EAssessmentResultTypeT3.Ngo).Returns(EFmSectionCategory.VIIv);
                    yield return new TestCaseData(EAssessmentResultTypeT3.Fv).Returns(EFmSectionCategory.Iv);
                    yield return new TestCaseData(EAssessmentResultTypeT3.Gr).Returns(EFmSectionCategory.Gr);
                }
            }

            public static IEnumerable Wbi0T7AssessmentResult
            {
                get
                {
                    yield return new TestCaseData(EAssessmentResultTypeT4.V).Returns(EFmSectionCategory.IIv);
                    yield return new TestCaseData(EAssessmentResultTypeT4.Vn).Returns(EFmSectionCategory.Vv);
                    yield return new TestCaseData(EAssessmentResultTypeT4.Ngo).Returns(EFmSectionCategory.VIIv);
                    yield return new TestCaseData(EAssessmentResultTypeT4.Fv).Returns(EFmSectionCategory.Iv);
                    yield return new TestCaseData(EAssessmentResultTypeT4.Gr).Returns(EFmSectionCategory.Gr);
                }
            }
        }
    }
}