using System;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;

namespace assemblage.kernel.acceptance.tests
{
    public class TestMethodInfo
    {
        public TestMethodInfo(Action<IFailureMechanismSection, IFailureMechanismResult> testMethodSimpleAssessment, 
            Action<IFailureMechanismSection, IFailureMechanismResult> testMethodDetailedAssessment, 
            Action<IFailureMechanismSection, IFailureMechanismResult> testMethodTailorMadeAssessment, 
            Action<IFailureMechanismSection, IFailureMechanismResult> testMethodCombinedAssessment)
        {
            TestMethodSimpleAssessment = testMethodSimpleAssessment;
            TestMethodDetailedAssessment = testMethodDetailedAssessment;
            TestMethodTailorMadeAssessment = testMethodTailorMadeAssessment;
            TestMethodCombinedAssessment = testMethodCombinedAssessment;
        }

        public Action<IFailureMechanismSection, IFailureMechanismResult> TestMethodSimpleAssessment { get; set; }

        public Action<IFailureMechanismSection, IFailureMechanismResult> TestMethodDetailedAssessment { get; set; }

        public Action<IFailureMechanismSection, IFailureMechanismResult> TestMethodTailorMadeAssessment { get; set; }

        public Action<IFailureMechanismSection, IFailureMechanismResult> TestMethodCombinedAssessment { get; set; }
    }
}