using System;
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Model.CategoryLimits
{
    /// <summary>
    /// This object us used to validate category boundaries once and remove this validation from all methods that use category boundaries.
    /// </summary>
    public class CategoriesList<TCategory> where TCategory : ICategoryLimits
    {
        /// <summary>
        /// The epsilon that is used when comparing category boundaries. Gaps between category boundaries smaller than Epsilon will not be taken into account.
        /// </summary>
        public static readonly double Epsilon = 1e-60;

        /// <summary>
        /// This constructor validates a list of category limits and assigns the correct list to the Categories property.
        /// </summary>
        /// <param name="categoryLimits"></param>
        public CategoriesList(TCategory[] categoryLimits)
        {
            Categories = CheckCategories(categoryLimits);
        }

        /// <summary>
        /// The list with categories. This list is guaranteed to span the complete probability range between 0 and 1.
        /// </summary>
        public IEnumerable<TCategory> Categories { get; }

        private TCategory[] CheckCategories(TCategory[] categoryLimits)
        {
            var categories = categoryLimits.OrderBy(c => c.LowerLimit).ToArray();
            var expectedCategoryBoundary = 0.0;
            
            foreach (var category in categories)
            {
                if (Math.Abs(category.LowerLimit - expectedCategoryBoundary) > Epsilon)
                {
                    throw new AssemblyException("Categories are not subsequent and do not fully cover the probability range",EAssemblyErrors.InvalidCategoryLimits);
                }

                expectedCategoryBoundary = category.UpperLimit;
            }

            if (Math.Abs(expectedCategoryBoundary - 1.0) > Epsilon)
            {
                throw new AssemblyException("Categories are not subsequent and do not fully cover the probability range", EAssemblyErrors.InvalidCategoryLimits);
            }

            return categories;
        }
    }
}
