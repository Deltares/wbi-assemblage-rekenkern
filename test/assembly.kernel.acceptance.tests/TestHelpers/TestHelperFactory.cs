using System.ComponentModel;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;

namespace assemblage.kernel.acceptance.tests.TestHelpers
{
    public static class TestHelperFactory
    {
        public static IFailureMechanismResultTestHelper CreateFailureMechanismTestHelper(IFailureMechanismResult failureMechanismResult)
        {
            switch (failureMechanismResult.Type)
            {
                case MechanismType.STBI:
                case MechanismType.STPH:
                case MechanismType.HTKW:
                case MechanismType.BSKW:
                    return new ProbabilisticFailureMechanismResultTestHelper(failureMechanismResult);
                case MechanismType.STKWp:
                case MechanismType.GEKB:
                    // TODO: Implement
                    return new Group1NoSimpleAssessmentFailureMechanismResultTestHelper(failureMechanismResult);
                case MechanismType.AGK:
                case MechanismType.GEBU:
                    // TODO: Implement
                    return new Group3FailureMechanismResultTestHelper(failureMechanismResult);
                case MechanismType.ZST:
                case MechanismType.DA:
                    // TODO: Implement
                    return new Group3NoSimpleAssessmentFailureMechanismTestHelper(failureMechanismResult);
                case MechanismType.GABI:
                case MechanismType.GABU:
                case MechanismType.STMI:
                case MechanismType.PKW:
                    // TODO: Implement
                    return new Group4FailureMechanismTestHelper(failureMechanismResult);
                case MechanismType.AWO:
                case MechanismType.STKWl:
                case MechanismType.INN:
                    // TODO: Implement
                    return new Group4NoDetailedAssessmentFailureMechanismTestHelper(failureMechanismResult);
                case MechanismType.STBU:
                    // TODO: Implement
                    return new STBUFailureMechanismTestHelper(failureMechanismResult);
                case MechanismType.HAV:
                case MechanismType.NWOoc:
                case MechanismType.NWOkl:
                case MechanismType.VLZV:
                case MechanismType.VLAF:
                    // TODO: Implement
                    return new Group5FailureMechanismTestHelper(failureMechanismResult);
                case MechanismType.NWObe:
                case MechanismType.NWObo:
                case MechanismType.VLGA:
                    // TODO: Implement
                    return new Group5NoDetailedAssessmentFailureMechanismTestHelper(failureMechanismResult);
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}
