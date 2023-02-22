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

using System.Linq;
using Assembly.Kernel.Acceptance.Test.TestHelpers.Categories;
using assembly.kernel.benchmark.tests.data.Data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.data.Data.Input.FailureMechanismSections;
using assembly.kernel.benchmark.tests.data.Data.Result;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using NUnit.Framework;

namespace Assembly.Kernel.Acceptance.Test.TestHelpers.FailureMechanism
{
    /// <summary>
    /// Tester for failure mechanisms.
    /// </summary>
    public class FailureMechanismResultTester : FailureMechanismResultTesterBase
    {
        private bool? boi0A1TestResult;
        private bool? boi0B1TestResult;
        private bool? boi0C1TestResult;
        private bool? boi0C2TestResult;

        /// <inheritdoc />
        public FailureMechanismResultTester(MethodResultsListing methodResults,
                                            ExpectedFailureMechanismResult expectedFailureMechanismResult,
                                            CategoriesList<InterpretationCategory> interpretationCategories) 
            : base(methodResults, expectedFailureMechanismResult, interpretationCategories) {}

        protected override void SetCombinedAssessmentMethodResult(bool result)
        {
            MethodResults.Boi0A1 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Boi0A1, boi0A1TestResult);
            MethodResults.Boi0B1 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Boi0B1, boi0B1TestResult);
            MethodResults.Boi0C1 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Boi0C1, boi0C1TestResult);
            MethodResults.Boi0C2 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Boi0C2, boi0C2TestResult);
            ResetTestResults();
        }

        protected override void TestCombinedAssessmentInternal()
        {
            var assembler = new AssessmentResultsTranslator();
            ResetTestResults();

            if (ExpectedFailureMechanismResult != null)
            {
                var exception = new AssertionException("Errors occurred");

                foreach (ExpectedFailureMechanismSection section in ExpectedFailureMechanismResult.Sections.OfType<ExpectedFailureMechanismSection>())
                {
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
                    Probability probability;
                    EInterpretationCategory category;

                    EAnalysisState analysisState = GetAnalysisState(relevance, refinementStatus);
                    if (analysisState == EAnalysisState.ProbabilityEstimated)
                    {
                        probability = assembler.DetermineRepresentativeProbabilityBoi0A1(refinementStatus == ERefinementStatus.Performed,
                                                                                         section.InitialMechanismProbabilitySection,
                                                                                         section.RefinedProbabilitySection);
                        category =
                            assembler.DetermineInterpretationCategoryFromFailureMechanismSectionProbabilityBoi0B1(
                                probability, InterpretationCategories);
                    }
                    else
                    {
                        category = assembler.DetermineInterpretationCategoryWithoutProbabilityEstimationBoi0C1(analysisState);
                        probability = assembler.TranslateInterpretationCategoryToProbabilityBoi0C2(category);
                    }

                    try
                    {
                        AssertHelper.AssertAreEqualProbabilities(section.ExpectedCombinedProbabilitySection, probability);
                        Assert.AreEqual(section.ExpectedInterpretationCategory, category);
                        if (analysisState == EAnalysisState.ProbabilityEstimated)
                        {
                            boi0A1TestResult = BenchmarkTestHelper.GetUpdatedMethodResult(boi0A1TestResult, true);
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
                        exception.Data.Add(section.SectionName, e);
                        if (analysisState == EAnalysisState.ProbabilityEstimated)
                        {
                            boi0A1TestResult = BenchmarkTestHelper.GetUpdatedMethodResult(boi0A1TestResult, false);
                            boi0B1TestResult = BenchmarkTestHelper.GetUpdatedMethodResult(boi0B1TestResult, false);
                        }
                        else
                        {
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
            MethodResults.Boi1A1 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Boi1A1, result);
        }

        protected override void TestAssessmentSectionResultInternal()
        {
            var assembler = new FailureMechanismResultAssembler();

            if (ExpectedFailureMechanismResult != null)
            {
                FailureMechanismAssemblyResult result;
                try
                {
                    result = assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(
                        ExpectedFailureMechanismResult.LengthEffectFactor,
                        ExpectedFailureMechanismResult.Sections
                                                      .OfType<ExpectedFailureMechanismSection>()
                                                      .Select(s => s.ExpectedCombinedProbabilitySection).ToArray(),
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
            MethodResults.Boi1A1P = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Boi1A1P, result);
        }

        protected override void TestAssessmentSectionResultPartialInternal()
        {
            var assembler = new FailureMechanismResultAssembler();

            if (ExpectedFailureMechanismResult != null)
            {
                FailureMechanismAssemblyResult result = assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(
                    ExpectedFailureMechanismResult.LengthEffectFactor,
                    ExpectedFailureMechanismResult.Sections
                                                  .OfType<ExpectedFailureMechanismSection>()
                                                  .Select(s => s.ExpectedCombinedProbabilitySection).ToArray(),
                    true);

                AssertHelper.AssertAreEqualProbabilities(ExpectedFailureMechanismResult.ExpectedCombinedProbabilityPartial, result.Probability);
                Assert.AreEqual(ExpectedFailureMechanismResult.ExpectedIsSectionsCorrelatedPartial, result.AssemblyMethod);
            }
        }

        private void ResetTestResults()
        {
            boi0A1TestResult = null;
            boi0B1TestResult = null;
            boi0C1TestResult = null;
            boi0C2TestResult = null;
        }
    }
}