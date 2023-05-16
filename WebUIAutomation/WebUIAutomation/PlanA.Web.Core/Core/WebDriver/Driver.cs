using System;
using System.Threading;
using OpenQA.Selenium;
using PlanA.Web.Core.Core.WebDriver.Factory;
using PlanA.Web.Core.Extensions;
using PlanA.Web.Core.Extensions.Selenium;
using PlanA.Web.Core.Logger;

namespace PlanA.Web.Core.Core.WebDriver;

public static class Driver
{
    private static readonly ThreadLocal<IWebDriver> ThreadLocalContext = new();

    public static IWebDriver Instance =>
        ThreadLocalContext.Value.CheckNotNull(nameof(Driver),
            "Driver = null. Probably driver was not initialized - use InitWebDriver() method first");

    public static void InitWebDriver(Type drive)
    {
        LoggingHelper.LogInformation($"Initialization of the driver ({drive.Name}) has been started for thread {Thread.CurrentThread.ManagedThreadId}");
        var driverCreator = DriverFactory.GetDriverCreator(drive);
        ThreadLocalContext.Value = driverCreator.GetLocalDriver();
    }

    public static void GoToPage(string urlPart)
    {
        var url = new Uri(TestConfig.BaseUrl, urlPart).AbsoluteUri;

        LoggingHelper.LogInformation($"Open page with url part:{urlPart}");

        Instance.NavigateTo(url);
        Instance.Url = url;
        Instance.WaitForPageToLoad();
    }

    public static void Close()
    {
        try
        {
            LoggingHelper.LogInformation("Try to close driver");
            ThreadLocalContext.Value.Quit();
        }
        catch (Exception ex)
        {
            LoggingHelper.LogError(ex.StackTrace);
        }
        finally
        {
            ThreadLocalContext.Value = null;
            LoggingHelper.LogInformation("Driver should be closed");
        }
    }
}