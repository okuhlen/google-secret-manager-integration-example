using Application.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SettingsLoader.Services;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

Console.WriteLine("Settings Loader v1.0");
Console.WriteLine("====================================");
Console.WriteLine();
Console.WriteLine("Now loading settings into secrets manager from the settings file. Please wait...");
Console.WriteLine();

var serviceCollection = new ServiceCollection();
serviceCollection.AddSingleton<IConfiguration>(configuration);
// To access application settings in a strongly-typed manner.
serviceCollection.Configure<GoogleServiceAccountSetting>(configuration.GetSection(nameof(GoogleServiceAccountSetting)));
serviceCollection.AddScoped<IGoogleSecretManager, GoogleSecretManager>();
serviceCollection.AddScoped<ISettingsManager, SettingsManager>();
var provider = serviceCollection.BuildServiceProvider();

var settingsManager = provider.GetRequiredService<ISettingsManager>();
Console.WriteLine("Now clearing settings...");
await settingsManager.ClearSecrets();
Console.WriteLine("Now loading settings...");
await settingsManager.LoadSecrets();
Console.WriteLine();
Console.WriteLine("Settings have been successfully loaded into Google Secret Manager. You may consume secrets in your app");
Console.ReadKey();