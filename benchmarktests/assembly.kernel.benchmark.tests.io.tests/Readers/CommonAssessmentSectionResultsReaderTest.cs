using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using assembly.kernel.benchmark.tests.data.Input;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.io.Readers;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FmSectionTypes;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using NUnit.Framework;

namespace assembly.kernel.benchmark.tests.io.tests.Readers
{
    [TestFixture]
    public class CommonAssessmentSectionResultsReaderTest : TestFileReaderTestBase
    {
        private MechanismType[] DirectMechanismTypes =
        {
            MechanismType.STBI, MechanismType.STBU, MechanismType.STPH, MechanismType.STMI, MechanismType.AGK,
            MechanismType.AWO, MechanismType.GEBU, MechanismType.GABU, MechanismType.GEKB, MechanismType.GABI,
            MechanismType.ZST, MechanismType.DA, MechanismType.HTKW, MechanismType.BSKW, MechanismType.PKW,
            MechanismType.STKWp, MechanismType.STKWl, MechanismType.INN, 
        };

        private EFmSectionCategory[] ExpectedDirectResults =
        {
            EFmSectionCategory.NotApplicable, EFmSectionCategory.Iv, EFmSectionCategory.Iv, EFmSectionCategory.IIv, EFmSectionCategory.Iv,
            EFmSectionCategory.IIv, EFmSectionCategory.Iv, EFmSectionCategory.Iv, EFmSectionCategory.Iv, EFmSectionCategory.Iv,
            EFmSectionCategory.NotApplicable, EFmSectionCategory.Iv, EFmSectionCategory.Iv, EFmSectionCategory.IIIv, EFmSectionCategory.Iv,
            EFmSectionCategory.NotApplicable, EFmSectionCategory.Iv, EFmSectionCategory.Iv
        };

        private MechanismType[] IndirectMechanismTypes =
        {
            MechanismType.VLGA, MechanismType.VLAF, MechanismType.VLZV, MechanismType.NWObe, MechanismType.NWObo,
            MechanismType.NWOkl, MechanismType.NWOoc, MechanismType.HAV
        };

        private EIndirectAssessmentResult[] ExpectedIndirectResults =
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
                        AssertResultsIsAsExpected(6700, 7100, ExpectedDirectResults[Array.IndexOf(DirectMechanismTypes,type)], sectionWithDirectCategory);
                    }
                    if (ninethSection is FmSectionWithIndirectCategory)
                    {
                        var sectionWithIndirectCategory = (FmSectionWithIndirectCategory)ninethSection;
                        AssertResultsIsAsExpected(6700, 7100, ExpectedIndirectResults[Array.IndexOf(IndirectMechanismTypes, type)], sectionWithIndirectCategory);
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

        private static Dictionary<string, WorksheetPart> ReadWorkSheetParts(WorkbookPart workbookPart)
        {
            var workSheetParts = new Dictionary<string, WorksheetPart>();

            foreach (var worksheetPart in workbookPart.WorksheetParts)
            {
                var sheet = GetSheetFromWorkSheet(workbookPart, worksheetPart);
                workSheetParts[sheet.Name] = worksheetPart;
            }

            return workSheetParts;
        }

        private static Sheet GetSheetFromWorkSheet
            (WorkbookPart workbookPart, WorksheetPart worksheetPart)
        {
            string relationshipId = workbookPart.GetIdOfPart(worksheetPart);
            IEnumerable<Sheet> sheets = workbookPart.Workbook.Sheets.Elements<Sheet>();
            return sheets.FirstOrDefault(s => s.Id.HasValue && s.Id.Value == relationshipId);
        }
    }
}
