using System;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.data.Result;

namespace assembly.kernel.benchmark.tests.TestHelpers.FailureMechanism
{
    public class FailureMechanismWithLengthEffectResultTester : FailureMechanismResultTesterBase
    {
        public FailureMechanismWithLengthEffectResultTester(MethodResultsListing methodResults, ExpectedFailureMechanismResult expectedFailureMechanismResult) : base(methodResults, expectedFailureMechanismResult)
        {
        }

        protected override void SetCombinedAssessmentMethodResult(bool result)
        {
            throw new NotImplementedException();
        }

        protected override void SetAssessmentSectionMethodResult(bool result)
        {
            throw new NotImplementedException();
        }

        protected override void SetAssessmentSectionMethodResultTemporal(bool result)
        {
            throw new NotImplementedException();
        }
    }
}
