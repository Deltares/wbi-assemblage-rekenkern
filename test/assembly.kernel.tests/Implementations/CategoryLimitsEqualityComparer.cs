using System.Collections;
using Assembly.Kernel.Model.Categories;

namespace Assembly.Kernel.Tests.Implementations
{
    /// <summary>
    /// Defines an equality comparer for ICategory limits.
    /// </summary>
    public class CategoryLimitsEqualityComparer : IComparer
    {
        /// <inheritdoc />
        public int Compare(object x, object y)
        {
            var categoryLimitsX = x as ICategoryLimits;
            var categoryLimitsY = y as ICategoryLimits;
            return categoryLimitsX != null &&
                   categoryLimitsY != null &&
                   categoryLimitsX.LowerLimit.IsNegligibleDifference(categoryLimitsY.LowerLimit) &&
                   categoryLimitsX.UpperLimit.IsNegligibleDifference(categoryLimitsY.UpperLimit) ? 0 : 1;
        }
    }
}