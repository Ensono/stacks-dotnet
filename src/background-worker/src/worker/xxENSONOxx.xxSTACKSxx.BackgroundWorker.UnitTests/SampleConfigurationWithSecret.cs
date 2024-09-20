using xxENSONOxx.xxSTACKSxx.BackgroundWorker.Secrets;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.UnitTests;

public class SampleConfigurationWithSecret
{
    public string TextValue { get; set; } = null!;
    public string WindowsTempFolder { get; set; } = null!;
    public string LinuxTempFolder { get; set; } = null!;
    public Secret EnvironmentSecret { get; set; } = null!;
    public Secret ImplicitEnvironmentSecret { get; set; } = null!;
    public Secret FilenameFileSecret { get; set; } = null!;
    public Secret RelativeFilenameFileSecret { get; set; } = null!;
    public Secret WindowsFullPathFileSecret { get; set; } = null!;
    public Secret LinuxFullPathFileSecret { get; set; } = null!;
    public Secret NonExistentRequiredEnvironmentSecret { get; set; } = null!;
    public Secret NonExistentRequiredFileSecret { get; set; } = null!;
    public Secret NonExistentOptionalEnvironmentSecret { get; set; } = null!;
    public Secret NonExistentOptionalFileSecret { get; set; } = null!;
}
