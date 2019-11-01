using System.IO;
using System.Linq;
using assembly.kernel.acceptance.tests.data.Input;
using assembly.kernel.acceptance.tests.io.Readers;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.CategoryLimits;
using DocumentFormat.OpenXml.Packaging;
using NUnit.Framework;

namespace assembly.kernel.acceptance.tests.io.tests.Readers
{
    [TestFixture]
    public class GeneralInformationReaderTest : TestFileReaderTestBase
    {
        [Test]
        public void ReaderReadsInformationCorrectly()
        {
            var testFile = Path.Combine(GetTestDir(), "Benchmarktool Excel assemblagetool - General information.xlsm");

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(testFile, false))
            {
                
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                WorksheetPart workSheetPart = workbookPart.WorksheetParts.First();

                var reader = new GeneralInformationReader(workSheetPart,workbookPart);

                var result = new BenchmarkTestInput();

                reader.Read(result);

                Assert.AreEqual(17527, result.Length, 0.5);
                Assert.AreEqual("22-1", result.Name);
                Assert.AreEqual(1 / 3000.0, result.SignallingNorm, 1e-8);
                Assert.AreEqual(1 / 1000.0, result.LowerBoundaryNorm, 1e-8);

                var categories = result.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssessmentSectionCategories.Categories;
                Assert.AreEqual(5, categories.Length);
                AssertAreEqualCategories(EAssessmentGrade.APlus, 0.0, result.SignallingNorm / 30.0, categories[0]);
                AssertAreEqualCategories(EAssessmentGrade.A, result.SignallingNorm / 30.0, result.SignallingNorm, categories[1]);
                AssertAreEqualCategories(EAssessmentGrade.B, result.SignallingNorm, result.LowerBoundaryNorm, categories[2]);
                AssertAreEqualCategories(EAssessmentGrade.C, result.LowerBoundaryNorm, result.LowerBoundaryNorm * 30.0, categories[3]);
                AssertAreEqualCategories(EAssessmentGrade.D, result.LowerBoundaryNorm * 30.0, 1.0, categories[4]);
            }
        }

        private void AssertAreEqualCategories(EAssessmentGrade expectedCategory, double expectedLowerLimit, double expectedUpperLimit, AssessmentSectionCategory assessmentSectionCategory)
        {
            Assert.AreEqual(expectedCategory, assessmentSectionCategory.Category);
            Assert.AreEqual(expectedLowerLimit, assessmentSectionCategory.LowerLimit);
            Assert.AreEqual(expectedUpperLimit, assessmentSectionCategory.UpperLimit);
        }
    }
}
