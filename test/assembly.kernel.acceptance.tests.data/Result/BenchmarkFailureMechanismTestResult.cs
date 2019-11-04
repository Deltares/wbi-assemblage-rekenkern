using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;

namespace assembly.kernel.acceptance.tests.data.Result
{
    public class BenchmarkFailureMechanismTestResult
    {
        public BenchmarkFailureMechanismTestResult(string name, MechanismType type, int group)
        {
            Name = name;
            Type = type;
            Group = group;
        }

        public string Name { get; set; }

        public MechanismType Type { get; set; }

        public int Group { get; set; }

        public bool? AreEqualCategoryBoundaries { get; set; }

        public bool AreEqualSimpleAssessmentResults { get; set; }

        public bool? AreEqualDetailedAssessmentResults { get; set; }

        public bool AreEqualTailorMadeAssessmentResults { get; set; }

        public bool AreEqualCombinedAssessmentResultsPerSection { get; set; }

        public bool AreEqualAssessmentResultPerAssessmentSection { get; set; }

        public bool AreEqualAssessmentResultPerAssessmentSectionTemporal { get; set; }

        public bool AreEqualCombinedResultsCombinedSections { get; set; }
    }
}