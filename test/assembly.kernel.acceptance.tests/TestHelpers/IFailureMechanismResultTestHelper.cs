using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;
using assembly.kernel.acceptance.tests.data.Result;

namespace assemblage.kernel.acceptance.tests.TestHelpers
{
    public interface IFailureMechanismResultTestHelper
    {
        void TestSimpleAssessment();

        void TestDetailedAssessment();

        void TestTailorMadeAssessment();

        void TestCombinedAssessment();

        void TestAssessmentSectionResult();

        void TestAssessmentSectionResultTemporal();
    }
}