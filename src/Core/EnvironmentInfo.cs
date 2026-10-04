using System;
using System.Runtime.InteropServices;

namespace Core;

public sealed record EnvironmentReport(
    string OsDescription,
    string FrameworkDescription,
    string ProcessArchitecture,
    string DetectedRid,
    string ReportedRid,
    string BaseDirectory,
    string TargetFrameworkNote);

public static class EnvironmentInfo
{
    public static EnvironmentReport Collect()
    {
#if NET10_0_OR_GREATER
        const string note = "Збірка під .NET 10.0 (LTS)";
#else
        const string note = "Збірка під .NET 8.0 (LTS)";
#endif

        return new EnvironmentReport(
            RuntimeInformation.OSDescription,
            RuntimeInformation.FrameworkDescription,
            RuntimeInformation.ProcessArchitecture.ToString(),
            DetectRid(),
            RuntimeInformation.RuntimeIdentifier,
            AppContext.BaseDirectory,
            note);
    }

    private static string DetectRid()
    {
        string os = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "win" :
                   RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "linux" :
                   RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "osx" : "unknown";

        string arch = RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X64 => "x64",
            Architecture.X86 => "x86",
            Architecture.Arm64 => "arm64",
            Architecture.Arm => "arm",
            _ => "unknown"
        };

        return $"{os}-{arch}";
    }
}