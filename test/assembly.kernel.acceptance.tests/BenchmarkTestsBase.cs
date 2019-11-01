using System;
using Assembly.Kernel.Model.CategoryLimits;
using MathNet.Numerics.Distributions;
using NUnit.Framework;

namespace assemblage.kernel.acceptance.tests
{
    public class BenchmarkTestsBase
    {
        // TODO: Merge these two methods somehow?
        protected static void AssertEqualCategoriesList(CategoriesList<AssessmentSectionCategory> expectedCategories,
        CategoriesList<AssessmentSectionCategory> categories, Action positiveResultAction)
        {
            Assert.AreEqual(expectedCategories.Categories.Length, categories.Categories.Length);
            for (int i = 0; i < categories.Categories.Length; i++)
            {
                AssertAreEqualCategories(
                    expectedCategories.Categories[i],
                    categories.Categories[i]);
            }

            positiveResultAction();
        }

        // TODO: Merge these two methods somehow?
        protected static void AssertEqualCategoriesList(CategoriesList<FailureMechanismCategory> expectedCategories,
            CategoriesList<FailureMechanismCategory> categories, Action positiveResultAction)
        {
            Assert.AreEqual(expectedCategories.Categories.Length, categories.Categories.Length);
            for (int i = 0; i < categories.Categories.Length; i++)
            {
                AssertAreEqualCategories(
                    expectedCategories.Categories[i],
                    categories.Categories[i]);
            }

            positiveResultAction();
        }

        private static void AssertAreEqualCategories<TCategory>(CategoryBase<TCategory> expectedCategory, CategoryBase<TCategory> calculatedCategory)
        {
            Assert.AreEqual(expectedCategory.Category, calculatedCategory.Category);
            AssertAreEqualProbabilities(expectedCategory.LowerLimit, calculatedCategory.LowerLimit);
            AssertAreEqualProbabilities(expectedCategory.UpperLimit, calculatedCategory.UpperLimit);
        }

        private static void AssertAreEqualProbabilities(double expectedProbability, double actualProbability)
        {
            Assert.AreEqual((double) ProbabilityToReliability(expectedProbability), (double) ProbabilityToReliability(actualProbability), 1e-3);
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
    }
}