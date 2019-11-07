using System.Collections.Generic;

namespace assembly.kernel.acceptance.tests.data.Result
{
    public class BenchmarkTestResult
    {
        public BenchmarkTestResult(string fileName, string testName)
        {
            FileName = fileName;
            TestName = testName;
            FailureMechanismResults = new List<BenchmarkFailureMechanismTestResult>();
            MethodResults = new MethodResultsListing();
        }

        public string FileName { get; }

        public string TestName { get; }

        public bool AreEqualCategoriesListAssessmentSection { get; set; }

        public IList<BenchmarkFailureMechanismTestResult> FailureMechanismResults { get; }

        public bool AreEqualCategoriesListGroup1and2 { get; set; }

        public bool AreEqualAssemblyResultGroup1and2 { get; set; }

        public bool AreEqualAssemblyResultGroup1and2Temporal { get; set; }

        public bool AreEqualAssemblyResultGroup3and4 { get; set; }

        public bool AreEqualAssemblyResultGroup3and4Temporal { get; set; }

        public bool AreEqualAssemblyResultFinalVerdict { get; set; }

        public bool AreEqualAssemblyResultFinalVerdictTemporal { get; set; }

        public bool AreEqualAssemblyResultCombinedSections { get; set; }

        public bool AreEqualAssemblyResultCombinedSectionsResults { get; set; }

        public bool AreEqualAssemblyResultCombinedSectionsResultsTemporal { get; set; }

        public MethodResultsListing MethodResults { get; }
    }
}
