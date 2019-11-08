using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;

namespace assembly.kernel.benchmark.tests.io.Readers.FailureMechanismSection
{
    public interface ISectionReader
    {
        /// <summary>
        /// Reads the specified section.
        /// </summary>
        /// <param name="iRow">Rownumber in the excel sheet of the section that needs to be read</param>
        /// <param name="startMeters">Start position of the section (already read)</param>
        /// <param name="endMeters">End position of the section (already read)</param>
        /// <returns></returns>
        IFailureMechanismSection ReadSection(int iRow, double startMeters, double endMeters);
    }
}