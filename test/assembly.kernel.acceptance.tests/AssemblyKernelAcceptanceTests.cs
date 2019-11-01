using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using assembly.kernel.acceptance.tests.data;
using assembly.kernel.acceptance.tests.data.Input;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;
using assembly.kernel.acceptance.tests.data.Result;
using assembly.kernel.acceptance.tests.io;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.CategoryLimits;
using MathNet.Numerics.Distributions;
using NUnit.Framework;

namespace assemblage.kernel.acceptance.tests
{
    [TestFixture]
    public class AssemblyKernelAcceptanceTests
    {
        [Test]
        public void RunAcceptanceTests()
        {
            IEnumerable<string> tests = AcquireAllAcceptanceTests();

            foreach (var test in tests)
            {
                RunAcceptanceTest(test);
            }
        }

        private static void RunAcceptanceTest(string fileName)
        {
            AcceptanceTestInput input = AssemblyExcelFileReader.Read(fileName);
            AcceptanceTestResult result = new AcceptanceTestResult();

            TestEqualNormCategories(input, result);

            foreach (var failureMechanism in input.ExpectedFailureMechanismsResults)
            {
                TestFailureMechanismAssembly(input, failureMechanism, result);
            }

            TestFinalVerdictAssembly(input, result);

            TestAssemblyOfCombinedSections(input, result);
        }

        private static void TestAssemblyOfCombinedSections(AcceptanceTestInput input, AcceptanceTestResult result)
        {
            // TODO:
            // Test combined sections
            // Test correct comnbined sections (amount and distances)
            // Loop over failure mechanisms
            {
                // Test correct result for all sections (per failure mechanism
            }
            // Test correct combined result per combined section
        }

        private static void TestFinalVerdictAssembly(AcceptanceTestInput input, AcceptanceTestResult result)
        {
            var categoriesCalculator = new CategoryLimitsCalculator();
            var categories = categoriesCalculator.CalculateFailureMechanismCategoryLimitsWbi11(
                new AssessmentSection(input.Length, input.SignallingNorm, input.LowerBoundaryNorm),
                new FailureMechanism(1, input.ExpectedSafetyAssessmentAssemblyResult.CombinedFailureMechanismProbabilitySpace));
            // TODO: Assert correct categories

            var assembler = new AssessmentGradeAssembler();
            // Test correct result for groups 1/2 and 3.4
            var resultGroup1and2 = assembler.AssembleAssessmentSectionWbi2B1(input.ExpectedFailureMechanismsResults
                .OfType<ProbabilisticFailureMechanismResult>()
                .Select(fm => fm.ExpectedAssessmentResult as FailureMechanismAssemblyResult), categories, false);
            var partialResultGroup1and2 = assembler.AssembleAssessmentSectionWbi2B1(input.ExpectedFailureMechanismsResults
                .OfType<ProbabilisticFailureMechanismResult>()
                .Select(fm => fm.ExpectedAssessmentResult as FailureMechanismAssemblyResult), categories, true);
            // TODO: Assert correct categories

            // Test correct assembly of final verdict

        }

        private static void TestFailureMechanismAssembly(AcceptanceTestInput input, IFailureMechanismResult failureMechanismResult, AcceptanceTestResult result)
        {
            // TODO:
            // If necessary test categories (section and fm level)

            // Loop over failure mechanism sections
            {
                // Test simple assessment

                // Test detailed assessment

                // Test tailormade assessment

                // Test combined assessment result
            }

            // Test assessment on failure mechanism level
        }

        private static void TestEqualNormCategories(AcceptanceTestInput input, AcceptanceTestResult result)
        {
            // Calculate categories based on input
            var calculator = new CategoryLimitsCalculator();

            try
            {
                var categories = calculator.CalculateAssessmentSectionCategoryLimitsWbi21(new AssessmentSection(
                    input.Length,
                    input.SignallingNorm, input.LowerBoundaryNorm));

                var expectedCategories = input.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssessmentSectionCategories;
                Assert.AreEqual(expectedCategories.Categories.Length, categories.Categories.Length);




                for (int i = 0; i < categories.Categories.Length; i++)
                {
                    AssertAreEqualCategories(expectedCategories.Categories[i], categories.Categories[i]);
                }

                result.AreEqualCategoriesListAssessmentSection = true;
            }
            catch (Exception)
            {
                result.AreEqualCategoriesListAssessmentSection = false;
            }
        }

        private static void AssertAreEqualCategories<TCategory>(CategoryBase<TCategory> expectedCategory, CategoryBase<TCategory> calculatedCategory)
        {
            Assert.AreEqual(expectedCategory.Category, calculatedCategory.Category);
            AssertAreEqualProbabilities(expectedCategory.LowerLimit, calculatedCategory.LowerLimit);
            AssertAreEqualProbabilities(expectedCategory.UpperLimit, calculatedCategory.UpperLimit);
        }

        private static void AssertAreEqualProbabilities(double expectedProbability, double actualProbability)
        {
            Assert.AreEqual(ProbabilityToReliability(expectedProbability), ProbabilityToReliability(actualProbability), 1e-3);
        }

        /// <summary>
        /// Calculates the reliability from a probability.
        /// </summary>
        /// <param name="probability">The probability to convert.</param>
        /// <returns>The reliability.</returns>
        private static double ProbabilityToReliability(double probability)
        {
            return Normal.InvCDF(0, 1, 1 - probability);
        }

        private IEnumerable<string> AcquireAllAcceptanceTests()
        {
            var testDirectory = Path.Combine(
                    Path.GetDirectoryName(
                            Uri.UnescapeDataString(new UriBuilder(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Path))
                        .Replace(@"\bin\Debug", ""),
                    "testdefinitions");

            return Directory.GetFiles(testDirectory, "*.xlsm");
        }
    }
}
