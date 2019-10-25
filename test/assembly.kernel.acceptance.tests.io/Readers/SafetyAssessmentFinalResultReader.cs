using System.Collections.Generic;
using assembly.kernel.acceptance.tests.data;
using Assembly.Kernel.Model.CategoryLimits;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.acceptance.tests.io.Readers
{
    public class SafetyAssessmentFinalResultReader : ExcelSheetReaderBase
    {
        public SafetyAssessmentFinalResultReader(WorksheetPart worksheetPart, WorkbookPart workbookPart) : base(worksheetPart, workbookPart)
        {
        }

        public void Read(AssessmentSection assessmentSection)
        {
            assessmentSection.SafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups1and2 =
                GetCellValueAsString("F", "Toetssporen in groep 1 en 2").ToFailureMechanismCategory();
            assessmentSection.SafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups1and2Probability =
                GetCellValueAsDouble("G", "Toetssporen in groep 1 en 2");
            assessmentSection.SafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups3and4 =
                GetCellValueAsString("F", "Toetssporen in groep 3 en 4").ToFailureMechanismCategory();
            assessmentSection.SafetyAssessmentAssemblyResult.ExpectedSafetyAssessmentAssemblyResult =
                GetCellValueAsString("F", "Combineren tot veiligheidsoordeel").ToAssessmentGrade();

            assessmentSection.SafetyAssessmentAssemblyResult.CombinedFailureMechanismProbabilitySpace =
                GetCellValueAsDouble("M", 10);

            var list = new List<FailureMechanismCategory>();
            var startRowCategories = GetRowId("Categorie") + 1;
            for (int iRow = startRowCategories; iRow <= startRowCategories + 5; iRow++)
            {
                list.Add(new FailureMechanismCategory(
                    GetCellValueAsString("D", iRow).ToFailureMechanismCategory(),
                    GetCellValueAsDouble("E", iRow),
                    GetCellValueAsDouble("F", iRow)));
            }

            assessmentSection.SafetyAssessmentAssemblyResult.ExpectedFailureMechanismCategories = new CategoriesList<FailureMechanismCategory>(list);
        }
    }
}
