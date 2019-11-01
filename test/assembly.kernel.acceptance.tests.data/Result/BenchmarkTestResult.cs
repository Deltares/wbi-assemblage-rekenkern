using System.Collections.Generic;

namespace assembly.kernel.acceptance.tests.data.Result
{
    public class BenchmarkTestResult
    {
        public BenchmarkTestResult()
        {
            FailureMechanismResults = new List<BenchmarkFailureMechanismTestResult>();
        }

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

        // TODO: List result per method
    }
}
