using SettingsLoader.Models;

namespace SettingsLoader.Services;

public interface IGoogleSecretManager
{
    Task LoadSecrets(List<SettingRow> rowSettings);

    Task ClearSecrets();
}