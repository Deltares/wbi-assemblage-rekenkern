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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assembly.Kernel.Acceptance.Test.TestHelpers;
using NUnit.Framework;

namespace Assembly.Kernel.Acceptance.Test
{
    /// <summary>
    /// Factory for creating benchmark test cases.
    /// </summary>
    public class BenchmarkTestCaseFactory
    {
        /// <summary>
        /// Gets all benchmark test cases.
        /// </summary>
        public static IEnumerable<TestCaseData> GetBenchmarkTestCases()
        {
            string testDirectory = Path.Combine(BenchmarkTestHelper.GetBenchmarkTestsDirectory(), "testdefinitions");
            string[] benchmarkTestFiles = Directory.GetFiles(testDirectory, "*.xlsx");
            
            return benchmarkTestFiles.Select(t => new TestCaseData(BenchmarkTestHelper.GetTestName(t), t)
            {
                TestName = BenchmarkTestHelper.GetTestName(t)
            });
        }
    }
}