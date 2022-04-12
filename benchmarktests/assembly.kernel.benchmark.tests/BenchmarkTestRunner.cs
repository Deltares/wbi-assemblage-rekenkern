#region Copyright (C) Rijkswaterstaat 2022. All rights reserved.

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

#endregion

using System;
using System.Linq;
using assembly.kernel.benchmark.tests.data.Input;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;
using assembly.kernel.benchmark.tests.data.Result;
using assembly.kernel.benchmark.tests.TestHelpers;
using assembly.kernel.benchmark.tests.TestHelpers.Categories;
using assembly.kernel.benchmark.tests.TestHelpers.FailureMechanism;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.AssessmentSection;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailureMechanismSections;
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

            CategoriesList<AssessmentSectionCategory> categories = calculator.CalculateAssessmentSectionCategoryLimitsBoi21(
                new AssessmentSection((Probability) input.SignalFloodingProbability, (Probability) input.MaximumAllowableFloodingProbability));
            CategoriesList<AssessmentSectionCategory> expectedCategories =
                input.ExpectedAssessmentSectionCategories;

            result.AreEqualCategoriesListAssessmentSection =
                AssertHelper.AssertEqualCategoriesList<AssessmentSectionCategory, EAssessmentGrade>(
                    expectedCategories, categories);
            result.MethodResults.Boi21 = result.AreEqualCategoriesListAssessmentSection;
        }

        /// <summary>
        /// Tests the interpretation categories.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="result">The result.</param>
        public static void TestEqualInterpretationCategories(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            var calculator = new CategoryLimitsCalculator();

            CategoriesList<InterpretationCategory> categories = calculator.CalculateInterpretationCategoryLimitsBoi01(
                new AssessmentSection((Probability)input.SignalFloodingProbability, (Probability)input.MaximumAllowableFloodingProbability));
            CategoriesList<InterpretationCategory> expectedCategories = input.ExpectedInterpretationCategories;

            result.AreEqualCategoriesListInterpretationCategories =
                AssertHelper.AssertEqualCategoriesList<InterpretationCategory, EInterpretationCategory>(
                    expectedCategories, categories);
            result.MethodResults.Boi01 = result.AreEqualCategoriesListInterpretationCategories;
        }

        /// <summary>
        /// Test the failure mechanism assembly.
        /// </summary>
        /// <param name="expectedFailureMechanismResult">The expected failure mechanism result.</param>
        /// <param name="testResult">The test result.</param>
        /// <param name="interpretationCategories">The interpretation categories needed to translate a probability to an interpretation category.</param>
        public static void TestFailureMechanismAssembly(ExpectedFailureMechanismResult expectedFailureMechanismResult,
                                                        BenchmarkTestResult testResult, CategoriesList<InterpretationCategory> interpretationCategories)
        {
            var failureMechanismTestResult =
                GetBenchmarkTestFailureMechanismResult(testResult, expectedFailureMechanismResult.Name, expectedFailureMechanismResult.MechanismId, expectedFailureMechanismResult.HasLengthEffect);

            var failureMechanismTester =
                expectedFailureMechanismResult.HasLengthEffect
                    ? new FailureMechanismWithLengthEffectResultTester(testResult.MethodResults, expectedFailureMechanismResult, interpretationCategories) as IFailureMechanismResultTester
                    : new FailureMechanismResultTester(testResult.MethodResults, expectedFailureMechanismResult, interpretationCategories);

            failureMechanismTestResult.AreEqualCombinedAssessmentResultsPerSection =
                failureMechanismTester.TestCombinedAssessment();
            failureMechanismTestResult.AreEqualAssessmentResultPerAssessmentSection =
                failureMechanismTester.TestAssessmentSectionResult();
            failureMechanismTestResult.AreEqualAssessmentResultPerAssessmentSectionPartial =
                failureMechanismTester.TestAssessmentSectionResultPartial();
        }

        /// <summary>
        /// Test the final verdict assembly.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="result">The result.</param>
        public static void TestFinalVerdictAssembly(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            TestCalculatingAssessmentSectionFailureProbability(input, result);
            TestDeterminingAssessmentGrade(input,result);
            TestCalculatingAssessmentSectionFailureProbabilityPartial(input, result);
            TestDeterminingAssessmentGradePartial(input, result);
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
            TestCombinedSectionsFinalResultsPartial(input, result);

            foreach (FailureMechanismSectionListWithFailureMechanismId failureMechanismsCombinedResult in input
                .ExpectedCombinedSectionResultPerFailureMechanism)
            {
                var mechanism = input.ExpectedFailureMechanismsResults.First(r => r.MechanismId == failureMechanismsCombinedResult.FailureMechanismId);
                TestCombinedSectionsFailureMechanismResults(input, result, mechanism.Name, failureMechanismsCombinedResult.FailureMechanismId, mechanism.HasLengthEffect);
            }
        }

        private static void TestCalculatingAssessmentSectionFailureProbability(BenchmarkTestInput input,
            BenchmarkTestResult result)
        {
            try
            {
                Probability probability;
                try
                {
                    var assembler = new AssessmentGradeAssembler();
                    probability = assembler.CalculateAssessmentSectionFailureProbabilityBoi2A1(
                        input.ExpectedFailureMechanismsResults.Select(fmr => fmr.ExpectedCombinedProbability), false);
                }
                catch (AssemblyException)
                {
                    Assert.IsFalse(input.ExpectedSafetyAssessmentAssemblyResult.CombinedProbability.IsDefined);
                    result.AreEqualAssemblyResultFinalVerdictProbability = true;
                    result.MethodResults.Boi2A1 = true;
                    return;
                }

                AssertHelper.AssertAreEqualProbabilities(
                    input.ExpectedSafetyAssessmentAssemblyResult.CombinedProbability, probability);
                result.AreEqualAssemblyResultFinalVerdictProbability = true;
                result.MethodResults.Boi2A1 = true;
            }
            catch (Exception)
            {
                result.AreEqualAssemblyResultFinalVerdictProbability = false;
                result.MethodResults.Boi2A1 = false;
            }
        }

        private static void TestCalculatingAssessmentSectionFailureProbabilityPartial(BenchmarkTestInput input,
            BenchmarkTestResult result)
        {
            try
            {
                Probability probability;
                try
                {
                    var assembler = new AssessmentGradeAssembler();
                    probability = assembler.CalculateAssessmentSectionFailureProbabilityBoi2A1(
                        input.ExpectedFailureMechanismsResults.Select(fmr => fmr.ExpectedCombinedProbabilityPartial), true);
                }
                catch (AssemblyException)
                {
                    Assert.IsFalse(input.ExpectedSafetyAssessmentAssemblyResult.CombinedProbabilityPartial.IsDefined);
                    result.AreEqualAssemblyResultFinalVerdictProbabilityPartial = true;
                    result.MethodResults.Boi2A1P = true;
                    return;
                }

                AssertHelper.AssertAreEqualProbabilities(
                    input.ExpectedSafetyAssessmentAssemblyResult.CombinedProbabilityPartial, probability);

                result.AreEqualAssemblyResultFinalVerdictProbabilityPartial = true;
                result.MethodResults.Boi2A1P = true;
            }
            catch (Exception )
            {
                result.AreEqualAssemblyResultFinalVerdictProbabilityPartial = false;
                result.MethodResults.Boi2A1P = false;
            }
        }

        private static void TestDeterminingAssessmentGrade(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            try
            {
                EAssessmentGrade assessmentGrade;
                try
                {
                    var assembler = new AssessmentGradeAssembler();
                    assessmentGrade =
                        assembler.DetermineAssessmentGradeBoi2B1(input.ExpectedSafetyAssessmentAssemblyResult.CombinedProbability, input.ExpectedAssessmentSectionCategories);
                }
                catch (AssemblyException)
                {
                    Assert.IsTrue(input.ExpectedSafetyAssessmentAssemblyResult.CombinedAssessmentGrade ==
                                  EExpectedAssessmentGrade.Exception);
                    result.AreEqualAssemblyResultFinalVerdict = true;
                    result.MethodResults.Boi2B1 = BenchmarkTestHelper.GetUpdatedMethodResult(result.MethodResults.Boi2B1, true);
                    return;
                }

                Assert.AreEqual(
                    input.ExpectedSafetyAssessmentAssemblyResult.CombinedAssessmentGrade.ToEAssessmentGrade(),
                    assessmentGrade);
                result.AreEqualAssemblyResultFinalVerdict = true;
                result.MethodResults.Boi2B1 = BenchmarkTestHelper.GetUpdatedMethodResult(result.MethodResults.Boi2B1, true);
            }
            catch (Exception )
            {
                result.AreEqualAssemblyResultFinalVerdict = false;
                result.MethodResults.Boi2B1 = BenchmarkTestHelper.GetUpdatedMethodResult(result.MethodResults.Boi2B1, false);
            }
        }

        private static void TestDeterminingAssessmentGradePartial(BenchmarkTestInput input,
            BenchmarkTestResult result)
        {
            try
            {
                EAssessmentGrade assessmentGrade;
                try
                {
                    var assembler = new AssessmentGradeAssembler();
                    assessmentGrade = assembler.DetermineAssessmentGradeBoi2B1(input.ExpectedSafetyAssessmentAssemblyResult.CombinedProbabilityPartial, input.ExpectedAssessmentSectionCategories);
                }
                catch (AssemblyException)
                {
                    Assert.IsTrue(input.ExpectedSafetyAssessmentAssemblyResult.CombinedAssessmentGradePartial ==
                                  EExpectedAssessmentGrade.Exception);
                    result.AreEqualAssemblyResultFinalVerdictPartial = true;
                    result.MethodResults.Boi2B1 = BenchmarkTestHelper.GetUpdatedMethodResult(result.MethodResults.Boi2B1,true);
                    return;
                }

                Assert.AreEqual(
                    input.ExpectedSafetyAssessmentAssemblyResult.CombinedAssessmentGradePartial
                        .ToEAssessmentGrade(), assessmentGrade);

                result.AreEqualAssemblyResultFinalVerdictPartial = true;
                result.MethodResults.Boi2B1 = BenchmarkTestHelper.GetUpdatedMethodResult(result.MethodResults.Boi2B1, true);
            }
            catch (Exception)
            {
                result.AreEqualAssemblyResultFinalVerdictPartial = false;
                result.MethodResults.Boi2B1 = BenchmarkTestHelper.GetUpdatedMethodResult(result.MethodResults.Boi2B1, false);
            }
        }

        private static void TestGeneratedCombinedSections(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            var assembler = new CommonFailureMechanismSectionAssembler();
            // BOI-3A-1
            var combinedSections = assembler.FindGreatestCommonDenominatorSectionsBoi3A1(
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
                    Assert.AreEqual(expectedSections[i].Start, calculatedSections[i].Start, 0.01);
                    Assert.AreEqual(expectedSections[i].End, calculatedSections[i].End, 0.01);
                }

                result.AreEqualAssemblyResultCombinedSections = true;
                result.MethodResults.Boi3A1 = true;
            }
            catch (AssertionException)
            {
                result.AreEqualAssemblyResultCombinedSections = false;
                result.MethodResults.Boi3A1 = false;
            }
        }

        private static void TestCombinedSectionsFinalResults(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            var assembler = new CommonFailureMechanismSectionAssembler();

            var calculatedResults = assembler
                                    .DetermineCombinedResultPerCommonSectionBoi3C1(
                                        input.ExpectedCombinedSectionResultPerFailureMechanism,
                                        false).ToArray();
            var expectedResults = input.ExpectedCombinedSectionResult.ToArray();
            try
            {
                Assert.AreEqual(expectedResults.Length, calculatedResults.Length);
                for (int i = 0; i < expectedResults.Length; i++)
                {
                    Assert.AreEqual(expectedResults[i].Start, calculatedResults[i].Start, 0.01);
                    Assert.AreEqual(expectedResults[i].End, calculatedResults[i].End, 0.01);
                    Assert.AreEqual(expectedResults[i].Category, calculatedResults[i].Category);
                }

                result.AreEqualAssemblyResultCombinedSectionsResults = true;
                result.MethodResults.Boi3C1 = true;
            }
            catch (AssertionException)
            {
                result.AreEqualAssemblyResultCombinedSectionsResults = false;
                result.MethodResults.Boi3C1 = false;
            }
        }

        private static void TestCombinedSectionsFinalResultsPartial(BenchmarkTestInput input,
                                                                     BenchmarkTestResult result)
        {
            var assembler = new CommonFailureMechanismSectionAssembler();

            var calculatedResults = assembler
                                    .DetermineCombinedResultPerCommonSectionBoi3C1(
                                        input.ExpectedCombinedSectionResultPerFailureMechanism,
                                        true).ToArray();
            var expectedResults = input.ExpectedCombinedSectionResultPartial.ToArray();
            try
            {
                Assert.AreEqual(expectedResults.Length, calculatedResults.Length);
                for (int i = 0; i < expectedResults.Length; i++)
                {
                    Assert.AreEqual(expectedResults[i].Start, calculatedResults[i].Start, 0.01);
                    Assert.AreEqual(expectedResults[i].End, calculatedResults[i].End, 0.01);
                    Assert.AreEqual(expectedResults[i].Category, calculatedResults[i].Category);
                }

                result.AreEqualAssemblyResultCombinedSectionsResultsPartial = true;
                result.MethodResults.Boi3C1P = true;
            }
            catch (AssertionException)
            {
                result.AreEqualAssemblyResultCombinedSectionsResultsPartial = false;
                result.MethodResults.Boi3C1P = false;
            }
        }

        private static void TestCombinedSectionsFailureMechanismResults(BenchmarkTestInput input,
                                                                        BenchmarkTestResult result, string mechanismName, string mechanismId, bool hasLengthEffect)
        {
            var assembler = new CommonFailureMechanismSectionAssembler();

            var combinedSections = new FailureMechanismSectionList(input.ExpectedCombinedSectionResult);
            var failureMechanismSectionList = new FailureMechanismSectionList(
                input.ExpectedFailureMechanismsResults.First(fm => fm.MechanismId == mechanismId).Sections
                    .OfType<ExpectedFailureMechanismSection>()
                    .Select(CreateExpectedFailureMechanismSectionWithResult));

            var calculatedSectionResults = assembler.TranslateFailureMechanismResultsToCommonSectionsBoi3B1(
                failureMechanismSectionList,
                combinedSections);

            var calculatedSections = calculatedSectionResults.Sections.OfType<FailureMechanismSectionWithCategory>().ToArray();
            var expectedSections = input.ExpectedCombinedSectionResultPerFailureMechanism.First(l => l.FailureMechanismId == mechanismId).Sections
                                        .OfType<FailureMechanismSectionWithCategory>().ToArray();

            var failureMechanismResult = GetBenchmarkTestFailureMechanismResult(result, mechanismName, mechanismId, hasLengthEffect);

            try
            {
                Assert.AreEqual(expectedSections.Length, calculatedSections.Length);
                for (int i = 0; i < expectedSections.Length; i++)
                {
                    Assert.AreEqual(expectedSections[i].Start, calculatedSections[i].Start, 0.01);
                    Assert.AreEqual(expectedSections[i].End, calculatedSections[i].End, 0.01);
                    Assert.AreEqual(expectedSections[i].Category, calculatedSections[i].Category);
                }

                failureMechanismResult.AreEqualCombinedResultsCombinedSections = true;
                result.MethodResults.Boi3B1 = BenchmarkTestHelper.GetUpdatedMethodResult(result.MethodResults.Boi3B1, true);
            }
            catch (AssertionException e)
            {
                Console.WriteLine("Error matching combined sections for {0}: {1}",failureMechanismResult.Name, e.Message);
                failureMechanismResult.AreEqualCombinedResultsCombinedSections = false;
                result.MethodResults.Boi3B1 = false;
            }
        }

        private static FailureMechanismSection CreateExpectedFailureMechanismSectionWithResult(ExpectedFailureMechanismSection section)
        {
            return new FailureMechanismSectionWithCategory(section.Start, section.End, section.ExpectedInterpretationCategory);
        }

        private static BenchmarkFailureMechanismTestResult GetBenchmarkTestFailureMechanismResult(
            BenchmarkTestResult result,
            string mechanismName, string mechanismId, bool hasLengthEffect)
        {
            var failureMechanismTestResult = result.FailureMechanismResults.FirstOrDefault(fmr => fmr.MechanismId == mechanismId);
            if (failureMechanismTestResult == null)
            {
                failureMechanismTestResult = new BenchmarkFailureMechanismTestResult(mechanismName, mechanismId, hasLengthEffect);
                result.FailureMechanismResults.Add(failureMechanismTestResult);
            }

            return failureMechanismTestResult;
        }
    }
}