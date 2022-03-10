using System.ComponentModel;
using Assembly.Kernel.Model.Categories;

namespace assembly.kernel.benchmark.tests.data.Input
{
    public static class EExpectedAssessmentGradeExtensions
    {
        /// <summary>
        /// Translates an expected assessment grade into the actual <seealso cref="EAssessmentGrade"/>.
        /// </summary>
        /// <param name="expectedAssessmentGrade"></param>
        /// <returns></returns>
        public static EAssessmentGrade ToEAssessmentGrade(this EExpectedAssessmentGrade expectedAssessmentGrade)
        {
            switch (expectedAssessmentGrade)
            {
                case EExpectedAssessmentGrade.APlus:
                    return EAssessmentGrade.APlus;
                case EExpectedAssessmentGrade.A:
                    return EAssessmentGrade.A;
                case EExpectedAssessmentGrade.B:
                    return EAssessmentGrade.B;
                case EExpectedAssessmentGrade.C:
                    return EAssessmentGrade.C;
                case EExpectedAssessmentGrade.D:
                    return EAssessmentGrade.D;
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}
