using System.Collections.Generic;
using assembly.kernel.acceptance.tests.data;
using assembly.kernel.acceptance.tests.data.FailureMechanisms;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.acceptance.tests.io.Readers
{
    public class FailureMechanismsReader : ExcelSheetReaderBase
    {
        public FailureMechanismsReader(WorksheetPart worksheetPart, WorkbookPart workbookPart) : base(worksheetPart, workbookPart)
        {
        }

        public void Read(AssessmentSection assessmentSection)
        {
            IFailureMechanism failureMechanism =
                FailureMechanismFactory.CreateFailureMechanism(
                    GetCellValueAsString("B", "Toetsspoor").ToMechanismType());

            ReadFailureMechanismInformation(failureMechanism);
            ReadFailureMechanismSections(failureMechanism);

            assessmentSection.FailureMechanisms.Add(failureMechanism);
        }

        private void ReadFailureMechanismInformation(IFailureMechanism failureMechanism)
        {
            // Read general parts
            failureMechanism.AccountForDuringAssembly = GetCellValueAsString("D", 1) == "Ja";
            failureMechanism.ExpectedAssessmentResult = GetCellValueAsString("D", "Toetsoordeel per toetsspoor per traject")
                .ToFailureMechanismCategory();
            failureMechanism.ExpectedTemporalAssessmentResult = GetCellValueAsString("D", "Tijdelijk Toetsoordeel per toetsspoor per traject")
                .ToFailureMechanismCategory();

            var group3FailureMechanism = failureMechanism as IGroup3FailureMechanism;
            if (group3FailureMechanism != null)
            {
                group3FailureMechanism.FailureMechanismProbabilitySpace = GetCellValueAsDouble("B", "ω Faalkansruimtefactor");
                group3FailureMechanism.LengthEffectFactor = GetCellValueAsDouble("B", "Ndsn (lengte effectfactor)");
                if (group3FailureMechanism.Group == 3)
                {
                    // TODO: Read categories
                    // group3FailureMechanism.ExpectedFailureMechanismSectionCategories = 
                }

                var probabilisticFailureMechanism = failureMechanism as IGroup1Or2FailureMechanism;
                if (probabilisticFailureMechanism != null)
                {
                    probabilisticFailureMechanism.ExpectedAssessmentResultProbability = GetCellValueAsDouble("F", "Toetsoordeel per toetsspoor per traject");
                    probabilisticFailureMechanism.ExpectedTemporalAssessmentResultProbability = GetCellValueAsDouble("F", "Tijdelijk Toetsoordeel per toetsspoor per traject");
                    // TODO: Read categories
                    // probabilisticFailureMechanism.ExpectedFailureMechanismCategories = //
                    // probabilisticFailureMechanism.ExpectedFailureMechanismSectionCategories = 
                }
            }

            var stbuFailureMechanism = failureMechanism as STBUFailureMechanism;
            if (stbuFailureMechanism != null)
            {
                stbuFailureMechanism.FailureMechanismProbabilitySpace = GetCellValueAsDouble("B", "ω Faalkansruimtefactor");
                stbuFailureMechanism.LengthEffectFactor = GetCellValueAsDouble("B", "Ndsn (lengte effectfactor)");
                stbuFailureMechanism.ExpectedCategoryDivisionProbability = GetCellValueAsDouble("B", "Peis;dsn ≤");
            }
        }

        private void ReadFailureMechanismSections(IFailureMechanism failureMechanism)
        {
            // TODO: Split to determine type of section and way to read (move to separate class?).
            var sections = new List<IFailureMechanismSection>();
            var iRow = GetRowId("Vakindeling") + 3;

            while (iRow <= MaxRow)
            {
                var startMeters = GetCellValueAsDouble("A", iRow) * 1000.0;
                var endMeters = GetCellValueAsDouble("B", iRow) * 1000.0;

                if (double.IsNaN(startMeters) || double.IsNaN(endMeters))
                {
                    break;
                }

                // TODO: Read section details and initialize section
                /*var failureMechanismSection = new FailureMechanismSection
                {
                    Name = ExcelReaderHelper.GetCellValueAsString(worksheetPart.Worksheet, "E" + iRow, workbookPart),
                    StartMeters = startMeters,
                    EndMeters = endMeters,
                    AssessmentResult = ExcelReaderHelper
                        .GetCellValueAsString(worksheetPart.Worksheet, "M" + iRow, workbookPart)
                        .ToFailureMechanismSectionAssemblyCategoryGroup(),
                    SimpleAssessmentResult = ExcelReaderHelper
                        .GetCellValueAsString(worksheetPart.Worksheet, "J" + iRow, workbookPart)
                        .ToFailureMechanismSectionAssemblyCategoryGroup(),
                    DetailedAssessmentResult = ExcelReaderHelper
                        .GetCellValueAsString(worksheetPart.Worksheet, "K" + iRow, workbookPart)
                        .ToFailureMechanismSectionAssemblyCategoryGroup(),
                    TailorMadeAssessmentResult = ExcelReaderHelper
                        .GetCellValueAsString(worksheetPart.Worksheet, "L" + iRow, workbookPart)
                        .ToFailureMechanismSectionAssemblyCategoryGroup(),
                    HasProbabilities = isProbabilisticMechanism,
                };*/

                /*if (isProbabilisticMechanism)
                {
                    var fColumnValueAsString =
                        ExcelReaderHelper.GetCellValueAsString(worksheetPart.Worksheet, "F" + iRow, workbookPart);
                    failureMechanismSection.SimpleAssessmentResultProbability =
                        fColumnValueAsString == "FV" || fColumnValueAsString == "NVT" ? 0.0 : double.NaN;

                    failureMechanismSection.DetailedAssessmentResultProbability =
                        ExcelReaderHelper.GetCellValueAsString(worksheetPart.Worksheet, "G" + iRow, workbookPart) == "NGO"
                            ? double.NaN
                            : ExcelReaderHelper.GetCellValueAsDouble(worksheetPart.Worksheet, "G" + iRow, workbookPart);

                    var hColumnValueAsString =
                        ExcelReaderHelper.GetCellValueAsString(worksheetPart.Worksheet, "H" + iRow, workbookPart);
                    failureMechanismSection.TailorMadeAssessmentResultProbability =
                        hColumnValueAsString == "NGO"
                            ? double.NaN
                            : hColumnValueAsString == "FV"
                                ? 0.0
                                : ExcelReaderHelper.GetCellValueAsDouble(worksheetPart.Worksheet, "H" + iRow, workbookPart);

                    failureMechanismSection.CombinedAssessmentResultProbability =
                        ExcelReaderHelper.GetCellValueAsDouble(worksheetPart.Worksheet, "P" + iRow, workbookPart);
                }*/

                //sections.Add(failureMechanismSection);
                iRow++;
            }

            failureMechanism.Sections = sections;
        }
    }
}
