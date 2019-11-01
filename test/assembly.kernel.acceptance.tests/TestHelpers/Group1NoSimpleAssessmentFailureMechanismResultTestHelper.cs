using System;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;

namespace assemblage.kernel.acceptance.tests.TestHelpers
{
    public class Group1NoSimpleAssessmentFailureMechanismResultTestHelper : IFailureMechanismResultTestHelper
    {
        public Group1NoSimpleAssessmentFailureMechanismResultTestHelper(IFailureMechanismResult failureMechanismResult)
        {
            throw new NotImplementedException();
        }

        public void TestSimpleAssessment()
        {
            throw new NotImplementedException();
            // TODO: Group1 implementation
            /*                else
                            {
                                var group1Section = section as Group1NoSimpleAssessmentFailureMechanismSection;
                                if (group1Section == null)
                                {
                                    throw new ArgumentException();
                                }

                                var result = assembler.TranslateAssessmentResultWbi0E3(group1Section.SimpleAssessmentResult);
                                var expectedResult = group1Section.ExpectedSimpleAssessmentAssemblyResult as
                                        FmSectionAssemblyDirectResultWithProbability;
                                Assert.AreEqual(expectedResult.Result, result.Result);
                                Assert.AreEqual(expectedResult.FailureProbability, result.FailureProbability);
                            }*/
        }

        public void TestDetailedAssessment()
        {
            throw new NotImplementedException();
            // TODO: Group1 implementation
            /*else
            {
                var group1Section = section as Group1NoSimpleAssessmentFailureMechanismSection;
                if (group1Section == null)
                {
                    throw new ArgumentException();
                }

                var result = assembler.TranslateAssessmentResultWbi0G3(group1Section.DetailedAssessmentResult, group1Section.DetailedAssessmentResultProbability, failureMechanismResult.ExpectedFailureMechanismSectionCategories);
                // TODO:
                Assert.AreEqual(group1Section.ExpectedSimpleAssessmentAssemblyResult, result);
            }*/
        }

        public void TestTailorMadeAssessment()
        {
            throw new NotImplementedException();
        }

        public void TestCombinedAssessment()
        {
            throw new NotImplementedException();
        }

        public void TestAssessmentSectionResult()
        {
            throw new NotImplementedException();
        }

        public void TestAssessmentSectionResultTemporal()
        {
            throw new NotImplementedException();
        }
    }
}