using Assembly.Kernel.Model.CategoryLimits;

namespace Assembly.Kernel.Tests.Model.CategoryLimits
{
    public class TestCategory : ICategoryLimits
    {
        public TestCategory(double lowerLimit, double upperLimit, string categoryIDentifyer = "")
        {
            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;
            CategoryIDentifyer = categoryIDentifyer;
        }

        public double UpperLimit { get; }
        public double LowerLimit { get; }

        public string CategoryIDentifyer { get; }
    }
}