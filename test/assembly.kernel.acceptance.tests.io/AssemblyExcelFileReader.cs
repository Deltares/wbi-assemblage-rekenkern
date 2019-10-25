using System.Collections.Generic;
using System.IO;
using System.Linq;
using assembly.kernel.acceptance.tests.data;
using assembly.kernel.acceptance.tests.data.FailureMechanisms;
using assembly.kernel.acceptance.tests.io.Readers;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.acceptance.tests.io
{
    public static class AssemblyExcelFileReader
    {
        public static AssessmentSection Read(string excelFileName)
        {
            if (!File.Exists(excelFileName))
            {
                return null;
            }

            var assessmentSection = new AssessmentSection();

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(excelFileName, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                var workSheetParts = ExcelReaderHelper.ReadWorkSheetParts(workbookPart);

                ReadGeneralAssessmentSectionInformation(workSheetParts["Trajectgegevens"], workbookPart, assessmentSection);

                // TODO: Implement correctly
                ReadFailureMechanismTab(workSheetParts["STBI"], workbookPart, assessmentSection);
                /*
                ReadFailureMechanismTab(workSheetParts["STBU"], workbookPart, assessmentSection);
                ReadFailureMechanismTab(workSheetParts["STPH"], workbookPart, assessmentSection);
                ReadFailureMechanismTab(workSheetParts["STMI"], workbookPart, assessmentSection);
                ReadFailureMechanismTab(workSheetParts["AGK"], workbookPart, assessmentSection);
                ReadFailureMechanismTab(workSheetParts["AWO"], workbookPart, assessmentSection);
                ReadFailureMechanismTab(workSheetParts["GEBU"], workbookPart, assessmentSection);
                ReadFailureMechanismTab(workSheetParts["GABU"], workbookPart, assessmentSection);
                ReadFailureMechanismTab(workSheetParts["GEKB"], workbookPart, assessmentSection);
                ReadFailureMechanismTab(workSheetParts["GABI"], workbookPart, assessmentSection);
                ReadFailureMechanismTab(workSheetParts["ZST"], workbookPart, assessmentSection);
                ReadFailureMechanismTab(workSheetParts["DA"], workbookPart, assessmentSection);
                ReadFailureMechanismTab(workSheetParts["HTKW"], workbookPart, assessmentSection);
                ReadFailureMechanismTab(workSheetParts["BSKW"], workbookPart, assessmentSection);
                ReadFailureMechanismTab(workSheetParts["PKW"], workbookPart, assessmentSection);
                ReadFailureMechanismTab(workSheetParts["STKWp"], workbookPart, assessmentSection);
                ReadFailureMechanismTab(workSheetParts["STKWl"], workbookPart, assessmentSection);
                ReadFailureMechanismTab(workSheetParts["VLGA"], workbookPart, assessmentSection);
                ReadFailureMechanismTab(workSheetParts["VLAF"], workbookPart, assessmentSection);
                ReadFailureMechanismTab(workSheetParts["VLZV"], workbookPart, assessmentSection);
                ReadFailureMechanismTab(workSheetParts["NWObe"], workbookPart, assessmentSection);
                ReadFailureMechanismTab(workSheetParts["NWObo"], workbookPart, assessmentSection);
                ReadFailureMechanismTab(workSheetParts["NWOkl"], workbookPart, assessmentSection);
                ReadFailureMechanismTab(workSheetParts["NWOoc"], workbookPart, assessmentSection);
                ReadFailureMechanismTab(workSheetParts["HAV"], workbookPart, assessmentSection);
                ReadFailureMechanismTab(workSheetParts["INN"], workbookPart, assessmentSection);
                */

                ReadSafetyAssessmentFinalResult(workSheetParts["Gecombineerd veiligheidsoordeel"], workbookPart, assessmentSection);

                ReadCombinedAssessmentSectionResults(workSheetParts["Gecombineerd totaal vakoordeel"], workbookPart, assessmentSection);

                return assessmentSection;
            }
        }

        private static void ReadGeneralAssessmentSectionInformation(WorksheetPart workSheetPart,
            WorkbookPart workbookPart, AssessmentSection assessmentSection)
        {
            new GeneralInformationReader(workSheetPart, workbookPart).Read(assessmentSection);
        }

        private static void ReadSafetyAssessmentFinalResult(WorksheetPart worksheetPart, WorkbookPart workbookPart,
            AssessmentSection assessmentSection)
        {
            new SafetyAssessmentFinalResultReader(worksheetPart, workbookPart).Read(assessmentSection);
        }

        private static void ReadCombinedAssessmentSectionResults(WorksheetPart worksheetPart,
            WorkbookPart workbookPart, AssessmentSection assessmentSection)
        {
            new CommonAssessmentSectionResultsReader(worksheetPart, workbookPart).Read(assessmentSection);
        }

        private static void ReadFailureMechanismTab(WorksheetPart worksheetPart, WorkbookPart workbookPart, AssessmentSection assessmentSection)
        {
            var maxRow = ExcelReaderHelper.GetMaxRow(worksheetPart);
            var keywordsDictionary = ExcelReaderHelper.ReadKeywordsDictionary(worksheetPart, workbookPart, maxRow);

            var group = ExcelReaderHelper.GetCellValueAsInt(worksheetPart.Worksheet,
                "B" + ExcelReaderHelper.GetRowId("Toetsspoorgroep", keywordsDictionary), workbookPart);
            if (group > 4)
            {
                return;
            }

            var code = ExcelReaderHelper.GetCellValueAsString(worksheetPart.Worksheet,
                    "B" + ExcelReaderHelper.GetRowId("Toetsspoor", keywordsDictionary), workbookPart)
                .ToMechanismType();
            var isRelevant = ExcelReaderHelper.GetCellValueAsString(worksheetPart.Worksheet, "D1", workbookPart) == "Ja";
            var isProbabilisticMechanism = group == 1 || group == 2;

            var failureMechanism = GetFailureMechanism(assessmentSection, code, isProbabilisticMechanism);

            ReadFailureMechanismAssessmentResultFromTab(worksheetPart, workbookPart, keywordsDictionary, failureMechanism, isRelevant, isProbabilisticMechanism);

            if (group < 4)
            {
                // TODO: Split for different types of mechanisms and put in separate class
                /*failureMechanism.ContributionFactor = ExcelReaderHelper.GetCellValueAsDouble(worksheetPart.Worksheet,
                    "B" + ExcelReaderHelper.GetRowId("ω Faalkansruimtefactor", keywordsDictionary), workbookPart);
                failureMechanism.LengthEffectFactor = ExcelReaderHelper.GetCellValueAsDouble(worksheetPart.Worksheet,
                    "B" + ExcelReaderHelper.GetRowId("Ndsn (lengte effectfactor)", keywordsDictionary), workbookPart);*/
            }

            if (isRelevant)
            {
                ReadFailureMechanismSectionAssessmentResultsFromTab(worksheetPart, workbookPart, assessmentSection, failureMechanism, keywordsDictionary, maxRow, isProbabilisticMechanism);
            }
        }

        private static IFailureMechanism GetFailureMechanism(AssessmentSection assessmentSection, MechanismType code, bool isProbabilisticMechanism)
        {
            var failureMechanism = assessmentSection.FailureMechanisms.FirstOrDefault(f => f.Type == code);
            if (failureMechanism == null)
            {
                // TODO: Call factory based on type/group
                //failureMechanism = isProbabilisticMechanism ? new FailureMechanismWithProbability() : new FailureMechanism();
                assessmentSection.FailureMechanisms.Add(failureMechanism);
            }

            return failureMechanism;
        }

        private static void ReadFailureMechanismSectionAssessmentResultsFromTab(WorksheetPart worksheetPart,
            WorkbookPart workbookPart, AssessmentSection assessmentSection, IFailureMechanism failureMechanism, Dictionary<string, int> keywordsDictionary, int maxRow, bool isProbabilisticMechanism)
        {
            // TODO: Split to determin type of section and way to read (move to separate class).
            var sections = new List<IFailureMechanismSection>();
            var iRow = GetStartRowIndexForFailureMechanismSectionResults(keywordsDictionary);

            while (iRow <= maxRow)
            {
                var startMeters = ExcelReaderHelper.GetCellValueAsDouble(worksheetPart.Worksheet, "A" + iRow, workbookPart) * 1000.0;
                var endMeters = ExcelReaderHelper.GetCellValueAsDouble(worksheetPart.Worksheet, "B" + iRow, workbookPart) * 1000.0;
                
                if (double.IsNaN(startMeters) || double.IsNaN(endMeters))
                {
                    break;
                }

                // TODO: Read section details and initialize section
                /*var failureMechanismSection = new FailureMechanismSection
                {
                    Name = ExcelReaderHelper.GetCellValueAsString(worksheetPart.Worksheet, "E" + iRow, workbookPart),
                    StartMeters = startMeters,
                    EndMeters = endMeters,
                    AssessmentResult = ExcelReaderHelper
                        .GetCellValueAsString(worksheetPart.Worksheet, "M" + iRow, workbookPart)
                        .ToFailureMechanismSectionAssemblyCategoryGroup(),
                    SimpleAssessmentResult = ExcelReaderHelper
                        .GetCellValueAsString(worksheetPart.Worksheet, "J" + iRow, workbookPart)
                        .ToFailureMechanismSectionAssemblyCategoryGroup(),
                    DetailedAssessmentResult = ExcelReaderHelper
                        .GetCellValueAsString(worksheetPart.Worksheet, "K" + iRow, workbookPart)
                        .ToFailureMechanismSectionAssemblyCategoryGroup(),
                    TailorMadeAssessmentResult = ExcelReaderHelper
                        .GetCellValueAsString(worksheetPart.Worksheet, "L" + iRow, workbookPart)
                        .ToFailureMechanismSectionAssemblyCategoryGroup(),
                    HasProbabilities = isProbabilisticMechanism,
                };*/

                if (isProbabilisticMechanism)
                {
                    /*var fColumnValueAsString =
                        ExcelReaderHelper.GetCellValueAsString(worksheetPart.Worksheet, "F" + iRow, workbookPart);
                    failureMechanismSection.SimpleAssessmentResultProbability =
                        fColumnValueAsString == "FV" || fColumnValueAsString == "NVT" ? 0.0 : double.NaN;

                    failureMechanismSection.DetailedAssessmentResultProbability =
                        ExcelReaderHelper.GetCellValueAsString(worksheetPart.Worksheet, "G" + iRow, workbookPart) == "NGO"
                            ? double.NaN
                            : ExcelReaderHelper.GetCellValueAsDouble(worksheetPart.Worksheet, "G" + iRow, workbookPart);

                    var hColumnValueAsString =
                        ExcelReaderHelper.GetCellValueAsString(worksheetPart.Worksheet, "H" + iRow, workbookPart);
                    failureMechanismSection.TailorMadeAssessmentResultProbability =
                        hColumnValueAsString == "NGO"
                            ? double.NaN
                            : hColumnValueAsString == "FV"
                                ? 0.0
                                : ExcelReaderHelper.GetCellValueAsDouble(worksheetPart.Worksheet, "H" + iRow, workbookPart);

                    failureMechanismSection.CombinedAssessmentResultProbability =
                        ExcelReaderHelper.GetCellValueAsDouble(worksheetPart.Worksheet, "P" + iRow, workbookPart);*/
                }

                //sections.Add(failureMechanismSection);
                iRow++;
            }

            failureMechanism.Sections = sections;
        }

        private static void ReadFailureMechanismAssessmentResultFromTab(WorksheetPart worksheetPart, WorkbookPart workbookPart,
            Dictionary<string, int> keywordsDictionary, IFailureMechanism failureMechanism, bool isRelevant, bool isProbabilisticMechanism)
        {
            var rowId = ExcelReaderHelper.GetRowId("Toetsoordeel per toetsspoor per traject", keywordsDictionary);
            if (rowId == -1)
            {
                rowId = ExcelReaderHelper.GetRowId("Toetsspooroordeel per toetsspoor per traject", keywordsDictionary);
            }

            failureMechanism.ExpectedAssessmentResult = ExcelReaderHelper.GetCellValueAsString(worksheetPart.Worksheet,"D" + rowId,workbookPart).ToFailureMechanismCategory();

            /*if (isProbabilisticMechanism)
            {
                ((FailureMechanismWithProbability)failureMechanism).Probability = ExcelReaderHelper.GetCellValueAsDouble(worksheetPart.Worksheet, "F" + rowId, workbookPart);
            }*/
        }

        private static int GetStartRowIndexForFailureMechanismSectionResults(Dictionary<string, int> keywordsDictionary)
        {
            var iRow = ExcelReaderHelper.GetRowId("Toetsresultaat per toetsspoor per vak", keywordsDictionary);
            if (iRow == -1)
            {
                iRow = ExcelReaderHelper.GetRowId("Toetsresultaat per toetsspoor per kunstwerk", keywordsDictionary);
            }
            if (iRow == -1)
            {
                iRow = ExcelReaderHelper.GetRowId("Toetsspooroordeel per toetsspoor per vak", keywordsDictionary);
            }
            if (iRow == -1)
            {
                iRow = ExcelReaderHelper.GetRowId("Toetsoordeel per toetsspoor per vak", keywordsDictionary);
            }

            iRow = iRow + 4;
            return iRow;
        }
    }
}
