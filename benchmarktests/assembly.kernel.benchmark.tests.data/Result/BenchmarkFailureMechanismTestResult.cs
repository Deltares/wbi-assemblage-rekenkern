using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;

namespace assembly.kernel.benchmark.tests.data.Result
{
    public class BenchmarkFailureMechanismTestResult
    {
        public BenchmarkFailureMechanismTestResult(string name, MechanismType type, int group)
        {
            Name = name;
            Type = type;
            Group = group;
        }

        /// <summary>
        /// Name of the failure mechanism
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Type / code of the failure mechanism
        /// </summary>
        public MechanismType Type { get; }

        /// <summary>
        /// Assembly group of the failure mechanism (1, 2, 3, 4 or 5).
        /// </summary>
        public int Group { get; }

        /// <summary>
        /// Indicates whether category boundaries where calculated correctly
        /// </summary>
        public bool? AreEqualCategoryBoundaries { get; set; }

        /// <summary>
        /// Indicates whether all simple assessment results where translated correctly during assembly
        /// </summary>
        public bool AreEqualSimpleAssessmentResults { get; set; }

        /// <summary>
        /// Indicates whether all detailed assessment results where translated correctly during assembly
        /// </summary>
        public bool? AreEqualDetailedAssessmentResults { get; set; }

        /// <summary>
        /// Indicates whether all tailor made assessment results where translated correctly during assembly
        /// </summary>
        public bool AreEqualTailorMadeAssessmentResults { get; set; }

        /// <summary>
        /// Indicates whether all assessment results per section where translated correctly to a combined assesment result per mechanism section during assembly.
        /// </summary>
        public bool AreEqualCombinedAssessmentResultsPerSection { get; set; }

        /// <summary>
        /// Indicates whether all assessment results per section where translated correctly to a combined assesment result for the failure mechanism during assembly.
        /// </summary>
        public bool AreEqualAssessmentResultPerAssessmentSection { get; set; }

        /// <summary>
        /// Indicates whether all assessment results per section where translated correctly to a combined assesment result for the failure mechanism during assembly while performing partial assembly.
        /// </summary>
        public bool AreEqualAssessmentResultPerAssessmentSectionTemporal { get; set; }

        public bool AreEqualCombinedResultsCombinedSections { get; set; }
    }
}