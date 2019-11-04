using System.ComponentModel;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;

namespace assemblage.kernel.acceptance.tests.TestHelpers
{
    public static class TestHelperFactory
    {
        public static IFailureMechanismResultTester CreateFailureMechanismTestHelper(IExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            switch (expectedFailureMechanismResult.Type)
            {
                case MechanismType.STBI:
                case MechanismType.STPH:
                case MechanismType.HTKW:
                case MechanismType.BSKW:
                    return new ProbabilisticFailureMechanismResultTester(expectedFailureMechanismResult);
                case MechanismType.STKWp:
                case MechanismType.GEKB:
                    return new Group1NoSimpleAssessmentFailureMechanismResultTester(expectedFailureMechanismResult);
                case MechanismType.AGK:
                case MechanismType.GEBU:
                    return new Group3FailureMechanismResultTester(expectedFailureMechanismResult);
                case MechanismType.ZST:
                case MechanismType.DA:
                    return new Group3NoSimpleAssessmentFailureMechanismTester(expectedFailureMechanismResult);
                case MechanismType.GABI:
                case MechanismType.GABU:
                case MechanismType.STMI:
                case MechanismType.PKW:
                    return new Group4FailureMechanismTester(expectedFailureMechanismResult);
                case MechanismType.AWO:
                case MechanismType.STKWl:
                case MechanismType.INN:
                    return new Group4NoDetailedAssessmentFailureMechanismTester(expectedFailureMechanismResult);
                case MechanismType.STBU:
                    return new StbuFailureMechanismTester(expectedFailureMechanismResult);
                case MechanismType.HAV:
                case MechanismType.NWOkl:
                case MechanismType.VLZV:
                case MechanismType.VLAF:
                    return new Group5FailureMechanismTester(expectedFailureMechanismResult);
                case MechanismType.NWOoc:
                    return new NwOocFailureMechanismTester(expectedFailureMechanismResult);
                case MechanismType.NWObe:
                case MechanismType.NWObo:
                case MechanismType.VLGA:
                    return new Group5NoDetailedAssessmentFailureMechanismTester(expectedFailureMechanismResult);
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        public static ICategoriesTester CreateCategoriesTester(IExpectedFailureMechanismResult expectedFailureMechanismResult, double lowerBoundaryNorm, double signallingNorm)
        {
            switch (expectedFailureMechanismResult.Type)
            {
                case MechanismType.STBI:
                case MechanismType.STPH:
                case MechanismType.HTKW:
                case MechanismType.BSKW:
                case MechanismType.STKWp:
                case MechanismType.GEKB:
                    return new ProbabilisticFailureMechanismCategoriesTester(expectedFailureMechanismResult, lowerBoundaryNorm, signallingNorm);
                case MechanismType.AGK:
                case MechanismType.GEBU:
                case MechanismType.ZST:
                case MechanismType.DA:
                    return new Group3FailureMechanismCategoriesTester(expectedFailureMechanismResult, lowerBoundaryNorm, signallingNorm);
                case MechanismType.STBU:
                    return new STBUCategoriesTester(expectedFailureMechanismResult, signallingNorm);
                default:
                    return null;
            }
        }
    }
}
