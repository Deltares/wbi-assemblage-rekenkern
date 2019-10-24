using Assembly.Kernel.Model;
using Assembly.Kernel.Model.CategoryLimits;

namespace assembly.kernel.acceptance.tests.data
{
    public class SafetyAssessmentAssemblyResult
    {
        public CategoriesList<AssessmentSectionCategory> ExpectedAssessmentSectionCategories { get; set; }

        public double CombinedFailureMechanismProbabilitySpace { get; set; }

        public CategoriesList<FailureMechanismCategory> ExpectedFailureMechanismCategories { get; set; }

        public EFailureMechanismCategory ExpectedAssemblyResultGroups1and2 { get; set; }

        public double ExpectedAssemblyResultGroups1and2Probability { get; set; }

        public EFailureMechanismCategory ExpectedAssemblyResultGroups3and4 { get; set; }

        public EAssessmentGrade ExpectedSafetyAssessmentAssemblyResult { get; set; }
    }
}