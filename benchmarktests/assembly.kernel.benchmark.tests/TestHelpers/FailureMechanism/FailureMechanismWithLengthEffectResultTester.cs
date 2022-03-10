using System;
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
    public class FailureMechanismWithLengthEffectResultTester : FailureMechanismResultTesterBase
    {
        public FailureMechanismWithLengthEffectResultTester(MethodResultsListing methodResults, ExpectedFailureMechanismResult expectedFailureMechanismResult, CategoriesList<InterpretationCategory> interpretationCategories) : base(methodResults, expectedFailureMechanismResult, interpretationCategories)
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
                foreach (var section in ExpectedFailureMechanismResult.Sections.OfType<ExpectedFailureMechanismSectionWithLengthEffect>())
                {
                    // WBI-0A-1 (direct with probability)
                    var result = assembler.TranslateAssessmentResultWbi0A2(
                        section.IsRelevant
                            ? double.IsNaN(section.InitialMechanismProbabilitySection) || double.IsNaN(section.InitialMechanismProbabilityProfile)
                                ? ESectionInitialMechanismProbabilitySpecification.RelevantNoProbabilitySpecification
                                : ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification
                            : ESectionInitialMechanismProbabilitySpecification.NotRelevant,
                        section.InitialMechanismProbabilityProfile,
                        section.InitialMechanismProbabilitySection,
                        section.RefinementStatus,
                        section.RefinedProbabilityProfile,
                        section.RefinedProbabilitySection,
                        InterpretationCategories);

                    try
                    {
                        AssertHelper.AssertAreEqualProbabilities(section.ExpectedCombinedProbabilitySection, result.ProbabilitySection);
                        Assert.AreEqual(section.ExpectedInterpretationCategory, result.InterpretationCategory);
                    }
                    catch (AssertionException e)
                    {
                        exception.Data.Add(section.SectionName, e);
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
                        ExpectedFailureMechanismResult.Sections.OfType<ExpectedFailureMechanismSectionWithLengthEffect>()
                            .Select(s =>
                                new FailureMechanismSectionAssemblyResult(s.ExpectedCombinedProbabilityProfile,
                                    s.ExpectedCombinedProbabilitySection,
                                    s.ExpectedInterpretationCategory)).ToArray(),
                        false);
                }
                catch (AssemblyException e)
                {
                    result = new FailureMechanismAssemblyResult(Probability.NaN, EFailureMechanismAssemblyMethod.Correlated);
                }

                AssertHelper.AssertAreEqualProbabilities(ExpectedFailureMechanismResult.ExpectedCombinedProbability, result.Probability);
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
                    ExpectedFailureMechanismResult.Sections.OfType<ExpectedFailureMechanismSectionWithLengthEffect>()
                        .Select(s =>
                            new FailureMechanismSectionAssemblyResult(s.ExpectedCombinedProbabilityProfile,
                                s.ExpectedCombinedProbabilitySection,
                                s.ExpectedInterpretationCategory)).ToArray(),
                    true);

                AssertHelper.AssertAreEqualProbabilities(ExpectedFailureMechanismResult.ExpectedCombinedProbabilityTemporal, result.Probability);
                Assert.AreEqual(ExpectedFailureMechanismResult.ExpectedIsSectionsCorrelatedTemporal, result.AssemblyMethod);
            }
        }
    }
}
