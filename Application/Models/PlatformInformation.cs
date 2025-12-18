using Application.Enums;

namespace Application.Models;

public sealed class PlatformInformation
{
    public OperatingSystemType OperatingSystemType { get; set; }
    
    public string Version { get; set; }
}