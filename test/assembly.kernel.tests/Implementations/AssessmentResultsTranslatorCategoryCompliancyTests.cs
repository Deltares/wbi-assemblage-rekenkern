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
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.AssessmentResultTypes;
using Assembly.Kernel.Model.FmSectionTypes;
using NUnit.Framework;

// ReSharper disable UnusedMember.Local

namespace Assembly.Kernel.Tests.Implementations
{
    public class AssessmentResultsTranslatorCategoryCompliancyTests
    {
        private IAssessmentResultsTranslator translator;

        [SetUp]
        public void Init()
        {
            translator = new AssessmentResultsTranslator();
        }

        [Test, TestCaseSource(
             typeof(CategoryCompliancyTestCases),
             nameof(CategoryCompliancyTestCases.Wbi0Gt6ResultSpecified))]
        public EFmSectionCategory Wbi0G6ResultSpecifiedTest(
            FmSectionCategoryCompliancyResults compliancyResults)
        {
            FmSectionAssemblyDirectResult translateResult =
                translator.TranslateAssessmentResultWbi0G6(compliancyResults);
            Assert.IsAssignableFrom<FmSectionAssemblyDirectResult>(translateResult);


            return translateResult.Result;
        }

        [Test]
        public void Wbi0G6ComplyDoesNotComplyTest()
        {
            var inputResults = new FmSectionCategoryCompliancyResults()
                .Set(EFmSectionCategory.Iv, ECategoryCompliancy.NoResult)
                .Set(EFmSectionCategory.IIv, ECategoryCompliancy.DoesNotComply)
                .Set(EFmSectionCategory.IIIv, ECategoryCompliancy.Complies)
                .Set(EFmSectionCategory.IVv, ECategoryCompliancy.Complies)
                .Set(EFmSectionCategory.Vv, ECategoryCompliancy.DoesNotComply);

            try
            {
                translator.TranslateAssessmentResultWbi0G6(inputResults);
            }
            catch (AssemblyException e)
            {
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.DoesNotComplyAfterComply, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception not thrown.");
        }

        [Test]
        public void Wbi0G6NullTest()
        {
            try
            {
                translator.TranslateAssessmentResultWbi0G6(null);
            }
            catch (AssemblyException e)
            {
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNull, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception not thrown.");
        }

        [Test, TestCaseSource(
             typeof(CategoryCompliancyTestCases),
             nameof(CategoryCompliancyTestCases.Wbi0Gt6ResultSpecified))]
        public EFmSectionCategory Wbi0T6ResultSpecifiedTest(
            FmSectionCategoryCompliancyResults compliancyResults)
        {
            var translateResult = translator.TranslateAssessmentResultWbi0T6(compliancyResults,
                EAssessmentResultTypeT3.ResultSpecified);

            Assert.IsAssignableFrom<FmSectionAssemblyDirectResult>(translateResult);

            return translateResult.Result;
        }

        [Test, TestCaseSource(
             typeof(CategoryCompliancyTestCases),
             nameof(CategoryCompliancyTestCases.Wbi0T6Specific))]
        public EFmSectionCategory Wbi0T6SpecificTest(EAssessmentResultTypeT3 assessmentResult)
        {
            var translateResult = translator.TranslateAssessmentResultWbi0T6(null, assessmentResult);

            Assert.IsAssignableFrom<FmSectionAssemblyDirectResult>(translateResult);

            return translateResult.Result;
        }

        [Test]
        public void Wbi0T6ComplyDoesNotComplyTest()
        {
            var inputResults = new FmSectionCategoryCompliancyResults()
                .Set(EFmSectionCategory.Iv, ECategoryCompliancy.NoResult)
                .Set(EFmSectionCategory.IIv, ECategoryCompliancy.DoesNotComply)
                .Set(EFmSectionCategory.IIIv, ECategoryCompliancy.Complies)
                .Set(EFmSectionCategory.IIIv, ECategoryCompliancy.Complies)
                .Set(EFmSectionCategory.IVv, ECategoryCompliancy.Complies)
                .Set(EFmSectionCategory.Vv, ECategoryCompliancy.DoesNotComply);

            try
            {
                translator.TranslateAssessmentResultWbi0T6(inputResults, EAssessmentResultTypeT3.ResultSpecified);
            }
            catch (AssemblyException e)
            {
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.DoesNotComplyAfterComply, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception not thrown.");
        }

        [Test]
        public void Wbi0T6NullTest()
        {
            try
            {
                translator.TranslateAssessmentResultWbi0T6(null, EAssessmentResultTypeT3.ResultSpecified);
            }
            catch (AssemblyException e)
            {
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNull, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception not thrown.");
        }

        [Test]
        public void Wbi0T6TestCompliancyAndAssessment()
        {
            var compliancyResults = new FmSectionCategoryCompliancyResults()
                .Set(EFmSectionCategory.Iv, ECategoryCompliancy.NoResult)
                .Set(EFmSectionCategory.IIv, ECategoryCompliancy.NoResult)
                .Set(EFmSectionCategory.IIIv, ECategoryCompliancy.Complies)
                .Set(EFmSectionCategory.IVv, ECategoryCompliancy.NoResult)
                .Set(EFmSectionCategory.Vv, ECategoryCompliancy.NoResult);

            try
            {
                translator.TranslateAssessmentResultWbi0T6(compliancyResults, EAssessmentResultTypeT3.Fv);
            }
            catch (AssemblyException e)
            {
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.TranslateAssessmentInvalidInput, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("No expected exception not thrown");
        }

        private sealed class CategoryCompliancyTestCases
        {
            public static IEnumerable Wbi0Gt6ResultSpecified
            {
                get
                {
                    yield return new TestCaseData(
                            new FmSectionCategoryCompliancyResults()
                                .Set(EFmSectionCategory.Iv, ECategoryCompliancy.Complies)
                                .Set(EFmSectionCategory.IIv, ECategoryCompliancy.Complies)
                                .Set(EFmSectionCategory.IIIv, ECategoryCompliancy.Complies)
                                .Set(EFmSectionCategory.IVv, ECategoryCompliancy.Complies)
                                .Set(EFmSectionCategory.Vv, ECategoryCompliancy.Complies))
                        .Returns(EFmSectionCategory.Iv)
                        .SetName("AllComply");

                    yield return new TestCaseData(
                            new FmSectionCategoryCompliancyResults()
                                .Set(EFmSectionCategory.Iv, ECategoryCompliancy.DoesNotComply)
                                .Set(EFmSectionCategory.IIv, ECategoryCompliancy.Complies)
                                .Set(EFmSectionCategory.IIIv, ECategoryCompliancy.Complies)
                                .Set(EFmSectionCategory.IVv, ECategoryCompliancy.Complies)
                                .Set(EFmSectionCategory.Vv, ECategoryCompliancy.Complies))
                        .Returns(EFmSectionCategory.IIv)
                        .SetName("IIv comply");

                    yield return new TestCaseData(
                            new FmSectionCategoryCompliancyResults()
                                .Set(EFmSectionCategory.Iv, ECategoryCompliancy.DoesNotComply)
                                .Set(EFmSectionCategory.IIv, ECategoryCompliancy.DoesNotComply)
                                .Set(EFmSectionCategory.IIIv, ECategoryCompliancy.Complies)
                                .Set(EFmSectionCategory.IVv, ECategoryCompliancy.Complies)
                                .Set(EFmSectionCategory.Vv, ECategoryCompliancy.Complies))
                        .Returns(EFmSectionCategory.IIIv)
                        .SetName("IIIv comply");

                    yield return new TestCaseData(
                            new FmSectionCategoryCompliancyResults()
                                .Set(EFmSectionCategory.Iv, ECategoryCompliancy.NoResult)
                                .Set(EFmSectionCategory.IIv, ECategoryCompliancy.DoesNotComply)
                                .Set(EFmSectionCategory.IIIv, ECategoryCompliancy.NoResult)
                                .Set(EFmSectionCategory.IVv, ECategoryCompliancy.Complies)
                                .Set(EFmSectionCategory.Vv, ECategoryCompliancy.Complies))
                        .Returns(EFmSectionCategory.IVv)
                        .SetName("IVv comply");

                    yield return new TestCaseData(
                            new FmSectionCategoryCompliancyResults()
                                .Set(EFmSectionCategory.Iv, ECategoryCompliancy.DoesNotComply)
                                .Set(EFmSectionCategory.IIv, ECategoryCompliancy.DoesNotComply)
                                .Set(EFmSectionCategory.IIIv, ECategoryCompliancy.NoResult)
                                .Set(EFmSectionCategory.IVv, ECategoryCompliancy.DoesNotComply)
                                .Set(EFmSectionCategory.Vv, ECategoryCompliancy.Complies))
                        .Returns(EFmSectionCategory.Vv)
                        .SetName("Vv comply");

                    yield return new TestCaseData(
                            new FmSectionCategoryCompliancyResults()
                                .Set(EFmSectionCategory.Iv, ECategoryCompliancy.NoResult)
                                .Set(EFmSectionCategory.IIv, ECategoryCompliancy.DoesNotComply)
                                .Set(EFmSectionCategory.IIIv, ECategoryCompliancy.NoResult)
                                .Set(EFmSectionCategory.IVv, ECategoryCompliancy.DoesNotComply)
                                .Set(EFmSectionCategory.Vv, ECategoryCompliancy.DoesNotComply))
                        .Returns(EFmSectionCategory.VIv)
                        .SetName("AllDoNotComply");

                    yield return new TestCaseData(
                            new FmSectionCategoryCompliancyResults()
                                .Set(EFmSectionCategory.Iv, ECategoryCompliancy.Complies)
                                .Set(EFmSectionCategory.IIv, ECategoryCompliancy.Complies)
                                .Set(EFmSectionCategory.IIIv, ECategoryCompliancy.Complies)
                                .Set(EFmSectionCategory.IVv, ECategoryCompliancy.Complies)
                                .Set(EFmSectionCategory.Vv, ECategoryCompliancy.Ngo))
                        .Returns(EFmSectionCategory.VIIv)
                        .SetName("Ngo");

                    yield return new TestCaseData(
                            new FmSectionCategoryCompliancyResults()
                                .Set(EFmSectionCategory.Iv, ECategoryCompliancy.Ngo)
                                .Set(EFmSectionCategory.IIv, ECategoryCompliancy.Complies)
                                .Set(EFmSectionCategory.IIIv, ECategoryCompliancy.Complies)
                                .Set(EFmSectionCategory.IVv, ECategoryCompliancy.Complies)
                                .Set(EFmSectionCategory.Vv, ECategoryCompliancy.Complies))
                        .Returns(EFmSectionCategory.VIIv)
                        .SetName("Ngo2");

                    yield return new TestCaseData(
                            new FmSectionCategoryCompliancyResults()
                                .Set(EFmSectionCategory.Iv, ECategoryCompliancy.NoResult)
                                .Set(EFmSectionCategory.IIv, ECategoryCompliancy.NoResult)
                                .Set(EFmSectionCategory.IIIv, ECategoryCompliancy.NoResult)
                                .Set(EFmSectionCategory.IVv, ECategoryCompliancy.NoResult)
                                .Set(EFmSectionCategory.Vv, ECategoryCompliancy.NoResult))
                        .Returns(EFmSectionCategory.Gr)
                        .SetName("AllNoResult");
                }
            }

            public static IEnumerable Wbi0Gt6Exceptions
            {
                get
                {
                    yield return new TestCaseData(null)
                        .Returns(EAssemblyErrors.ValueMayNotBeNull)
                        .SetName("Input is null");

                    yield return new TestCaseData(
                            new FmSectionCategoryCompliancyResults()
                                .Set(EFmSectionCategory.Iv, ECategoryCompliancy.NoResult)
                                .Set(EFmSectionCategory.IIv, ECategoryCompliancy.DoesNotComply)
                                .Set(EFmSectionCategory.IIIv, ECategoryCompliancy.Complies)
                                .Set(EFmSectionCategory.IVv, ECategoryCompliancy.Complies)
                                .Set(EFmSectionCategory.Vv, ECategoryCompliancy.DoesNotComply))
                        .Returns(EAssemblyErrors.DoesNotComplyAfterComply)
                        .SetName("Does not comply after comply");
                }
            }

            public static IEnumerable Wbi0T6Specific
            {
                get
                {
                    yield return new TestCaseData(EAssessmentResultTypeT3.Fv).Returns(EFmSectionCategory.Iv)
                        .SetName("AssessmentResult: Failure probability negligible");
                    yield return new TestCaseData(EAssessmentResultTypeT3.Gr).Returns(EFmSectionCategory.Gr)
                        .SetName("AssessmentResult: No result");
                    yield return new TestCaseData(EAssessmentResultTypeT3.Ngo).Returns(EFmSectionCategory.VIIv)
                        .SetName("AssessmentResult: No judgement yet");
                }
            }
        }
    }
}