using System;
using System.Collections.Generic;
using System.Linq;
using assembly.kernel.acceptance.tests.data;
using Assembly.Kernel.Model;
using DocumentFormat.OpenXml.Packaging;
using AssessmentSection = assembly.kernel.acceptance.tests.data.AssessmentSection;

namespace assembly.kernel.acceptance.tests.io.Readers
{
    public class CommonAssessmentSectionResultsReader : ExcelSheetReaderBase
    {
        private readonly Dictionary<MechanismType, bool> failureMechanisms = new Dictionary<MechanismType, bool>
        {
            {MechanismType.STBI, true},
            {MechanismType.STBU, true},
            {MechanismType.STPH, true},
            {MechanismType.STMI, true},
            {MechanismType.AGK, true},
            {MechanismType.AWO, true},
            {MechanismType.GEBU, true},
            {MechanismType.GABU, true},
            {MechanismType.GEKB, true},
            {MechanismType.GABI, true},
            {MechanismType.ZST, true},
            {MechanismType.DA, true},
            {MechanismType.HTKW, true},
            {MechanismType.BSKW, true},
            {MechanismType.PKW, true},
            {MechanismType.STKWp, true},
            {MechanismType.STKWl, true},
            {MechanismType.VLGA, false},
            {MechanismType.VLAF, false},
            {MechanismType.VLZV, false},
            {MechanismType.NWObe, false},
            {MechanismType.NWObo, false},
            {MechanismType.NWOkl, false},
            {MechanismType.NWOoc, false},
            {MechanismType.HAV, false},
            {MechanismType.INN, true}
        };

        private readonly string[] columnStrings =
        {
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U",
            "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE"
        };

        public CommonAssessmentSectionResultsReader(WorksheetPart worksheetPart, WorkbookPart workbookPart) : base(worksheetPart, workbookPart)
        {
        }

        public void Read(AssessmentSection assessmentSection)
        {
            var commonSections = new List<FmSectionWithDirectCategory>();

            var failureMechanismSpecificCommonSectionsWithDirectResults = new Dictionary<MechanismType, List<FmSectionWithDirectCategory>>();
            var failureMechanismSpecificCommonSectionsWithIndirectResults = new Dictionary<MechanismType, List<FmSectionWithIndirectCategory>>();
            foreach (var failureMechanismsKey in failureMechanisms.Keys)
            {
                if (failureMechanisms[failureMechanismsKey])
                {
                    failureMechanismSpecificCommonSectionsWithDirectResults[failureMechanismsKey] = new List<FmSectionWithDirectCategory>();
                }
                else
                {
                    failureMechanismSpecificCommonSectionsWithIndirectResults[failureMechanismsKey] = new List<FmSectionWithIndirectCategory>();
                }
            }

            var columnKeys = GetColumnKeys(3);

            int iRow = 4;
            while (iRow <= MaxRow)
            {
                var startMeters = GetCellValueAsDouble("A",iRow) * 1000.0;
                var endMeters = GetCellValueAsDouble("B", iRow) * 1000.0;
                if (double.IsNaN(startMeters) || double.IsNaN(endMeters))
                {
                    break;
                }

                AddSectionToList(commonSections, "C", iRow, startMeters, endMeters);
                foreach (var directResultPair in failureMechanismSpecificCommonSectionsWithDirectResults)
                {
                    AddSectionToList(directResultPair.Value, columnKeys[directResultPair.Key], iRow, startMeters, endMeters);
                }
                foreach (var indirectResultPair in failureMechanismSpecificCommonSectionsWithIndirectResults)
                {
                    AddSectionToList(indirectResultPair.Value, columnKeys[indirectResultPair.Key], iRow, startMeters, endMeters);
                }

                iRow++;
            }

            var resultsPerFailureMechanism =
                failureMechanismSpecificCommonSectionsWithDirectResults.Select(kv =>
                        new FailureMechanismSectionList(kv.Key.ToString("D"), kv.Value))
                    .Concat(failureMechanismSpecificCommonSectionsWithIndirectResults.Select(kv =>
                        new FailureMechanismSectionList(kv.Key.ToString("D"), kv.Value)));

            assessmentSection.ExpectedCommonSectionsResults = new AssemblyResult(resultsPerFailureMechanism,commonSections);
        }

        private void AddSectionToList(List<FmSectionWithIndirectCategory> list, string columnReference, int iRow, double startMeters, double endMeters)
        {
            var category = GetCellValueAsString(columnReference, iRow).ToIndirectFailureMechanismSectionCategory();
            list.Add(new FmSectionWithIndirectCategory(startMeters, endMeters, category));
        }

        private void AddSectionToList(List<FmSectionWithDirectCategory> list, string columnReference, int iRow,
            double startMeters, double endMeters)
        {
            var category = GetCellValueAsString(columnReference,iRow).ToFailureMechanismSectionCategory();
            list.Add(new FmSectionWithDirectCategory(startMeters, endMeters, category));
        }

        private Dictionary<MechanismType, string> GetColumnKeys(int iRow)
        {
            var dict = new Dictionary<MechanismType, string>();
            foreach (var columnString in columnStrings.Skip(4))
            {
                try
                {
                    var type = GetCellValueAsString(columnString, iRow).ToMechanismType();
                    dict[type] = columnString;
                }
                catch (Exception e)
            {
                    // Skip column. Not important
                }
            }

            return dict;
        }
    }
}
