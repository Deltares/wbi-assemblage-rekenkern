﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using assembly.kernel.acceptance.tests.io.Readers;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.CategoryLimits;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MathNet.Numerics.Distributions;
using NUnit.Framework;
using AssessmentSection = assembly.kernel.acceptance.tests.data.AssessmentSection;

namespace assembly.kernel.acceptance.tests.io.tests.Readers
{
    [TestFixture]
    public class SafetyAssessmentResultReaderTest : TestFileReaderTestBase
    {
        [Test]
        public void ReaderReadsInformationCorrectly()
        {
            var testFile = Path.Combine(GetTestDir(), "Benchmarktool Excel assemblagetool (v1_0_1_0) 0_03.xlsm");

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(testFile, false))
            {

                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                var workSheetParts = ReadWorkSheetParts(workbookPart);
                var workSheetPart = workSheetParts["Gecombineerd veiligheidsoordeel"];

                var reader = new SafetyAssessmentFinalResultReader(workSheetPart, workbookPart);

                var result = new AssessmentSection();

                reader.Read(result);

                var assemblyResult = result.SafetyAssessmentAssemblyResult;
                Assert.AreEqual(0.58, assemblyResult.CombinedFailureMechanismProbabilitySpace, 0.001);
                Assert.AreEqual(EFailureMechanismCategory.IIIt, assemblyResult.ExpectedAssemblyResultGroups1and2);
                Assert.AreEqual(4.26e-4, assemblyResult.ExpectedAssemblyResultGroups1and2Probability, 1e-6);
                Assert.AreEqual(EFailureMechanismCategory.Vt, assemblyResult.ExpectedAssemblyResultGroups3and4);
                Assert.AreEqual(EAssessmentGrade.C, assemblyResult.ExpectedSafetyAssessmentAssemblyResult);

                var categories = assemblyResult.ExpectedFailureMechanismCategories.Categories;
                Assert.AreEqual(6, categories.Length);
                AssertAreEqualCategories(EFailureMechanismCategory.It, 0.0, 6.44e-6, categories[0]);
                AssertAreEqualCategories(EFailureMechanismCategory.IIt, 6.44e-6, 1.93e-4, categories[1]);
                AssertAreEqualCategories(EFailureMechanismCategory.IIIt, 1.93e-4, 5.80e-4, categories[2]);
                AssertAreEqualCategories(EFailureMechanismCategory.IVt, 5.80e-4, 1.00e-3, categories[3]);
                AssertAreEqualCategories(EFailureMechanismCategory.Vt, 1.00e-3, 3.00e-2, categories[4]);
                AssertAreEqualCategories(EFailureMechanismCategory.VIt, 3.00e-2, 1.00, categories[5]);
            }
        }

        private void AssertAreEqualCategories(EFailureMechanismCategory expectedCategory, double expectedLowerLimit,
            double expectedUpperLimit, FailureMechanismCategory assessmentSectionCategory)
        {
            Assert.AreEqual(expectedCategory, assessmentSectionCategory.Category);
            AssertAreEqualProbabilities(expectedLowerLimit, assessmentSectionCategory.LowerLimit);
            AssertAreEqualProbabilities(expectedUpperLimit, assessmentSectionCategory.UpperLimit);
        }

        private void AssertAreEqualProbabilities(double expectedProbability, double actualProbability)
        {
            Assert.AreEqual(ProbabilityToReliability(expectedProbability), ProbabilityToReliability(actualProbability),1e-3);
        }

        /// <summary>
        /// Calculates the reliability from a probability.
        /// </summary>
        /// <param name="probability">The probability to convert.</param>
        /// <returns>The reliability.</returns>
        private static double ProbabilityToReliability(double probability)
        {
            return Normal.InvCDF(0, 1, 1 - probability);
        }

        private static Dictionary<string, WorksheetPart> ReadWorkSheetParts(WorkbookPart workbookPart)
        {
            var workSheetParts = new Dictionary<string, WorksheetPart>();

            foreach (var worksheetPart in workbookPart.WorksheetParts)
            {
                var sheet = GetSheetFromWorkSheet(workbookPart, worksheetPart);
                workSheetParts[sheet.Name] = worksheetPart;
            }

            return workSheetParts;
        }

        private static Sheet GetSheetFromWorkSheet
            (WorkbookPart workbookPart, WorksheetPart worksheetPart)
        {
            string relationshipId = workbookPart.GetIdOfPart(worksheetPart);
            IEnumerable<Sheet> sheets = workbookPart.Workbook.Sheets.Elements<Sheet>();
            return sheets.FirstOrDefault(s => s.Id.HasValue && s.Id.Value == relationshipId);
        }
    }
}