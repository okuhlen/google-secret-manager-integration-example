namespace SettingsLoader.Services;

public interface ISettingsManager
{
    Task LoadSecrets();

    Task ClearSecrets();
}