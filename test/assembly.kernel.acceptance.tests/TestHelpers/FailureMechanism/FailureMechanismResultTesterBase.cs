using System;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;
using assembly.kernel.acceptance.tests.data.Result;

namespace assemblage.kernel.acceptance.tests.TestHelpers.FailureMechanism
{
    public abstract class FailureMechanismResultTesterBase<TFailureMechanismResult> : BenchmarkTestsBase, IFailureMechanismResultTester where TFailureMechanismResult : class, IExpectedFailureMechanismResult
    {
        protected readonly TFailureMechanismResult ExpectedFailureMechanismResult;
        protected readonly MethodResultsListing MethodResults;

        protected FailureMechanismResultTesterBase(MethodResultsListing methodResults, IExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            ExpectedFailureMechanismResult = expectedFailureMechanismResult as TFailureMechanismResult;
            this.MethodResults = methodResults;
            if (ExpectedFailureMechanismResult == null)
            {
                throw new ArgumentException();
            }
        }

        public bool TestSimpleAssessment()
        {
            try
            {
                TestSimpleAssessmentInternal();
                SetSimpleAssessmentMethodResult(true);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}: Eenvoudige toets - {1}", ExpectedFailureMechanismResult.Name, e.Message);
                SetSimpleAssessmentMethodResult(false);
                return false;
            }
        }

        protected abstract void SetSimpleAssessmentMethodResult(bool result);

        protected abstract void TestSimpleAssessmentInternal();

        public virtual bool? TestDetailedAssessment()
        {
            try
            {
                TestDetailedAssessmentInternal();
                SetDetailedAssessmentMethodResult(true);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}: Gedetailleerde toets - {1}", ExpectedFailureMechanismResult.Name, e.Message);
                SetDetailedAssessmentMethodResult(false);
                return false;
            }
        }
        protected virtual void SetDetailedAssessmentMethodResult(bool result) { }

        protected virtual void TestDetailedAssessmentInternal() { }

        public virtual bool TestTailorMadeAssessment()
        {
            try
            {
                TestTailorMadeAssessmentInternal();
                SetTailorMadeAssessmentMethodResult(true);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}: Toets op maat - {1}", ExpectedFailureMechanismResult.Name, e.Message);
                SetTailorMadeAssessmentMethodResult(false);
                return false;
            }
        }

        protected abstract void SetTailorMadeAssessmentMethodResult(bool result);

        protected abstract void TestTailorMadeAssessmentInternal();

        public virtual bool TestCombinedAssessment()
        {
            try
            {
                TestCombinedAssessmentInternal();
                SetCombinedAssessmentMethodResult(true);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}: Gecombineerd toetsoordeel per vak - {1}", ExpectedFailureMechanismResult.Name, e.Message);
                SetCombinedAssessmentMethodResult(false);
                return false;
            }
        }

        protected abstract void SetCombinedAssessmentMethodResult(bool result);

        protected abstract void TestCombinedAssessmentInternal();

        public virtual bool TestAssessmentSectionResult()
        {
            try
            {
                TestAssessmentSectionResultInternal();
                SetAssessmentSectionMethodResult(true);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}: Toetsoordeel per traject - {1}", ExpectedFailureMechanismResult.Name, e.Message);
                SetAssessmentSectionMethodResult(false);
                return false;
            }
        }

        protected abstract void SetAssessmentSectionMethodResult(bool result);

        protected abstract void TestAssessmentSectionResultInternal();

        public virtual bool TestAssessmentSectionResultTemporal()
        {
            try
            {
                TestAssessmentSectionResultTemporalInternal();
                SetAssessmentSectionMethodResultTemporal(true);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}: Voorlopig toetsoordeel per traject - {1}", ExpectedFailureMechanismResult.Name, e.Message);
                SetAssessmentSectionMethodResultTemporal(false);
                return false;
            }
        }

        protected abstract void SetAssessmentSectionMethodResultTemporal(bool result);

        protected abstract void TestAssessmentSectionResultTemporalInternal();
    }
}