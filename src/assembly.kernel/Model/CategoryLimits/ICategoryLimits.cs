namespace Assembly.Kernel.Model.CategoryLimits
{
    /// <summary>
    /// Interface for a category with upper and lower limit
    /// </summary>
    public interface ICategoryLimits
    {
        /// <summary>
        /// The upper limit of the category
        /// </summary>
        double UpperLimit { get; }

        /// <summary>
        /// The lower limit of the category
        /// </summary>
        double LowerLimit { get; }
    }
}