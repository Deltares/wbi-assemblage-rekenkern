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
            // TODO: Implement
            return expectedFailureMechanismResult.HasLengthEffect
                ? new StbuFailureMechanismResultTester(methodResults, expectedFailureMechanismResult)
                : new StbuFailureMechanismResultTester(methodResults, expectedFailureMechanismResult);
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
            return new STBUCategoriesTester(methodResults, expectedFailureMechanismResult, signallingNorm,
                lowerBoundaryNorm);
        }
    }
}