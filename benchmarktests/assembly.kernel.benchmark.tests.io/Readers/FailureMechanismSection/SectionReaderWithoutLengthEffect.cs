using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.benchmark.tests.io.Readers.FailureMechanismSection
{
    public class SectionReaderWithoutLengthEffect : ExcelSheetReaderBase, ISectionReader<ExpectedFailureMechanismSection>
    {
        public SectionReaderWithoutLengthEffect(WorksheetPart worksheetPart, WorkbookPart workbookPart) : base(worksheetPart, workbookPart, "B")
        {
        }

        public ExpectedFailureMechanismSection ReadSection(int iRow, double startMeters, double endMeters)
        {
            string sectionName = GetCellValueAsString("B", iRow);
            bool isRelevant = GetCellValueAsString("E", iRow) == "Ja";
            Probability probabilityInitialMechanismSection = new Probability(GetCellValueAsDouble("G", iRow));
            bool refinedAnalysisNecessary = GetCellValueAsString("H", iRow) == "Ja";
            Probability refinedProbabilitySection = new Probability(GetCellValueAsDouble("I", iRow));
            Probability expectedCombinedProbabilitySection = new Probability(GetCellValueAsDouble("J", iRow));
            EInterpretationCategory expectedInterpretationCategory = GetCellValueAsString("K", iRow).ToInterpretationCategory();

            var eRefinementStatus = !refinedAnalysisNecessary ? ERefinementStatus.NotNecessary :
                double.IsNaN(refinedProbabilitySection) ? ERefinementStatus.Necessary : ERefinementStatus.Performed;

            return new ExpectedFailureMechanismSection(sectionName,
                startMeters,
                endMeters,
                isRelevant,
                probabilityInitialMechanismSection,
                eRefinementStatus,
                refinedProbabilitySection,
                expectedCombinedProbabilitySection,
                expectedInterpretationCategory);
        }
    }
}