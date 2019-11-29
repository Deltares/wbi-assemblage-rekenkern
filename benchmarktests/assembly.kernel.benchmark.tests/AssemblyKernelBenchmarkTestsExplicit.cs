using System.IO;
using System.Linq;
using assembly.kernel.benchmark.tests.data.Input;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.data.Result;
using assembly.kernel.benchmark.tests.io;
using NUnit.Framework;

namespace assembly.kernel.benchmark.tests
{
    [TestFixture]
    public class AssemblyKernelBenchmarkTestsExplicit : BenchmarkTestsBase
    {
        [Test, Explicit("Run only local")]
        public void RunBenchmarkTest()
        {
            var testDirectory = Path.Combine(GetBenchmarkTestsDirectory(), "testdefinitions");
            var fileName = Directory.GetFiles(testDirectory, "*traject 30-4*.xlsm").First();
            var testName = GetTestName(fileName);

            BenchmarkTestInput input = AssemblyExcelFileReader.Read(fileName, testName);
            BenchmarkTestResult testResult = new BenchmarkTestResult(fileName, testName);

            BenchmarkTestRunner.TestEqualNormCategories(input, testResult);

            foreach (IExpectedFailureMechanismResult expectedFailureMechanismResult in input
                .ExpectedFailureMechanismsResults)
            {
                BenchmarkTestRunner.TestFailureMechanismAssembly(expectedFailureMechanismResult, input.LowerBoundaryNorm,
                    input.SignallingNorm, testResult);
            }

            BenchmarkTestRunner.TestFinalVerdictAssembly(input, testResult);

            BenchmarkTestRunner.TestAssemblyOfCombinedSections(input, testResult);
        }
    }
}