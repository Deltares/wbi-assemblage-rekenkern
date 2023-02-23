// Copyright (C) Rijkswaterstaat 2022. All rights reserved.
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

using System.Collections.Generic;
using Assembly.Kernel.Acceptance.TestUtil.Data.Input;
using Assembly.Kernel.Acceptance.TestUtil.Data.Input.FailureMechanisms;
using Assembly.Kernel.Acceptance.TestUtil.Data.Input.FailureMechanismSections;
using Assembly.Kernel.Acceptance.TestUtil.IO.FailureMechanismSection;
using Assembly.Kernel.Model;
using DocumentFormat.OpenXml.Packaging;

namespace Assembly.Kernel.Acceptance.TestUtil.IO
{
    /// <summary>
    /// Reader to read failure mechanism.
    /// </summary>
    public class FailureMechanismsReader : ExcelSheetReaderBase
    {
        private const double KilometersToMeters = 1000.0;
        private readonly SectionReaderFactory sectionReaderFactory;

        /// <summary>
        /// Creates a new instance of <see cref="FailureMechanismsReader"/>.
        /// </summary>
        /// <param name="worksheetPart">The worksheet for which to create a dictionary.</param>
        /// <param name="workbookPart">The workbook part of the workbook that contains this worksheet.</param>
        public FailureMechanismsReader(WorksheetPart worksheetPart, WorkbookPart workbookPart)
            : base(worksheetPart, workbookPart, "B")
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
            bool hasLengthEffect = GetCellValueAsString("C", "Lengte-effect") == "Ja";
            string failureMechanismType = GetCellValueAsString("C", "Faalpad");
            string assemblyMethod = GetCellValueAsString("C", "Methode");
            var expectedFailureMechanismResult = new ExpectedFailureMechanismResult(
                failureMechanismType, mechanismId, hasLengthEffect, assemblyMethod);

            ReadGeneralInformation(expectedFailureMechanismResult);
            ReadFailureMechanismSections(expectedFailureMechanismResult);

            benchmarkTestInput.ExpectedFailureMechanismsResults.Add(expectedFailureMechanismResult);
        }

        private void ReadGeneralInformation(ExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            expectedFailureMechanismResult.ExpectedCombinedProbabilityPartial = new Probability(GetCellValueAsDouble("C", "Faalkans tussentijds"));
            expectedFailureMechanismResult.ExpectedIsSectionsCorrelatedPartial = ToCorrelation(GetCellValueAsString("C", "Vakken gecorreleerd?") == "Ja");
            expectedFailureMechanismResult.ExpectedCombinedProbability = new Probability(GetCellValueAsDouble("C", "Faalkans"));
            expectedFailureMechanismResult.ExpectedIsSectionsCorrelated = ToCorrelation(
                double.IsNaN(expectedFailureMechanismResult.ExpectedCombinedProbability)
                || expectedFailureMechanismResult.ExpectedIsSectionsCorrelatedPartial == EFailureMechanismAssemblyMethod.Correlated);
            expectedFailureMechanismResult.LengthEffectFactor = GetCellValueAsDouble("C", "Ntraject");
        }

        private static EFailureMechanismAssemblyMethod ToCorrelation(bool correlated)
        {
            return correlated
                       ? EFailureMechanismAssemblyMethod.Correlated
                       : EFailureMechanismAssemblyMethod.Uncorrelated;
        }

        private void ReadFailureMechanismSections(ExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            var sections = new List<IExpectedFailureMechanismSection>();
            int startRow = GetRowId("Vaknaam") + 1;
            ISectionReader<IExpectedFailureMechanismSection> sectionReader = sectionReaderFactory.CreateReader(expectedFailureMechanismResult.HasLengthEffect);

            int iRow = startRow;
            while (iRow <= MaxRow)
            {
                double startMeters = GetCellValueAsDouble("C", iRow) * KilometersToMeters;
                double endMeters = GetCellValueAsDouble("D", iRow) * KilometersToMeters;

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