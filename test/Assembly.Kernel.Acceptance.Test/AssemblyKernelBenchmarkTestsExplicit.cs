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
    public class AssemblyKernelBenchmarkTestsExplicit
    {
        [Test]
        [Explicit("Run only local")]
        public void RunBenchmarkTest()
        {
            string testDirectory = Path.Combine(BenchmarkTestHelper.GetTestDataPath("Assembly.Kernel.Acceptance.Test"),
                                                "definitions");
            string fileName = Directory.GetFiles(testDirectory, "*traject 30-4*.xlsx").First();
            string testName = BenchmarkTestHelper.GetTestName(fileName);

            BenchmarkTestInput input = AssemblyExcelFileReader.Read(fileName);
            var testResult = new BenchmarkTestResult(fileName, testName);

            BenchmarkTestRunner.TestEqualNormCategories(input, testResult);
            BenchmarkTestRunner.TestEqualInterpretationCategories(input, testResult);

            foreach (ExpectedFailureMechanismResult expectedFailureMechanismResult in input.ExpectedFailureMechanismsResults)
            {
                BenchmarkTestRunner.TestFailureMechanismAssembly(
                    expectedFailureMechanismResult, testResult, input.ExpectedInterpretationCategories);
            }

            BenchmarkTestRunner.TestFinalVerdictAssembly(input, testResult);
            BenchmarkTestRunner.TestAssemblyOfCombinedSections(input, testResult);
        }
    }
}