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

                ReadFailureMechanism(workSheetParts["STBI"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["STBU"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["STPH"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["STMI"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["AGK"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["AWO"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["GEBU"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["GABU"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["GEKB"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["GABI"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["ZST"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["DA"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["HTKW"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["BSKW"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["PKW"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["STKWp"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["STKWl"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["VLGA"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["VLAF"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["VLZV"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["NWObe"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["NWObo"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["NWOkl"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["NWOoc"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["HAV"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["INN"], workbookPart, assessmentSection);

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

        private static void ReadFailureMechanism(WorksheetPart worksheetPart,
            WorkbookPart workbookPart, AssessmentSection assessmentSection)
        {
            new FailureMechanismsReader(worksheetPart, workbookPart).Read(assessmentSection);
        }
    }
}
