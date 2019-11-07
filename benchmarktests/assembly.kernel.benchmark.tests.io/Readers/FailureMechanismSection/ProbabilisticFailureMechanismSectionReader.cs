using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;
using Assembly.Kernel.Model.FmSectionTypes;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.benchmark.tests.io.Readers.FailureMechanismSection
{
    public class ProbabilisticFailureMechanismSectionReader : ExcelSheetReaderBase, ISectionReader
    {
        private readonly bool lengthEffectPresent;

        public ProbabilisticFailureMechanismSectionReader(WorksheetPart worksheetPart, WorkbookPart workbookPart, bool lengthEffectPresent)
            : base(worksheetPart, workbookPart)
        {
            this.lengthEffectPresent = lengthEffectPresent;
        }

        public IFailureMechanismSection ReadSection(int iRow, double startMeters, double endMeters)
        {
            var cellFValueAsString = GetCellValueAsString("F", iRow);
            var simpleProbability = cellFValueAsString.ToLower() == "fv" || cellFValueAsString.ToLower() == "nvt"
                ? 0.0
                : double.NaN;
            var lengthEffectFactor = lengthEffectPresent ? GetCellValueAsDouble("P", iRow) : 1.0;
            var detailedAssessmentResultProbability = GetCellValueAsDouble("G", iRow);
            var expectedDetailedAssessmentResultProbability = detailedAssessmentResultProbability * lengthEffectFactor;
            var cellHValueAsString = GetCellValueAsString("H", iRow);
            var tailorMadeAssessmentResultProbability = cellHValueAsString.ToLower() == "fv" ? 0.0 : GetCellValueAsDouble("H", iRow);

            return new ProbabilisticFailureMechanismSection
            {
                SectionName = GetCellValueAsString("E", iRow),
                Start = startMeters,
                End = endMeters,
                LengthEffectFactor = lengthEffectFactor,
                SimpleAssessmentResult = cellFValueAsString.ToEAssessmentResultTypeE1(),
                SimpleAssessmentResultProbability = simpleProbability,
                ExpectedSimpleAssessmentAssemblyResult = new FmSectionAssemblyDirectResultWithProbability(
                    GetCellValueAsString("J", iRow).ToFailureMechanismSectionCategory(),
                    simpleProbability),
                DetailedAssessmentResult = GetCellValueAsString("G", iRow).ToEAssessmentResultTypeG2(true),
                DetailedAssessmentResultProbability = detailedAssessmentResultProbability,
                ExpectedDetailedAssessmentAssemblyResult = new FmSectionAssemblyDirectResultWithProbability(
                    GetCellValueAsString("K", iRow).ToFailureMechanismSectionCategory(),
                    expectedDetailedAssessmentResultProbability),
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