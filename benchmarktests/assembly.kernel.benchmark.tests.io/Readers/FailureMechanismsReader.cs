#region Copyright (C) Rijkswaterstaat 2019. All rights reserved
// Copyright (C) Rijkswaterstaat 2019. All rights reserved.
//
// This file is part of the Assembly kernel.
//
// Assembly kernel is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
//
// All names, logos, and references to "Rijkswaterstaat" are registered trademarks of
// Rijkswaterstaat and remain full property of Rijkswaterstaat at all times.
// All rights reserved.
#endregion

using System.Collections.Generic;
using assembly.kernel.benchmark.tests.data.Input;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;
using assembly.kernel.benchmark.tests.io.Readers.FailureMechanismSection;
using Assembly.Kernel.Model;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.benchmark.tests.io.Readers
{
    /// <summary>
    /// Reader to read failure mechanism.
    /// </summary>
    public class FailureMechanismsReader : ExcelSheetReaderBase
    {
        private readonly SectionReaderFactory sectionReaderFactory;

        /// <summary>
        /// Creates a new instance of <see cref="FailureMechanismsReader"/>.
        /// </summary>
        /// <param name="worksheetPart">The worksheet for which to create a dictionary</param>
        /// <param name="workbookPart">Thw workbook part of the workbook that contains this worksheet</param>
        public FailureMechanismsReader(WorksheetPart worksheetPart, WorkbookPart workbookPart) : base(worksheetPart, workbookPart, "B")
        {
            sectionReaderFactory = new SectionReaderFactory(worksheetPart, workbookPart);
        }

        /// <summary>
        /// Reads all relevant input and expected output for a specific failure mechanism.
        /// </summary>
        /// <param name="benchmarkTestInput">The test input.</param>
        /// <param name="mechanismId">String used to identify the failure mechanism.</param>
        public void Read(BenchmarkTestInput benchmarkTestInput, string mechanismId)
        {
            ExpectedFailureMechanismResult expectedFailureMechanismResult =
                FailureMechanismResultFactory.CreateFailureMechanism(
                    GetCellValueAsString("C", "Faalpad"),
                    mechanismId,
                    GetCellValueAsString("C", "Lengte-effect") == "Ja");

            ReadGeneralInformation(expectedFailureMechanismResult);
            ReadFailureMechanismSections(expectedFailureMechanismResult);

            benchmarkTestInput.ExpectedFailureMechanismsResults.Add(expectedFailureMechanismResult);
        }

        private void ReadGeneralInformation(ExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            expectedFailureMechanismResult.ExpectedCombinedProbabilityTemporal = new Probability(GetCellValueAsDouble("C", "Faalkans tussentijds"));
            expectedFailureMechanismResult.ExpectedIsSectionsCorrelatedTemporal = ToCorrelation(GetCellValueAsString("C", "Vakken gecorreleerd?") == "Ja");
            expectedFailureMechanismResult.ExpectedCombinedProbability = new Probability(GetCellValueAsDouble("C", "Faalkans"));
            expectedFailureMechanismResult.ExpectedIsSectionsCorrelated = ToCorrelation(double.IsNaN(expectedFailureMechanismResult.ExpectedCombinedProbability) || expectedFailureMechanismResult.ExpectedIsSectionsCorrelatedTemporal == EFailureMechanismAssemblyMethod.Correlated); 
            expectedFailureMechanismResult.LengthEffectFactor = GetCellValueAsDouble("C", "Ntraject");
        }

        private EFailureMechanismAssemblyMethod ToCorrelation(bool correlated)
        {
            return correlated ? EFailureMechanismAssemblyMethod.Correlated: EFailureMechanismAssemblyMethod.UnCorrelated;
        }

        private void ReadFailureMechanismSections(ExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            var sections = new List<IExpectedFailureMechanismSection>();
            var startRow = GetRowId("Vaknaam") + 1;
            var sectionReader = sectionReaderFactory.CreateReader(expectedFailureMechanismResult.HasLengthEffect);

            var iRow = startRow;
            while (iRow <= MaxRow)
            {
                var startMeters = GetCellValueAsDouble("C", iRow) * 1000.0;
                var endMeters = GetCellValueAsDouble("D", iRow) * 1000.0;

                if (double.IsNaN(startMeters) || double.IsNaN(endMeters))
                {
                    break;
                }

                sections.Add(sectionReader.ReadSection(iRow, startMeters, endMeters));

                iRow++;
            }

            expectedFailureMechanismResult.Sections = sections;
        }
    }
}