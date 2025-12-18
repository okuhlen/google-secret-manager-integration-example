using Application.Services;
using Application.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace WebApiExample.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public sealed class SampleController(ISecretManager settingsManager, 
    IOptions<MyTestConfiguration> myOtherConfiguration) : Controller
{
    
    /// <summary>
    /// This is a sample action that returns a list of secrets from the secret manager
    /// </summary>
    /// <returns>A list of secrets on the secret manager</returns>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var secrets = await settingsManager.ListSecrets();
        
        return Ok(secrets);
    }

    /// <summary>
    /// This is a sample action that creates a new secret in the secret manager
    /// </summary>
    /// <param name="model"></param>
    /// <returns>HTTP status 204 if successful</returns>
    [HttpPost]
    public IActionResult Create([FromForm] string key, [FromForm] string value)
    {
        if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
        {
            return BadRequest("Key and value are required.");
        }
        
        settingsManager.AddSecret(key, value);
        return Created();
    }

    /// <summary>
    /// This is a sample action that fetches a custom setting from the configuration source.
    /// This is our test setting which was added by the BitwardenSecretManagerProvider.cs
    /// </summary>
    /// <returns>An object representing the custom setting</returns>
    [HttpGet]
    public IActionResult FetchCustomSetting()
    {
        var value = myOtherConfiguration.Value;

        return Ok(value);
    }
}