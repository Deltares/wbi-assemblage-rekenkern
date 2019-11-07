namespace assembly.kernel.benchmark.tests.TestHelpers.FailureMechanism
{
    public interface IFailureMechanismResultTester
    {
        bool TestSimpleAssessment();

        bool? TestDetailedAssessment();

        bool TestTailorMadeAssessment();

        bool TestCombinedAssessment();

        bool TestAssessmentSectionResult();

        bool TestAssessmentSectionResultTemporal();
    }
}