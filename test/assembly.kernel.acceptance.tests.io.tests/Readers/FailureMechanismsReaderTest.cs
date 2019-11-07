using System.IO;
using System.Linq;
using assembly.kernel.acceptance.tests.data.Input;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;
using assembly.kernel.acceptance.tests.io.Readers;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FmSectionTypes;
using DocumentFormat.OpenXml.Packaging;
using NUnit.Framework;

namespace assembly.kernel.acceptance.tests.io.tests.Readers
{
    [TestFixture]
    public class FailureMechanismsReaderTest : TestFileReaderTestBase
    {
        private string testFile;

        [SetUp]
        public void Setup()
        {
            testFile = Path.Combine(GetTestDir(), "Benchmarktool Excel assemblagetool (v1_0_1_0) 0_03.xlsm");
        }

        [Test]
        public void ReaderReadsGroup3InformationCorrectly()
        {
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(testFile, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                var workSheetParts = ReadWorkSheetParts(workbookPart);
                var aGKWorkSheetPart = workSheetParts["AGK"];

                var reader = new FailureMechanismsReader(aGKWorkSheetPart, workbookPart);

                var result = new BenchmarkTestInput();

                reader.Read(result);

                Assert.AreEqual(1, result.ExpectedFailureMechanismsResults.Count);
                IExpectedFailureMechanismResult expectedFailureMechanismResult = result.ExpectedFailureMechanismsResults.First();
                Assert.AreEqual(3, expectedFailureMechanismResult.Group);
                Assert.AreEqual(MechanismType.AGK, expectedFailureMechanismResult.Type);
                Assert.AreEqual(true, expectedFailureMechanismResult.AccountForDuringAssembly);
                Assert.AreEqual(EFailureMechanismCategory.VIIt, expectedFailureMechanismResult.ExpectedAssessmentResult);
                Assert.AreEqual(EFailureMechanismCategory.IVt, expectedFailureMechanismResult.ExpectedAssessmentResultTemporal);

                var group3FailureMechanism = expectedFailureMechanismResult as IGroup3ExpectedFailureMechanismResult;
                Assert.IsNotNull(group3FailureMechanism);
                Assert.AreEqual(0.05, group3FailureMechanism.FailureMechanismProbabilitySpace);
                Assert.AreEqual(19.3, group3FailureMechanism.LengthEffectFactor, 9e-2);

                var categories = group3FailureMechanism.ExpectedFailureMechanismSectionCategories.Categories.ToArray();
                Assert.AreEqual(6, categories.Length);
                AssertAreEqualCategories(EFmSectionCategory.Iv, 0.0, 2.88e-8, categories[0]);
                AssertAreEqualCategories(EFmSectionCategory.IIv, 2.88e-8, 8.64e-7, categories[1]);
                AssertAreEqualCategories(EFmSectionCategory.IIIv, 8.64e-7, 2.59e-6, categories[2]);
                AssertAreEqualCategories(EFmSectionCategory.IVv, 2.59e-6, 1.00e-3, categories[3]);
                AssertAreEqualCategories(EFmSectionCategory.Vv, 1.00e-3, 3.00e-2, categories[4]);
                AssertAreEqualCategories(EFmSectionCategory.VIv, 3.00e-2, 1.0, categories[5]);
            }
        }

        [Test]
        public void ReaderReadsGroup2InformationCorrectly()
        {
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(testFile, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                var workSheetParts = ReadWorkSheetParts(workbookPart);
                var sTPHWorkSheetPart = workSheetParts["STPH"];

                var reader = new FailureMechanismsReader(sTPHWorkSheetPart, workbookPart);

                var result = new BenchmarkTestInput();

                reader.Read(result);

                Assert.AreEqual(1, result.ExpectedFailureMechanismsResults.Count);
                IExpectedFailureMechanismResult expectedFailureMechanismResult = result.ExpectedFailureMechanismsResults.First();
                Assert.AreEqual(2, expectedFailureMechanismResult.Group);
                Assert.AreEqual(MechanismType.STPH, expectedFailureMechanismResult.Type);
                Assert.AreEqual(true, expectedFailureMechanismResult.AccountForDuringAssembly);
                Assert.AreEqual(EFailureMechanismCategory.VIIt, expectedFailureMechanismResult.ExpectedAssessmentResult);
                Assert.AreEqual(EFailureMechanismCategory.IIt, expectedFailureMechanismResult.ExpectedAssessmentResultTemporal);

                var probabilisticFailureMechanism = expectedFailureMechanismResult as IProbabilisticExpectedFailureMechanismResult;
                Assert.IsNotNull(probabilisticFailureMechanism);
                Assert.AreEqual(0.24, probabilisticFailureMechanism.FailureMechanismProbabilitySpace);
                Assert.AreEqual(26.7, probabilisticFailureMechanism.LengthEffectFactor, 1e-1);
                AssertAreEqualProbabilities(double.NaN, probabilisticFailureMechanism.ExpectedAssessmentResultProbability);
                AssertAreEqualProbabilities(4.04e-5, probabilisticFailureMechanism.ExpectedAssessmentResultProbabilityTemporal);

                var categories = probabilisticFailureMechanism.ExpectedFailureMechanismSectionCategories.Categories.ToArray();
                Assert.AreEqual(6, categories.Length);
                AssertAreEqualCategories(EFmSectionCategory.Iv, 0.0, 9.98E-08, categories[0]);
                AssertAreEqualCategories(EFmSectionCategory.IIv, 9.98E-08, 2.99e-6, categories[1]);
                AssertAreEqualCategories(EFmSectionCategory.IIIv, 2.99e-6, 8.98e-6, categories[2]);
                AssertAreEqualCategories(EFmSectionCategory.IVv, 8.98e-6, 1.00e-3, categories[3]);
                AssertAreEqualCategories(EFmSectionCategory.Vv, 1.00e-3, 3.00e-2, categories[4]);
                AssertAreEqualCategories(EFmSectionCategory.VIv, 3.00e-2, 1.0, categories[5]);

                var categoriesFailureMechanism = probabilisticFailureMechanism.ExpectedFailureMechanismCategories.Categories.ToArray();
                Assert.AreEqual(6, categoriesFailureMechanism.Length);
                AssertAreEqualCategories(EFailureMechanismCategory.It, 0.0, 2.67e-6, categoriesFailureMechanism[0]);
                AssertAreEqualCategories(EFailureMechanismCategory.IIt, 2.67e-6, 8.00e-5, categoriesFailureMechanism[1]);
                AssertAreEqualCategories(EFailureMechanismCategory.IIIt, 8.00e-5, 2.40e-4, categoriesFailureMechanism[2]);
                AssertAreEqualCategories(EFailureMechanismCategory.IVt, 2.40e-4, 1.00e-3, categoriesFailureMechanism[3]);
                AssertAreEqualCategories(EFailureMechanismCategory.Vt, 1.00e-3, 3.00e-2, categoriesFailureMechanism[4]);
                AssertAreEqualCategories(EFailureMechanismCategory.VIt, 3.00e-2, 1.0, categoriesFailureMechanism[5]);
            }
        }

        [Test]
        public void ReaderReadsGroup1InformationCorrectly()
        {
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(testFile, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                var workSheetParts = ReadWorkSheetParts(workbookPart);
                var gEKBWorkSheetPart = workSheetParts["GEKB"];

                var reader = new FailureMechanismsReader(gEKBWorkSheetPart, workbookPart);

                var result = new BenchmarkTestInput();

                reader.Read(result);

                Assert.AreEqual(1, result.ExpectedFailureMechanismsResults.Count);
                IExpectedFailureMechanismResult expectedFailureMechanismResult = result.ExpectedFailureMechanismsResults.First();
                Assert.AreEqual(1, expectedFailureMechanismResult.Group);
                Assert.AreEqual(MechanismType.GEKB, expectedFailureMechanismResult.Type);
                Assert.AreEqual(true, expectedFailureMechanismResult.AccountForDuringAssembly);
                Assert.AreEqual(EFailureMechanismCategory.VIIt, expectedFailureMechanismResult.ExpectedAssessmentResult);
                Assert.AreEqual(EFailureMechanismCategory.IIt, expectedFailureMechanismResult.ExpectedAssessmentResultTemporal);

                var probabilisticFailureMechanism = expectedFailureMechanismResult as IProbabilisticExpectedFailureMechanismResult;
                Assert.IsNotNull(probabilisticFailureMechanism);
                Assert.AreEqual(0.24, probabilisticFailureMechanism.FailureMechanismProbabilitySpace);
                Assert.AreEqual(2.0, probabilisticFailureMechanism.LengthEffectFactor, 9e-2);
                AssertAreEqualProbabilities(double.NaN, probabilisticFailureMechanism.ExpectedAssessmentResultProbability);
                AssertAreEqualProbabilities(4.88e-5, probabilisticFailureMechanism.ExpectedAssessmentResultProbabilityTemporal);

                var categories = probabilisticFailureMechanism.ExpectedFailureMechanismSectionCategories.Categories.ToArray();
                Assert.AreEqual(6, categories.Length);
                AssertAreEqualCategories(EFmSectionCategory.Iv, 0.0, 1.33e-6, categories[0]);
                AssertAreEqualCategories(EFmSectionCategory.IIv, 1.33e-6, 4.00e-5, categories[1]);
                AssertAreEqualCategories(EFmSectionCategory.IIIv, 4.00e-5, 1.20e-4, categories[2]);
                AssertAreEqualCategories(EFmSectionCategory.IVv, 1.20e-4, 1.00e-3, categories[3]);
                AssertAreEqualCategories(EFmSectionCategory.Vv, 1.00e-3, 3.00e-2, categories[4]);
                AssertAreEqualCategories(EFmSectionCategory.VIv, 3.00e-2, 1.0, categories[5]);

                var categoriesFailureMechanism = probabilisticFailureMechanism.ExpectedFailureMechanismCategories.Categories.ToArray();
                Assert.AreEqual(6, categoriesFailureMechanism.Length);
                AssertAreEqualCategories(EFailureMechanismCategory.It, 0.0, 2.67e-6, categoriesFailureMechanism[0]);
                AssertAreEqualCategories(EFailureMechanismCategory.IIt, 2.67e-6, 8.00e-5, categoriesFailureMechanism[1]);
                AssertAreEqualCategories(EFailureMechanismCategory.IIIt, 8.00e-5, 2.40e-4, categoriesFailureMechanism[2]);
                AssertAreEqualCategories(EFailureMechanismCategory.IVt, 2.40e-4, 1.00e-3, categoriesFailureMechanism[3]);
                AssertAreEqualCategories(EFailureMechanismCategory.Vt, 1.00e-3, 3.00e-2, categoriesFailureMechanism[4]);
                AssertAreEqualCategories(EFailureMechanismCategory.VIt, 3.00e-2, 1.0, categoriesFailureMechanism[5]);
            }
        }

        [Test]
        public void ReaderReadsGroup4InformationCorrectly()
        {
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(testFile, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                var workSheetParts = ReadWorkSheetParts(workbookPart);
                var sTKWlWorkSheetPart = workSheetParts["STKWl"];

                var reader = new FailureMechanismsReader(sTKWlWorkSheetPart, workbookPart);

                var result = new BenchmarkTestInput();

                reader.Read(result);

                Assert.AreEqual(1, result.ExpectedFailureMechanismsResults.Count);
                IExpectedFailureMechanismResult expectedFailureMechanismResult = result.ExpectedFailureMechanismsResults.First();
                Assert.AreEqual(4, expectedFailureMechanismResult.Group);
                Assert.AreEqual(MechanismType.STKWl, expectedFailureMechanismResult.Type);
                Assert.AreEqual(true, expectedFailureMechanismResult.AccountForDuringAssembly);
                Assert.AreEqual(EFailureMechanismCategory.VIIt, expectedFailureMechanismResult.ExpectedAssessmentResult);
                Assert.AreEqual(EFailureMechanismCategory.IIt, expectedFailureMechanismResult.ExpectedAssessmentResultTemporal);
            }
        }

        [Test]
        public void ReaderReadsSTBUInformationCorrectly()
        {
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(testFile, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                var workSheetParts = ReadWorkSheetParts(workbookPart);
                var sTBUWorkSheetPart = workSheetParts["STBU"];

                var reader = new FailureMechanismsReader(sTBUWorkSheetPart, workbookPart);

                var result = new BenchmarkTestInput();

                reader.Read(result);

                Assert.AreEqual(1, result.ExpectedFailureMechanismsResults.Count);
                IExpectedFailureMechanismResult expectedFailureMechanismResult = result.ExpectedFailureMechanismsResults.First();
                Assert.AreEqual(4, expectedFailureMechanismResult.Group);
                Assert.AreEqual(MechanismType.STBU, expectedFailureMechanismResult.Type);
                Assert.AreEqual(true, expectedFailureMechanismResult.AccountForDuringAssembly);
                Assert.AreEqual(EFailureMechanismCategory.VIIt, expectedFailureMechanismResult.ExpectedAssessmentResult);
                Assert.AreEqual(EFailureMechanismCategory.Vt, expectedFailureMechanismResult.ExpectedAssessmentResultTemporal);

                var stbuFailureMechanism = expectedFailureMechanismResult as StbuExpectedFailureMechanismResult;
                Assert.IsNotNull(stbuFailureMechanism);
                Assert.AreEqual(0.04, stbuFailureMechanism.FailureMechanismProbabilitySpace);
                Assert.AreEqual(13.7, stbuFailureMechanism.LengthEffectFactor, 9e-2);
                AssertAreEqualProbabilities(9.71e-6, stbuFailureMechanism.ExpectedSectionsCategoryDivisionProbability);
            }
        }

        [Test]
        public void ReaderReadsGroup5InformationCorrectly()
        {
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(testFile, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                var workSheetParts = ReadWorkSheetParts(workbookPart);
                var vLGAWorkSheetPart = workSheetParts["VLGA"];

                var reader = new FailureMechanismsReader(vLGAWorkSheetPart, workbookPart);

                var result = new BenchmarkTestInput();

                reader.Read(result);

                Assert.AreEqual(1, result.ExpectedFailureMechanismsResults.Count);
                IExpectedFailureMechanismResult expectedFailureMechanismResult = result.ExpectedFailureMechanismsResults.First();
                Assert.AreEqual(5, expectedFailureMechanismResult.Group);
                Assert.AreEqual(MechanismType.VLGA, expectedFailureMechanismResult.Type);
                Assert.AreEqual(true, expectedFailureMechanismResult.AccountForDuringAssembly);
                Assert.AreEqual(EIndirectAssessmentResult.Ngo, expectedFailureMechanismResult.ExpectedAssessmentResult);
                Assert.AreEqual(EIndirectAssessmentResult.FactoredInOtherFailureMechanism, expectedFailureMechanismResult.ExpectedAssessmentResultTemporal);
            }
        }
    }
}
