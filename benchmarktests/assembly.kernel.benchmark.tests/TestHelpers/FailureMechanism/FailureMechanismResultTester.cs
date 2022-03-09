using System;
using System.Collections.Generic;
using System.Linq;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;
using assembly.kernel.benchmark.tests.data.Result;
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
            MethodResults.Wbi0A2 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Wbi0A2, result);
        }

        protected override void TestCombinedAssessmentInternal()
        {
            var assembler = new AssessmentResultsTranslator();

            if (ExpectedFailureMechanismResult != null)
            {
                var exception = new AssertionException("Errors occurred");
                foreach (var section in ExpectedFailureMechanismResult.Sections.OfType<ExpectedFailureMechanismSection>())
                {
                    // WBI-0A-1 (direct with probability)
                    var result = assembler.TranslateAssessmentResultWbi0A2(
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
                        Assert.AreEqual(section.ExpectedCombinedProbabilitySection.Value, result.ProbabilitySection.Value);
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
            MethodResults.Wbi1B1 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Wbi1B1, result);
        }

        protected override void TestAssessmentSectionResultInternal()
        {
            var assembler = new FailureMechanismResultAssembler();

            if (ExpectedFailureMechanismResult != null)
            {
                FailureMechanismAssemblyResult result = null;
                try
                {
                    result = assembler.AssembleFailureMechanismWbi1B1(
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
                    result = new FailureMechanismAssemblyResult(Probability.NaN, EFailureMechanismAssemblyMethod.Correlated);
                }
                
                Assert.AreEqual(ExpectedFailureMechanismResult.ExpectedCombinedProbability.Value, result.Probability.Value);
                Assert.AreEqual(ExpectedFailureMechanismResult.ExpectedIsSectionsCorrelated, result.AssemblyMethod);
            }
        }

        protected override void SetAssessmentSectionMethodResultTemporal(bool result)
        {
            MethodResults.Wbi1B1 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Wbi1B1, result);
        }

        protected override void TestAssessmentSectionResultTemporalInternal()
        {
            var assembler = new FailureMechanismResultAssembler();

            if (ExpectedFailureMechanismResult != null)
            {
                var result = assembler.AssembleFailureMechanismWbi1B1(
                    ExpectedFailureMechanismResult.LengthEffectFactor,
                    ExpectedFailureMechanismResult.Sections.OfType<ExpectedFailureMechanismSection>()
                        .Select(s =>
                            new FailureMechanismSectionAssemblyResult(s.ExpectedCombinedProbabilitySection,
                                s.ExpectedCombinedProbabilitySection,
                                s.ExpectedInterpretationCategory)).ToArray(),
                    true);

                Assert.AreEqual(ExpectedFailureMechanismResult.ExpectedCombinedProbabilityTemporal.Value, result.Probability.Value);
                Assert.AreEqual(ExpectedFailureMechanismResult.ExpectedIsSectionsCorrelatedTemporal, result.AssemblyMethod);
            }
        }
    }
}
