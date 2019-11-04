using System;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;

namespace assemblage.kernel.acceptance.tests.TestHelpers
{
    public abstract class FailureMechanismResultTesterBase<TFailureMechanismResult> : IFailureMechanismResultTester where TFailureMechanismResult : IExpectedFailureMechanismResult
    {
        protected readonly TFailureMechanismResult expectedFailureMechanismResult;

        public FailureMechanismResultTesterBase(IExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            this.expectedFailureMechanismResult = (TFailureMechanismResult)expectedFailureMechanismResult;
        }

        public virtual bool? TestSimpleAssessment()
        {
            try
            {
                TestSimpleAssessmentInternal();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}: Eenvoudige toets - {1}", expectedFailureMechanismResult.Name, e.Message);
                return false;
            }
        }

        protected virtual void TestSimpleAssessmentInternal(){}

        public virtual bool? TestDetailedAssessment()
        {
            try
            {
                TestDetailedAssessmentInternal();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}: Gedetailleerde toets - {1}", expectedFailureMechanismResult.Name, e.Message);
                return false;
            }
        }
        protected virtual void TestDetailedAssessmentInternal() { }

        public virtual bool TestTailorMadeAssessment()
        {
            try
            {
                TestTailorMadeAssessmentInternal();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}: Toets op maat - {1}", expectedFailureMechanismResult.Name, e.Message);
                return false;
            }
        }
        protected virtual void TestTailorMadeAssessmentInternal() { }

        public virtual bool TestCombinedAssessment()
        {
            try
            {
                TestCombinedAssessmentInternal();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}: Gecombineerd toetsoordeel per vak - {1}", expectedFailureMechanismResult.Name, e.Message);
                return false;
            }
        }
        protected virtual void TestCombinedAssessmentInternal() { }

        public virtual bool TestAssessmentSectionResult()
        {
            try
            {
                TestAssessmentSectionResultInternal();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}: Toetsoordeel per traject - {1}", expectedFailureMechanismResult.Name, e.Message);
                return false;
            }
        }
        protected virtual void TestAssessmentSectionResultInternal() { }

        public virtual bool TestAssessmentSectionResultTemporal()
        {
            try
            {
                TestAssessmentSectionResultTemporalInternal();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}: Voorlopig toetsoordeel per traject - {1}", expectedFailureMechanismResult.Name, e.Message);
                return false;
            }
        }
        protected abstract void TestAssessmentSectionResultTemporalInternal();
    }
}