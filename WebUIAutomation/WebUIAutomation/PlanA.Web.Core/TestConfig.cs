using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace PlanA.Web.Core;

public static class TestConfig
{
    public static IConfiguration ConfigurationBuilder =>
        new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
            .Build();

    public static string UserName => ConfigurationBuilder[nameof(UserName)];

    public static string UserPassword => ConfigurationBuilder[nameof(UserPassword)];

    public static string Browser => ConfigurationBuilder[nameof(Browser)];

    public static int DefaultTimeout => int.Parse(ConfigurationBuilder[nameof(DefaultTimeout)]);

    public static int ElementTimeout => int.Parse(ConfigurationBuilder[nameof(ElementTimeout)]);
    public static bool VideoRecording => bool.Parse(ConfigurationBuilder[nameof(VideoRecording)]);

    public static Uri BaseUrl => new Uri(ConfigurationBuilder[nameof(BaseUrl)]);

    public static Uri ApiUrl => new Uri(ConfigurationBuilder[nameof(ApiUrl)]);
}