namespace SettingsLoader.Models;

public sealed class SettingRow
{
    public string ObjectName { get; set; }
    
    public string Key { get; set; }
    
    public string Value { get; set; }
    
    public string? Note { get; set; }
    
    public bool IsDevelopment { get; set; }
    
    public bool IsProduction { get; set; }
}