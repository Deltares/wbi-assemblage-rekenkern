using System;
using System.IO;
using System.Linq;
using assembly.kernel.benchmark.tests.data.Input;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.io.Readers;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FmSectionTypes;
using DocumentFormat.OpenXml.Packaging;
using NUnit.Framework;

namespace assembly.kernel.benchmark.tests.io.tests.Readers
{
    [TestFixture]
    public class CommonAssessmentSectionResultsReaderTest : TestFileReaderTestBase
    {
        private readonly MechanismType[] directMechanismTypes =
        {
            MechanismType.STBI, MechanismType.STBU, MechanismType.STPH, MechanismType.STMI, MechanismType.AGK,
            MechanismType.AWO, MechanismType.GEBU, MechanismType.GABU, MechanismType.GEKB, MechanismType.GABI,
            MechanismType.ZST, MechanismType.DA, MechanismType.HTKW, MechanismType.BSKW, MechanismType.PKW,
            MechanismType.STKWp, MechanismType.STKWl, MechanismType.INN, 
        };

        private readonly EFmSectionCategory[] expectedDirectResults =
        {
            EFmSectionCategory.NotApplicable, EFmSectionCategory.Iv, EFmSectionCategory.Iv, EFmSectionCategory.IIv, EFmSectionCategory.Iv,
            EFmSectionCategory.IIv, EFmSectionCategory.Iv, EFmSectionCategory.Iv, EFmSectionCategory.Iv, EFmSectionCategory.Iv,
            EFmSectionCategory.NotApplicable, EFmSectionCategory.Iv, EFmSectionCategory.Iv, EFmSectionCategory.IIIv, EFmSectionCategory.Iv,
            EFmSectionCategory.NotApplicable, EFmSectionCategory.Iv, EFmSectionCategory.Iv
        };

        private readonly MechanismType[] indirectMechanismTypes =
        {
            MechanismType.VLGA, MechanismType.VLAF, MechanismType.VLZV, MechanismType.NWObe, MechanismType.NWObo,
            MechanismType.NWOkl, MechanismType.NWOoc, MechanismType.HAV
        };

        private readonly EIndirectAssessmentResult[] expectedIndirectResults =
        {
            EIndirectAssessmentResult.FvEt, EIndirectAssessmentResult.FvEt, EIndirectAssessmentResult.FvEt, EIndirectAssessmentResult.FvEt, EIndirectAssessmentResult.FvEt,
            EIndirectAssessmentResult.FvEt, EIndirectAssessmentResult.FactoredInOtherFailureMechanism, EIndirectAssessmentResult.FvEt
        };

        [Test]
        public void ReaderReadsInformationCorrectly()
        {
            var testFile = Path.Combine(GetTestDir(), "Benchmarktool Excel assemblagetool (v1_0_1_0) 0_03.xlsm");

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(testFile, false))
            {

                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                var workSheetParts = ReadWorkSheetParts(workbookPart);
                var workSheetPart = workSheetParts["Gecombineerd totaal vakoordeel"];

                var reader = new CommonAssessmentSectionResultsReader(workSheetPart, workbookPart);

                var result = new BenchmarkTestInput();

                reader.Read(result);

                Assert.AreEqual(40, result.ExpectedCombinedSectionResult.Count());
                AssertResultsIsAsExpected(6700, 7100, EFmSectionCategory.IIIv, result.ExpectedCombinedSectionResult.ElementAt(9));
                AssertResultsIsAsExpected(11800,  12100, EFmSectionCategory.Vv, result.ExpectedCombinedSectionResult.ElementAt(18));
                AssertResultsIsAsExpected(12100, 12700, EFmSectionCategory.VIIv, result.ExpectedCombinedSectionResult.ElementAt(19));

                AssertResultsIsAsExpected(6700, 7100, EFmSectionCategory.IIIv, result.ExpectedCombinedSectionResultTemporal.ElementAt(9));
                AssertResultsIsAsExpected(11800, 12100, EFmSectionCategory.Vv, result.ExpectedCombinedSectionResultTemporal.ElementAt(18));
                AssertResultsIsAsExpected(12100, 12700, EFmSectionCategory.Vv, result.ExpectedCombinedSectionResultTemporal.ElementAt(19));

                Assert.AreEqual(26, result.ExpectedCombinedSectionResultPerFailureMechanism.Count());
                foreach (var failureMechanismSectionList in result.ExpectedCombinedSectionResultPerFailureMechanism)
                {
                    Assert.AreEqual(40, failureMechanismSectionList.Sections.Count());
                    FailureMechanismSection ninethSection = failureMechanismSectionList.Sections.ElementAt(9);
                    var type = failureMechanismSectionList.FailureMechanismId.ToMechanismType();
                    if (ninethSection is FmSectionWithDirectCategory)
                    {
                        var sectionWithDirectCategory = (FmSectionWithDirectCategory) ninethSection;
                        AssertResultsIsAsExpected(6700, 7100, expectedDirectResults[Array.IndexOf(directMechanismTypes,type)], sectionWithDirectCategory);
                    }
                    if (ninethSection is FmSectionWithIndirectCategory)
                    {
                        var sectionWithIndirectCategory = (FmSectionWithIndirectCategory)ninethSection;
                        AssertResultsIsAsExpected(6700, 7100, expectedIndirectResults[Array.IndexOf(indirectMechanismTypes, type)], sectionWithIndirectCategory);
                    }
                }
            }
        }

        private void AssertResultsIsAsExpected(double start, double end, EIndirectAssessmentResult category, FmSectionWithIndirectCategory section)
        {
            Assert.AreEqual(start, section.SectionStart);
            Assert.AreEqual(end, section.SectionEnd);
            Assert.AreEqual(category, section.Category);
        }

        private void AssertResultsIsAsExpected(double start, double end, EFmSectionCategory category, FmSectionWithDirectCategory section)
        {
            Assert.AreEqual(start, section.SectionStart);
            Assert.AreEqual(end, section.SectionEnd);
            Assert.AreEqual(category, section.Category);
        }
    }
}