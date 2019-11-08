using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;

namespace assembly.kernel.benchmark.tests.io.Readers.FailureMechanismSection
{
    public interface ISectionReader
    {
        IFailureMechanismSection ReadSection(int iRow, double startMeters, double endMeters);
    }
}