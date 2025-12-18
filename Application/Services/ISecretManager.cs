namespace Application.Services;

public interface ISecretManager
{
    Task<List<string>> ListSecrets();
    
    Task AddSecret(string secretName, string secretValue);
}