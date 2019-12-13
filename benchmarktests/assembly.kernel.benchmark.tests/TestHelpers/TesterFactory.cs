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
    /// <summary>
    /// Factory to create instances of testers.
    /// </summary>
    public static class TesterFactory
    {
        /// <summary>
        /// Creates instances of failure mechanism testers.
        /// </summary>
        /// <param name="methodResults">The method results.</param>
        /// <param name="expectedFailureMechanismResult">The expected failure mechanism results.</param>
        /// <returns>An instance of <see cref="IFailureMechanismResultTester"/>.</returns>
        public static IFailureMechanismResultTester CreateFailureMechanismTester(MethodResultsListing methodResults,
                                                                                 IExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            switch (expectedFailureMechanismResult.Type)
            {
                case MechanismType.STBI:
                case MechanismType.STPH:
                case MechanismType.HTKW:
                case MechanismType.BSKW:
                    return new ProbabilisticFailureMechanismResultTester(methodResults, expectedFailureMechanismResult);
                case MechanismType.STKWp:
                case MechanismType.GEKB:
                    return new Group1NoSimpleAssessmentFailureMechanismResultTester(methodResults, expectedFailureMechanismResult);
                case MechanismType.AGK:
                case MechanismType.GEBU:
                    return new Group3FailureMechanismResultTester(methodResults, expectedFailureMechanismResult);
                case MechanismType.ZST:
                case MechanismType.DA:
                    return new Group3NoSimpleAssessmentFailureMechanismTester(methodResults, expectedFailureMechanismResult);
                case MechanismType.GABI:
                case MechanismType.GABU:
                case MechanismType.STMI:
                case MechanismType.PKW:
                    return new Group4FailureMechanismResultTester(methodResults, expectedFailureMechanismResult);
                case MechanismType.AWO:
                case MechanismType.STKWl:
                case MechanismType.INN:
                    return new Group4NoDetailedAssessmentFailureMechanismResultTester(methodResults, expectedFailureMechanismResult);
                case MechanismType.STBU:
                    return new StbuFailureMechanismResultTester(methodResults, expectedFailureMechanismResult);
                case MechanismType.HAV:
                case MechanismType.NWOkl:
                case MechanismType.VLZV:
                case MechanismType.VLAF:
                    return new Group5FailureMechanismResultTester(methodResults, expectedFailureMechanismResult);
                case MechanismType.NWOoc:
                    return new NwOocFailureMechanismResultTester(methodResults, expectedFailureMechanismResult);
                case MechanismType.NWObe:
                case MechanismType.NWObo:
                case MechanismType.VLGA:
                    return new Group5NoDetailedAssessmentFailureMechanismResultTester(methodResults, expectedFailureMechanismResult);
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        /// <summary>
        /// Creates instances of <see cref="ICategoriesTester"/>.
        /// </summary>
        /// <param name="methodResults">The method results.</param>
        /// <param name="expectedFailureMechanismResult">The expected failure mechanism result.</param>
        /// <param name="lowerBoundaryNorm">The lower boundary norm.</param>
        /// <param name="signallingNorm">The signalling norm.</param>
        public static ICategoriesTester CreateCategoriesTester(MethodResultsListing methodResults,
                                                               IExpectedFailureMechanismResult expectedFailureMechanismResult,
                                                               double lowerBoundaryNorm, double signallingNorm)
        {
            switch (expectedFailureMechanismResult.Type)
            {
                case MechanismType.STBI:
                case MechanismType.STPH:
                case MechanismType.HTKW:
                case MechanismType.BSKW:
                case MechanismType.STKWp:
                case MechanismType.GEKB:
                    return new ProbabilisticFailureMechanismCategoriesTester(methodResults, expectedFailureMechanismResult, lowerBoundaryNorm, signallingNorm);
                case MechanismType.AGK:
                case MechanismType.GEBU:
                case MechanismType.ZST:
                case MechanismType.DA:
                    return new Group3FailureMechanismCategoriesTester(methodResults, expectedFailureMechanismResult, lowerBoundaryNorm, signallingNorm);
                case MechanismType.STBU:
                    return new STBUCategoriesTester(methodResults, expectedFailureMechanismResult, signallingNorm, lowerBoundaryNorm);
                default:
                    return null;
            }
        }
    }
}
