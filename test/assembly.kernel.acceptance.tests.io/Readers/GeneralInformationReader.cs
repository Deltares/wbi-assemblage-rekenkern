using System.Collections.Generic;
using assembly.kernel.acceptance.tests.data;
using Assembly.Kernel.Model.CategoryLimits;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.acceptance.tests.io.Readers
{
    public class GeneralInformationReader : ExcelSheetReaderBase
    {
        public GeneralInformationReader(WorksheetPart worksheetPart, WorkbookPart workbookPart) : base(worksheetPart, workbookPart) { }

        public void Read(AssessmentSection assessmentSection)
        {
            assessmentSection.SignallingNorm = GetCellValueAsDouble("D","Signaleringswaarde [terugkeertijd]");
            assessmentSection.LowerBoundaryNorm = GetCellValueAsDouble("D","Ondergrens [terugkeertijd]");
            assessmentSection.Length = GetCellValueAsDouble("B", "Trajectlengte [m]");
            assessmentSection.Name = GetCellValueAsString("B", "Dijktraject");

            var list = new List<AssessmentSectionCategory>();
            var startRowCategories = GetRowId("Categorie") + 1;
            for (int iRow = startRowCategories; iRow <= startRowCategories + 4; iRow++)
            {
                list.Add(new AssessmentSectionCategory(
                    GetCellValueAsString("A", iRow).ToAssessmentGrade(),
                    GetCellValueAsDouble("B", iRow),
                    GetCellValueAsDouble("C", iRow)));
            }

            assessmentSection.SafetyAssessmentAssemblyResult.ExpectedAssessmentSectionCategories = new CategoriesList<AssessmentSectionCategory>(list);
        }
    }
}
