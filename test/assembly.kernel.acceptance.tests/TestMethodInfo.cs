using System;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;

namespace assemblage.kernel.acceptance.tests
{
    public class TestMethodInfo
    {
        public TestMethodInfo(Action<IFailureMechanismSection, IExpectedFailureMechanismResult> testMethodSimpleAssessment, 
            Action<IFailureMechanismSection, IExpectedFailureMechanismResult> testMethodDetailedAssessment, 
            Action<IFailureMechanismSection, IExpectedFailureMechanismResult> testMethodTailorMadeAssessment, 
            Action<IFailureMechanismSection, IExpectedFailureMechanismResult> testMethodCombinedAssessment)
        {
            TestMethodSimpleAssessment = testMethodSimpleAssessment;
            TestMethodDetailedAssessment = testMethodDetailedAssessment;
            TestMethodTailorMadeAssessment = testMethodTailorMadeAssessment;
            TestMethodCombinedAssessment = testMethodCombinedAssessment;
        }

        public Action<IFailureMechanismSection, IExpectedFailureMechanismResult> TestMethodSimpleAssessment { get; set; }

        public Action<IFailureMechanismSection, IExpectedFailureMechanismResult> TestMethodDetailedAssessment { get; set; }

        public Action<IFailureMechanismSection, IExpectedFailureMechanismResult> TestMethodTailorMadeAssessment { get; set; }

        public Action<IFailureMechanismSection, IExpectedFailureMechanismResult> TestMethodCombinedAssessment { get; set; }
    }
}