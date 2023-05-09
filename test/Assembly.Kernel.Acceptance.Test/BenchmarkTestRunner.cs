// Copyright (C) Stichting Deltares and State of the Netherlands 2023. All rights reserved.
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
// All names, logos, and references to "Deltares" are registered trademarks of
// Stichting Deltares and remain full property of Stichting Deltares at all times.
// All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Acceptance.Test.TestHelpers.Categories;
using Assembly.Kernel.Acceptance.Test.TestHelpers.FailureMechanism;
using Assembly.Kernel.Acceptance.TestUtil;
using Assembly.Kernel.Acceptance.TestUtil.Data.Input;
using Assembly.Kernel.Acceptance.TestUtil.Data.Input.FailureMechanisms;
using Assembly.Kernel.Acceptance.TestUtil.Data.Input.FailureMechanismSections;
using Assembly.Kernel.Acceptance.TestUtil.Data.Result;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailureMechanismSections;
using NUnit.Framework;

namespace Assembly.Kernel.Acceptance.Test
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
            CategoriesList<AssessmentSectionCategory> expectedCategories = input.ExpectedAssessmentSectionCategories;

            result.AreEqualCategoriesListAssessmentSection = AssertHelper.AssertEqualCategoriesList<AssessmentSectionCategory, EAssessmentGrade>(
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
                new AssessmentSection((Probability) input.SignalFloodingProbability, (Probability) input.MaximumAllowableFloodingProbability));
            CategoriesList<InterpretationCategory> expectedCategories = input.ExpectedInterpretationCategories;

            result.AreEqualCategoriesListInterpretationCategories = AssertHelper.AssertEqualCategoriesList<InterpretationCategory, EInterpretationCategory>(
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
            BenchmarkFailureMechanismTestResult failureMechanismTestResult = CreateOrGetBenchmarkTestFailureMechanismResult(
                testResult, expectedFailureMechanismResult);

            FailureMechanismResultTesterBase failureMechanismTester;
            if (expectedFailureMechanismResult.HasLengthEffect)
            {
                failureMechanismTester = new FailureMechanismWithLengthEffectResultTester(
                    testResult.MethodResults, expectedFailureMechanismResult, interpretationCategories);
            }
            else
            {
                failureMechanismTester = new FailureMechanismResultTester(
                    testResult.MethodResults, expectedFailureMechanismResult, interpretationCategories);
            }

            failureMechanismTestResult.AreEqualFailureMechanismSectionsResults = failureMechanismTester.TestFailureMechanismSectionResults();
            failureMechanismTestResult.AreEqualFailureMechanismResult = failureMechanismTester.TestFailureMechanismResult();
            failureMechanismTestResult.AreEqualFailureMechanismResultPartial = failureMechanismTester.TestFailureMechanismResultPartial();
            failureMechanismTestResult.AreEqualFailureMechanismTheoreticalBoundaries = failureMechanismTester.TestFailureMechanismTheoreticalBoundaries();
            failureMechanismTestResult.AreEqualFailureMechanismTheoreticalBoundariesPartial = failureMechanismTester.TestFailureMechanismTheoreticalBoundariesPartial();
        }

        /// <summary>
        /// Test the final verdict assembly.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="result">The result.</param>
        public static void TestFinalVerdictAssembly(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            TestCalculatingAssessmentSectionFailureProbability(input, result);
            TestDeterminingAssessmentGrade(input, result);
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

            List<FailureMechanismSectionListWithFailureMechanismId> failureMechanismsCombinedResults = input.ExpectedCombinedSectionResultPerFailureMechanism;
            foreach (FailureMechanismSectionListWithFailureMechanismId failureMechanismsCombinedResult in failureMechanismsCombinedResults)
            {
                ExpectedFailureMechanismResult mechanism = input.ExpectedFailureMechanismsResults.First(
                    r => r.MechanismId == failureMechanismsCombinedResult.FailureMechanismId);

                TestCombinedSectionsFailureMechanismResults(input, result, mechanism);
            }
        }

        private static void TestCalculatingAssessmentSectionFailureProbability(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            var assembler = new AssessmentGradeAssembler();

            IEnumerable<Probability> correlatedProbabilities = input.ExpectedFailureMechanismsResults
                                                                    .Where(fmr => fmr.IsCorrelated)
                                                                    .Select(fmr => fmr.ExpectedCombinedProbability);
            IEnumerable<Probability> uncorrelatedProbabilities = input.ExpectedFailureMechanismsResults
                                                                      .Where(fmr => !fmr.IsCorrelated)
                                                                      .Select(fmr => fmr.ExpectedCombinedProbability);

            Func<Probability> assemblyMethod;
            Action<bool> setResultAction;
            if (correlatedProbabilities.Any())
            {
                assemblyMethod = () => assembler.CalculateAssessmentSectionFailureProbabilityBoi2A2(
                    correlatedProbabilities, uncorrelatedProbabilities, false);
                setResultAction = r => result.MethodResults.Boi2A2 = BenchmarkTestHelper.GetUpdatedMethodResult(result.MethodResults.Boi2A2, r);
            }
            else
            {
                assemblyMethod = () => assembler.CalculateAssessmentSectionFailureProbabilityBoi2A1(
                    uncorrelatedProbabilities, false);
                setResultAction = r => result.MethodResults.Boi2A1 = BenchmarkTestHelper.GetUpdatedMethodResult(result.MethodResults.Boi2A1, r);
            }

            bool assemblyResult;

            try
            {
                Probability probability = assemblyMethod();

                assemblyResult = AssertHelper.AreEqualProbabilities(input.ExpectedSafetyAssessmentAssemblyResult.CombinedProbability, probability);
            }
            catch (AssemblyException)
            {
                assemblyResult = !input.ExpectedSafetyAssessmentAssemblyResult.CombinedProbability.IsDefined;
            }

            result.AreEqualAssemblyResultFinalVerdictProbability = assemblyResult;
            setResultAction(assemblyResult);
        }

        private static void TestCalculatingAssessmentSectionFailureProbabilityPartial(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            var assembler = new AssessmentGradeAssembler();

            IEnumerable<Probability> correlatedProbabilities = input.ExpectedFailureMechanismsResults
                                                                    .Where(fmr => fmr.IsCorrelated)
                                                                    .Select(fmr => fmr.ExpectedCombinedProbabilityPartial);
            IEnumerable<Probability> uncorrelatedProbabilities = input.ExpectedFailureMechanismsResults
                                                                      .Where(fmr => !fmr.IsCorrelated)
                                                                      .Select(fmr => fmr.ExpectedCombinedProbabilityPartial);

            Func<Probability> assemblyMethod;
            Action<bool> setResultAction;
            if (correlatedProbabilities.Any())
            {
                assemblyMethod = () => assembler.CalculateAssessmentSectionFailureProbabilityBoi2A2(
                    correlatedProbabilities, uncorrelatedProbabilities, true);
                setResultAction = r => result.MethodResults.Boi2A2P = BenchmarkTestHelper.GetUpdatedMethodResult(result.MethodResults.Boi2A2P, r);
            }
            else
            {
                assemblyMethod = () => assembler.CalculateAssessmentSectionFailureProbabilityBoi2A1(
                    uncorrelatedProbabilities, true);
                setResultAction = r => result.MethodResults.Boi2A1P = BenchmarkTestHelper.GetUpdatedMethodResult(result.MethodResults.Boi2A1P, r);
            }

            bool assemblyResult;

            try
            {
                Probability probability = assemblyMethod();

                assemblyResult = AssertHelper.AreEqualProbabilities(input.ExpectedSafetyAssessmentAssemblyResult.CombinedProbabilityPartial, probability);
            }
            catch (AssemblyException)
            {
                assemblyResult = !input.ExpectedSafetyAssessmentAssemblyResult.CombinedProbabilityPartial.IsDefined;
            }

            result.AreEqualAssemblyResultFinalVerdictProbabilityPartial = assemblyResult;
            setResultAction(assemblyResult);
        }

        private static void TestDeterminingAssessmentGrade(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            bool assemblyResult;
            try
            {
                var assembler = new AssessmentGradeAssembler();
                EAssessmentGrade assessmentGrade = assembler.DetermineAssessmentGradeBoi2B1(
                    input.ExpectedSafetyAssessmentAssemblyResult.CombinedProbability,
                    input.ExpectedAssessmentSectionCategories);

                assemblyResult = input.ExpectedSafetyAssessmentAssemblyResult.CombinedAssessmentGrade.ToEAssessmentGrade() == assessmentGrade;
            }
            catch (AssemblyException)
            {
                assemblyResult = input.ExpectedSafetyAssessmentAssemblyResult.CombinedAssessmentGrade == EExpectedAssessmentGrade.Exception;
            }

            result.AreEqualAssemblyResultFinalVerdict = assemblyResult;
            result.MethodResults.Boi2B1 = BenchmarkTestHelper.GetUpdatedMethodResult(result.MethodResults.Boi2B1, assemblyResult);
        }

        private static void TestDeterminingAssessmentGradePartial(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            bool assemblyResult;
            try
            {
                var assembler = new AssessmentGradeAssembler();
                EAssessmentGrade assessmentGrade = assembler.DetermineAssessmentGradeBoi2B1(
                    input.ExpectedSafetyAssessmentAssemblyResult.CombinedProbabilityPartial,
                    input.ExpectedAssessmentSectionCategories);

                assemblyResult = input.ExpectedSafetyAssessmentAssemblyResult.CombinedAssessmentGradePartial.ToEAssessmentGrade() == assessmentGrade;
            }
            catch (AssemblyException)
            {
                assemblyResult = input.ExpectedSafetyAssessmentAssemblyResult.CombinedAssessmentGradePartial == EExpectedAssessmentGrade.Exception;
            }

            result.AreEqualAssemblyResultFinalVerdictPartial = assemblyResult;
            result.MethodResults.Boi2B1 = BenchmarkTestHelper.GetUpdatedMethodResult(result.MethodResults.Boi2B1, assemblyResult);
        }

        private static void TestGeneratedCombinedSections(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            var assembler = new CommonFailureMechanismSectionAssembler();

            FailureMechanismSectionList combinedSections = assembler.FindGreatestCommonDenominatorSectionsBoi3A1(
                input.ExpectedFailureMechanismsResults.Select(
                    fm => new FailureMechanismSectionListWithFailureMechanismId(
                        fm.Name, fm.Sections.Select(s => new FailureMechanismSection(s.Start, s.End)))),
                input.Length);

            try
            {
                FailureMechanismSectionWithCategory[] expectedSections = input.ExpectedCombinedSectionResult.ToArray();
                FailureMechanismSection[] calculatedSections = combinedSections.Sections.ToArray();
                Assert.AreEqual(expectedSections.Length, calculatedSections.Length);
                for (var i = 0; i < expectedSections.Length; i++)
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

            FailureMechanismSectionWithCategory[] calculatedResults = assembler.DetermineCombinedResultPerCommonSectionBoi3C1(
                                                                                   input.ExpectedCombinedSectionResultPerFailureMechanism, false)
                                                                               .ToArray();
            FailureMechanismSectionWithCategory[] expectedResults = input.ExpectedCombinedSectionResult.ToArray();
            try
            {
                Assert.AreEqual(expectedResults.Length, calculatedResults.Length);
                for (var i = 0; i < expectedResults.Length; i++)
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

            FailureMechanismSectionWithCategory[] calculatedResults = assembler.DetermineCombinedResultPerCommonSectionBoi3C1(
                                                                                   input.ExpectedCombinedSectionResultPerFailureMechanism, true)
                                                                               .ToArray();
            FailureMechanismSectionWithCategory[] expectedResults = input.ExpectedCombinedSectionResultPartial.ToArray();
            try
            {
                Assert.AreEqual(expectedResults.Length, calculatedResults.Length);
                for (var i = 0; i < expectedResults.Length; i++)
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

        private static void TestCombinedSectionsFailureMechanismResults(BenchmarkTestInput input, BenchmarkTestResult result,
                                                                        ExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            string mechanismId = expectedFailureMechanismResult.MechanismId;

            var combinedSections = new FailureMechanismSectionList(input.ExpectedCombinedSectionResult);
            var failureMechanismSectionList = new FailureMechanismSectionList(input.ExpectedFailureMechanismsResults
                                                                                   .First(fm => fm.MechanismId == mechanismId)
                                                                                   .Sections
                                                                                   .OfType<ExpectedFailureMechanismSection>()
                                                                                   .Select(CreateExpectedFailureMechanismSectionWithResult));

            var assembler = new CommonFailureMechanismSectionAssembler();
            FailureMechanismSectionList calculatedSectionResults = assembler.TranslateFailureMechanismResultsToCommonSectionsBoi3B1(
                failureMechanismSectionList, combinedSections);

            FailureMechanismSectionWithCategory[] calculatedSections = calculatedSectionResults.Sections
                                                                                               .OfType<FailureMechanismSectionWithCategory>()
                                                                                               .ToArray();
            FailureMechanismSectionWithCategory[] expectedSections = input.ExpectedCombinedSectionResultPerFailureMechanism
                                                                          .First(l => l.FailureMechanismId == mechanismId)
                                                                          .Sections
                                                                          .OfType<FailureMechanismSectionWithCategory>()
                                                                          .ToArray();

            BenchmarkFailureMechanismTestResult failureMechanismResult = CreateOrGetBenchmarkTestFailureMechanismResult(
                result, expectedFailureMechanismResult);

            try
            {
                Assert.AreEqual(expectedSections.Length, calculatedSections.Length);
                for (var i = 0; i < expectedSections.Length; i++)
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
                Console.WriteLine($"Error matching combined sections for {failureMechanismResult.Name}: {e.Message}");
                failureMechanismResult.AreEqualCombinedResultsCombinedSections = false;
                result.MethodResults.Boi3B1 = BenchmarkTestHelper.GetUpdatedMethodResult(result.MethodResults.Boi3B1, false);
            }
        }

        private static FailureMechanismSection CreateExpectedFailureMechanismSectionWithResult(ExpectedFailureMechanismSection section)
        {
            return new FailureMechanismSectionWithCategory(section.Start, section.End, section.ExpectedInterpretationCategory);
        }

        private static BenchmarkFailureMechanismTestResult CreateOrGetBenchmarkTestFailureMechanismResult(
            BenchmarkTestResult result, ExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            string mechanismId = expectedFailureMechanismResult.MechanismId;

            BenchmarkFailureMechanismTestResult failureMechanismTestResult = result.FailureMechanismResults.FirstOrDefault(
                fmr => fmr.MechanismId == mechanismId);

            if (failureMechanismTestResult == null)
            {
                failureMechanismTestResult = new BenchmarkFailureMechanismTestResult(
                    expectedFailureMechanismResult.Name, mechanismId,
                    expectedFailureMechanismResult.HasLengthEffect,
                    expectedFailureMechanismResult.AssemblyMethod,
                    expectedFailureMechanismResult.IsCorrelated);

                result.FailureMechanismResults.Add(failureMechanismTestResult);
            }

            return failureMechanismTestResult;
        }
    }
}