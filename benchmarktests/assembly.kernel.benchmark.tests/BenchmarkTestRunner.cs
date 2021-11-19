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
using System.Collections.Generic;
using System.Linq;
using assembly.kernel.benchmark.tests.data.Input;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;
using assembly.kernel.benchmark.tests.data.Result;
using assembly.kernel.benchmark.tests.io;
using assembly.kernel.benchmark.tests.TestHelpers;
using assembly.kernel.benchmark.tests.TestHelpers.Categories;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.CategoryLimits;
using Assembly.Kernel.Model.FmSectionTypes;
using NUnit.Framework;

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
                new AssessmentSection(
                    input.Length,
                    input.SignallingNorm, input.LowerBoundaryNorm));
            CategoriesList<AssessmentSectionCategory> expectedCategories =
                input.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssessmentSectionCategories;

            result.AreEqualCategoriesListAssessmentSection =
                AssertHelper.AssertEqualCategoriesList<AssessmentSectionCategory, EAssessmentGrade>(
                    expectedCategories, categories);
            result.MethodResults.Wbi21 = result.AreEqualCategoriesListAssessmentSection;
        }

        /// <summary>
        /// Test the failure mechanism assembly.
        /// </summary>
        /// <param name="expectedFailureMechanismResult">The expected failure mechanism result.</param>
        /// <param name="lowerBoundaryNorm">The lower boundary norm.</param>
        /// <param name="signallingNorm">The signalling norm.</param>
        /// <param name="testResult">The test result.</param>
        public static void TestFailureMechanismAssembly(IExpectedFailureMechanismResult expectedFailureMechanismResult,
                                                        double lowerBoundaryNorm, double signallingNorm,
                                                        BenchmarkTestResult testResult)
        {
            var failureMechanismTestResult =
                GetBenchmarkTestFailureMechanismResult(testResult, expectedFailureMechanismResult.Type);

            failureMechanismTestResult.AreEqualCategoryBoundaries =
                TesterFactory
                    .CreateCategoriesTester(testResult.MethodResults, expectedFailureMechanismResult, lowerBoundaryNorm,
                                            signallingNorm)
                    ?.TestCategories();

            var failureMechanismTestHelper =
                TesterFactory.CreateFailureMechanismTester(testResult.MethodResults, expectedFailureMechanismResult);

            failureMechanismTestResult.AreEqualDetailedAssessmentResults = failureMechanismTestHelper.TestDetailedAssessment();
            failureMechanismTestResult.AreEqualTailorMadeAssessmentResults =
                failureMechanismTestHelper.TestTailorMadeAssessment();
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

            foreach (FailurePathSectionList failureMechanismsCombinedResult in input
                .ExpectedCombinedSectionResultPerFailureMechanism)
            {
                TestCombinedSectionsFailureMechanismResults(input, result,
                                                            failureMechanismsCombinedResult.FailurePathId.ToMechanismType());
            }
        }

        private static void TestProbabilisticFailureMechanismsResultsTemporal(BenchmarkTestInput input,
                                                                              BenchmarkTestResult result)
        {
            var assembler = new AssessmentGradeAssembler();
            var probabilisticFailureMechanismResultsTemporal = input.ExpectedFailureMechanismsResults
                                                                    .OfType<ProbabilisticExpectedFailureMechanismResult>()
                                                                    .Select(fm =>
                                                                                new FailurePathAssemblyResult(fm.ExpectedAssessmentResultProbabilityTemporal));
            var categories = input.ExpectedSafetyAssessmentAssemblyResult
                .ExpectedAssessmentSectionCategories;
            var resultGroup1And2Temporal = assembler.AssembleAssessmentSectionWbi2B1(
                probabilisticFailureMechanismResultsTemporal, categories,
                true);
            try
            {
                Assert.IsNotNull(resultGroup1And2Temporal);
                Assert.AreEqual(resultGroup1And2Temporal.Category,
                                input.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups1and2Temporal);
                Assert.AreEqual(resultGroup1And2Temporal.FailureProbability,
                                input.ExpectedSafetyAssessmentAssemblyResult
                                     .ExpectedAssemblyResultGroups1and2ProbabilityTemporal);
                result.AreEqualAssemblyResultGroup1and2Temporal = true;
                result.MethodResults.Wbi2B1T = true;
            }
            catch (AssertionException)
            {
                result.AreEqualAssemblyResultGroup1and2Temporal = false;
                result.MethodResults.Wbi2B1T = false;
            }
        }

        private static void TestProbabilisticFailureMechanismsResults(BenchmarkTestInput input,
                                                                      BenchmarkTestResult result)
        {
            IEnumerable<FailurePathAssemblyResult> probabilisticFailureMechanismResults = input
                                                                                               .ExpectedFailureMechanismsResults
                                                                                               .OfType<
                                                                                                   ProbabilisticExpectedFailureMechanismResult
                                                                                               >()
                                                                                               .Select(fm =>
                                                                                                           new
                                                                                                               FailurePathAssemblyResult(
                                                                                                                   fm
                                                                                                                       .ExpectedAssessmentResultProbability));
            // Test correct result for groups 1/2 and 3.4, WBI-2B-1
            var assembler = new AssessmentGradeAssembler();
            var categories = input.ExpectedSafetyAssessmentAssemblyResult
                .ExpectedAssessmentSectionCategories;
            var resultGroup1And2 = assembler.AssembleAssessmentSectionWbi2B1(probabilisticFailureMechanismResults,
                                                                             categories,
                                                                             false);
            try
            {
                Assert.IsNotNull(resultGroup1And2);
                Assert.AreEqual(resultGroup1And2.Category,
                                input.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups1and2);
                Assert.AreEqual(resultGroup1And2.FailureProbability,
                                input.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups1and2Probability);
                result.AreEqualAssemblyResultGroup1and2 = true;
                result.MethodResults.Wbi2B1 = true;
            }
            catch (AssertionException)
            {
                result.AreEqualAssemblyResultGroup1and2 = false;
                result.MethodResults.Wbi2B1 = false;
            }
        }

        private static void TestGeneratedCombinedSections(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            var assembler = new CommonFailureMechanismSectionAssembler();
            // WBI-3A-1
            var combinedSections = assembler.FindGreatestCommonDenominatorSectionsWbi3A1(
                input.ExpectedFailureMechanismsResults.Select(
                         fm => new FailurePathSectionList(fm.Name,
                                                               fm.Sections.Select(
                                                                   s => new FailurePathSection(s.Start, s.End))))
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
                                                                        BenchmarkTestResult result, MechanismType type)
        {
            var assembler = new CommonFailureMechanismSectionAssembler();

            var combinedSections = new FailurePathSectionList("", input.ExpectedCombinedSectionResult);
            var calculatedSectionResults = assembler.TranslateFailureMechanismResultsToCommonSectionsWbi3B1(
                new FailurePathSectionList(
                    type.ToString("D"),
                    input.ExpectedFailureMechanismsResults.First(fm => fm.Type == type).Sections
                         .Select(CreateExpectedFailureMechanismSectionWithResult)),
                combinedSections);

            var calculatedSections = calculatedSectionResults.Sections.ToArray();
            var expectedSections = input.ExpectedCombinedSectionResultPerFailureMechanism.First(l =>
                                                                                                    l.FailurePathId ==
                                                                                                    type.ToString("D")).Sections
                                        .ToArray();

            var fmResult = GetBenchmarkTestFailureMechanismResult(result, type);

            try
            {
                Assert.AreEqual(expectedSections.Length, calculatedSections.Length);
                for (int i = 0; i < expectedSections.Length; i++)
                {
                    Assert.AreEqual(expectedSections[i].SectionStart, calculatedSections[i].SectionStart, 0.01);
                    Assert.AreEqual(expectedSections[i].SectionEnd, calculatedSections[i].SectionEnd, 0.01);
                    Assert.AreEqual(((FailurePathSectionWithResult) expectedSections[i]).Category,
                                        ((FailurePathSectionWithResult) calculatedSections[i]).Category);
                }

                fmResult.AreEqualCombinedResultsCombinedSections = true;
                result.MethodResults.Wbi3B1 = BenchmarkTestHelper.GetUpdatedMethodResult(result.MethodResults.Wbi3B1, true);
            }
            catch (AssertionException)
            {
                fmResult.AreEqualCombinedResultsCombinedSections = false;
                result.MethodResults.Wbi3B1 = false;
            }
        }

        private static FailurePathSection CreateExpectedFailureMechanismSectionWithResult(IFailureMechanismSection section)
        {
            var directMechanism = section as IFailureMechanismSection<EFmSectionCategory>;
            if (directMechanism != null)
            {
                return new FailurePathSectionWithResult(directMechanism.Start, directMechanism.End,
                                                       EInterpretationCategory.Gr);
            }

            throw new InvalidOperationException();
        }

        private static BenchmarkFailureMechanismTestResult GetBenchmarkTestFailureMechanismResult(
            BenchmarkTestResult result,
            MechanismType type)
        {
            var failureMechanismTestResult = result.FailureMechanismResults.FirstOrDefault(fmr => fmr.Type == type);
            if (failureMechanismTestResult == null)
            {
                failureMechanismTestResult =
                    BenchmarkTestFailureMechanismResultFactory.CreateFailureMechanismTestResult(type);
                result.FailureMechanismResults.Add(failureMechanismTestResult);
            }

            return failureMechanismTestResult;
        }
    }
}