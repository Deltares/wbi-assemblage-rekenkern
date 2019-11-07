using System;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanismSections;
using Assembly.Kernel.Model.FmSectionTypes;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.acceptance.tests.io.Readers.FailureMechanismSection
{
    public class Group3FailureMechanismSectionReader : ExcelSheetReaderBase, ISectionReader
    {
        public Group3FailureMechanismSectionReader(WorksheetPart worksheetPart, WorkbookPart workbookPart)
            : base(worksheetPart, workbookPart)
        {
        }

        public IFailureMechanismSection ReadSection(int iRow, double startMeters, double endMeters)
        {
            var cellHValueAsString = GetCellValueAsString("H", iRow);

            return new Group3FailureMechanismSection
            {
                SectionName = GetCellValueAsString("E", iRow),
                Start = startMeters,
                End = endMeters,
                SimpleAssessmentResult = GetCellValueAsString("F", iRow).ToEAssessmentResultTypeE1(),
                ExpectedSimpleAssessmentAssemblyResult = new FmSectionAssemblyDirectResult(GetCellValueAsString("J", iRow).ToFailureMechanismSectionCategory()),
                DetailedAssessmentResult = GetCellValueAsString("G", iRow).ToEAssessmentResultTypeG2(false),
                DetailedAssessmentResultValue = GetCellValueAsString("G", iRow).ToFailureMechanismSectionCategory(),
                ExpectedDetailedAssessmentAssemblyResult = new FmSectionAssemblyDirectResult(GetCellValueAsString("K", iRow).ToFailureMechanismSectionCategory()),
                TailorMadeAssessmentResult = cellHValueAsString.ToEAssessmentResultTypeT3(false),
                TailorMadeAssessmentResultCategory = RetrieveTailorMadeAssessmentResultCategory(cellHValueAsString),
                ExpectedTailorMadeAssessmentAssemblyResult = new FmSectionAssemblyDirectResult(GetCellValueAsString("L", iRow).ToFailureMechanismSectionCategory()),
                ExpectedCombinedResult = GetCellValueAsString("M", iRow).ToFailureMechanismSectionCategory(),
            };
        }

        private EFmSectionCategory RetrieveTailorMadeAssessmentResultCategory(string str)
        {
            try
            {
                return str.ToFailureMechanismSectionCategory();
            }
            catch (Exception)
            {
                return EFmSectionCategory.Gr;
            }
        }
    }
}