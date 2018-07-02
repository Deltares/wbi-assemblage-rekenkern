using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model.CategoryLimits;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Model.CategoryLimits
{
    [TestFixture]
    public class CategoriesListTest
    {
        private static IEnumerable InconsistantCategoryBoundariesTestCases
        {
            get
            {
                yield return new TestCaseData(
                    new List<TestCategory> {new TestCategory(0.0, 0.9)});

                yield return new TestCaseData(
                    new List<TestCategory>
                    {
                        new TestCategory(0.1, 0.2),
                        new TestCategory(0.2, 0.5),
                        new TestCategory(0.5, 1.0)
                    });

                yield return new TestCaseData(
                       new  List < TestCategory >
                    {
                        new TestCategory(0.0, 2e-40),
                        new TestCategory(2e-41, 0.2),
                        new TestCategory(0.2, 0.5),
                        new TestCategory(0.5, 1.0)
                    });


                yield return new TestCaseData(
                    new List<TestCategory>
                    {
                        new TestCategory(0.0, 0.1),
                        new TestCategory(0.1, 0.2),
                        new TestCategory(0.15, 0.5),
                        new TestCategory(0.5, 1.0)
                    });

                yield return new TestCaseData(
                    new List<TestCategory>
                    {
                        new TestCategory(0.0, 0.1),
                        new TestCategory(0.1, 0.2),
                        new TestCategory(0.05, 0.5),
                        new TestCategory(0.5, 1.0)
                    });

                yield return new TestCaseData(
                    new List<TestCategory>
                    {
                        new TestCategory(0.0, 0.1),
                        new TestCategory(0.1, 0.2),
                        new TestCategory(0.2, 0.5),
                        new TestCategory(0.5+1e-16, 1.0)
                    });

                yield return new TestCaseData(
                    new List<TestCategory>
                    {
                        new TestCategory(0.0, 0.1),
                        new TestCategory(0.2, 0.5),
                        new TestCategory(0.5, 1.0)
                    });
            }
        }

        [Test,
         TestCaseSource(
             typeof(CategoriesListTest),
             nameof(InconsistantCategoryBoundariesTestCases))]
        public void CheckForInconsistantCategories(IEnumerable<TestCategory> categories)
        {
            try
            {
                var list = new CategoriesList<TestCategory>(categories.ToArray());
            }
            catch (AssemblyException e)
            {
                Assert.IsNotNull(e.Errors);
                Assert.AreEqual(1,e.Errors.Count());
                Assert.AreEqual(EAssemblyErrors.InvalidCategoryLimits,e.Errors.First().ErrorCode);
                return;
            }
            
            Assert.Fail("Expected exception, but did not recieve one.");
        }

        [Test]
        public void CtorAcceptsCorrectListOfCategories()
        {
            var list = new CategoriesList<TestCategory>(new[]
            {
                new TestCategory(0.0, 0.5),
                new TestCategory(0.5, 1.0),
            });

            Assert.IsNotNull(list);
            Assert.AreEqual(2, list.Categories.Length);
        }
    }
}
