using System.Collections.Generic;

namespace assembly.kernel.benchmark.tests.data.Result
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

        /// <summary>
        /// Name of the file that contains the definition of the benchmark test
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Name of the benchmark test
        /// </summary>
        public string TestName { get; }

        /// <summary>
        /// Indicates whether category boundaries where calculated correctly
        /// </summary>
        public bool AreEqualCategoriesListAssessmentSection { get; set; }

        /// <summary>
        /// Provides a list of benchmark test results per failure mechanism
        /// </summary>
        public IList<BenchmarkFailureMechanismTestResult> FailureMechanismResults { get; }

        /// <summary>
        /// Indicates whether the cateories for the combined failure mechanisms in group 1 and 2 was calcualted correctly.
        /// </summary>
        public bool AreEqualCategoriesListGroup1and2 { get; set; }

        /// <summary>
        /// Indicates whether assembly for the failure mechanisms in group 1 and 2 was correctly during assembly.
        /// </summary>
        public bool AreEqualAssemblyResultGroup1and2 { get; set; }

        /// <summary>
        /// Indicates whether assembly for the failure mechanisms in group 1 and 2 was correctly during partial assembly.
        /// </summary>
        public bool AreEqualAssemblyResultGroup1and2Temporal { get; set; }

        /// <summary>
        /// Indicates whether assembly for the failure mechanisms in group 3 and 4 was correctly during assembly.
        /// </summary>
        public bool AreEqualAssemblyResultGroup3and4 { get; set; }

        /// <summary>
        /// Indicates whether assembly for the failure mechanisms in group 3 and 4 was correctly during partial assembly.
        /// </summary>
        public bool AreEqualAssemblyResultGroup3and4Temporal { get; set; }

        /// <summary>
        /// Indicates whether assembly of the final verdict was correctly.
        /// </summary>
        public bool AreEqualAssemblyResultFinalVerdict { get; set; }

        /// <summary>
        /// Indicates whether assembly of the final verdict was correctly during partial assembly.
        /// </summary>
        public bool AreEqualAssemblyResultFinalVerdictTemporal { get; set; }

        /// <summary>
        /// Indicates whether the combined sections where determined correctly
        /// </summary>
        public bool AreEqualAssemblyResultCombinedSections { get; set; }

        /// <summary>
        /// Indicates whether the assessment results per combined section were calculated correctly.
        /// </summary>
        public bool AreEqualAssemblyResultCombinedSectionsResults { get; set; }

        /// <summary>
        /// Indicates whether the assessment results per combined section were calculated correctly during partial assembly.
        /// </summary>
        public bool AreEqualAssemblyResultCombinedSectionsResultsTemporal { get; set; }

        /// <summary>
        /// Provides a list of benchmark test results per assembly method
        /// </summary>
        public MethodResultsListing MethodResults { get; }
    }
}