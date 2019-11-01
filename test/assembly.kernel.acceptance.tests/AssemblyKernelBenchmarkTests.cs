using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Assembly.Kernel.Model.CategoryLimits;
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
            BenchmarkTestResult result = new BenchmarkTestResult();

            TestEqualNormCategories(input, result);

            foreach (IFailureMechanismResult failureMechanism in input.ExpectedFailureMechanismsResults)
            {
                TestFailureMechanismAssembly(input, failureMechanism, result);
            }

            TestFinalVerdictAssembly(input, result);

            TestAssemblyOfCombinedSections(input, result);
        }

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
            catch (Exception )
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
            catch (Exception )
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
                        Assert.AreEqual(((FmSectionWithDirectCategory) expectedSections[i]).Category,
                            ((FmSectionWithDirectCategory) calculatedSections[i]).Category);
                    }
                    else
                    {
                        Assert.AreEqual(((FmSectionWithIndirectCategory) expectedSections[i]).Category,
                            ((FmSectionWithIndirectCategory) calculatedSections[i]).Category);
                    }
                }

                fmResult.AreEqualCombinedResultsCombinedSections = true;
            }
            catch (Exception )
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

        #region Test Assembly on assessment section level
        private static void TestFinalVerdictAssembly(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            // WBI-1-1
            var probabilisticFailureMechanismsCategories = TestCombinedProbabilisticFailureMechanismsCategoriesList(input, result);
            TestProbabilisticFailureMechanismsResults(input, result, probabilisticFailureMechanismsCategories);
            TestProbabilisticFailureMechanismsResultsTemporal(input, result, probabilisticFailureMechanismsCategories);
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
            var group3or4FailureMechanismResultsTemporal = input.ExpectedFailureMechanismsResults
                .Where(fm => fm.Group == 3 || fm.Group == 4)
                .Select(fm =>
                    new FailureMechanismAssemblyResult(
                        CastToEnum<EFailureMechanismCategory>(fm.ExpectedAssessmentResultTemporal),
                        double.NaN));
            var resultGroup3and4Temporal =
                assembler.AssembleAssessmentSectionWbi2A1(group3or4FailureMechanismResultsTemporal, true);
            try
            {
                Assert.IsNotNull(resultGroup3and4Temporal);
                Assert.AreEqual(resultGroup3and4Temporal,
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
            var group3or4FailureMechanismResults = input.ExpectedFailureMechanismsResults
                .Where(fm => fm.Group == 3 || fm.Group == 4)
                .Select(fm =>
                    new FailureMechanismAssemblyResult(
                        CastToEnum<EFailureMechanismCategory>(fm.ExpectedAssessmentResult),
                        double.NaN));
            var resultGroup3and4 = assembler.AssembleAssessmentSectionWbi2A1(group3or4FailureMechanismResults, false);
            try
            {
                Assert.IsNotNull(resultGroup3and4);
                Assert.AreEqual(resultGroup3and4,
                    input.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups3and4);
                result.AreEqualAssemblyResultGroup3and4 = true;
            }
            catch (Exception)
            {
                result.AreEqualAssemblyResultGroup3and4 = false;
            }
        }

        private static void TestProbabilisticFailureMechanismsResultsTemporal(BenchmarkTestInput input,
            BenchmarkTestResult result, CategoriesList<FailureMechanismCategory> categories)
        {
            var assembler = new AssessmentGradeAssembler();
            var probabilisticFailureMechanismResultsTemporal = input.ExpectedFailureMechanismsResults
                .OfType<ProbabilisticFailureMechanismResult>()
                .Select(fm =>
                    new FailureMechanismAssemblyResult(
                        CastToEnum<EFailureMechanismCategory>(fm.ExpectedAssessmentResultTemporal),
                        fm.ExpectedAssessmentResultProbabilityTemporal));
            var resultGroup1and2Temporal = assembler.AssembleAssessmentSectionWbi2B1(
                probabilisticFailureMechanismResultsTemporal,
                categories,
                true);
            try
            {
                Assert.IsNotNull(resultGroup1and2Temporal);
                Assert.AreEqual(resultGroup1and2Temporal.Category,
                    input.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups1and2Temporal);
                Assert.AreEqual(resultGroup1and2Temporal.FailureProbability,
                    input.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups1and2ProbabilityTemporal);
                result.AreEqualAssemblyResultGroup1and2Temporal = true;
            }
            catch (Exception)
            {
                result.AreEqualAssemblyResultGroup1and2Temporal = false;
            }
        }

        private static void TestProbabilisticFailureMechanismsResults(BenchmarkTestInput input,
            BenchmarkTestResult result, CategoriesList<FailureMechanismCategory> categories)
        {
            IEnumerable<FailureMechanismAssemblyResult> probabilisticFailureMechanismResults = input
                .ExpectedFailureMechanismsResults
                .OfType<ProbabilisticFailureMechanismResult>()
                .Select(fm =>
                    new FailureMechanismAssemblyResult(
                        CastToEnum<EFailureMechanismCategory>(fm.ExpectedAssessmentResult),
                        fm.ExpectedAssessmentResultProbability));

            // Test correct result for groups 1/2 and 3.4, WBI-2B-1
            var assembler = new AssessmentGradeAssembler();
            var resultGroup1and2 = assembler.AssembleAssessmentSectionWbi2B1(probabilisticFailureMechanismResults,
                categories,
                false);
            try
            {
                Assert.IsNotNull(resultGroup1and2);
                Assert.AreEqual(resultGroup1and2.Category,
                    input.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups1and2);
                Assert.AreEqual(resultGroup1and2.FailureProbability,
                    input.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups1and2Probability);
                result.AreEqualAssemblyResultGroup1and2 = true;
            }
            catch (Exception)
            {
                result.AreEqualAssemblyResultGroup1and2 = false;
            }
        }

        private static CategoriesList<FailureMechanismCategory> TestCombinedProbabilisticFailureMechanismsCategoriesList(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            var categoriesCalculator = new CategoryLimitsCalculator();

            var categories = categoriesCalculator.CalculateFailureMechanismCategoryLimitsWbi11(
                new AssessmentSection(input.Length, input.SignallingNorm, input.LowerBoundaryNorm),
                new FailureMechanism(1, input.ExpectedSafetyAssessmentAssemblyResult.CombinedFailureMechanismProbabilitySpace));

            AssertEqualCategoriesList(
                input.ExpectedSafetyAssessmentAssemblyResult.ExpectedCombinedFailureMechanismCategoriesGroup1and2,
                categories,
                () => { result.AreEqualCategoriesListGroup1and2 = true; });

            return categories;
        }

        private static T CastToEnum<T>(object o)
        {
            T enumVal = (T)Enum.ToObject(typeof(T), o);
            return enumVal;
        }

        #endregion

        private static void TestFailureMechanismAssembly(BenchmarkTestInput input, IFailureMechanismResult failureMechanismResult, BenchmarkTestResult result)
        {
            var group3OrHigherFailureMechanismResult = failureMechanismResult as IGroup3FailureMechanismResult;
            if (group3OrHigherFailureMechanismResult != null)
            {
                // TODO: test categories (section and fm level)
            }

            var fmResult = GetBenchmarkTestFailureMechanismResult(result, failureMechanismResult.Type);
            var testHelper = TestHelperFactory.CreateFailureMechanismTestHelper(failureMechanismResult);

            // TODO: What about results that are neither positive nor negative (no detailed assessmnet for example)
            try
            {
                testHelper.TestSimpleAssessment();
                fmResult.AreEqualSimpleAssessmentResults = true;
            }
            catch (Exception )
            {
                fmResult.AreEqualSimpleAssessmentResults = false;
            }

            try
            {
                testHelper.TestDetailedAssessment();
                fmResult.AreEqualDetailedAssessmentResults = true;
            }
            catch (Exception)
            {
                fmResult.AreEqualDetailedAssessmentResults = false;
            }

            try
            {
                testHelper.TestTailorMadeAssessment();
                fmResult.AreEqualTailorMadeAssessmentResults = true;
            }
            catch (Exception)
            {
                fmResult.AreEqualTailorMadeAssessmentResults = false;
            }

            try
            {
                testHelper.TestCombinedAssessment();
                fmResult.AreEqualCombinedAssessmentResultsPerSection = true;
            }
            catch (Exception)
            {
                fmResult.AreEqualCombinedAssessmentResultsPerSection = false;
            }

            try
            {
                testHelper.TestAssessmentSectionResult();
                fmResult.AreEqualAssessmentResultPerAssessmentSection = true;
            }
            catch (Exception )
            {
                fmResult.AreEqualAssessmentResultPerAssessmentSection = false;
            }

            try
            {
                testHelper.TestAssessmentSectionResultTemporal();
                fmResult.AreEqualAssessmentResultPerAssessmentSectionTemporal = true;
            }
            catch (Exception)
            {
                fmResult.AreEqualAssessmentResultPerAssessmentSectionTemporal = false;
            }
        }

        /*private static void TestCombinedSectionAssessment(IFailureMechanismResult failureMechanism)
        {
            var assembler = new FailureMechanismResultAssembler();

            if (failureMechanism.Group < 5)
            {
                // WBI-1A-1
                var result = assembler.AssembleFailureMechanismWbi1A1(
                    new[]
                    {
                        section.ExpectedSimpleAssessmentAssemblyResult as FmSectionAssemblyDirectResult,
                        section.ExpectedDetailedAssessmentAssemblyResult as FmSectionAssemblyDirectResult,
                        section.ExpectedTailorMadeAssessmentAssemblyResult as FmSectionAssemblyDirectResult,
                    },
                    false
                );

                Assert.AreEqual((section as FailureMechanismSectionBase<EFmSectionCategory>).ExpectedCombinedResult, result);
            }
            else
            {
                // WBI-1A-2
                var result = assembler.AssembleFailureMechanismWbi1A2(
                    new[]
                    {
                        section.ExpectedSimpleAssessmentAssemblyResult as FmSectionAssemblyIndirectResult,
                        section.ExpectedDetailedAssessmentAssemblyResult as FmSectionAssemblyIndirectResult,
                        section.ExpectedTailorMadeAssessmentAssemblyResult as FmSectionAssemblyIndirectResult,
                    },
                    false
                );

                Assert.AreEqual((section as FailureMechanismSectionBase<EFmSectionCategory>).ExpectedCombinedResult, result);
            }
        }*/
        #region Norm categories on assessment section level

        private static void TestEqualNormCategories(BenchmarkTestInput input, BenchmarkTestResult result)
        {
            var calculator = new CategoryLimitsCalculator();

            try
            {
                // WBI-2-1
                var categories = calculator.CalculateAssessmentSectionCategoryLimitsWbi21(new AssessmentSection(
                    input.Length,
                    input.SignallingNorm, input.LowerBoundaryNorm));
                var expectedCategories = input.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssessmentSectionCategories;

                AssertEqualCategoriesList(expectedCategories, categories, () => { result.AreEqualCategoriesListAssessmentSection = true; });
            }
            catch (Exception)
            {
                result.AreEqualCategoriesListAssessmentSection = false;
                // TODO: Administer result per assembly method (WBI-2-1)
            }
        }

        #endregion

        private static BenchmarkTestFailureMechanismResult GetBenchmarkTestFailureMechanismResult(BenchmarkTestResult result,
            MechanismType type)
        {
            var fmResult = result.FailureMechanismResults.FirstOrDefault(fmr => fmr.Type == type);
            if (fmResult == null)
            {
                fmResult = BenchmarkTestFailureMechanismResultFactory.CreateFailureMechanismResult(type);
                result.FailureMechanismResults.Add(fmResult);
            }

            return fmResult;
        }

        private IEnumerable<string> AcquireAllBenchmarkTests()
        {
            var testDirectory = Path.Combine(
                    Path.GetDirectoryName(
                            Uri.UnescapeDataString(new UriBuilder(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Path))
                        .Replace(@"\bin\Debug", ""),
                    "testdefinitions");

            return Directory.GetFiles(testDirectory, "*.xlsm");
        }
    }
}
