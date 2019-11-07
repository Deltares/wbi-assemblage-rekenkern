using System.ComponentModel;
using assemblage.kernel.acceptance.tests.TestHelpers.Categories;
using assemblage.kernel.acceptance.tests.TestHelpers.FailureMechanism;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;
using assembly.kernel.acceptance.tests.data.Result;

namespace assemblage.kernel.acceptance.tests.TestHelpers
{
    public static class TesterFactory
    {
        public static IFailureMechanismResultTester CreateFailureMechanismTester(MethodResultsListing testResult, IExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            switch (expectedFailureMechanismResult.Type)
            {
                case MechanismType.STBI:
                case MechanismType.STPH:
                case MechanismType.HTKW:
                case MechanismType.BSKW:
                    return new ProbabilisticFailureMechanismResultTester(testResult, expectedFailureMechanismResult);
                case MechanismType.STKWp:
                case MechanismType.GEKB:
                    return new Group1NoSimpleAssessmentFailureMechanismResultTester(testResult, expectedFailureMechanismResult);
                case MechanismType.AGK:
                case MechanismType.GEBU:
                    return new Group3FailureMechanismResultTester(testResult, expectedFailureMechanismResult);
                case MechanismType.ZST:
                case MechanismType.DA:
                    return new Group3NoSimpleAssessmentFailureMechanismTester(testResult, expectedFailureMechanismResult);
                case MechanismType.GABI:
                case MechanismType.GABU:
                case MechanismType.STMI:
                case MechanismType.PKW:
                    return new Group4FailureMechanismTester(testResult, expectedFailureMechanismResult);
                case MechanismType.AWO:
                case MechanismType.STKWl:
                case MechanismType.INN:
                    return new Group4NoDetailedAssessmentFailureMechanismTester(testResult, expectedFailureMechanismResult);
                case MechanismType.STBU:
                    return new StbuFailureMechanismTester(testResult, expectedFailureMechanismResult);
                case MechanismType.HAV:
                case MechanismType.NWOkl:
                case MechanismType.VLZV:
                case MechanismType.VLAF:
                    return new Group5FailureMechanismTester(testResult, expectedFailureMechanismResult);
                case MechanismType.NWOoc:
                    return new NwOocFailureMechanismTester(testResult, expectedFailureMechanismResult);
                case MechanismType.NWObe:
                case MechanismType.NWObo:
                case MechanismType.VLGA:
                    return new Group5NoDetailedAssessmentFailureMechanismTester(testResult, expectedFailureMechanismResult);
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        public static ICategoriesTester CreateCategoriesTester(MethodResultsListing result, IExpectedFailureMechanismResult expectedFailureMechanismResult, double lowerBoundaryNorm, double signallingNorm)
        {
            switch (expectedFailureMechanismResult.Type)
            {
                case MechanismType.STBI:
                case MechanismType.STPH:
                case MechanismType.HTKW:
                case MechanismType.BSKW:
                case MechanismType.STKWp:
                case MechanismType.GEKB:
                    return new ProbabilisticFailureMechanismCategoriesTester(result, expectedFailureMechanismResult, lowerBoundaryNorm, signallingNorm);
                case MechanismType.AGK:
                case MechanismType.GEBU:
                case MechanismType.ZST:
                case MechanismType.DA:
                    return new Group3FailureMechanismCategoriesTester(result, expectedFailureMechanismResult, lowerBoundaryNorm, signallingNorm);
                case MechanismType.STBU:
                    return new STBUCategoriesTester(result, expectedFailureMechanismResult, signallingNorm);
                default:
                    return null;
            }
        }
    }
}
