using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;
using Assembly.Kernel.Model.FmSectionTypes;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.benchmark.tests.io.Readers.FailureMechanismSection
{
    public class Group4FailureMechanismSectionReader : ExcelSheetReaderBase, ISectionReader
    {
        /// <summary>
        /// Creates an instance of the Group4FailureMechanismSectionReader.
        /// </summary>
        /// <param name="worksheetPart">The WorksheetPart that contains information on this failure mechanism</param>
        /// <param name="workbookPart">The workbook containing the specified worksheet</param>
        public Group4FailureMechanismSectionReader(WorksheetPart worksheetPart, WorkbookPart workbookPart)
            : base(worksheetPart, workbookPart)
        {
        }

        public IFailureMechanismSection ReadSection(int iRow, double startMeters, double endMeters)
        {
            return new Group4FailureMechanismSection
            {
                SectionName = GetCellValueAsString("E", iRow),
                Start = startMeters,
                End = endMeters,
                SimpleAssessmentResult = GetCellValueAsString("F", iRow).ToEAssessmentResultTypeE1(),
                ExpectedSimpleAssessmentAssemblyResult = new FmSectionAssemblyDirectResult(GetCellValueAsString("J", iRow).ToFailureMechanismSectionCategory()),
                DetailedAssessmentResult = GetCellValueAsString("G", iRow).ToEAssessmentResultTypeG1(),
                ExpectedDetailedAssessmentAssemblyResult = new FmSectionAssemblyDirectResult(GetCellValueAsString("K", iRow).ToFailureMechanismSectionCategory()),
                TailorMadeAssessmentResult = GetCellValueAsString("H", iRow).ToEAssessmentResultTypeT1(),
                ExpectedTailorMadeAssessmentAssemblyResult = new FmSectionAssemblyDirectResult(GetCellValueAsString("L", iRow).ToFailureMechanismSectionCategory()),
                ExpectedCombinedResult = GetCellValueAsString("M", iRow).ToFailureMechanismSectionCategory(),
            };
        }
    }
}