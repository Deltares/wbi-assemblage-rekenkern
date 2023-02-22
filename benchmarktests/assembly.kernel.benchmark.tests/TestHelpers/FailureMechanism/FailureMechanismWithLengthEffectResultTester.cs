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

using System.Collections.Generic;
using System.Linq;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;
using assembly.kernel.benchmark.tests.data.Result;
using assembly.kernel.benchmark.tests.TestHelpers.Categories;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailureMechanismSections;
using NUnit.Framework;

namespace assembly.kernel.benchmark.tests.TestHelpers.FailureMechanism
{
    /// <summary>
    /// Tester for the methods related to a failure mechanism with length-effect.
    /// </summary>
    public class FailureMechanismWithLengthEffectResultTester : FailureMechanismResultTesterBase
    {
        private bool? boi0A2TestResult;
        private bool? boi0B1TestResult;
        private bool? boi0C1TestResult;
        private bool? boi0C2TestResult;
        private bool? boi0D1TestResult;
        private bool? boi0D2TestResult;

        /// <inheritdoc />
        public FailureMechanismWithLengthEffectResultTester(MethodResultsListing methodResults, ExpectedFailureMechanismResult expectedFailureMechanismResult, CategoriesList<InterpretationCategory> interpretationCategories) : base(methodResults, expectedFailureMechanismResult, interpretationCategories) {}

        protected override void SetCombinedAssessmentMethodResult(bool result)
        {
            MethodResults.Boi0A2 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Boi0A2, boi0A2TestResult);
            MethodResults.Boi0B1 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Boi0B1, boi0B1TestResult);
            MethodResults.Boi0C1 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Boi0C1, boi0C1TestResult);
            MethodResults.Boi0C2 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Boi0C2, boi0C2TestResult);
            MethodResults.Boi0D1 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Boi0D1, boi0D1TestResult);
            MethodResults.Boi0D2 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Boi0D2, boi0D2TestResult);
            ResetTestResults();
        }

        protected override void TestCombinedAssessmentInternal()
        {
            var assembler = new AssessmentResultsTranslator();
            ResetTestResults();

            if (ExpectedFailureMechanismResult != null)
            {
                var exception = new AssertionException("Errors occurred");
                IEnumerable<ExpectedFailureMechanismSectionWithLengthEffect> failureMechanismSectionWithLengthEffects
                    = ExpectedFailureMechanismResult.Sections.OfType<ExpectedFailureMechanismSectionWithLengthEffect>();
                foreach (ExpectedFailureMechanismSectionWithLengthEffect section in failureMechanismSectionWithLengthEffects)
                {
                    Probability calculatedCombinedSectionProbability = assembler.CalculateProfileProbabilityToSectionProbabilityBoi0D1(
                        section.ExpectedCombinedProbabilityProfile, section.LengthEffectFactorCombinedProbability);
                    Probability calculatedCombinedProfileProbability = assembler.CalculateSectionProbabilityToProfileProbabilityBoi0D2(
                        section.ExpectedCombinedProbabilitySection, section.LengthEffectFactorCombinedProbability);

                    ESectionInitialMechanismProbabilitySpecification relevance;
                    if (!section.IsRelevant)
                    {
                        relevance = ESectionInitialMechanismProbabilitySpecification.NotRelevant;
                    }
                    else
                    {
                        relevance = double.IsNaN(section.InitialMechanismProbabilitySection) 
                                        ? ESectionInitialMechanismProbabilitySpecification.RelevantNoProbabilitySpecification 
                                        : ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification;
                    }

                    ERefinementStatus refinementStatus = section.RefinementStatus;
                    ResultWithProfileAndSectionProbabilities probabilitiesResult;
                    EInterpretationCategory category;

                    EAnalysisState analysisState = GetAnalysisState(relevance, refinementStatus);
                    if (analysisState == EAnalysisState.ProbabilityEstimated)
                    {
                        probabilitiesResult = assembler.DetermineRepresentativeProbabilitiesBoi0A2(
                            refinementStatus == ERefinementStatus.Performed, section.InitialMechanismProbabilityProfile,
                            section.InitialMechanismProbabilitySection, section.RefinedProbabilityProfile,
                            section.RefinedProbabilitySection);
                        category = assembler.DetermineInterpretationCategoryFromFailureMechanismSectionProbabilityBoi0B1(
                            probabilitiesResult.ProbabilitySection, InterpretationCategories);
                    }
                    else
                    {
                        category = assembler.DetermineInterpretationCategoryWithoutProbabilityEstimationBoi0C1(analysisState);
                        Probability probability = assembler.TranslateInterpretationCategoryToProbabilityBoi0C2(category);
                        probabilitiesResult = new ResultWithProfileAndSectionProbabilities(probability, probability);
                    }

                    try
                    {
                        Assert.IsTrue(calculatedCombinedSectionProbability.IsNegligibleDifference(section.ExpectedCombinedProbabilitySection));
                        boi0D1TestResult = BenchmarkTestHelper.GetUpdatedMethodResult(boi0D1TestResult, true);
                    }
                    catch (AssertionException e)
                    {
                        exception.Data.Add(section.SectionName + " (BOI-0D-1)", e);
                        boi0D1TestResult = BenchmarkTestHelper.GetUpdatedMethodResult(boi0D1TestResult, false);
                    }

                    try
                    {
                        Assert.IsTrue(calculatedCombinedProfileProbability.IsNegligibleDifference(section.ExpectedCombinedProbabilityProfile));
                        boi0D2TestResult = BenchmarkTestHelper.GetUpdatedMethodResult(boi0D2TestResult, true);
                    }
                    catch (AssertionException e)
                    {
                        exception.Data.Add(section.SectionName + " (BOI-0D-2)", e);
                        boi0D2TestResult = BenchmarkTestHelper.GetUpdatedMethodResult(boi0D2TestResult, false);
                    }

                    try
                    {
                        AssertHelper.AssertAreEqualProbabilities(section.ExpectedCombinedProbabilityProfile, probabilitiesResult.ProbabilityProfile);
                        AssertHelper.AssertAreEqualProbabilities(section.ExpectedCombinedProbabilitySection, probabilitiesResult.ProbabilitySection);
                        Assert.AreEqual(section.ExpectedInterpretationCategory, category);
                        if (analysisState == EAnalysisState.ProbabilityEstimated)
                        {
                            boi0A2TestResult = BenchmarkTestHelper.GetUpdatedMethodResult(boi0A2TestResult, true);
                            boi0B1TestResult = BenchmarkTestHelper.GetUpdatedMethodResult(boi0B1TestResult, true);
                        }
                        else
                        {
                            boi0C1TestResult = BenchmarkTestHelper.GetUpdatedMethodResult(boi0C1TestResult, true);
                            boi0C2TestResult = BenchmarkTestHelper.GetUpdatedMethodResult(boi0C2TestResult, true);
                        }
                    }
                    catch (AssertionException e)
                    {
                        if (analysisState == EAnalysisState.ProbabilityEstimated)
                        {
                            exception.Data.Add(section.SectionName + " (BOI-0A-2 / BOI-0B-1)", e);
                            boi0A2TestResult = BenchmarkTestHelper.GetUpdatedMethodResult(boi0A2TestResult, false);
                            boi0B1TestResult = BenchmarkTestHelper.GetUpdatedMethodResult(boi0B1TestResult, false);
                        }
                        else
                        {
                            exception.Data.Add(section.SectionName + " (BOI-0C-*)", e);
                            boi0C1TestResult = BenchmarkTestHelper.GetUpdatedMethodResult(boi0C1TestResult, false);
                            boi0C2TestResult = BenchmarkTestHelper.GetUpdatedMethodResult(boi0C2TestResult, false);
                        }
                    }
                }

                if (exception.Data.Count > 0)
                {
                    throw exception;
                }
            }
        }

        protected override void SetAssessmentSectionMethodResult(bool result)
        {
            MethodResults.Boi1A2 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Boi1A2, result);
        }

        protected override void TestAssessmentSectionResultInternal()
        {
            var assembler = new FailureMechanismResultAssembler();

            if (ExpectedFailureMechanismResult != null)
            {
                FailureMechanismAssemblyResult result = null;
                try
                {
                    result = assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(
                        ExpectedFailureMechanismResult.LengthEffectFactor,
                        ExpectedFailureMechanismResult.Sections
                                                      .OfType<ExpectedFailureMechanismSectionWithLengthEffect>()
                                                      .Select(s => new ResultWithProfileAndSectionProbabilities(
                                                                  s.ExpectedCombinedProbabilityProfile,
                                                                  s.ExpectedCombinedProbabilitySection))
                                                      .ToArray(),
                        false);
                }
                catch (AssemblyException)
                {
                    result = new FailureMechanismAssemblyResult(Probability.Undefined, EFailureMechanismAssemblyMethod.Correlated);
                }

                AssertHelper.AssertAreEqualProbabilities(ExpectedFailureMechanismResult.ExpectedCombinedProbability, result.Probability);
                Assert.AreEqual(ExpectedFailureMechanismResult.ExpectedIsSectionsCorrelated, result.AssemblyMethod);
            }
        }

        protected override void SetAssessmentSectionMethodResultPartial(bool result)
        {
            MethodResults.Boi1A2P = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Boi1A2P, result);
        }

        protected override void TestAssessmentSectionResultPartialInternal()
        {
            var assembler = new FailureMechanismResultAssembler();

            if (ExpectedFailureMechanismResult != null)
            {
                FailureMechanismAssemblyResult result = assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(
                    ExpectedFailureMechanismResult.LengthEffectFactor,
                    ExpectedFailureMechanismResult.Sections
                                                  .OfType<ExpectedFailureMechanismSectionWithLengthEffect>()
                                                  .Select(s => new ResultWithProfileAndSectionProbabilities(
                                                              s.ExpectedCombinedProbabilityProfile,
                                                              s.ExpectedCombinedProbabilitySection))
                                                  .ToArray(),
                    true);

                AssertHelper.AssertAreEqualProbabilities(ExpectedFailureMechanismResult.ExpectedCombinedProbabilityPartial, result.Probability);
                Assert.AreEqual(ExpectedFailureMechanismResult.ExpectedIsSectionsCorrelatedPartial, result.AssemblyMethod);
            }
        }

        private void ResetTestResults()
        {
            boi0A2TestResult = null;
            boi0B1TestResult = null;
            boi0C1TestResult = null;
            boi0C2TestResult = null;
        }
    }
}