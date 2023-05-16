using System;
using Serilog;

namespace PlanA.Web.Core.Logger;

public static class LoggingHelper
{
    private static readonly Serilog.Core.Logger _logger = new LoggerConfiguration()
        .ReadFrom.Configuration(TestConfig.ConfigurationBuilder)
        .CreateLogger();

    public static void LogInformation(string message)
    {
        _logger.Information(message);
        _logger.Debug(message);
        _logger.Error(message);
    }

    public static void LogDebug(string message)
    {
        _logger.Debug(message);
    }

    public static void LogError(string message, Exception exception = null)
    {
        message = "[ERROR]: " + message;

        if (exception != null) _logger.Error(message, exception);

        _logger.Error(message);
    }
}