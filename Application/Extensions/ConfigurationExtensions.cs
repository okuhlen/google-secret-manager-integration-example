using Application.Configuration;
using Application.Settings;
using Microsoft.Extensions.Configuration;

namespace Application.Extensions;

public static class ConfigurationExtensions
{
    public static IConfigurationBuilder AddGoogleSecretManagerConfiguration(this IConfigurationBuilder builder)
    {
        var tempConfig = builder.Build();
        var serviceAccountCredential =
            tempConfig.GetSection(nameof(GoogleServiceAccountSetting)).Get<GoogleServiceAccountSetting>();
        return builder.Add(new GoogleSecretManagerSource(serviceAccountCredential!));
    }
}