using System.Collections.Generic;
using assembly.kernel.acceptance.tests.data;
using assembly.kernel.acceptance.tests.data.FailureMechanisms;
using assembly.kernel.acceptance.tests.io.Readers.FailureMechanismSection;
using Assembly.Kernel.Model.CategoryLimits;
using Assembly.Kernel.Model.FmSectionTypes;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.acceptance.tests.io.Readers
{
    public class FailureMechanismsReader : ExcelSheetReaderBase
    {
        public SectionReaderFactory SectionReaderFactory { get; }

        public FailureMechanismsReader(WorksheetPart worksheetPart, WorkbookPart workbookPart) : base(worksheetPart, workbookPart)
        {
            SectionReaderFactory = new SectionReaderFactory(worksheetPart, workbookPart);
        }

        public void Read(AssessmentSection assessmentSection)
        {
            IFailureMechanism failureMechanism =
                FailureMechanismFactory.CreateFailureMechanism(
                    GetCellValueAsString("B", "Toetsspoor").ToMechanismType());

            ReadGeneralInformation(failureMechanism);
            ReadGroup3FailureMechanismProperties(failureMechanism);
            ReadProbabilisticFailureMechanismProperties(failureMechanism);
            ReadSTBUFailureMechanismSpecificProperties(failureMechanism);
            ReadFailureMechanismSections(failureMechanism);

            assessmentSection.FailureMechanisms.Add(failureMechanism);
        }

        private void ReadGeneralInformation(IFailureMechanism failureMechanism)
        {
            failureMechanism.AccountForDuringAssembly = GetCellValueAsString("D", 1) == "Ja";
            var assessmentResultString = GetCellValueAsString("D", "Toetsoordeel per toetsspoor per traject");
            var temporalAssessmentResultString = GetCellValueAsString("D", "Tijdelijk Toetsoordeel per toetsspoor per traject");
            if (failureMechanism.Group > 4)
            {
                failureMechanism.ExpectedAssessmentResult = assessmentResultString.ToIndirectFailureMechanismSectionCategory();
                failureMechanism.ExpectedTemporalAssessmentResult = temporalAssessmentResultString.ToIndirectFailureMechanismSectionCategory();
            }
            else
            {
                failureMechanism.ExpectedAssessmentResult = assessmentResultString.ToFailureMechanismCategory();
                failureMechanism.ExpectedTemporalAssessmentResult = temporalAssessmentResultString.ToFailureMechanismCategory();
            }
        }

        private void ReadSTBUFailureMechanismSpecificProperties(IFailureMechanism failureMechanism)
        {
            var stbuFailureMechanism = failureMechanism as STBUFailureMechanism;
            if (stbuFailureMechanism != null)
            {
                stbuFailureMechanism.FailureMechanismProbabilitySpace = GetCellValueAsDouble("B", "ω Faalkansruimtefactor");
                stbuFailureMechanism.LengthEffectFactor = GetCellValueAsDouble("B", "Ndsn (lengte effectfactor)");
                stbuFailureMechanism.ExpectedSctionsCategoryDivisionProbability = GetCellValueAsDouble("B", "Peis;dsn ≤");
            }
        }

        private void ReadProbabilisticFailureMechanismProperties(IFailureMechanism failureMechanism)
        {
            var probabilisticFailureMechanism = failureMechanism as IProbabilisticFailureMechanism;
            if (probabilisticFailureMechanism != null)
            {
                probabilisticFailureMechanism.ExpectedAssessmentResultProbability =
                    GetCellValueAsDouble("F", "Toetsoordeel per toetsspoor per traject");
                probabilisticFailureMechanism.ExpectedTemporalAssessmentResultProbability =
                    GetCellValueAsDouble("F", "Tijdelijk Toetsoordeel per toetsspoor per traject");
                ReadFailureMechanismCategories(probabilisticFailureMechanism);
                ReadSectionCategories(probabilisticFailureMechanism);
            }
        }

        private void ReadGroup3FailureMechanismProperties(IFailureMechanism failureMechanism)
        {
            var group3FailureMechanism = failureMechanism as IGroup3FailureMechanism;
            if (group3FailureMechanism != null)
            {
                group3FailureMechanism.FailureMechanismProbabilitySpace = GetCellValueAsDouble("B", "ω Faalkansruimtefactor");
                group3FailureMechanism.LengthEffectFactor = GetCellValueAsDouble("B", "Ndsn (lengte effectfactor)");
                if (group3FailureMechanism.Group == 3)
                {
                    ReadGroup3SectionCategoryBoundaries(group3FailureMechanism);
                }
            }
        }

        #region Read Categories
        private void ReadSectionCategories(IProbabilisticFailureMechanism failureMechanism)
        {
            var headerRowId = GetRowId("Categorie");

            var categories = new List<FmSectionCategory>();
            for (int i = headerRowId + 1; i < headerRowId + 7; i++)
            {
                var category = GetCellValueAsString("D", i).ToFailureMechanismSectionCategory();
                var lowerLimit = GetCellValueAsDouble("E", i);
                var upperLimit = GetCellValueAsDouble("F", i);
                categories.Add(new FmSectionCategory(category, lowerLimit, upperLimit));
            }

            failureMechanism.ExpectedFailureMechanismSectionCategories = new CategoriesList<FmSectionCategory>(categories);
        }

        private void ReadFailureMechanismCategories(IProbabilisticFailureMechanism failureMechanism)
        {
            var headerRowId = GetRowId("Categorie");

            var categories = new List<FailureMechanismCategory>();
            for (int i = headerRowId + 1; i < headerRowId + 7; i++)
            {
                var category = GetCellValueAsString("A", i).ToFailureMechanismCategory();
                var lowerLimit = GetCellValueAsDouble("B", i);
                var upperLimit = GetCellValueAsDouble("C", i);
                categories.Add(new FailureMechanismCategory(category, lowerLimit, upperLimit));
            }

            failureMechanism.ExpectedFailureMechanismCategories = new CategoriesList<FailureMechanismCategory>(categories);
        }

        private void ReadGroup3SectionCategoryBoundaries(IGroup3FailureMechanism failureMechanism)
        {
            var headerRowId = GetRowId("Categorie");

            var categories = new List<FmSectionCategory>();
            var lastCategoryBoundary = 0.0;
            for (int i = headerRowId + 1; i < headerRowId + 6; i++)
            {
                var category = GetCellValueAsString("A", i).ToFailureMechanismSectionCategory();
                var currentBoundary = GetCellValueAsDouble("B", i);
                categories.Add(new FmSectionCategory(category, lastCategoryBoundary, currentBoundary));
                lastCategoryBoundary = currentBoundary;
            }
            categories.Add(new FmSectionCategory(EFmSectionCategory.VIv, lastCategoryBoundary, 1.0));

            failureMechanism.ExpectedFailureMechanismSectionCategories = new CategoriesList<FmSectionCategory>(categories);
        }

        #endregion

        #region Read Sections
        private void ReadFailureMechanismSections(IFailureMechanism failureMechanism)
        {
            var sections = new List<IFailureMechanismSection>();
            var startRow = GetRowId("Vakindeling") + 3;

            var sectionReader = SectionReaderFactory.CreateReader(failureMechanism.Type);

            var iRow = startRow;
            while (iRow <= MaxRow)
            {
                var startMeters = GetCellValueAsDouble("A", iRow) * 1000.0;
                var endMeters = GetCellValueAsDouble("B", iRow) * 1000.0;

                if (double.IsNaN(startMeters) || double.IsNaN(endMeters))
                {
                    break;
                }

                sections.Add(sectionReader.ReadSection(iRow, startMeters, endMeters));

                iRow++;
            }

            failureMechanism.Sections = sections;
        }
        #endregion
    }
}
