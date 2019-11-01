using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanismSections;
using Assembly.Kernel.Model.FmSectionTypes;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.acceptance.tests.io.Readers.FailureMechanismSection
{
    public class STBUFailureMechanismSectionReader : ExcelSheetReaderBase, ISectionReader
    {
        public STBUFailureMechanismSectionReader(WorksheetPart worksheetPart, WorkbookPart workbookPart)
            : base(worksheetPart, workbookPart)
        {
        }

        public IFailureMechanismSection ReadSection(int iRow, double startMeters, double endMeters)
        {
            var cellJValueAsString = GetCellValueAsString("J", iRow);
            var simpleProbability = cellJValueAsString.ToLower() == "fv" || cellJValueAsString.ToLower() == "nvt"
                ? 0.0
                : double.NaN;
            var detailedAssessmentResultProbability = GetCellValueAsDouble("G", iRow);
            var cellHValueAsString = GetCellValueAsString("H", iRow);
            var tailorMadeAssessmentResultProbability = cellHValueAsString.ToLower() == "fv" ? 0.0 : GetCellValueAsDouble("H", iRow);

            return new STBUFailureMechanismSection
            {
                SectionName = GetCellValueAsString("E", iRow),
                Start = startMeters,
                End = endMeters,
                SimpleAssessmentResult = GetCellValueAsString("F", iRow).ToEAssessmentResultTypeE1(),
                ExpectedSimpleAssessmentAssemblyResult = new FmSectionAssemblyDirectResultWithProbability(
                    cellJValueAsString.ToFailureMechanismSectionCategory(),
                    simpleProbability),
                DetailedAssessmentResult = GetCellValueAsString("G", iRow).ToEAssessmentResultTypeG2(),
                DetailedAssessmentResultProbability = detailedAssessmentResultProbability,
                ExpectedDetailedAssessmentAssemblyResult = new FmSectionAssemblyDirectResultWithProbability(
                    GetCellValueAsString("K", iRow).ToFailureMechanismSectionCategory(),
                    detailedAssessmentResultProbability),
                TailorMadeAssessmentResult = cellHValueAsString.ToEAssessmentResultTypeT4(),
                TailorMadeAssessmentResultProbability = tailorMadeAssessmentResultProbability,
                ExpectedTailorMadeAssessmentAssemblyResult = new FmSectionAssemblyDirectResultWithProbability(
                    GetCellValueAsString("L", iRow).ToFailureMechanismSectionCategory(),
                    tailorMadeAssessmentResultProbability),
                ExpectedCombinedResult = GetCellValueAsString("M", iRow).ToFailureMechanismSectionCategory()
            };
        }
    }
}