namespace assemblage.kernel.acceptance.tests.TestHelpers
{
    public interface IFailureMechanismResultTester
    {
        bool? TestSimpleAssessment();

        bool? TestDetailedAssessment();

        bool TestTailorMadeAssessment();

        bool TestCombinedAssessment();

        bool TestAssessmentSectionResult();

        bool TestAssessmentSectionResultTemporal();
    }
}