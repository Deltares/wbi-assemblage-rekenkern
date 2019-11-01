using Assembly.Kernel.Model;
using Assembly.Kernel.Model.CategoryLimits;

namespace assembly.kernel.acceptance.tests.data.Input
{
    public class SafetyAssessmentAssemblyResult
    {
        /// <summary>
        /// The combined probability space (faalkansruimte) for all relevant failure mechanisms
        /// in groups 1 and 2 (the probabilistic mechanisms).
        /// </summary>
        public double CombinedFailureMechanismProbabilitySpace { get; set; }

        /// <summary>
        /// The expected section categories (A+ to D) on the highest (assessment section) level.
        /// </summary>
        public CategoriesList<AssessmentSectionCategory> ExpectedAssessmentSectionCategories { get; set; }

        /// <summary>
        /// The expected categories (It to VIt) for the combined failure mechanisms in
        /// groups 1 and 2 (the probabilistic mechanisms).
        /// </summary>
        public CategoriesList<FailureMechanismCategory> ExpectedCombinedFailureMechanismCategoriesGroup1and2 { get; set; }

        /// <summary>
        /// The expected result (toetsoordeel) for the combined failure mechanisms
        /// in group 1 and 2 (the probabilistic mechanisms).
        /// </summary>
        public EFailureMechanismCategory ExpectedAssemblyResultGroups1and2 { get; set; }

        /// <summary>
        /// The expected result (toetsoordeel) for the combined failure mechanisms
        /// in group 1 and 2 (the probabilistic mechanisms) as a result of temporal assessment.
        /// </summary>
        public EFailureMechanismCategory ExpectedAssemblyResultGroups1and2Temporal { get; set; }

        /// <summary>
        /// The expected estimated probability of flooding for the combined
        /// failure mechanisms in group 1 and 2 (the probabilistic mechanisms).
        /// </summary>
        public double ExpectedAssemblyResultGroups1and2Probability { get; set; }

        /// <summary>
        /// The expected estimated probability of flooding for the combined
        /// failure mechanisms in group 1 and 2 (the probabilistic mechanisms) as a result of temporal assessment.
        /// </summary>
        public double ExpectedAssemblyResultGroups1and2ProbabilityTemporal { get; set; }

        /// <summary>
        /// The expected result (toetsoordeel) for the combined failure mechanisms
        /// in group 3 and 4 (the non-probabilistic direct failure mechanisms)
        /// </summary>
        public EFailureMechanismCategory ExpectedAssemblyResultGroups3and4 { get; set; }

        /// <summary>
        /// The expected result (toetsoordeel) for the combined failure mechanisms
        /// in group 3 and 4 (the non-probabilistic direct failure mechanisms) as a result of temporal assessment.
        /// </summary>
        public EFailureMechanismCategory ExpectedAssemblyResultGroups3and4Temporal { get; set; }

        /// <summary>
        /// The expected safety assessment verdict (A+ to D) for the assessment
        /// section (final result).
        /// </summary>
        public EAssessmentGrade ExpectedSafetyAssessmentAssemblyResult { get; set; }

        /// <summary>
        /// The expected safety assessment verdict (A+ to D) for the assessment
        /// section (final result) as a result of temporal assessment.
        /// </summary>
        public EAssessmentGrade ExpectedSafetyAssessmentAssemblyResultTemporal { get; set; }
    }
}