using System;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.benchmark.tests.io.Readers.FailureMechanismSection
{
    public class SectionReaderWithLengthEffect : ExcelSheetReaderBase, ISectionReader<ExpectedFailureMechanismSectionWithLengthEffect>
    {
        public SectionReaderWithLengthEffect(WorksheetPart worksheetPart, WorkbookPart workbookPart) : base(worksheetPart,workbookPart,"B")
        {
        }

        public ExpectedFailureMechanismSectionWithLengthEffect ReadSection(int iRow, double startMeters, double endMeters)
        {
            string sectionName = GetCellValueAsString("B", iRow);
            bool isRelevant = GetCellValueAsString("E", iRow) == "Ja";
            Probability probabilityInitialMechanismProfile = new Probability(GetCellValueAsDouble("G", iRow));
            Probability probabilityInitialMechanismSection = new Probability(GetCellValueAsDouble("H", iRow));
            bool refinedAnalysisNecessary = GetCellValueAsString("I", iRow) == "Ja";
            Probability refinedProbabilityProfile = new Probability(GetCellValueAsDouble("J", iRow));
            Probability refinedProbabilitySection = new Probability(GetCellValueAsDouble("K", iRow));
            Probability expectedCombinedProbabilityProfile = new Probability(GetCellValueAsDouble("L", iRow));
            Probability expectedCombinedProbabilitySection = new Probability(GetCellValueAsDouble("M", iRow));
            EInterpretationCategory expectedInterpretationCategory = GetCellValueAsString("O", iRow).ToInterpretationCategory();
            double expectedLengthEffect = GetCellValueAsDouble("N", iRow);

            var eRefinementStatus = !refinedAnalysisNecessary ? ERefinementStatus.NotNecessary :
                double.IsNaN(refinedProbabilityProfile) ? ERefinementStatus.Necessary : ERefinementStatus.Performed;

            return new ExpectedFailureMechanismSectionWithLengthEffect(sectionName, 
                startMeters, 
                endMeters, 
                isRelevant,
                probabilityInitialMechanismProfile, 
                probabilityInitialMechanismSection,
                eRefinementStatus,
                refinedProbabilityProfile,
                refinedProbabilitySection,
                expectedCombinedProbabilityProfile,
                expectedCombinedProbabilitySection, 
                expectedInterpretationCategory, 
                expectedLengthEffect);
        }
    }
}