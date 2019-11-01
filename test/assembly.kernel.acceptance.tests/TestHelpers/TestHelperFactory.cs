using System.ComponentModel;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;

namespace assemblage.kernel.acceptance.tests.TestHelpers
{
    public static class TestHelperFactory
    {
        public static IFailureMechanismResultTestHelper CreateFailureMechanismTestHelper(IExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            switch (expectedFailureMechanismResult.Type)
            {
                case MechanismType.STBI:
                case MechanismType.STPH:
                case MechanismType.HTKW:
                case MechanismType.BSKW:
                    return new ProbabilisticFailureMechanismResultTestHelper(expectedFailureMechanismResult);
                case MechanismType.STKWp:
                case MechanismType.GEKB:
                    return new Group1NoSimpleAssessmentFailureMechanismResultTestHelper(expectedFailureMechanismResult);
                case MechanismType.AGK:
                case MechanismType.GEBU:
                    return new Group3FailureMechanismResultTestHelper(expectedFailureMechanismResult);
                case MechanismType.ZST:
                case MechanismType.DA:
                    return new Group3NoSimpleAssessmentFailureMechanismTestHelper(expectedFailureMechanismResult);
                case MechanismType.GABI:
                case MechanismType.GABU:
                case MechanismType.STMI:
                case MechanismType.PKW:
                    return new Group4FailureMechanismTestHelper(expectedFailureMechanismResult);
                case MechanismType.AWO:
                case MechanismType.STKWl:
                case MechanismType.INN:
                    return new Group4NoDetailedAssessmentFailureMechanismTestHelper(expectedFailureMechanismResult);
                case MechanismType.STBU:
                    return new STBUFailureMechanismTestHelper(expectedFailureMechanismResult);
                case MechanismType.HAV:
                case MechanismType.NWOkl:
                case MechanismType.VLZV:
                case MechanismType.VLAF:
                    return new Group5FailureMechanismTestHelper(expectedFailureMechanismResult);
                case MechanismType.NWOoc:
                    return new NWOocFailureMechanismTestHelper(expectedFailureMechanismResult);
                case MechanismType.NWObe:
                case MechanismType.NWObo:
                case MechanismType.VLGA:
                    return new Group5NoDetailedAssessmentFailureMechanismTestHelper(expectedFailureMechanismResult);
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}
