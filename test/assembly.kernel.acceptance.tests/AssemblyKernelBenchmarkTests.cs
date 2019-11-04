using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using assemblage.kernel.acceptance.tests.TestHelpers;
using assembly.kernel.acceptance.tests.data.Input;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanismSections;
using assembly.kernel.acceptance.tests.data.Result;
using assembly.kernel.acceptance.tests.io;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FmSectionTypes;
using NUnit.Framework;

namespace assemblage.kernel.acceptance.tests
{
    [TestFixture]
    public class AssemblyKernelBenchmarkTests : BenchmarkTestsBase
    {
        [Test]
        public void RunAllBenchmarkTests()
        {
            IEnumerable<string> tests = AcquireAllBenchmarkTests();

            foreach (var test in tests)
            {
                RunBenchmarkTest(test);
            }
        }

        private static void RunBenchmarkTest(string fileName)
        {
            BenchmarkTestInput input = AssemblyExcelFileReader.Read(fileName);
            BenchmarkTestResult testResult = new BenchmarkTestResult();

            TestEqualNormCategories(input, testResult);

            foreach (IExpectedFailureMechanismResult expectedFailureMechanismResult in input.ExpectedFailureMechanismsResults)
            {
                TestFailureMechanismAssembly(expectedFailureMechanismResult, input.LowerBoundaryNorm, input.SignallingNorm, testResult);
            }

            TestFinalVerdictAssembly(input, testResult);

            TestAssemblyOfCombinedSections(input, testResult);
        }

        #region Norm categories on assessment section level

        private static void TestEqualNormCategories(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            var calculator = new CategoryLimitsCalculator();

            // WBI-2-1
            var categories = calculator.CalculateAssessmentSectionCategoryLimitsWbi21(new AssessmentSection(
                input.Length,
                input.SignallingNorm, input.LowerBoundaryNorm));
            var expectedCategories = input.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssessmentSectionCategories;

            result.AreEqualCategoriesListAssessmentSection = AssertEqualCategoriesList(expectedCategories, categories);
        }

        #endregion

        #region FailureMechanism specific results

        private static void TestFailureMechanismAssembly(IExpectedFailureMechanismResult expectedFailureMechanismResult,
            double lowerBoundaryNorm, double signallingNorm, BenchmarkTestResult testResult)
        {
            var failureMechanismTestResult = GetBenchmarkTestFailureMechanismResult(testResult, expectedFailureMechanismResult.Type);

            failureMechanismTestResult.AreEqualCategoryBoundaries =
                TesterFactory
                    .CreateCategoriesTester(expectedFailureMechanismResult, lowerBoundaryNorm, signallingNorm)
                    ?.TestCategories();

            var failureMechanismTestHelper = TesterFactory.CreateFailureMechanismTester(expectedFailureMechanismResult);
            failureMechanismTestResult.AreEqualSimpleAssessmentResults = failureMechanismTestHelper.TestSimpleAssessment();
            failureMechanismTestResult.AreEqualDetailedAssessmentResults = failureMechanismTestHelper.TestDetailedAssessment();
            failureMechanismTestResult.AreEqualTailorMadeAssessmentResults = failureMechanismTestHelper.TestTailorMadeAssessment();
            failureMechanismTestResult.AreEqualCombinedAssessmentResultsPerSection = failureMechanismTestHelper.TestCombinedAssessment();
            failureMechanismTestResult.AreEqualAssessmentResultPerAssessmentSection = failureMechanismTestHelper.TestAssessmentSectionResult();
            failureMechanismTestResult.AreEqualAssessmentResultPerAssessmentSectionTemporal = failureMechanismTestHelper.TestAssessmentSectionResultTemporal();
        }

        #endregion

        #region Test Assembly on assessment section level
        private static void TestFinalVerdictAssembly(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            // WBI-1-1
            TestCombinedProbabilisticFailureMechanismsCategoriesList(input, result);
            TestProbabilisticFailureMechanismsResults(input, result);
            TestProbabilisticFailureMechanismsResultsTemporal(input, result);
            TestGroup3And4FailureMechanismsResults(input, result);
            TestGroup3And4FailureMechanismsResultsTemporal(input, result);
            TestFinalAssessmentGrade(input, result);
            TestFinalAssessmentGradeTemporal(input, result);
        }

        private static void TestFinalAssessmentGradeTemporal(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            var assembler = new AssessmentGradeAssembler();
            var resultFinalVerdictTemporal = assembler.AssembleAssessmentSectionWbi2C1(
                input.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups3and4Temporal,
                new FailureMechanismAssemblyResult(
                    input.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups1and2Temporal,
                    input.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups1and2ProbabilityTemporal));
            try
            {
                Assert.AreEqual(input.ExpectedSafetyAssessmentAssemblyResult.ExpectedSafetyAssessmentAssemblyResultTemporal,
                    resultFinalVerdictTemporal);
                result.AreEqualAssemblyResultFinalVerdictTemporal = true;
            }
            catch (Exception)
            {
                result.AreEqualAssemblyResultFinalVerdictTemporal = false;
            }
        }

        private static void TestFinalAssessmentGrade(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            var assembler = new AssessmentGradeAssembler();
            var resultFinalVerdict = assembler.AssembleAssessmentSectionWbi2C1(
                input.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups3and4,
                new FailureMechanismAssemblyResult(
                    input.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups1and2,
                    input.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups1and2Probability));
            try
            {
                Assert.AreEqual(input.ExpectedSafetyAssessmentAssemblyResult.ExpectedSafetyAssessmentAssemblyResult,
                    resultFinalVerdict);
                result.AreEqualAssemblyResultFinalVerdict = true;
            }
            catch (Exception)
            {
                result.AreEqualAssemblyResultFinalVerdict = false;
            }
        }

        private static void TestGroup3And4FailureMechanismsResultsTemporal(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            var assembler = new AssessmentGradeAssembler();
            var group3Or4FailureMechanismResultsTemporal = input.ExpectedFailureMechanismsResults
                .Where(fm => fm.Group == 3 || fm.Group == 4)
                .Select(fm =>
                    new FailureMechanismAssemblyResult(
                        CastToEnum<EFailureMechanismCategory>(fm.ExpectedAssessmentResultTemporal),
                        double.NaN));
            var resultGroup3And4Temporal =
                assembler.AssembleAssessmentSectionWbi2A1(group3Or4FailureMechanismResultsTemporal, true);
            try
            {
                Assert.IsNotNull(resultGroup3And4Temporal);
                Assert.AreEqual(resultGroup3And4Temporal,
                    input.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups3and4Temporal);
                result.AreEqualAssemblyResultGroup3and4Temporal = true;
            }
            catch (Exception)
            {
                result.AreEqualAssemblyResultGroup3and4Temporal = false;
            }
        }

        private static void TestGroup3And4FailureMechanismsResults(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            var assembler = new AssessmentGradeAssembler();
            var group3Or4FailureMechanismResults = input.ExpectedFailureMechanismsResults
                .Where(fm => fm.Group == 3 || fm.Group == 4)
                .Select(fm =>
                    new FailureMechanismAssemblyResult(
                        CastToEnum<EFailureMechanismCategory>(fm.ExpectedAssessmentResult),
                        double.NaN));
            var resultGroup3And4 = assembler.AssembleAssessmentSectionWbi2A1(group3Or4FailureMechanismResults, false);
            try
            {
                Assert.IsNotNull(resultGroup3And4);
                Assert.AreEqual(resultGroup3And4,
                    input.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups3and4);
                result.AreEqualAssemblyResultGroup3and4 = true;
            }
            catch (Exception)
            {
                result.AreEqualAssemblyResultGroup3and4 = false;
            }
        }

        private static void TestProbabilisticFailureMechanismsResultsTemporal(BenchmarkTestInput input,
            BenchmarkTestResult result)
        {
            var assembler = new AssessmentGradeAssembler();
            var probabilisticFailureMechanismResultsTemporal = input.ExpectedFailureMechanismsResults
                .OfType<ProbabilisticExpectedFailureMechanismResult>()
                .Select(fm =>
                    new FailureMechanismAssemblyResult(
                        CastToEnum<EFailureMechanismCategory>(fm.ExpectedAssessmentResultTemporal),
                        fm.ExpectedAssessmentResultProbabilityTemporal));
            var categories = input.ExpectedSafetyAssessmentAssemblyResult.ExpectedCombinedFailureMechanismCategoriesGroup1and2;

            var resultGroup1And2Temporal = assembler.AssembleAssessmentSectionWbi2B1(
                probabilisticFailureMechanismResultsTemporal,
                categories,
                true);
            try
            {
                Assert.IsNotNull(resultGroup1And2Temporal);
                Assert.AreEqual(resultGroup1And2Temporal.Category,
                    input.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups1and2Temporal);
                Assert.AreEqual(resultGroup1And2Temporal.FailureProbability,
                    input.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups1and2ProbabilityTemporal);
                result.AreEqualAssemblyResultGroup1and2Temporal = true;
            }
            catch (Exception)
            {
                result.AreEqualAssemblyResultGroup1and2Temporal = false;
            }
        }

        private static void TestProbabilisticFailureMechanismsResults(BenchmarkTestInput input,
            BenchmarkTestResult result)
        {
            IEnumerable<FailureMechanismAssemblyResult> probabilisticFailureMechanismResults = input
                .ExpectedFailureMechanismsResults
                .OfType<ProbabilisticExpectedFailureMechanismResult>()
                .Select(fm =>
                    new FailureMechanismAssemblyResult(
                        CastToEnum<EFailureMechanismCategory>(fm.ExpectedAssessmentResult),
                        fm.ExpectedAssessmentResultProbability));
            var categories = input.ExpectedSafetyAssessmentAssemblyResult.ExpectedCombinedFailureMechanismCategoriesGroup1and2;

            // Test correct result for groups 1/2 and 3.4, WBI-2B-1
            var assembler = new AssessmentGradeAssembler();
            var resultGroup1And2 = assembler.AssembleAssessmentSectionWbi2B1(probabilisticFailureMechanismResults,
                categories,
                false);
            try
            {
                Assert.IsNotNull(resultGroup1And2);
                Assert.AreEqual(resultGroup1And2.Category,
                    input.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups1and2);
                Assert.AreEqual(resultGroup1And2.FailureProbability,
                    input.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups1and2Probability);
                result.AreEqualAssemblyResultGroup1and2 = true;
            }
            catch (Exception)
            {
                result.AreEqualAssemblyResultGroup1and2 = false;
            }
        }

        private static void TestCombinedProbabilisticFailureMechanismsCategoriesList(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            var categoriesCalculator = new CategoryLimitsCalculator();

            var categories = categoriesCalculator.CalculateFailureMechanismCategoryLimitsWbi11(
                new AssessmentSection(input.Length, input.SignallingNorm, input.LowerBoundaryNorm),
                new FailureMechanism(1, input.ExpectedSafetyAssessmentAssemblyResult.CombinedFailureMechanismProbabilitySpace));

            result.AreEqualCategoriesListGroup1and2  = AssertEqualCategoriesList(
                input.ExpectedSafetyAssessmentAssemblyResult.ExpectedCombinedFailureMechanismCategoriesGroup1and2,
                categories);
        }

        private static T CastToEnum<T>(object o)
        {
            T enumVal = (T)Enum.ToObject(typeof(T), o);
            return enumVal;
        }

        #endregion

        #region Test Combined sections

        private static void TestAssemblyOfCombinedSections(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            TestGeneratedCombinedSections(input, result);
            TestCombinedSectionsFinalResults(input, result);
            TestCombinedSectionsFinalResultsTemporal(input, result);

            foreach (FailureMechanismSectionList failureMechanismsCombinedResult in input.ExpectedCombinedSectionResultPerFailureMechanism)
            {
                TestCombinedSectionsFailureMechanismResults(input, result,
                    failureMechanismsCombinedResult.FailureMechanismId.ToMechanismType());
            }
        }

        private static void TestGeneratedCombinedSections(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            var assembler = new CommonFailureMechanismSectionAssembler();
            // WBI-3A-1
            var combinedSections = assembler.FindGreatestCommonDenominatorSectionsWbi3A1(
                input.ExpectedFailureMechanismsResults.Select(
                    fm => new FailureMechanismSectionList(fm.Name,
                        fm.Sections.Select(s => new FailureMechanismSection(s.Start, s.End)))).ToArray()
                , input.Length);

            try
            {
                Assert.IsNotNull(combinedSections);
                var expectedSections = input.ExpectedCombinedSectionResult.ToArray();
                var calculatedSections = combinedSections.Sections.ToArray();
                Assert.AreEqual(expectedSections.Length, calculatedSections.Length);
                for (int i = 0; i < expectedSections.Length; i++)
                {
                    Assert.AreEqual(expectedSections[i].SectionStart, calculatedSections[i].SectionStart, 0.01);
                    Assert.AreEqual(expectedSections[i].SectionEnd, calculatedSections[i].SectionEnd, 0.01);
                }

                result.AreEqualAssemblyResultCombinedSections = true;
            }
            catch (Exception)
            {
                result.AreEqualAssemblyResultCombinedSections = false;
            }
        }

        private static void TestCombinedSectionsFinalResults(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            var assembler = new CommonFailureMechanismSectionAssembler();

            var calculatedResults = assembler.DeterminCombinedResultPerCommonSectionWbi3C1(input.ExpectedCombinedSectionResultPerFailureMechanism, false).ToArray();
            var expectedResults = input.ExpectedCombinedSectionResult.ToArray();
            try
            {
                Assert.AreEqual(expectedResults.Length, calculatedResults.Length);
                for (int i = 0; i < expectedResults.Length; i++)
                {
                    Assert.AreEqual(expectedResults[i].SectionStart, calculatedResults[i].SectionStart, 0.01);
                    Assert.AreEqual(expectedResults[i].SectionEnd, calculatedResults[i].SectionEnd, 0.01);
                    Assert.AreEqual(expectedResults[i].Category, calculatedResults[i].Category);
                }

                result.AreEqualAssemblyResultCombinedSectionsResults = true;
            }
            catch (Exception)
            {
                result.AreEqualAssemblyResultCombinedSectionsResults = false;
            }
        }

        private static void TestCombinedSectionsFinalResultsTemporal(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            var assembler = new CommonFailureMechanismSectionAssembler();

            var calculatedResults = assembler.DeterminCombinedResultPerCommonSectionWbi3C1(input.ExpectedCombinedSectionResultPerFailureMechanism, true).ToArray();
            var expectedResults = input.ExpectedCombinedSectionResultTemporal.ToArray();
            try
            {
                Assert.AreEqual(expectedResults.Length, calculatedResults.Length);
                for (int i = 0; i < expectedResults.Length; i++)
                {
                    Assert.AreEqual(expectedResults[i].SectionStart, calculatedResults[i].SectionStart, 0.01);
                    Assert.AreEqual(expectedResults[i].SectionEnd, calculatedResults[i].SectionEnd, 0.01);
                    Assert.AreEqual(expectedResults[i].Category, calculatedResults[i].Category);
                }

                result.AreEqualAssemblyResultCombinedSectionsResultsTemporal = true;
            }
            catch (Exception)
            {
                result.AreEqualAssemblyResultCombinedSectionsResultsTemporal = false;
            }
        }

        private static void TestCombinedSectionsFailureMechanismResults(BenchmarkTestInput input, BenchmarkTestResult result, MechanismType type)
        {
            var assembler = new CommonFailureMechanismSectionAssembler();

            var combinedSections = new FailureMechanismSectionList("", input.ExpectedCombinedSectionResult);
            var calculatedSectionResults = assembler.TranslateFailureMechanismResultsToCommonSectionsWbi3B1(
                new FailureMechanismSectionList(
                    type.ToString("D"),
                    input.ExpectedFailureMechanismsResults.First(fm => fm.Type == type).Sections
                        .Select(CreateFailureMechanismSectionWithResult)),
                combinedSections);

            var isDirectMechanism = input.ExpectedFailureMechanismsResults.First(fm => fm.Type == type).Group < 5;
            var calculatedSections = calculatedSectionResults.Sections.ToArray();
            var expectedSections = input.ExpectedCombinedSectionResultPerFailureMechanism.First(l =>
                l.FailureMechanismId == type.ToString("D")).Sections.ToArray();

            var fmResult = GetBenchmarkTestFailureMechanismResult(result, type);

            try
            {
                Assert.AreEqual(expectedSections.Length, calculatedSections.Length);
                for (int i = 0; i < expectedSections.Length; i++)
                {
                    Assert.AreEqual(expectedSections[i].SectionStart, calculatedSections[i].SectionStart, 0.01);
                    Assert.AreEqual(expectedSections[i].SectionEnd, calculatedSections[i].SectionEnd, 0.01);
                    if (isDirectMechanism)
                    {
                        Assert.AreEqual(((FmSectionWithDirectCategory)expectedSections[i]).Category,
                            ((FmSectionWithDirectCategory)calculatedSections[i]).Category);
                    }
                    else
                    {
                        Assert.AreEqual(((FmSectionWithIndirectCategory)expectedSections[i]).Category,
                            ((FmSectionWithIndirectCategory)calculatedSections[i]).Category);
                    }
                }

                fmResult.AreEqualCombinedResultsCombinedSections = true;
            }
            catch (Exception)
            {
                fmResult.AreEqualCombinedResultsCombinedSections = false;
            }
        }

        private static FailureMechanismSection CreateFailureMechanismSectionWithResult(IFailureMechanismSection section)
        {
            var directMechanism = section as FailureMechanismSectionBase<EFmSectionCategory>;
            return directMechanism != null
                ? new FmSectionWithDirectCategory(directMechanism.Start, directMechanism.End,
                    directMechanism.ExpectedCombinedResult)
                : (FailureMechanismSection)new FmSectionWithIndirectCategory(section.Start, section.End,
                    ((FailureMechanismSectionBase<EIndirectAssessmentResult>)section).ExpectedCombinedResult);
        }

        #endregion

        private static BenchmarkFailureMechanismTestResult GetBenchmarkTestFailureMechanismResult(BenchmarkTestResult result,
            MechanismType type)
        {
            var failureMechanismTestResult = result.FailureMechanismResults.FirstOrDefault(fmr => fmr.Type == type);
            if (failureMechanismTestResult == null)
            {
                failureMechanismTestResult = BenchmarkTestFailureMechanismResultFactory.CreateFailureMechanismTestResult(type);
                result.FailureMechanismResults.Add(failureMechanismTestResult);
            }

            return failureMechanismTestResult;
        }

        private IEnumerable<string> AcquireAllBenchmarkTests()
        {
            var projectFileDirectory =
                Path.GetDirectoryName(
                    Uri.UnescapeDataString(new UriBuilder(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
                        .Path));
            if (projectFileDirectory == null)
            {
                throw new ArgumentException();
            }
            var testDirectory = Path.Combine(projectFileDirectory.Replace(@"\bin\Debug", ""),"testdefinitions");

            return Directory.GetFiles(testDirectory, "*.xlsm");
        }
    }
}
