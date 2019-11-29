using System.Collections.Generic;
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
    public class AssemblyKernelBenchmarkTests : BenchmarkTestsBase
    {
        private string reportDirectory;
        private Dictionary<string, BenchmarkTestResult> testResults;

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

        public class BenchmarkTestCaseFactory
        {
            public static IEnumerable<TestCaseData> BenchmarkTestCases => AcquireAllBenchmarkTests().ToArray()
                .Select(t => new TestCaseData(GetTestName(t), t) {TestName = GetTestName(t)});
        }

        [Test, TestCaseSource(typeof(BenchmarkTestCaseFactory), nameof(BenchmarkTestCaseFactory.BenchmarkTestCases))]
        public void RunBenchmarkTest(string testName, string fileName)
        {
            BenchmarkTestInput input = AssemblyExcelFileReader.Read(fileName, testName);
            BenchmarkTestResult testResult = new BenchmarkTestResult(fileName, testName);

            BenchmarkTestRunner.TestEqualNormCategories(input, testResult);

            foreach (IExpectedFailureMechanismResult expectedFailureMechanismResult in input
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

        private static string PrepareReportDirectory()
        {
            var reportDirectory = Path.Combine(GetBenchmarkTestsDirectory(), "testresults");
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