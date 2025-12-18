using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace Application.Settings;

/// <summary>
/// To understand how this class was constructed, please have a look at the google-secrets.json file that contains the configuration for the Google Service Account. This file is used to configure the Google Secret Manager client library in the application.
/// </summary>
public class GoogleServiceAccountSetting
{
    [ConfigurationKeyName("type")]
    [JsonPropertyName("type")]
    public string Type { get; set; }
    
    [ConfigurationKeyName("project_id")]
    [JsonPropertyName("project_id")]
    public string ProjectId { get; set; }
    
    [ConfigurationKeyName("private_key")]
    [JsonPropertyName("private_key")]
    public string PrivateKey { get; set; }
    
    [ConfigurationKeyName("private_key_id")]
    [JsonPropertyName("private_key_id")]
    public string PrivateKeyId { get; set; }
    
    [ConfigurationKeyName("client_email")]
    [JsonPropertyName("client_email")]
    public string ClientEmail { get; set; }
    
    [ConfigurationKeyName("client_id")]
    [JsonPropertyName("client_id")]
    public string ClientId { get; set; }
    
    [ConfigurationKeyName("auth_uri")]
    [JsonPropertyName("auth_uri")]
    public string AuthUri { get; set; }
    
    [ConfigurationKeyName("token_uri")]
    [JsonPropertyName("token_uri")]
    public string TokenUri { get; set; }
    
    [ConfigurationKeyName("auth_provider_x509_cert_url")]
    [JsonPropertyName("auth_provider_x509_cert_url")]
    public string AuthProviderX509CertUrl { get; set; }
    
    [ConfigurationKeyName("client_x509_cert_url")]
    [JsonPropertyName("client_x509_cert_url")]
    public string ClientX509CertUrl { get; set; }
    
    [ConfigurationKeyName("universe_domain")]
    [JsonPropertyName("universe_domain")]
    public string UniverseDomain { get; set; }
    
    public static string GetJsonPropertyName(string propertyName)
    {
        var property = typeof(GoogleServiceAccountSetting)
            .GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

        if (property != null)
        {
            var attribute = property.GetCustomAttribute<JsonPropertyNameAttribute>();
            return attribute?.Name;
        }

        return null; // Return null if the property is not found or has no JsonPropertyName attribute
    }
}