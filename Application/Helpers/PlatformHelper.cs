using System.Runtime.InteropServices;
using Application.Enums;
using Application.Models;

namespace Application.Helpers;

public static class PlatformHelper
{
    public static PlatformInformation GetServerInformation()
    {
        var model = new PlatformInformation();
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            model.OperatingSystemType = OperatingSystemType.Linux;
            model.Version = Environment.OSVersion.Version.ToString();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            model.OperatingSystemType = OperatingSystemType.Windows;
            model.Version = Environment.OSVersion.Version.ToString();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            model.OperatingSystemType = OperatingSystemType.MacOS;
            model.Version = Environment.OSVersion.Version.ToString();
        }
        else
        {
            throw new NotSupportedException("We cannot determine what OS the current machine is using");
        }
        
        return model;
    }
}