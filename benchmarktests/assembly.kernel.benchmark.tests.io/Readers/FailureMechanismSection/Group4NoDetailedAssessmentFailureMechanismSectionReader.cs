using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;
using Assembly.Kernel.Model.FmSectionTypes;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.benchmark.tests.io.Readers.FailureMechanismSection
{
    public class Group4NoDetailedAssessmentFailureMechanismSectionReader : ExcelSheetReaderBase, ISectionReader
    {
        public Group4NoDetailedAssessmentFailureMechanismSectionReader(WorksheetPart worksheetPart, WorkbookPart workbookPart)
            : base(worksheetPart, workbookPart)
        {
        }

        public IFailureMechanismSection ReadSection(int iRow, double startMeters, double endMeters)
        {
            return new Group4NoDetailedAssessmentFailureMechanismSection
            {
                SectionName = GetCellValueAsString("E", iRow),
                Start = startMeters,
                End = endMeters,
                SimpleAssessmentResult = GetCellValueAsString("F", iRow).ToEAssessmentResultTypeE1(),
                ExpectedSimpleAssessmentAssemblyResult =
                    new FmSectionAssemblyDirectResult(GetCellValueAsString("J", iRow)
                        .ToFailureMechanismSectionCategory()),
                ExpectedDetailedAssessmentAssemblyResult =
                    new FmSectionAssemblyDirectResult(GetCellValueAsString("K", iRow)
                        .ToFailureMechanismSectionCategory()),
                TailorMadeAssessmentResult = GetCellValueAsString("H", iRow).ToEAssessmentResultTypeT1(),
                ExpectedTailorMadeAssessmentAssemblyResult =
                    new FmSectionAssemblyDirectResult(GetCellValueAsString("L", iRow)
                        .ToFailureMechanismSectionCategory()),
                ExpectedCombinedResult = GetCellValueAsString("M", iRow).ToFailureMechanismSectionCategory(),
            };
        }
    }
}