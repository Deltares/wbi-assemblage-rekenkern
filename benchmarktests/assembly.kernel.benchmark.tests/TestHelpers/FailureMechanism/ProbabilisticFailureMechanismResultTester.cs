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

using System.Linq;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;
using assembly.kernel.benchmark.tests.data.Result;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FmSectionTypes;
using NUnit.Framework;

namespace assembly.kernel.benchmark.tests.TestHelpers.FailureMechanism
{
    public class ProbabilisticFailureMechanismResultTester : FailureMechanismResultTesterBase<ProbabilisticExpectedFailureMechanismResult>
    {
        public ProbabilisticFailureMechanismResultTester(MethodResultsListing methodResults,
                                                         IExpectedFailureMechanismResult expectedFailureMechanismResult) : base(
            methodResults, expectedFailureMechanismResult) {}

        protected override void TestDetailedAssessmentInternal()
        {
            var assembler = new AssessmentResultsTranslator();

            foreach (var section in ExpectedFailureMechanismResult.Sections)
            {
                var probabilisticSection = section as ProbabilisticFailureMechanismSection;
                if (probabilisticSection != null)
                {
                    FmSectionAssemblyDirectResultWithProbability result;
                    if (ExpectedFailureMechanismResult.Type == MechanismType.STBI ||
                        ExpectedFailureMechanismResult.Type == MechanismType.STPH)
                    {
                        // WBI-0G-5
                        result = assembler.TranslateAssessmentResultWbi0G5(probabilisticSection.LengthEffectFactor,
                                                                           probabilisticSection.DetailedAssessmentResult,
                                                                           probabilisticSection
                                                                               .DetailedAssessmentResultProbability,
                                                                           ExpectedFailureMechanismResult
                                                                               .ExpectedFailureMechanismSectionCategories);
                    }
                    else
                    {
                        // WBI-0G-3
                        result = assembler.TranslateAssessmentResultWbi0G3(
                            probabilisticSection.DetailedAssessmentResult,
                            probabilisticSection.DetailedAssessmentResultProbability,
                            ExpectedFailureMechanismResult.ExpectedFailureMechanismSectionCategories);
                    }

                    var expectedResult =
                        probabilisticSection.ExpectedDetailedAssessmentAssemblyResult as
                            FmSectionAssemblyDirectResultWithProbability;
                    Assert.AreEqual(expectedResult.Result, result.Result);
                    Assert.AreEqual(expectedResult.FailureProbability, result.FailureProbability);
                }
            }
        }

        protected override void TestTailorMadeAssessmentInternal()
        {
            var assembler = new AssessmentResultsTranslator();

            foreach (var section in ExpectedFailureMechanismResult.Sections)
            {
                var probabilisticSection = section as ProbabilisticFailureMechanismSection;
                if (probabilisticSection != null)
                {
                    FmSectionAssemblyDirectResultWithProbability result;
                    if (ExpectedFailureMechanismResult.Type == MechanismType.STBI ||
                        ExpectedFailureMechanismResult.Type == MechanismType.STPH)
                    {
                        // WBI-0T-5
                        result = assembler.TranslateAssessmentResultWbi0T5(probabilisticSection.LengthEffectFactor,
                                                                           probabilisticSection.TailorMadeAssessmentResult,
                                                                           probabilisticSection
                                                                               .TailorMadeAssessmentResultProbability,
                                                                           ExpectedFailureMechanismResult
                                                                               .ExpectedFailureMechanismSectionCategories);
                    }
                    else
                    {
                        // WBI-0T-3
                        result = assembler.TranslateAssessmentResultWbi0T3(
                            probabilisticSection.TailorMadeAssessmentResult,
                            probabilisticSection.TailorMadeAssessmentResultProbability,
                            ExpectedFailureMechanismResult.ExpectedFailureMechanismSectionCategories);
                    }

                    var expectedResult =
                        probabilisticSection.ExpectedTailorMadeAssessmentAssemblyResult as
                            FmSectionAssemblyDirectResultWithProbability;
                    Assert.AreEqual(expectedResult.Result, result.Result);
                    Assert.AreEqual(expectedResult.FailureProbability, result.FailureProbability);
                }
            }
        }

        protected override void TestCombinedAssessmentInternal()
        {
            var assembler = new AssessmentResultsTranslator();

            if (ExpectedFailureMechanismResult != null)
            {
                foreach (var section in ExpectedFailureMechanismResult.Sections.OfType<IProbabilisticFailureMechanismSection>())
                {
                    // WBI-0A-1 (direct with probability)
                    var result = assembler.TranslateAssessmentResultWbi0A1(
                        section.ExpectedDetailedAssessmentAssemblyResult as
                            FmSectionAssemblyDirectResultWithProbability,
                        section.ExpectedTailorMadeAssessmentAssemblyResult as
                            FmSectionAssemblyDirectResultWithProbability);

                    Assert.IsInstanceOf<FmSectionAssemblyDirectResultWithProbability>(result);
                    Assert.AreEqual(section.ExpectedCombinedResult, result.Result);
                    Assert.AreEqual(section.ExpectedCombinedResultProbability, result.FailureProbability);
                }
            }
        }

        protected override void TestAssessmentSectionResultInternal()
        {
            var assembler = new FailureMechanismResultAssembler();

            // WBI-1B-1
            FailureMechanismAssemblyResult result = assembler.AssembleFailureMechanismWbi1B1(
                new Assembly.Kernel.Model.FailureMechanism(ExpectedFailureMechanismResult.LengthEffectFactor,
                                                           ExpectedFailureMechanismResult.FailureMechanismProbabilitySpace),
                ExpectedFailureMechanismResult.Sections.Select(CreateFmSectionAssemblyDirectResultWithProbabilities),
                ExpectedFailureMechanismResult.ExpectedFailureMechanismCategories,
                false
            );

            Assert.AreEqual(ExpectedFailureMechanismResult.ExpectedAssessmentResult, result.Category);
            Assert.AreEqual(ExpectedFailureMechanismResult.ExpectedAssessmentResultProbability, result.FailureProbability);
        }

        protected override void TestAssessmentSectionResultTemporalInternal()
        {
            var assembler = new FailureMechanismResultAssembler();

            // WBI-1B-1
            FailureMechanismAssemblyResult result = assembler.AssembleFailureMechanismWbi1B1(
                new Assembly.Kernel.Model.FailureMechanism(ExpectedFailureMechanismResult.LengthEffectFactor,
                                                           ExpectedFailureMechanismResult.FailureMechanismProbabilitySpace),
                ExpectedFailureMechanismResult.Sections.Select(CreateFmSectionAssemblyDirectResultWithProbabilities),
                ExpectedFailureMechanismResult.ExpectedFailureMechanismCategories,
                true
            );

            Assert.AreEqual(ExpectedFailureMechanismResult.ExpectedAssessmentResultTemporal, result.Category);
            Assert.AreEqual(ExpectedFailureMechanismResult.ExpectedAssessmentResultProbabilityTemporal,
                            result.FailureProbability);
        }

        protected override void SetDetailedAssessmentMethodResult(bool result)
        {
            switch (ExpectedFailureMechanismResult.Type)
            {
                case MechanismType.STBI:
                case MechanismType.STPH:
                    MethodResults.Wbi0G5 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Wbi0G5, result);
                    break;
                default:
                    MethodResults.Wbi0G3 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Wbi0G3, result);
                    break;
            }
        }

        protected override void SetTailorMadeAssessmentMethodResult(bool result)
        {
            switch (ExpectedFailureMechanismResult.Type)
            {
                case MechanismType.STBI:
                case MechanismType.STPH:
                    MethodResults.Wbi0T5 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Wbi0T5, result);
                    break;
                default:
                    MethodResults.Wbi0T3 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Wbi0T3, result);
                    break;
            }
        }

        protected override void SetCombinedAssessmentMethodResult(bool result)
        {
            MethodResults.Wbi0A1 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Wbi0A1, result);
        }

        protected override void SetAssessmentSectionMethodResult(bool result)
        {
            MethodResults.Wbi1B1 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Wbi1B1, result);
        }

        protected override void SetAssessmentSectionMethodResultTemporal(bool result)
        {
            MethodResults.Wbi1B1T = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Wbi1B1T, result);
        }

        private FmSectionAssemblyDirectResultWithProbabilities CreateFmSectionAssemblyDirectResultWithProbabilities(
            IFailureMechanismSection section)
        {
            var directMechanismSection = section as FailureMechanismSectionBase<EFmSectionCategory>;
            var probabilisticMechanismSection = section as IProbabilisticFailureMechanismSection;
            return new FmSectionAssemblyDirectResultWithProbabilities(directMechanismSection.ExpectedCombinedResult,
                probabilisticMechanismSection
                    .ExpectedCombinedResultProbability, probabilisticMechanismSection
                    .ExpectedCombinedResultProbability);
        }
    }
}