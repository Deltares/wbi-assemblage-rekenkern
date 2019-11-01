using System.Collections.Generic;
using assembly.kernel.acceptance.tests.data;
using assembly.kernel.acceptance.tests.data.Input;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;
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

        public void Read(AcceptanceTestInput acceptanceTestInput)
        {
            IFailureMechanismResult failureMechanismResult =
                FailureMechanismFactory.CreateFailureMechanism(
                    GetCellValueAsString("B", "Toetsspoor").ToMechanismType());

            ReadGeneralInformation(failureMechanismResult);
            ReadGroup3FailureMechanismProperties(failureMechanismResult);
            ReadProbabilisticFailureMechanismProperties(failureMechanismResult);
            ReadSTBUFailureMechanismSpecificProperties(failureMechanismResult);
            ReadFailureMechanismSections(failureMechanismResult);

            acceptanceTestInput.ExpectedFailureMechanismsResults.Add(failureMechanismResult);
        }

        private void ReadGeneralInformation(IFailureMechanismResult failureMechanismResult)
        {
            failureMechanismResult.AccountForDuringAssembly = GetCellValueAsString("D", 1) == "Ja";
            var assessmentResultString = GetCellValueAsString("D", "Toetsoordeel per toetsspoor per traject");
            var temporalAssessmentResultString = GetCellValueAsString("D", "Tijdelijk Toetsoordeel per toetsspoor per traject");
            if (failureMechanismResult.Group > 4)
            {
                failureMechanismResult.ExpectedAssessmentResult = assessmentResultString.ToIndirectFailureMechanismSectionCategory();
                failureMechanismResult.ExpectedTemporalAssessmentResult = temporalAssessmentResultString.ToIndirectFailureMechanismSectionCategory();
            }
            else
            {
                failureMechanismResult.ExpectedAssessmentResult = assessmentResultString.ToFailureMechanismCategory();
                failureMechanismResult.ExpectedTemporalAssessmentResult = temporalAssessmentResultString.ToFailureMechanismCategory();
            }
        }

        private void ReadSTBUFailureMechanismSpecificProperties(IFailureMechanismResult failureMechanismResult)
        {
            var stbuFailureMechanism = failureMechanismResult as StbuFailureMechanismResult;
            if (stbuFailureMechanism != null)
            {
                stbuFailureMechanism.FailureMechanismProbabilitySpace = GetCellValueAsDouble("B", "ω Faalkansruimtefactor");
                stbuFailureMechanism.LengthEffectFactor = GetCellValueAsDouble("B", "Ndsn (lengte effectfactor)");
                stbuFailureMechanism.ExpectedSectionsCategoryDivisionProbability = GetCellValueAsDouble("B", "Peis;dsn ≤");
            }
        }

        private void ReadProbabilisticFailureMechanismProperties(IFailureMechanismResult failureMechanismResult)
        {
            var probabilisticFailureMechanism = failureMechanismResult as IProbabilisticFailureMechanismResult;
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

        private void ReadGroup3FailureMechanismProperties(IFailureMechanismResult failureMechanismResult)
        {
            var group3FailureMechanism = failureMechanismResult as IGroup3FailureMechanismResult;
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
        private void ReadSectionCategories(IProbabilisticFailureMechanismResult failureMechanismResult)
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

            failureMechanismResult.ExpectedFailureMechanismSectionCategories = new CategoriesList<FmSectionCategory>(categories);
        }

        private void ReadFailureMechanismCategories(IProbabilisticFailureMechanismResult failureMechanismResult)
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

            failureMechanismResult.ExpectedFailureMechanismCategories = new CategoriesList<FailureMechanismCategory>(categories);
        }

        private void ReadGroup3SectionCategoryBoundaries(IGroup3FailureMechanismResult failureMechanismResult)
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

            failureMechanismResult.ExpectedFailureMechanismSectionCategories = new CategoriesList<FmSectionCategory>(categories);
        }

        #endregion

        #region Read Sections
        private void ReadFailureMechanismSections(IFailureMechanismResult failureMechanismResult)
        {
            var sections = new List<IFailureMechanismSection>();
            var startRow = GetRowId("Vakindeling") + 3;

            var sectionReader = SectionReaderFactory.CreateReader(failureMechanismResult.Type);

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

            failureMechanismResult.Sections = sections;
        }
        #endregion
    }
}
