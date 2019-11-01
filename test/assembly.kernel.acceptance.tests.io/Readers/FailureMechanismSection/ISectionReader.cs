using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;

namespace assembly.kernel.acceptance.tests.io.Readers.FailureMechanismSection
{
    public interface ISectionReader
    {
        IFailureMechanismSection ReadSection(int iRow, double startMeters, double endMeters);
    }
}