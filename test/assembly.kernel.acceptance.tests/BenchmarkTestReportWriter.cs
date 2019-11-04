using System;
using System.IO;
using assembly.kernel.acceptance.tests.data.Result;

namespace assemblage.kernel.acceptance.tests
{
    public static class BenchmarkTestReportWriter
    {
        public static void WriteReport(int order, BenchmarkTestResult result, string reportDirectory)
        {
            var template = GetReportTemplate();

            template = template.Replace("$BenchmarkTestName$", Path.GetFileNameWithoutExtension(result.FileName).Replace("_",@"\_"));
            template = template.Replace("$Order$", order.ToString());

            template = ReplaceFailureMechanismsTableWithResult(template, result);
            template = ReplaceCategoriesKeywordsWithResult(template, result);
            template = ReplaceFinalVerdictKeywordsWithResult(template, result);
            template = ReplaceCommonSectionsKeywordsWithResult(template, result);

            WriteReportToDestination(template, reportDirectory, GetTargetFileNameFromInputName(result.FileName));
        }

        private static string ReplaceFailureMechanismsTableWithResult(string template, BenchmarkTestResult result)
        {

            var str = "";
            for (var index = 0; index < result.FailureMechanismResults.Count; index++)
            {
                var m = result.FailureMechanismResults[index];
                str += m.Name + " & " + m.Type.ToString("G") + " & " + m.Group + " & " +
                       ToResultText(m.AreEqualCategoryBoundaries) + " & " +
                       ToResultText(m.AreEqualSimpleAssessmentResults) + " & " +
                       ToResultText(m.AreEqualDetailedAssessmentResults) + " & " +
                       ToResultText(m.AreEqualTailorMadeAssessmentResults) + " & " +
                       ToResultText(m.AreEqualCombinedAssessmentResultsPerSection) + " & " +
                       ToResultText(m.AreEqualAssessmentResultPerAssessmentSection) + " & " +
                       ToResultText(m.AreEqualAssessmentResultPerAssessmentSectionTemporal) + " & " +
                       ToResultText(m.AreEqualCombinedResultsCombinedSections);
                if (index != result.FailureMechanismResults.Count - 1)
                {
                    str += @" \B \\ \T" + "\n";
                }
            }

            return template.Replace("$FailureMechanismResultsTable$", str);
        }

        private static string ReplaceCategoriesKeywordsWithResult(string template, BenchmarkTestResult result)
        {
            template = template.Replace("$AreEqualCategoriesListAssessmentSection$", ToResultText(result.AreEqualCategoriesListAssessmentSection));
            template = template.Replace("$AreEqualCategoriesListGroup1and2$", ToResultText(result.AreEqualCategoriesListGroup1and2));
            return template;
        }

        private static string ReplaceFinalVerdictKeywordsWithResult(string template, BenchmarkTestResult result)
        {
            template = template.Replace("$AreEqualAssemblyResultGroup1and2$", ToResultText(result.AreEqualAssemblyResultGroup1and2));
            template = template.Replace("$AreEqualAssemblyResultGroup1and2Temporal$", ToResultText(result.AreEqualAssemblyResultGroup1and2Temporal));
            template = template.Replace("$AreEqualAssemblyResultGroup3and4$", ToResultText(result.AreEqualAssemblyResultGroup3and4));
            template = template.Replace("$AreEqualAssemblyResultGroup3and4Temporal$", ToResultText(result.AreEqualAssemblyResultGroup3and4Temporal));
            template = template.Replace("$AreEqualAssemblyResultFinalVerdict$", ToResultText(result.AreEqualAssemblyResultFinalVerdict));
            template = template.Replace("$AreEqualAssemblyResultFinalVerdictTemporal$", ToResultText(result.AreEqualAssemblyResultFinalVerdictTemporal));
            return template;
        }

        private static string ReplaceCommonSectionsKeywordsWithResult(string template, BenchmarkTestResult result)
        {
            template = template.Replace("$AreEqualAssemblyResultCombinedSections$", ToResultText(result.AreEqualAssemblyResultCombinedSections));
            template = template.Replace("$AreEqualAssemblyResultCombinedSectionsResults$", ToResultText(result.AreEqualAssemblyResultCombinedSectionsResults));
            template = template.Replace("$AreEqualAssemblyResultCombinedSectionsResultsTemporal$", ToResultText(result.AreEqualAssemblyResultCombinedSectionsResultsTemporal));
            return template;
        }

        private static void WriteReportToDestination(string template, string reportDirectory, string fileName)
        {
            var destinationFileName = Path.Combine(reportDirectory, fileName.Replace(" ","_"));
            if (File.Exists(destinationFileName))
            {
                throw new ArgumentException();
            }

            File.WriteAllText(destinationFileName, template);
        }

        private static string GetTargetFileNameFromInputName(string resultFileName)
        {
            return Path.GetFileNameWithoutExtension(resultFileName) + ".tex";
        }

        private static string GetReportTemplate()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var resourceName = "assemblage.kernel.acceptance.tests.Resources.reporttemplate.tex";

            string templateString;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    templateString = reader.ReadToEnd();
                }
            }

            return templateString;
        }

        private static string ToResultText(bool? result)
        {
            if (result == null)
            {
                return @"\nmark";
            }

            return (bool) result ? @"\cmark" : @"\xmark";
        }
    }
}
