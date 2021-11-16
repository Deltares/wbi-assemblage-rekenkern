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
    /// <summary>
    /// Result tester for failure mechanisms in group 5 without detailed assessment.
    /// </summary>
    public class Group5NoDetailedAssessmentFailureMechanismResultTester : FailureMechanismResultTesterBase<Group4Or5ExpectedFailureMechanismResult>
    {
        /// <summary>
        /// Creates a new instance of <see cref="Group5NoDetailedAssessmentFailureMechanismResultTester"/>.
        /// </summary>
        /// <param name="methodResults">The method results.</param>
        /// <param name="expectedFailureMechanismResult">The expected failure mechanism results.</param>
        public Group5NoDetailedAssessmentFailureMechanismResultTester(MethodResultsListing methodResults,
                                                                      IExpectedFailureMechanismResult
                                                                          expectedFailureMechanismResult)
            : base(methodResults, expectedFailureMechanismResult) {}

        public override bool? TestDetailedAssessment()
        {
            return null;
        }

        protected override void TestTailorMadeAssessmentInternal()
        {
            var assembler = new AssessmentResultsTranslator();

            foreach (var section in ExpectedFailureMechanismResult.Sections)
            {
                var group5NoDetailedAssessmentFailureMechanismSection =
                    section as Group5NoDetailedAssessmentFailureMechanismSection;
                if (group5NoDetailedAssessmentFailureMechanismSection != null)
                {
                    // WBI-0T-2
                    FmSectionAssemblyIndirectResult result =
                        assembler.TranslateAssessmentResultWbi0T2(
                            group5NoDetailedAssessmentFailureMechanismSection.TailorMadeAssessmentResult);

                    var expectedResult =
                        group5NoDetailedAssessmentFailureMechanismSection.ExpectedTailorMadeAssessmentAssemblyResult as
                            FmSectionAssemblyIndirectResult;
                    Assert.AreEqual(expectedResult.Result, result.Result);
                }
            }
        }

        protected override void TestCombinedAssessmentInternal()
        {
            var assembler = new AssessmentResultsTranslator();

            if (ExpectedFailureMechanismResult != null)
            {
                foreach (var section in ExpectedFailureMechanismResult
                                        .Sections.OfType<Group5NoDetailedAssessmentFailureMechanismSection>())
                {
                    // WBI-0A-1 (direct with probability)
                    var result = assembler.TranslateAssessmentResultWbi0A1(
                        section.ExpectedDetailedAssessmentAssemblyResult as FmSectionAssemblyIndirectResult,
                        section.ExpectedTailorMadeAssessmentAssemblyResult as FmSectionAssemblyIndirectResult);

                    Assert.IsInstanceOf<FmSectionAssemblyIndirectResult>(result);
                    Assert.AreEqual(section.ExpectedCombinedResult, result.Result);
                }
            }
        }

        protected override void TestAssessmentSectionResultInternal()
        {
            var assembler = new FailureMechanismResultAssembler();

            // WBI-1A-2
            var result = assembler.AssembleFailureMechanismWbi1A2(
                ExpectedFailureMechanismResult.Sections.Select(CreateFmSectionAssemblyIndirectResult),
                false
            );

            Assert.AreEqual(ExpectedFailureMechanismResult.ExpectedAssessmentResult, result);
        }

        protected override void TestAssessmentSectionResultTemporalInternal()
        {
            var assembler = new FailureMechanismResultAssembler();

            // WBI-1A-2
            var result = assembler.AssembleFailureMechanismWbi1A2(
                ExpectedFailureMechanismResult.Sections.Select(CreateFmSectionAssemblyIndirectResult),
                true
            );

            Assert.AreEqual(ExpectedFailureMechanismResult.ExpectedAssessmentResultTemporal, result);
        }

        protected override void SetTailorMadeAssessmentMethodResult(bool result)
        {
            MethodResults.Wbi0T2 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Wbi0T2, result);
        }

        protected override void SetCombinedAssessmentMethodResult(bool result)
        {
            MethodResults.Wbi0A1 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Wbi0A1, result);
        }

        protected override void SetAssessmentSectionMethodResult(bool result)
        {
            MethodResults.Wbi1A2 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Wbi1A2, result);
        }

        protected override void SetAssessmentSectionMethodResultTemporal(bool result)
        {
            MethodResults.Wbi1A2T = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Wbi1A2T, result);
        }

        private FmSectionAssemblyIndirectResult CreateFmSectionAssemblyIndirectResult(IFailureMechanismSection section)
        {
            var directMechanismSection = section as FailureMechanismSectionBase<EIndirectAssessmentResult>;
            return new FmSectionAssemblyIndirectResult(directMechanismSection.ExpectedCombinedResult);
        }
    }
}