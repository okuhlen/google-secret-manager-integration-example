using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using Application.Enums;
using Application.Helpers;
using Application.Settings;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.SecretManager.V1;
using Microsoft.Extensions.Configuration;

namespace Application.Configuration;
public sealed class GoogleSecretManagerProvider(
    GoogleServiceAccountSetting serviceCredential, bool isDevelopment)
    : ConfigurationProvider
{
    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.Default,
    };

    public override void Load()
    {
        var platformInfo = PlatformHelper.GetServerInformation();
        SecretManagerServiceClient? client = null;
        if (platformInfo.OperatingSystemType == OperatingSystemType.Linux)
        {
            client = GoogleClientHelper.CreateSecretManagerClientFromEnvironment();
        }
        else
        {
            client = GoogleClientHelper.CreateSecretManagerClient(JsonSerializer.Serialize(serviceCredential, _options));
        }
        var settingsCollection = new Dictionary<string, string>();
        var projectName = new ProjectName(serviceCredential!.ProjectId);
        var secrets = client.ListSecrets(new ListSecretsRequest()
        {
            ParentAsProjectName = projectName
        });
        if (!secrets.Any())
        {
            throw new Exception("The secret manager store does not have any settings to load. Please check again");
        }

        var filteredSecrets = secrets.Where(secret =>
        {
            var parts = secret.SecretName.SecretId.Split('_');
            if (parts.Length != 3)
            {
                return false; // Unexpected format, skip
            }

            var isDevFlag = bool.TryParse(parts[1], out var isDev) && isDev;
            var isProdFlag = bool.TryParse(parts[2], out var isProd) && isProd;

            return isDevelopment ? isDevFlag : isProdFlag;
        }).ToList();
        
        var subItem = new Dictionary<string, string>();
        foreach (var secret in filteredSecrets)
        {
            var secretValue = client.AccessSecretVersion(new AccessSecretVersionRequest
            {
                SecretVersionName = new SecretVersionName(projectName.ProjectId, secret.SecretName.SecretId, "latest")
            });
            
            var jsonObject = JsonSerializer.Deserialize<JsonObject>(secretValue.Payload.Data.ToStringUtf8()!)!;
            foreach (var key in jsonObject.Select(x => x.Key).ToList())
            {
                var parts = secret.SecretName.SecretId.Split('_');
                var keyName = $"{parts[0]}:{key}";
                var keyValue = jsonObject[key].ToString();
                subItem.Add(keyName, keyValue);
            }
        }

        Data = subItem;
        base.Load();
    }
}