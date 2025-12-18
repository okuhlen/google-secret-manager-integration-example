using System.Text;
using Application.Constants;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.SecretManager.V1;

namespace Application.Helpers;

public static class GoogleClientHelper
{
    public static SecretManagerServiceClient CreateSecretManagerClient(string jsonSetting)
    {
        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonSetting));
        var clientCredential = ServiceAccountCredential.FromServiceAccountData(memoryStream);
        var builder = new SecretManagerServiceClientBuilder();
        builder.Credential = clientCredential;
        
        return builder.Build();
    }

    public static SecretManagerServiceClient CreateSecretManagerClientFromEnvironment()
    {
        var credential = GoogleCredential.FromFile(SettingNames.GoogleCredentialsJsonLocation);
        var builder = new SecretManagerServiceClientBuilder
        {
            Credential = credential,
        };
        return builder.Build();
    }
}