﻿#region Copyright (C) Rijkswaterstaat 2022. All rights reserved

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

            CategoriesList<AssessmentSectionCategory> categories = calculator.CalculateAssessmentSectionCategoryLimitsBoi21(
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

            CategoriesList<InterpretationCategory> categories = calculator.CalculateInterpretationCategoryLimitsBoi01(
                new AssessmentSection((Probability)input.SignalingNorm, (Probability)input.LowerBoundaryNorm));
            CategoriesList<InterpretationCategory> expectedCategories = input.ExpectedInterpretationCategories;

            result.AreEqualCategoriesListInterpretationCategories =
                AssertHelper.AssertEqualCategoriesList<InterpretationCategory, EInterpretationCategory>(
                    expectedCategories, categories);
            result.MethodResults.Wbi03 = result.AreEqualCategoriesListInterpretationCategories;
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
            failureMechanismTestResult.AreEqualAssessmentResultPerAssessmentSectionTemporal =
                failureMechanismTester.TestAssessmentSectionResultTemporal();
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
            AssessmentSectionResult assemblerResult = null;
            try
            {
                var assembler = new AssessmentGradeAssembler();
                assemblerResult = assembler.CalculateAssessmentSectionFailureProbabilityBoi2B1(
                    input.ExpectedFailureMechanismsResults.Select(fmr => fmr.ExpectedCombinedProbabilityTemporal),
                    input.ExpectedAssessmentSectionCategories, true);
            }
            catch (AssemblyException)
            {
                Assert.IsTrue(input.ExpectedSafetyAssessmentAssemblyResult.ExpectedCombinedAssessmentGradeTemporal ==
                              EExpectedAssessmentGrade.Exception);
                result.AreEqualAssemblyResultFinalVerdictTemporal = true;
                result.AreEqualAssemblyResultFinalVerdictProbabilityTemporal = true;
                result.MethodResults.Wbi2B1T = true;
                return;
            }

            Assert.IsNotNull(assemblerResult);
            AssertHelper.AssertAreEqualProbabilities(
                input.ExpectedSafetyAssessmentAssemblyResult.ExpectedCombinedProbabilityTemporal,
                new Probability(assemblerResult.FailureProbability));
            Assert.AreEqual(
                input.ExpectedSafetyAssessmentAssemblyResult.ExpectedCombinedAssessmentGradeTemporal.ToEAssessmentGrade(),
                assemblerResult.Category);

            result.AreEqualAssemblyResultFinalVerdictTemporal = true;
            result.AreEqualAssemblyResultFinalVerdictProbabilityTemporal = true;
            result.MethodResults.Wbi2B1T = true;
        }

        private static void TestProbabilisticFailureMechanismsResults(BenchmarkTestInput input,
            BenchmarkTestResult result)
        {
            result.MethodResults.Wbi2B1 = false;
            AssessmentSectionResult assemblerResult = null;
            try
            {
                var assembler = new AssessmentGradeAssembler();
                assemblerResult = assembler.CalculateAssessmentSectionFailureProbabilityBoi2B1(
                    input.ExpectedFailureMechanismsResults.Select(fmr => fmr.ExpectedCombinedProbability),
                    input.ExpectedAssessmentSectionCategories, false);
            }
            catch (AssemblyException)
            {
                Assert.IsTrue(input.ExpectedSafetyAssessmentAssemblyResult.ExpectedCombinedAssessmentGrade ==
                              EExpectedAssessmentGrade.Exception);
                result.AreEqualAssemblyResultFinalVerdict = true;
                result.AreEqualAssemblyResultFinalVerdictProbability = true;
                result.MethodResults.Wbi2B1 = true;
                return;
            }

            Assert.IsNotNull(assemblerResult);
            AssertHelper.AssertAreEqualProbabilities(
                input.ExpectedSafetyAssessmentAssemblyResult.ExpectedCombinedProbability,
                new Probability(assemblerResult.FailureProbability));
            Assert.AreEqual(
                input.ExpectedSafetyAssessmentAssemblyResult.ExpectedCombinedAssessmentGrade.ToEAssessmentGrade(),
                assemblerResult.Category);

            result.AreEqualAssemblyResultFinalVerdict = true;
            result.AreEqualAssemblyResultFinalVerdictProbability = true;
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
                    .OfType<ExpectedFailureMechanismSection>()
                    .Select(CreateExpectedFailureMechanismSectionWithResult));

            var calculatedSectionResults = assembler.TranslateFailureMechanismResultsToCommonSectionsWbi3B1(
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
                    Assert.AreEqual(expectedSections[i].SectionStart, calculatedSections[i].SectionStart, 0.01);
                    Assert.AreEqual(expectedSections[i].SectionEnd, calculatedSections[i].SectionEnd, 0.01);
                    Assert.AreEqual(expectedSections[i].Category, calculatedSections[i].Category);
                }

                failureMechanismResult.AreEqualCombinedResultsCombinedSections = true;
                result.MethodResults.Wbi3B1 = BenchmarkTestHelper.GetUpdatedMethodResult(result.MethodResults.Wbi3B1, true);
            }
            catch (AssertionException e)
            {
                Console.WriteLine("Error matching combined sections for {0}: {1}",failureMechanismResult.Name, e.Message);
                failureMechanismResult.AreEqualCombinedResultsCombinedSections = false;
                result.MethodResults.Wbi3B1 = false;
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
                failureMechanismTestResult =
                    BenchmarkTestFailureMechanismResultFactory.CreateFailureMechanismTestResult(mechanismName,mechanismId,hasLengthEffect);
                result.FailureMechanismResults.Add(failureMechanismTestResult);
            }

            return failureMechanismTestResult;
        }
    }
}