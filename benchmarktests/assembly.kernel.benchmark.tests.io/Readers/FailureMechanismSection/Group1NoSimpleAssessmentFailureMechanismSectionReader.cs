using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;
using Assembly.Kernel.Model.FmSectionTypes;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.benchmark.tests.io.Readers.FailureMechanismSection
{
    public class Group1NoSimpleAssessmentFailureMechanismSectionReader : ExcelSheetReaderBase, ISectionReader
    {
        public Group1NoSimpleAssessmentFailureMechanismSectionReader(WorksheetPart worksheetPart, WorkbookPart workbookPart)
            : base(worksheetPart, workbookPart)
        {
        }

        public IFailureMechanismSection ReadSection(int iRow, double startMeters, double endMeters)
        {
            var cellFValueAsString = GetCellValueAsString("F", iRow);
            var simpleProbability = cellFValueAsString.ToLower() == "nvt"
                ? 0.0
                : double.NaN;
            var detailedAssessmentResultProbability = GetCellValueAsDouble("G", iRow);
            var cellHValueAsString = GetCellValueAsString("H", iRow);
            var tailorMadeAssessmentResultProbability = cellHValueAsString.ToLower() == "fv" ? 0.0 : GetCellValueAsDouble("H", iRow);

            return new Group1NoSimpleAssessmentFailureMechanismSection
            {
                SectionName = GetCellValueAsString("E", iRow),
                Start = startMeters,
                End = endMeters,
                SimpleAssessmentResult = cellFValueAsString.ToEAssessmentResultTypeE2(),
                SimpleAssessmentResultProbability = simpleProbability,
                ExpectedSimpleAssessmentAssemblyResult = new FmSectionAssemblyDirectResultWithProbability(
                    GetCellValueAsString("J", iRow).ToFailureMechanismSectionCategory(),
                    simpleProbability),
                DetailedAssessmentResult = GetCellValueAsString("G", iRow).ToEAssessmentResultTypeG2(true),
                DetailedAssessmentResultProbability = detailedAssessmentResultProbability,
                ExpectedDetailedAssessmentAssemblyResult = new FmSectionAssemblyDirectResultWithProbability(
                    GetCellValueAsString("K", iRow).ToFailureMechanismSectionCategory(),
                    detailedAssessmentResultProbability),
                TailorMadeAssessmentResult = cellHValueAsString.ToEAssessmentResultTypeT3(true),
                TailorMadeAssessmentResultProbability = tailorMadeAssessmentResultProbability,
                ExpectedTailorMadeAssessmentAssemblyResult = new FmSectionAssemblyDirectResultWithProbability(
                    GetCellValueAsString("L", iRow).ToFailureMechanismSectionCategory(),
                    tailorMadeAssessmentResultProbability),
                ExpectedCombinedResult = GetCellValueAsString("M", iRow).ToFailureMechanismSectionCategory(),
                ExpectedCombinedResultProbability = GetCellValueAsDouble("N", iRow)
            };
        }
    }
}