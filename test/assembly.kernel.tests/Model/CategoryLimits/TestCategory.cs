using Assembly.Kernel.Model.CategoryLimits;

namespace Assembly.Kernel.Tests.Model.CategoryLimits
{
    public class TestCategory : ICategoryLimits
    {
        public TestCategory(double lowerLimit, double upperLimit)
        {
            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;
        }

        public double UpperLimit { get; }
        public double LowerLimit { get; }
    }
}