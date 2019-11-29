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