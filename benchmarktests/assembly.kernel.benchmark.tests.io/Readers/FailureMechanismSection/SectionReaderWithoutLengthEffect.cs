using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.benchmark.tests.io.Readers.FailureMechanismSection
{
    /// <summary>
    /// Section reader for sections that include length effect.
    /// </summary>
    public class SectionReaderWithoutLengthEffect : ExcelSheetReaderBase, ISectionReader<ExpectedFailureMechanismSection>
    {
        /// <summary>
        /// Constructor of the section reader.
        /// </summary>
        /// <param name="worksheetPart">Required <seealso cref="WorksheetPart"/>.</param>
        /// <param name="workbookPart">Required <seealso cref="WorkbookPart"/>.</param>
        public SectionReaderWithoutLengthEffect(WorksheetPart worksheetPart, WorkbookPart workbookPart) : base(worksheetPart, workbookPart, "B")
        {
        }

        /// <summary>
        /// Read the section on a specific row.
        /// </summary>
        /// <param name="iRow">Row index of the row in Excel that must be read.</param>
        /// <param name="startMeters">Already read start of the section in meters along the assessment section.</param>
        /// <param name="endMeters">Already read end of the section in meters along the assessment section.</param>
        /// <returns></returns>
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