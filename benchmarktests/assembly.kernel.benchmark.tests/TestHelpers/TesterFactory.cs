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

using System.ComponentModel;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.data.Result;
using assembly.kernel.benchmark.tests.TestHelpers.Categories;
using assembly.kernel.benchmark.tests.TestHelpers.FailureMechanism;

namespace assembly.kernel.benchmark.tests.TestHelpers
{
    public static class TesterFactory
    {
        public static IFailureMechanismResultTester CreateFailureMechanismTester(MethodResultsListing testResult, IExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            switch (expectedFailureMechanismResult.Type)
            {
                case MechanismType.STBI:
                case MechanismType.STPH:
                case MechanismType.HTKW:
                case MechanismType.BSKW:
                    return new ProbabilisticFailureMechanismResultTester(testResult, expectedFailureMechanismResult);
                case MechanismType.STKWp:
                case MechanismType.GEKB:
                    return new Group1NoSimpleAssessmentFailureMechanismResultTester(testResult, expectedFailureMechanismResult);
                case MechanismType.AGK:
                case MechanismType.GEBU:
                    return new Group3FailureMechanismResultTester(testResult, expectedFailureMechanismResult);
                case MechanismType.ZST:
                case MechanismType.DA:
                    return new Group3NoSimpleAssessmentFailureMechanismTester(testResult, expectedFailureMechanismResult);
                case MechanismType.GABI:
                case MechanismType.GABU:
                case MechanismType.STMI:
                case MechanismType.PKW:
                    return new Group4FailureMechanismTester(testResult, expectedFailureMechanismResult);
                case MechanismType.AWO:
                case MechanismType.STKWl:
                case MechanismType.INN:
                    return new Group4NoDetailedAssessmentFailureMechanismTester(testResult, expectedFailureMechanismResult);
                case MechanismType.STBU:
                    return new StbuFailureMechanismTester(testResult, expectedFailureMechanismResult);
                case MechanismType.HAV:
                case MechanismType.NWOkl:
                case MechanismType.VLZV:
                case MechanismType.VLAF:
                    return new Group5FailureMechanismTester(testResult, expectedFailureMechanismResult);
                case MechanismType.NWOoc:
                    return new NwOocFailureMechanismTester(testResult, expectedFailureMechanismResult);
                case MechanismType.NWObe:
                case MechanismType.NWObo:
                case MechanismType.VLGA:
                    return new Group5NoDetailedAssessmentFailureMechanismTester(testResult, expectedFailureMechanismResult);
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        public static ICategoriesTester CreateCategoriesTester(MethodResultsListing result, IExpectedFailureMechanismResult expectedFailureMechanismResult, double lowerBoundaryNorm, double signallingNorm)
        {
            switch (expectedFailureMechanismResult.Type)
            {
                case MechanismType.STBI:
                case MechanismType.STPH:
                case MechanismType.HTKW:
                case MechanismType.BSKW:
                case MechanismType.STKWp:
                case MechanismType.GEKB:
                    return new ProbabilisticFailureMechanismCategoriesTester(result, expectedFailureMechanismResult, lowerBoundaryNorm, signallingNorm);
                case MechanismType.AGK:
                case MechanismType.GEBU:
                case MechanismType.ZST:
                case MechanismType.DA:
                    return new Group3FailureMechanismCategoriesTester(result, expectedFailureMechanismResult, lowerBoundaryNorm, signallingNorm);
                case MechanismType.STBU:
                    return new STBUCategoriesTester(result, expectedFailureMechanismResult, signallingNorm, lowerBoundaryNorm);
                default:
                    return null;
            }
        }
    }
}
