using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections
{
    public class ExpectedFailureMechanismSectionWithLengthEffect :ExpectedFailureMechanismSection
    {
        public ExpectedFailureMechanismSectionWithLengthEffect(string sectionName, 
            double start, 
            double end, 
            bool isRelevant, 
            Probability initialMechanismProbabilityProfile, 
            Probability initialMechanismProbabilitySection, 
            ERefinementStatus refinementStatus,
            Probability refinedProbabilityProfile,
            Probability refinedProbabilitySection,
            Probability expectedProbabilityProfile,
            Probability expectedCombinedProbabilitySection, 
            EInterpretationCategory expectedInterpretationCategory, 
            double expectedLengthEffect) 
            : base(sectionName, start, end, isRelevant, initialMechanismProbabilitySection, refinementStatus, refinedProbabilitySection, expectedCombinedProbabilitySection, expectedInterpretationCategory)
        {
            InitialMechanismProbabilityProfile = initialMechanismProbabilityProfile;
            RefinedProbabilityProfile = refinedProbabilityProfile;
            ExpectedLengthEffect = expectedLengthEffect;
            ExpectedProbabilityProfile = expectedProbabilityProfile;
        }

        public Probability InitialMechanismProbabilityProfile { get; }

        public Probability RefinedProbabilityProfile { get; }

        public double ExpectedLengthEffect { get; }

        public Probability ExpectedProbabilityProfile { get; }
    }
}