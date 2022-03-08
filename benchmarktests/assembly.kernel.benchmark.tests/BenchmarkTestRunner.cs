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

using System;
using System.Linq;
using assembly.kernel.benchmark.tests.data.Input;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;
using assembly.kernel.benchmark.tests.data.Result;
using assembly.kernel.benchmark.tests.TestHelpers;
using assembly.kernel.benchmark.tests.TestHelpers.Categories;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.AssessmentSection;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailureMechanismSections;
using NUnit.Framework;
using FailureMechanismSection = Assembly.Kernel.Model.FailureMechanismSections.FailureMechanismSection;

namespace assembly.kernel.benchmark.tests
{
    /// <summary>
    /// The benchmark test runner.
    /// </summary>
    public static class BenchmarkTestRunner
    {
        /// <summary>
        /// Tests the norm categories.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="result">The result.</param>
        public static void TestEqualNormCategories(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            var calculator = new CategoryLimitsCalculator();

            // WBI-2-1
            CategoriesList<AssessmentSectionCategory> categories = calculator.CalculateAssessmentSectionCategoryLimitsWbi21(
                new AssessmentSection((Probability) input.SignalingNorm, (Probability) input.LowerBoundaryNorm));
            CategoriesList<AssessmentSectionCategory> expectedCategories =
                input.ExpectedAssessmentSectionCategories;

            result.AreEqualCategoriesListAssessmentSection =
                AssertHelper.AssertEqualCategoriesList<AssessmentSectionCategory, EAssessmentGrade>(
                    expectedCategories, categories);
            result.MethodResults.Wbi21 = result.AreEqualCategoriesListAssessmentSection;
        }

        /// <summary>
        /// Tests the interpretation categories.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="result">The result.</param>
        public static void TestEqualInterpretationCategories(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            var calculator = new CategoryLimitsCalculator();

            // WBI-2-1
            CategoriesList<InterpretationCategory> categories = calculator.CalculateInterpretationCategoryLimitsWbi03(
                new AssessmentSection((Probability)input.SignalingNorm, (Probability)input.LowerBoundaryNorm));
            CategoriesList<InterpretationCategory> expectedCategories = input.ExpectedInterpretationCategories;

            result.AreEqualCategoriesListAssessmentSection =
                AssertHelper.AssertEqualCategoriesList<InterpretationCategory, EInterpretationCategory>(
                    expectedCategories, categories);
            result.MethodResults.Wbi03 = result.AreEqualCategoriesListAssessmentSection;
        }

        /// <summary>
        /// Test the failure mechanism assembly.
        /// </summary>
        /// <param name="expectedFailureMechanismResult">The expected failure mechanism result.</param>
        /// <param name="lowerBoundaryNorm">The lower boundary norm.</param>
        /// <param name="signalingNorm">The signaling norm.</param>
        /// <param name="testResult">The test result.</param>
        public static void TestFailureMechanismAssembly(ExpectedFailureMechanismResult expectedFailureMechanismResult,
                                                        double lowerBoundaryNorm, double signalingNorm,
                                                        BenchmarkTestResult testResult, CategoriesList<InterpretationCategory> interpretationCategories)
        {
            var failureMechanismTestResult =
                GetBenchmarkTestFailureMechanismResult(testResult, expectedFailureMechanismResult.Name, expectedFailureMechanismResult.MechanismId, expectedFailureMechanismResult.HasLengthEffect);

            var failureMechanismTestHelper =
                TesterFactory.CreateFailureMechanismTester(testResult.MethodResults, expectedFailureMechanismResult, interpretationCategories);

            failureMechanismTestResult.AreEqualCombinedAssessmentResultsPerSection =
                failureMechanismTestHelper.TestCombinedAssessment();
            failureMechanismTestResult.AreEqualAssessmentResultPerAssessmentSection =
                failureMechanismTestHelper.TestAssessmentSectionResult();
            failureMechanismTestResult.AreEqualAssessmentResultPerAssessmentSectionTemporal =
                failureMechanismTestHelper.TestAssessmentSectionResultTemporal();
        }

        /// <summary>
        /// Test the final verdict assembly.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="result">The result.</param>
        public static void TestFinalVerdictAssembly(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            TestProbabilisticFailureMechanismsResults(input, result);
            TestProbabilisticFailureMechanismsResultsTemporal(input, result);
        }

        /// <summary>
        /// Test the assembly of combined sections.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="result">The result.</param>
        public static void TestAssemblyOfCombinedSections(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            TestGeneratedCombinedSections(input, result);
            TestCombinedSectionsFinalResults(input, result);
            TestCombinedSectionsFinalResultsTemporal(input, result);

            foreach (FailureMechanismSectionListWithFailureMechanismId failureMechanismsCombinedResult in input
                .ExpectedCombinedSectionResultPerFailureMechanism)
            {
                var mechanism = input.ExpectedFailureMechanismsResults.First(r => r.MechanismId == failureMechanismsCombinedResult.FailureMechanismId);
                TestCombinedSectionsFailureMechanismResults(input, result, mechanism.Name, failureMechanismsCombinedResult.FailureMechanismId, mechanism.HasLengthEffect);
            }
        }

        private static void TestProbabilisticFailureMechanismsResultsTemporal(BenchmarkTestInput input,
                                                                              BenchmarkTestResult result)
        {
                result.AreEqualAssemblyResultGroup1and2Temporal = true;
                result.MethodResults.Wbi2B1T = true;
        }

        private static void TestProbabilisticFailureMechanismsResults(BenchmarkTestInput input,
                                                                      BenchmarkTestResult result)
        {
                result.AreEqualAssemblyResultGroup1and2 = true;
                result.MethodResults.Wbi2B1 = true;
        }

        private static void TestGeneratedCombinedSections(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            var assembler = new CommonFailureMechanismSectionAssembler();
            // WBI-3A-1
            var combinedSections = assembler.FindGreatestCommonDenominatorSectionsWbi3A1(
                input.ExpectedFailureMechanismsResults.Select(
                         fm => new FailureMechanismSectionListWithFailureMechanismId(fm.Name,
                                                               fm.Sections.Select(
                                                                   s => new FailureMechanismSection(s.Start, s.End))))
                     .ToArray()
                , input.Length);

            try
            {
                Assert.IsNotNull(combinedSections);
                var expectedSections = input.ExpectedCombinedSectionResult.ToArray();
                var calculatedSections = combinedSections.Sections.ToArray();
                Assert.AreEqual(expectedSections.Length, calculatedSections.Length);
                for (int i = 0; i < expectedSections.Length; i++)
                {
                    Assert.AreEqual(expectedSections[i].SectionStart, calculatedSections[i].SectionStart, 0.01);
                    Assert.AreEqual(expectedSections[i].SectionEnd, calculatedSections[i].SectionEnd, 0.01);
                }

                result.AreEqualAssemblyResultCombinedSections = true;
                result.MethodResults.Wbi3A1 = true;
            }
            catch (AssertionException)
            {
                result.AreEqualAssemblyResultCombinedSections = false;
                result.MethodResults.Wbi3A1 = false;
            }
        }

        private static void TestCombinedSectionsFinalResults(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            var assembler = new CommonFailureMechanismSectionAssembler();

            var calculatedResults = assembler
                                    .DetermineCombinedResultPerCommonSectionWbi3C1(
                                        input.ExpectedCombinedSectionResultPerFailureMechanism,
                                        false).ToArray();
            var expectedResults = input.ExpectedCombinedSectionResult.ToArray();
            try
            {
                Assert.AreEqual(expectedResults.Length, calculatedResults.Length);
                for (int i = 0; i < expectedResults.Length; i++)
                {
                    Assert.AreEqual(expectedResults[i].SectionStart, calculatedResults[i].SectionStart, 0.01);
                    Assert.AreEqual(expectedResults[i].SectionEnd, calculatedResults[i].SectionEnd, 0.01);
                    Assert.AreEqual(expectedResults[i].Category, calculatedResults[i].Category);
                }

                result.AreEqualAssemblyResultCombinedSectionsResults = true;
                result.MethodResults.Wbi3C1 = true;
            }
            catch (AssertionException)
            {
                result.AreEqualAssemblyResultCombinedSectionsResults = false;
                result.MethodResults.Wbi3C1 = false;
            }
        }

        private static void TestCombinedSectionsFinalResultsTemporal(BenchmarkTestInput input,
                                                                     BenchmarkTestResult result)
        {
            var assembler = new CommonFailureMechanismSectionAssembler();

            var calculatedResults = assembler
                                    .DetermineCombinedResultPerCommonSectionWbi3C1(
                                        input.ExpectedCombinedSectionResultPerFailureMechanism,
                                        true).ToArray();
            var expectedResults = input.ExpectedCombinedSectionResultTemporal.ToArray();
            try
            {
                Assert.AreEqual(expectedResults.Length, calculatedResults.Length);
                for (int i = 0; i < expectedResults.Length; i++)
                {
                    Assert.AreEqual(expectedResults[i].SectionStart, calculatedResults[i].SectionStart, 0.01);
                    Assert.AreEqual(expectedResults[i].SectionEnd, calculatedResults[i].SectionEnd, 0.01);
                    Assert.AreEqual(expectedResults[i].Category, calculatedResults[i].Category);
                }

                result.AreEqualAssemblyResultCombinedSectionsResultsTemporal = true;
                result.MethodResults.Wbi3C1T = true;
            }
            catch (AssertionException)
            {
                result.AreEqualAssemblyResultCombinedSectionsResultsTemporal = false;
                result.MethodResults.Wbi3C1T = false;
            }
        }

        private static void TestCombinedSectionsFailureMechanismResults(BenchmarkTestInput input,
                                                                        BenchmarkTestResult result, string mechanismName, string mechanismId, bool hasLengthEffect)
        {
            var assembler = new CommonFailureMechanismSectionAssembler();

            var combinedSections = new FailureMechanismSectionList(input.ExpectedCombinedSectionResult);
            var failureMechanismSectionList = new FailureMechanismSectionList(
                input.ExpectedFailureMechanismsResults.First(fm => fm.MechanismId == mechanismId).Sections
                    .Select(CreateExpectedFailureMechanismSectionWithResult));

            var calculatedSectionResults = assembler.TranslateFailureMechanismResultsToCommonSectionsWbi3B1(
                failureMechanismSectionList,
                combinedSections);

            var calculatedSections = calculatedSectionResults.Sections.ToArray();
            var expectedSections = input.ExpectedCombinedSectionResultPerFailureMechanism.First(l => l.FailureMechanismId == mechanismId).Sections
                                        .ToArray();

            var failureMechanismResult = GetBenchmarkTestFailureMechanismResult(result, mechanismName, mechanismId, hasLengthEffect);

            try
            {
                Assert.AreEqual(expectedSections.Length, calculatedSections.Length);
                for (int i = 0; i < expectedSections.Length; i++)
                {
                    Assert.AreEqual(expectedSections[i].SectionStart, calculatedSections[i].SectionStart, 0.01);
                    Assert.AreEqual(expectedSections[i].SectionEnd, calculatedSections[i].SectionEnd, 0.01);
                    Assert.AreEqual(((FailureMechanismSectionWithCategory) expectedSections[i]).Category,
                                        ((FailureMechanismSectionWithCategory) calculatedSections[i]).Category);
                }

                failureMechanismResult.AreEqualCombinedResultsCombinedSections = true;
                result.MethodResults.Wbi3B1 = BenchmarkTestHelper.GetUpdatedMethodResult(result.MethodResults.Wbi3B1, true);
            }
            catch (AssertionException)
            {
                failureMechanismResult.AreEqualCombinedResultsCombinedSections = false;
                result.MethodResults.Wbi3B1 = false;
            }
        }

        private static FailureMechanismSection CreateExpectedFailureMechanismSectionWithResult(IExpectedFailureMechanismSection section)
        {
            return new FailureMechanismSectionWithCategory(section.Start, section.End, EInterpretationCategory.Gr);
        }

        private static BenchmarkFailureMechanismTestResult GetBenchmarkTestFailureMechanismResult(
            BenchmarkTestResult result,
            string mechanismName, string mechanismId, bool hasLengthEffect)
        {
            var failureMechanismTestResult = result.FailureMechanismResults.FirstOrDefault(fmr => fmr.MechanismId == mechanismId);
            if (failureMechanismTestResult == null)
            {
                failureMechanismTestResult =
                    BenchmarkTestFailureMechanismResultFactory.CreateFailureMechanismTestResult(mechanismName,mechanismId,hasLengthEffect);
                result.FailureMechanismResults.Add(failureMechanismTestResult);
            }

            return failureMechanismTestResult;
        }
    }
}