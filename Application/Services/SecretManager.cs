using Application.Settings;
using Google.Cloud.SecretManager.V1;
using Microsoft.Extensions.Options;
using Google.Api.Gax.ResourceNames;

namespace Application.Services;

public sealed class SecretManager(IOptions<GoogleServiceAccountSetting> googleConfiguration) : ISecretManager
{
    private readonly GoogleServiceAccountSetting _settings = googleConfiguration.Value;

    /// <summary>
    ///  NOTE: The code in this method was generated using Junie AI agent.
    /// </summary>
    /// <returns></returns>
    public async Task<List<string>> ListSecrets()
    {
        var client = await SecretManagerServiceClient.CreateAsync();
        var projectName = ProjectName.FromProject(_settings.ProjectId);
        var secrets = new List<string>();

        // List all secrets in the project
        var listSecretsRequest = client.ListSecrets(projectName);
        
        foreach (var secret in listSecretsRequest)
        {
            try
            {
                // Attempt to get the value of the latest version
                var secretVersionName = new SecretVersionName(_settings.ProjectId, secret.SecretName.SecretId, "latest");
                var valueResponse = await client.AccessSecretVersionAsync(secretVersionName);
                var secretValue = valueResponse.Payload.Data.ToStringUtf8();
                
                secrets.Add($"{secret.SecretName.SecretId}: {secretValue}");
            }
            catch (Exception)
            {
                // If a secret has no versions, we just list the name
                secrets.Add($"{secret.SecretName.SecretId}: [No Version Found]");
            }
        }

        return secrets;
    }

    /// <summary>
    ///  NOTE: The code in this method was generated using Junie AI agent.
    /// </summary>
    /// <returns></returns>
    public async Task AddSecret(string secretName, string secretValue)
    {
        var client = await SecretManagerServiceClient.CreateAsync();
        var projectName = ProjectName.FromProject(_settings.ProjectId);

        // 1. Create the secret container
        var secret = new Secret
        {
            Replication = new Replication
            {
                Automatic = new Replication.Types.Automatic()
            }
        };

        var createdSecret = await client.CreateSecretAsync(projectName, secretName, secret);

        // 2. Add the secret payload (the actual value)
        var payload = new SecretPayload
        {
            Data = Google.Protobuf.ByteString.CopyFromUtf8(secretValue)
        };

        await client.AddSecretVersionAsync(createdSecret.SecretName, payload);
    }
}