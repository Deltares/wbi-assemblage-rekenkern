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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using assembly.kernel.benchmark.tests.data.Input;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.data.Result;
using assembly.kernel.benchmark.tests.io;
using assembly.kernel.benchmark.tests.TestHelpers;
using NUnit.Framework;

namespace assembly.kernel.benchmark.tests
{
    [TestFixture]
    public class AssemblyKernelBenchmarkTests
    {
        private string reportDirectory;
        private Dictionary<string, BenchmarkTestResult> testResults;

        [Test, TestCaseSource(typeof(BenchmarkTestCaseFactory), nameof(BenchmarkTestCaseFactory.BenchmarkTestCases))]
        [Ignore("These tests can only be updated after several issues are implemented. Expected results are not correct anymore since it is not possible to adjust the benchmark test.")]
        public void RunBenchmarkTest(string testName, string fileName)
        {
            BenchmarkTestInput input = AssemblyExcelFileReader.Read(fileName, testName);
            BenchmarkTestResult testResult = new BenchmarkTestResult(fileName, testName);

            BenchmarkTestRunner.TestEqualNormCategories(input, testResult);

            foreach (ExpectedFailureMechanismResult expectedFailureMechanismResult in input
                .ExpectedFailureMechanismsResults)
            {
                BenchmarkTestRunner.TestFailureMechanismAssembly(expectedFailureMechanismResult,
                                                                 input.LowerBoundaryNorm,
                                                                 input.SignallingNorm, testResult);
            }

            BenchmarkTestRunner.TestFinalVerdictAssembly(input, testResult);

            BenchmarkTestRunner.TestAssemblyOfCombinedSections(input, testResult);

            testResults[testName] = testResult;
        }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            // Clear results directory
            reportDirectory = PrepareReportDirectory();

            // initialize testresults
            testResults = new Dictionary<string, BenchmarkTestResult>();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // Report all testresults into a LaTeX file
            for (int i = 0; i < testResults.Count; i++)
            {
                BenchmarkTestReportWriter.WriteReport(i, testResults.ElementAt(i).Value, reportDirectory);
            }

            BenchmarkTestReportWriter.WriteSummary(Path.Combine(reportDirectory, "Summary.tex"), testResults);
        }

        private static string PrepareReportDirectory()
        {
            var reportDirectory = Path.Combine(BenchmarkTestHelper.GetBenchmarkTestsDirectory(), "testresults");
            if (Directory.Exists(reportDirectory))
            {
                var di = new DirectoryInfo(reportDirectory);

                foreach (FileInfo file in di.GetFiles().Where(name => !name.Name.EndsWith(".gitignore")))
                {
                    file.Delete();
                }

                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }

            Directory.CreateDirectory(reportDirectory);
            return reportDirectory;
        }
    }
}