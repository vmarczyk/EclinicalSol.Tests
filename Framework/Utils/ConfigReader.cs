using Microsoft.Extensions.Configuration;

namespace EclinicalSol.Tests.Framework.Utils;

// This is a utility class for reading configuration values from appsettings.json, such as the base URL and browser type.
// It uses Microsoft.Extensions.Configuration to load the configuration file.
public static class ConfigReader
{
    private static readonly IConfiguration Config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build();

    public static string GetBaseUrl()
    {
        var environment = Config["environment"]
            ?? throw new InvalidOperationException("'environment' key is missing in appsettings.json.");

        var baseUrl = Config[$"environments:{environment}:baseUrl"]
            ?? throw new InvalidOperationException($"Base URL for environment '{environment}' is not configured.");

        return baseUrl;
    }

    public static string GetBrowser() => Config["browser"] ?? "chrome";

    public static bool IsHeadless() => bool.TryParse(Config["headless"], out var v) && v;
}
