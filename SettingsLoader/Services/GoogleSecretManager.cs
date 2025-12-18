using System.Text.Json;
using Application.Extensions;
using Application.Helpers;
using Application.Settings;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.SecretManager.V1;
using Google.Protobuf;
using Microsoft.Extensions.Options;
using SettingsLoader.Models;

namespace SettingsLoader.Services;

public sealed class GoogleSecretManager(IOptions<GoogleServiceAccountSetting> googleServiceAccountSettings) : IGoogleSecretManager
{
    public async Task LoadSecrets(List<SettingRow> rowSettings)
    {
        var client =
            GoogleClientHelper.CreateSecretManagerClient(JsonSerializer.Serialize(googleServiceAccountSettings.Value));
        if (rowSettings.IsNullOrEmpty())
        {
            throw new Exception("No settings were found to load.");
        }

        var settingGroupings = rowSettings.ToLookup(row => $"{row.ObjectName}_{row.IsDevelopment}_{row.IsProduction}");
        var projectName = new ProjectName(googleServiceAccountSettings.Value.ProjectId);
        
        foreach (var settingGroup in settingGroupings)
        {
            var secret = new Secret
            {
                Replication = new Replication
                {
                    Automatic = new Replication.Types.Automatic()
                }
            };

            // Create the secret
            var addedSecret = await client.CreateSecretAsync(new CreateSecretRequest
            {
                ParentAsProjectName = projectName,
                SecretId = settingGroup.Key,
                Secret = secret
            });

            // Build the payload JSON object
            var jsonPayload = new Dictionary<string, object>();
            foreach (var row in settingGroup)
            {
                jsonPayload[row.Key] = row.Value;
            }

            // Serialize to JSON and upload
            var payloadString = JsonSerializer.Serialize(jsonPayload, new JsonSerializerOptions
            {
                WriteIndented = false
            });

            var secretPayload = new SecretPayload
            {
                Data = ByteString.CopyFromUtf8(payloadString)
            };

            // Add the secret version with the payload
            var createdSecretVersion = await client.AddSecretVersionAsync(new AddSecretVersionRequest
            {
                ParentAsSecretName = addedSecret.SecretName,
                Payload = secretPayload
            });
            
            //enable the secret version
            var secretVersion = new SecretVersionName(projectName.ProjectId, addedSecret.SecretName.SecretId,
                createdSecretVersion.SecretVersionName.SecretVersionId);
            await client.EnableSecretVersionAsync(secretVersion);
        }
    }

    public async Task ClearSecrets()
    {
        var client =
            GoogleClientHelper.CreateSecretManagerClient(JsonSerializer.Serialize(googleServiceAccountSettings.Value));
        var projectName = new ProjectName(googleServiceAccountSettings.Value.ProjectId);
        
        foreach (var secret in client.ListSecrets(projectName))
        {
            await client.DeleteSecretAsync(new DeleteSecretRequest
            {
                SecretName = secret.SecretName
            });
        }
    }
}