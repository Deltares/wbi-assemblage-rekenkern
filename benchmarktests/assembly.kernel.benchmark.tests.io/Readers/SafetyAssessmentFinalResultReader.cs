using System.Collections.Generic;
using assembly.kernel.benchmark.tests.data.Input;
using Assembly.Kernel.Model.CategoryLimits;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.benchmark.tests.io.Readers
{
    public class SafetyAssessmentFinalResultReader : ExcelSheetReaderBase
    {
        /// <summary>
        /// Creates an instance of the SafetyAssessmentFinalResultReader.
        /// </summary>
        /// <param name="worksheetPart"></param>
        /// <param name="workbookPart"></param>
        public SafetyAssessmentFinalResultReader(WorksheetPart worksheetPart, WorkbookPart workbookPart) : base(worksheetPart, workbookPart)
        {
        }

        /// <summary>
        /// Reads the final verdict worksheet of a benchmark test definition.
        /// </summary>
        /// <param name="benchmarkTestInput"></param>
        public void Read(BenchmarkTestInput benchmarkTestInput)
        {
            benchmarkTestInput.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups1and2 =
                GetCellValueAsString("D", "Toetssporen in groep 1 en 2").ToFailureMechanismCategory();
            benchmarkTestInput.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups1and2Probability =
                GetCellValueAsDouble("E", "Toetssporen in groep 1 en 2");
            benchmarkTestInput.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups3and4 =
                GetCellValueAsString("D", "Toetssporen in groep 3 en 4").ToFailureMechanismCategory();
            benchmarkTestInput.ExpectedSafetyAssessmentAssemblyResult.ExpectedSafetyAssessmentAssemblyResult =
                GetCellValueAsString("D", "Combineren tot veiligheidsoordeel").ToAssessmentGrade();

            benchmarkTestInput.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups1and2Temporal =
                GetCellValueAsString("F", "Toetssporen in groep 1 en 2").ToFailureMechanismCategory();
            benchmarkTestInput.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups1and2ProbabilityTemporal =
                GetCellValueAsDouble("G", "Toetssporen in groep 1 en 2");
            benchmarkTestInput.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups3and4Temporal =
                GetCellValueAsString("F", "Toetssporen in groep 3 en 4").ToFailureMechanismCategory();
            benchmarkTestInput.ExpectedSafetyAssessmentAssemblyResult.ExpectedSafetyAssessmentAssemblyResultTemporal =
                GetCellValueAsString("F", "Combineren tot veiligheidsoordeel").ToAssessmentGrade();

            benchmarkTestInput.ExpectedSafetyAssessmentAssemblyResult.CombinedFailureMechanismProbabilitySpace =
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

            benchmarkTestInput.ExpectedSafetyAssessmentAssemblyResult.ExpectedCombinedFailureMechanismCategoriesGroup1and2 = new CategoriesList<FailureMechanismCategory>(list);
        }
    }
}
