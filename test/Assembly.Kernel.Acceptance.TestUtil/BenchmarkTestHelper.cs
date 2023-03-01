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

using System;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Assembly.Kernel.Acceptance.TestUtil
{
    /// <summary>
    /// Helper methods for benchmark tests.
    /// </summary>
    public static class BenchmarkTestHelper
    {
        /// <summary>
        /// Gets the updated method result.
        /// </summary>
        /// <param name="currentResult">The current result.</param>
        /// <param name="newResult">The new result.</param>
        /// <returns>The updated result.</returns>
        public static bool? GetUpdatedMethodResult(bool? currentResult, bool? newResult)
        {
            if (!newResult.HasValue)
            {
                return currentResult;
            }

            if (currentResult.HasValue)
            {
                return currentResult.Value && newResult.Value;
            }

            return newResult;
        }

        /// <summary>
        /// Gets the test name for the file name.
        /// </summary>
        /// <param name="testFileName">The file name.</param>
        /// <returns>The test name.</returns>
        public static string GetTestName(string testFileName)
        {
            string fileName = Path.GetFileNameWithoutExtension(testFileName);
            if (fileName == null)
            {
                Assert.Fail(testFileName);
            }

            string testNameNoPrefix = fileName.Replace("Benchmarktest_", "");
            int ind = testNameNoPrefix.IndexOf("_(v");
            return ind > -1 ? testNameNoPrefix.Substring(0, ind) : testNameNoPrefix;
        }

        /// <summary>
        /// Gets the test data path.
        /// </summary>
        /// <returns>The test data path.</returns>
        public static string GetTestDataPath(string projectName)
        {
            return Path.Combine(GetSolutionRoot(), "test", projectName, "test-data");
        }

        private static string GetSolutionRoot()
        {
            const string solutionName = "Assembly.sln";
            var testContext = new TestContext(new TestExecutionContext.AdhocContext());
            string curDir = testContext.TestDirectory;
            while (Directory.Exists(curDir) && !File.Exists(curDir + @"\" + solutionName))
            {
                curDir += "/../";
            }

            if (!File.Exists(Path.Combine(curDir, solutionName)))
            {
                throw new InvalidOperationException(
                    $"Solution file '{solutionName}' not found in any folder of '{Directory.GetCurrentDirectory()}'.");
            }

            return Path.GetFullPath(curDir);
        }
    }
}