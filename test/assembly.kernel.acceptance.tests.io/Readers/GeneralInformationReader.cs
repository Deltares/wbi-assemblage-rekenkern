using System.Collections.Generic;
using assembly.kernel.acceptance.tests.data;
using assembly.kernel.acceptance.tests.data.Input;
using Assembly.Kernel.Model.CategoryLimits;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.acceptance.tests.io.Readers
{
    public class GeneralInformationReader : ExcelSheetReaderBase
    {
        public GeneralInformationReader(WorksheetPart worksheetPart, WorkbookPart workbookPart) : base(worksheetPart, workbookPart) { }

        public void Read(AcceptanceTestInput acceptanceTestInput)
        {
            acceptanceTestInput.SignallingNorm = GetCellValueAsDouble("D","Signaleringswaarde [terugkeertijd]");
            acceptanceTestInput.LowerBoundaryNorm = GetCellValueAsDouble("D","Ondergrens [terugkeertijd]");
            acceptanceTestInput.Length = GetCellValueAsDouble("B", "Trajectlengte [m]");
            acceptanceTestInput.Name = GetCellValueAsString("B", "Dijktraject");

            var list = new List<AssessmentSectionCategory>();
            var startRowCategories = GetRowId("Categorie") + 1;
            for (int iRow = startRowCategories; iRow <= startRowCategories + 4; iRow++)
            {
                list.Add(new AssessmentSectionCategory(
                    GetCellValueAsString("A", iRow).ToAssessmentGrade(),
                    GetCellValueAsDouble("B", iRow),
                    GetCellValueAsDouble("C", iRow)));
            }

            acceptanceTestInput.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssessmentSectionCategories = new CategoriesList<AssessmentSectionCategory>(list);
        }
    }
}
