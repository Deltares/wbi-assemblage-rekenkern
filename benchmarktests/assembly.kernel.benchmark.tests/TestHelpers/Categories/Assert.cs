using System;
using Assembly.Kernel.Model.CategoryLimits;
using MathNet.Numerics.Distributions;
using NUnit.Framework;

namespace assembly.kernel.benchmark.tests.TestHelpers.Categories
{
    public static class Assert
    {
        // TODO: Merge these three methods somehow?
        public static bool AssertEqualCategoriesList(CategoriesList<AssessmentSectionCategory> expectedCategories,
            CategoriesList<AssessmentSectionCategory> categories)
        {
            try
            {
                NUnit.Framework.Assert.AreEqual(expectedCategories.Categories.Length, categories.Categories.Length);
                for (int i = 0; i < categories.Categories.Length; i++)
                {
                    AssertAreEqualCategories(
                        expectedCategories.Categories[i],
                        categories.Categories[i]);
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        // TODO: Merge these three methods somehow?
        public static bool AssertEqualCategoriesList(CategoriesList<FailureMechanismCategory> expectedCategories,
            CategoriesList<FailureMechanismCategory> categories)
        {
            try
            {
                NUnit.Framework.Assert.AreEqual(expectedCategories.Categories.Length, categories.Categories.Length);
                for (int i = 0; i < categories.Categories.Length; i++)
                {
                    AssertAreEqualCategories(
                        expectedCategories.Categories[i],
                        categories.Categories[i]);
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        // TODO: Merge these three methods somehow?
        public static bool AssertEqualCategoriesList(CategoriesList<FmSectionCategory> expectedCategories,
            CategoriesList<FmSectionCategory> categories)
        {
            try
            {
                NUnit.Framework.Assert.AreEqual(expectedCategories.Categories.Length, categories.Categories.Length);
                for (int i = 0; i < categories.Categories.Length; i++)
                {
                    AssertAreEqualCategories(
                        expectedCategories.Categories[i],
                        categories.Categories[i]);
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        private static void AssertAreEqualCategories<TCategory>(CategoryBase<TCategory> expectedCategory,
            CategoryBase<TCategory> calculatedCategory)
        {
            NUnit.Framework.Assert.AreEqual(expectedCategory.Category, calculatedCategory.Category);
            AssertAreEqualProbabilities(expectedCategory.LowerLimit, calculatedCategory.LowerLimit);
            AssertAreEqualProbabilities(expectedCategory.UpperLimit, calculatedCategory.UpperLimit);
        }

        private static void AssertAreEqualProbabilities(double expectedProbability, double actualProbability)
        {
            NUnit.Framework.Assert.AreEqual(ProbabilityToReliability(expectedProbability), ProbabilityToReliability(actualProbability),
                1e-3);
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
