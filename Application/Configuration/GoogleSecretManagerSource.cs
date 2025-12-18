using Application.Constants;
using Application.Settings;
using Microsoft.Extensions.Configuration;

namespace Application.Configuration;

public class GoogleSecretManagerSource(GoogleServiceAccountSetting serviceAccountSetting) : IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        var shouldLoadDevelopmentSettings = Environment.GetEnvironmentVariable(SettingNames.AspNetCoreEnvironment)!.Equals(
            ConstantValues.Development);
        
        return new GoogleSecretManagerProvider(serviceAccountSetting, shouldLoadDevelopmentSettings);
    }
}