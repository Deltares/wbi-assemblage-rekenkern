﻿// Copyright (C) Stichting Deltares and State of the Netherlands 2023. All rights reserved.
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
// All names, logos, and references to "Deltares" are registered trademarks of
// Stichting Deltares and remain full property of Stichting Deltares at all times.
// All rights reserved.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assembly.Kernel.Acceptance.TestUtil;
using Assembly.Kernel.Acceptance.TestUtil.Data.Input;
using Assembly.Kernel.Acceptance.TestUtil.Data.Input.FailureMechanisms;
using Assembly.Kernel.Acceptance.TestUtil.Data.Result;
using Assembly.Kernel.Acceptance.TestUtil.IO;
using NUnit.Framework;

namespace Assembly.Kernel.Acceptance.Test
{
    [TestFixture]
    public class AssemblyKernelBenchmarkTests
    {
        private const string nameOfSummaryTex = "Summary.tex";
        private string reportDirectory;
        private string summaryTargetFileName;
        private IDictionary<string, BenchmarkTestResult> testResults;

        [Test]
        [TestCaseSource(typeof(BenchmarkTestCaseFactory), nameof(BenchmarkTestCaseFactory.GetBenchmarkTestCases))]
        public void RunBenchmarkTest(string testName, string fileName)
        {
            BenchmarkTestInput input = AssemblyExcelFileReader.Read(fileName);
            var testResult = new BenchmarkTestResult(fileName, testName);

            BenchmarkTestRunner.TestEqualNormCategories(input, testResult);
            BenchmarkTestRunner.TestEqualInterpretationCategories(input, testResult);

            foreach (ExpectedFailureMechanismResult expectedFailureMechanismResult in input.ExpectedFailureMechanismsResults)
            {
                BenchmarkTestRunner.TestFailureMechanismAssembly(expectedFailureMechanismResult, testResult,
                                                                 input.ExpectedInterpretationCategories);
            }

            BenchmarkTestRunner.TestFinalVerdictAssembly(input, testResult);
            BenchmarkTestRunner.TestAssemblyOfCombinedSections(input, testResult);

            testResults.Add(testName, testResult);
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            reportDirectory = Path.Combine(BenchmarkTestHelper.GetTestDataPath("Assembly.Kernel.Acceptance.Test"), "results");
            summaryTargetFileName = Path.Combine(reportDirectory, nameOfSummaryTex);
            CreateOrCleanReportDirectory();

            testResults = new Dictionary<string, BenchmarkTestResult>();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            BenchmarkTestReportWriter.WriteReports(testResults.Select(tr => tr.Value), reportDirectory);
            BenchmarkTestReportWriter.WriteSummary(summaryTargetFileName, testResults);
        }

        private void CreateOrCleanReportDirectory()
        {
            if (!Directory.Exists(reportDirectory))
            {
                Directory.CreateDirectory(reportDirectory);
                return;
            }

            var directoryInfo = new DirectoryInfo(reportDirectory);

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}