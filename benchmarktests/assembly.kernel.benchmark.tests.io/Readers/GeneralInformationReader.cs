using System.Collections.Generic;
using assembly.kernel.benchmark.tests.data.Input;
using Assembly.Kernel.Model.CategoryLimits;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.benchmark.tests.io.Readers
{
    public class GeneralInformationReader : ExcelSheetReaderBase
    {
        /// <summary>
        /// Creates an instance of the GeneralInformationReader, used to read general information of an assessment section.
        /// </summary>
        /// <param name="worksheetPart"></param>
        /// <param name="workbookPart"></param>
        public GeneralInformationReader(WorksheetPart worksheetPart, WorkbookPart workbookPart) : base(worksheetPart, workbookPart) { }

        /// <summary>
        /// Reads the general information of an assessment section.
        /// </summary>
        /// <param name="benchmarkTestInput"></param>
        public void Read(BenchmarkTestInput benchmarkTestInput)
        {
            benchmarkTestInput.SignallingNorm = GetCellValueAsDouble("D","Signaleringswaarde [terugkeertijd]");
            benchmarkTestInput.LowerBoundaryNorm = GetCellValueAsDouble("D","Ondergrens [terugkeertijd]");
            benchmarkTestInput.Length = GetCellValueAsDouble("B", "Trajectlengte [m]");

            var list = new List<AssessmentSectionCategory>();
            var startRowCategories = GetRowId("Categorie") + 1;
            for (int iRow = startRowCategories; iRow <= startRowCategories + 4; iRow++)
            {
                list.Add(new AssessmentSectionCategory(
                    GetCellValueAsString("A", iRow).ToAssessmentGrade(),
                    GetCellValueAsDouble("B", iRow),
                    GetCellValueAsDouble("C", iRow)));
            }

            benchmarkTestInput.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssessmentSectionCategories = new CategoriesList<AssessmentSectionCategory>(list);
        }
    }
}
