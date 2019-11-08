using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;
using Assembly.Kernel.Model.FmSectionTypes;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.benchmark.tests.io.Readers.FailureMechanismSection
{
    public class NWOocFailureMechanismSectionReader : ExcelSheetReaderBase, ISectionReader
    {
        // TODO: Test
        /// <summary>
        /// Creates an instance of the NWOocFailureMechanismSectionReader.
        /// </summary>
        /// <param name="worksheetPart">The WorksheetPart that contains information on this failure mechanism</param>
        /// <param name="workbookPart">The workbook containing the specified worksheet</param>
        public NWOocFailureMechanismSectionReader(WorksheetPart worksheetPart, WorkbookPart workbookPart)
            : base(worksheetPart, workbookPart)
        {
        }

        public IFailureMechanismSection ReadSection(int iRow, double startMeters, double endMeters)
        {
            return new NWOocFailureMechanismSection
            {
                SectionName = GetCellValueAsString("E", iRow),
                Start = startMeters,
                End = endMeters,
                SimpleAssessmentResult = GetCellValueAsString("F", iRow).ToEAssessmentResultTypeE2(),
                ExpectedSimpleAssessmentAssemblyResult =
                    new FmSectionAssemblyIndirectResult(GetCellValueAsString("J", iRow).ToIndirectFailureMechanismSectionCategory()),
                DetailedAssessmentResult = GetCellValueAsString("G", iRow).ToEAssessmentResultTypeG1(),
                ExpectedDetailedAssessmentAssemblyResult =
                    new FmSectionAssemblyIndirectResult(GetCellValueAsString("K", iRow).ToIndirectFailureMechanismSectionCategory()),
                TailorMadeAssessmentResult = GetCellValueAsString("H", iRow).ToEAssessmentResultTypeT2(),
                ExpectedTailorMadeAssessmentAssemblyResult =
                    new FmSectionAssemblyIndirectResult(GetCellValueAsString("L", iRow).ToIndirectFailureMechanismSectionCategory()),
                ExpectedCombinedResult = GetCellValueAsString("M", iRow).ToIndirectFailureMechanismSectionCategory(),
            };
        }
    }
}