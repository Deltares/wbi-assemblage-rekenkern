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

using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace assembly.kernel.benchmark.tests
{
    public class BenchmarkTestsBase
    {
        protected static string GetTestName(string testFileName)
        {
            var fileName = Path.GetFileNameWithoutExtension(testFileName);
            if (fileName == null)
            {
                Assert.Fail(testFileName);
            }

            var testNameNoPrefix = fileName.Replace("Benchmarktest_", "");
            var ind = testNameNoPrefix.IndexOf("_(v");
            if (ind > -1)
            {
                return testNameNoPrefix.Substring(0, ind);
            }

            return testNameNoPrefix;
        }

        protected static IEnumerable<string> AcquireAllBenchmarkTests()
        {
            var testDirectory = Path.Combine(GetBenchmarkTestsDirectory(), "testdefinitions");
            return Directory.GetFiles(testDirectory, "*.xlsm");
        }

        protected static string GetBenchmarkTestsDirectory()
        {
            return Path.Combine(GetSolutionRoot(), "benchmarktests");
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

        public static bool GetUpdatedMethodResult(bool? currentResult, bool newResult)
        {
            return currentResult == null
                ? newResult
                : (bool) currentResult && newResult;
        }

        
    }
}