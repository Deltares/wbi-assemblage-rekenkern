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
using assembly.kernel.benchmark.tests.data.Input;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.data.Result;
using assembly.kernel.benchmark.tests.io;
using assembly.kernel.benchmark.tests.TestHelpers;
using NUnit.Framework;

namespace assembly.kernel.benchmark.tests
{
    [TestFixture]
    public class AssemblyKernelBenchmarkTestsExplicit
    {
        [Test, Explicit("Run only local")]
        public void RunBenchmarkTest()
        {
            var testDirectory = Path.Combine(BenchmarkTestHelper.GetBenchmarkTestsDirectory(), "testdefinitions");
            var fileName = Directory.GetFiles(testDirectory, "*traject 30-4*.xlsx").First();
            var testName = BenchmarkTestHelper.GetTestName(fileName);

            BenchmarkTestInput input = AssemblyExcelFileReader.Read(fileName);
            BenchmarkTestResult testResult = new BenchmarkTestResult(fileName, testName);

            BenchmarkTestRunner.TestEqualNormCategories(input, testResult);
            BenchmarkTestRunner.TestEqualInterpretationCategories(input, testResult);

            foreach (ExpectedFailureMechanismResult expectedFailureMechanismResult in input
                .ExpectedFailureMechanismsResults)
            {
                BenchmarkTestRunner.TestFailureMechanismAssembly(expectedFailureMechanismResult,
                    testResult,
                    input.ExpectedInterpretationCategories);
            }

            BenchmarkTestRunner.TestFinalVerdictAssembly(input, testResult);

            BenchmarkTestRunner.TestAssemblyOfCombinedSections(input, testResult);
        }
    }
}