

using Application.Extensions;
using Application.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddGoogleSecretManagerConfiguration();
builder.Services.AddControllers();
builder.Services.AddScoped<ISecretManager, SecretManager>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();

app.UseHttpsRedirection();

app.UseRouting();
app.UseEndpoints((endpoints) =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action}/{id?}",
        defaults: new { controller = "Home", action = "Index" });
    endpoints.MapControllers();
});

app.Run();