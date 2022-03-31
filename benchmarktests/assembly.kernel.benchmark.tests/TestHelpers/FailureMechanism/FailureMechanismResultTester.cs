#region Copyright (C) Rijkswaterstaat 2022. All rights reserved

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
    public class FailureMechanismResultTester : FailureMechanismResultTesterBase
    {
        public FailureMechanismResultTester(MethodResultsListing methodResults,
            ExpectedFailureMechanismResult expectedFailureMechanismResult,
            CategoriesList<InterpretationCategory> interpretationCategories) : base(methodResults, expectedFailureMechanismResult, interpretationCategories)
        {
        }

        protected override void SetCombinedAssessmentMethodResult(bool result)
        {
            MethodResults.StepZeroAggregationMethod = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.StepZeroAggregationMethod, result);
        }

        protected override void TestCombinedAssessmentInternal()
        {
            var assembler = new AssessmentResultsTranslator();

            if (ExpectedFailureMechanismResult != null)
            {
                var exception = new AssertionException("Errors occurred");
                foreach (var section in ExpectedFailureMechanismResult.Sections.OfType<ExpectedFailureMechanismSection>())
                {
                    var result = assembler.TranslateAssessmentResultAggregatedMethod(
                        section.IsRelevant
                            ? double.IsNaN(section.InitialMechanismProbabilitySection)
                                ? ESectionInitialMechanismProbabilitySpecification.RelevantNoProbabilitySpecification
                                : ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification
                            : ESectionInitialMechanismProbabilitySpecification.NotRelevant,
                        section.InitialMechanismProbabilitySection,
                        section.RefinementStatus,
                        section.RefinedProbabilitySection,
                        this.InterpretationCategories);

                    try
                    {
                        AssertHelper.AssertAreEqualProbabilities(section.ExpectedCombinedProbabilitySection, result.ProbabilitySection);
                        Assert.AreEqual(section.ExpectedInterpretationCategory, result.InterpretationCategory);
                    }
                    catch (AssertionException e)
                    {
                        exception.Data.Add(section.SectionName,e);
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
                        ExpectedFailureMechanismResult.Sections.OfType<ExpectedFailureMechanismSection>()
                            .Select(s =>
                                new FailureMechanismSectionAssemblyResult(s.ExpectedCombinedProbabilitySection,
                                    s.ExpectedCombinedProbabilitySection,
                                    s.ExpectedInterpretationCategory)).ToArray(),
                        false);
                }
                catch (AssemblyException e)
                {
                    result = new FailureMechanismAssemblyResult(Probability.Undefined, EFailureMechanismAssemblyMethod.Correlated);
                }

                AssertHelper.AssertAreEqualProbabilities(ExpectedFailureMechanismResult.ExpectedCombinedProbability, result.Probability);
                Assert.AreEqual(ExpectedFailureMechanismResult.ExpectedIsSectionsCorrelated, result.AssemblyMethod);
            }
        }

        protected override void SetAssessmentSectionMethodResultPartial(bool result)
        {
            MethodResults.Boi1A2 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Boi1A2, result);
        }

        protected override void TestAssessmentSectionResultPartialInternal()
        {
            var assembler = new FailureMechanismResultAssembler();

            if (ExpectedFailureMechanismResult != null)
            {
                var result = assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(
                    ExpectedFailureMechanismResult.LengthEffectFactor,
                    ExpectedFailureMechanismResult.Sections.OfType<ExpectedFailureMechanismSection>()
                        .Select(s =>
                            new FailureMechanismSectionAssemblyResult(s.ExpectedCombinedProbabilitySection,
                                s.ExpectedCombinedProbabilitySection,
                                s.ExpectedInterpretationCategory)).ToArray(),
                    true);

                AssertHelper.AssertAreEqualProbabilities(ExpectedFailureMechanismResult.ExpectedCombinedProbabilityPartial, result.Probability);
                Assert.AreEqual(ExpectedFailureMechanismResult.ExpectedIsSectionsCorrelatedTemporal, result.AssemblyMethod);
            }
        }
    }
}
