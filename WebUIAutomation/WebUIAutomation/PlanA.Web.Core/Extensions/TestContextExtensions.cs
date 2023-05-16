using System;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using PlanA.Web.Core.Logger;

namespace PlanA.Web.Core.Extensions;

public static class TestContextExtensions
{
    public static bool IsTestFailed(this TestContext context)
    {
        return context.Result.Outcome.Status.Equals(TestStatus.Failed);
    }

    public static string GetTestName(this TestContext testContext)
    {
        var testName = testContext?.Test?.Name ?? string.Empty;

        int bracketStartPosition;

        if ((bracketStartPosition = testName.IndexOf("(", StringComparison.Ordinal)) > 0) testName = testName.Substring(0, bracketStartPosition);

        return testName;
    }

    public static string SaveScreenAt(this Screenshot screenshot, string testresultFolderPath)
    {
        Directory.CreateDirectory(testresultFolderPath);
        var path = Path.Combine(testresultFolderPath, $"Failure_{DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm")}.jpg");

        try
        {
            screenshot.SaveAsFile(path, ScreenshotImageFormat.Jpeg);
        }
        catch (Exception ex)
        {
            LoggingHelper.LogError("Failed to save screen");
            LoggingHelper.LogError(ex.ToString());
        }

        return path;
    }
}