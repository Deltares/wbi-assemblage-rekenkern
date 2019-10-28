using System.IO;
using assembly.kernel.acceptance.tests.data;
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

                ReadFailureMechanismTab(workSheetParts["STBI"], workbookPart, assessmentSection);
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

        private static void ReadFailureMechanismTab(WorksheetPart worksheetPart,
            WorkbookPart workbookPart, AssessmentSection assessmentSection)
        {
            new FailureMechanismsReader(worksheetPart, workbookPart).Read(assessmentSection);
        }
    }
}
