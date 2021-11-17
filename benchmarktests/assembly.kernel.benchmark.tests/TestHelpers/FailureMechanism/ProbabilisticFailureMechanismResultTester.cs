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