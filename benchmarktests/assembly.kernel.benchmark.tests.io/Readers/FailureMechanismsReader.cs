using System.Collections.Generic;
using assembly.kernel.benchmark.tests.data.Input;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;
using assembly.kernel.benchmark.tests.io.Readers.FailureMechanismSection;
using Assembly.Kernel.Model.CategoryLimits;
using Assembly.Kernel.Model.FmSectionTypes;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.benchmark.tests.io.Readers
{
    public class FailureMechanismsReader : ExcelSheetReaderBase
    {
        private readonly SectionReaderFactory sectionReaderFactory;

        /// <summary>
        /// Creates an instance of the FailureMechanismReader
        /// </summary>
        /// <param name="worksheetPart"></param>
        /// <param name="workbookPart"></param>
        public FailureMechanismsReader(WorksheetPart worksheetPart, WorkbookPart workbookPart) : base(worksheetPart, workbookPart)
        {
            sectionReaderFactory = new SectionReaderFactory(worksheetPart, workbookPart);
        }

        /// <summary>
        /// Reads all relevant input and expected output for a specific failure mechanism.
        /// </summary>
        /// <param name="benchmarkTestInput"></param>
        public void Read(BenchmarkTestInput benchmarkTestInput)
        {
            IExpectedFailureMechanismResult expectedFailureMechanismResult =
                FailureMechanismFactory.CreateFailureMechanism(
                    GetCellValueAsString("B", "Toetsspoor").ToMechanismType());

            ReadGeneralInformation(expectedFailureMechanismResult);
            ReadGroup3FailureMechanismProperties(expectedFailureMechanismResult);
            ReadProbabilisticFailureMechanismProperties(expectedFailureMechanismResult);
            ReadSTBUFailureMechanismSpecificProperties(expectedFailureMechanismResult);
            ReadFailureMechanismSections(expectedFailureMechanismResult);

            benchmarkTestInput.ExpectedFailureMechanismsResults.Add(expectedFailureMechanismResult);
        }

        private void ReadGeneralInformation(IExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            expectedFailureMechanismResult.AccountForDuringAssembly = GetCellValueAsString("D", 1) == "Ja";
            var assessmentResultString = GetCellValueAsString("D", "Toetsoordeel per toetsspoor per traject");
            var temporalAssessmentResultString = GetCellValueAsString("D", "Tijdelijk Toetsoordeel per toetsspoor per traject");
            if (expectedFailureMechanismResult.Group > 4)
            {
                expectedFailureMechanismResult.ExpectedAssessmentResult = assessmentResultString.ToIndirectFailureMechanismSectionCategory();
                expectedFailureMechanismResult.ExpectedAssessmentResultTemporal = temporalAssessmentResultString.ToIndirectFailureMechanismSectionCategory();
            }
            else
            {
                expectedFailureMechanismResult.ExpectedAssessmentResult = assessmentResultString.ToFailureMechanismCategory();
                expectedFailureMechanismResult.ExpectedAssessmentResultTemporal = temporalAssessmentResultString.ToFailureMechanismCategory();
            }
        }

        private void ReadSTBUFailureMechanismSpecificProperties(IExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            var stbuFailureMechanism = expectedFailureMechanismResult as StbuExpectedFailureMechanismResult;
            if (stbuFailureMechanism != null)
            {
                stbuFailureMechanism.FailureMechanismProbabilitySpace = GetCellValueAsDouble("B", "ω Faalkansruimtefactor");
                stbuFailureMechanism.LengthEffectFactor = GetCellValueAsDouble("B", "Ndsn (lengte effectfactor)");
                stbuFailureMechanism.UseSignallingNorm = GetCellValueAsString("A", 14) == "Signaleringswaarde";
                stbuFailureMechanism.ExpectedSectionsCategoryDivisionProbability = GetCellValueAsDouble("B", "Peis;dsn ≤");
            }
        }

        private void ReadProbabilisticFailureMechanismProperties(IExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            var probabilisticFailureMechanism = expectedFailureMechanismResult as IProbabilisticExpectedFailureMechanismResult;
            if (probabilisticFailureMechanism != null)
            {
                probabilisticFailureMechanism.ExpectedAssessmentResultProbability =
                    GetCellValueAsDouble("F", "Toetsoordeel per toetsspoor per traject");
                probabilisticFailureMechanism.ExpectedAssessmentResultProbabilityTemporal =
                    GetCellValueAsDouble("F", "Tijdelijk Toetsoordeel per toetsspoor per traject");
                ReadFailureMechanismCategories(probabilisticFailureMechanism);
                ReadSectionCategories(probabilisticFailureMechanism);
            }
        }

        private void ReadGroup3FailureMechanismProperties(IExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            var group3FailureMechanism = expectedFailureMechanismResult as IGroup3ExpectedFailureMechanismResult;
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
        private void ReadSectionCategories(IProbabilisticExpectedFailureMechanismResult expectedFailureMechanismResult)
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

            expectedFailureMechanismResult.ExpectedFailureMechanismSectionCategories = new CategoriesList<FmSectionCategory>(categories);
        }

        private void ReadFailureMechanismCategories(IProbabilisticExpectedFailureMechanismResult expectedFailureMechanismResult)
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

            expectedFailureMechanismResult.ExpectedFailureMechanismCategories = new CategoriesList<FailureMechanismCategory>(categories);
        }

        private void ReadGroup3SectionCategoryBoundaries(IGroup3ExpectedFailureMechanismResult expectedFailureMechanismResult)
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

            expectedFailureMechanismResult.ExpectedFailureMechanismSectionCategories = new CategoriesList<FmSectionCategory>(categories);
        }

        #endregion

        #region Read Sections
        private void ReadFailureMechanismSections(IExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            var sections = new List<IFailureMechanismSection>();
            var startRow = GetRowId("Vakindeling") + 3;
            var sectionReader = sectionReaderFactory.CreateReader(expectedFailureMechanismResult.Type);

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

            expectedFailureMechanismResult.Sections = sections;
        }
        #endregion
    }
}
