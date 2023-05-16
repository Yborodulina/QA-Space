using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace PlanA.Web.Core.Core.WebDriver.Factory;

public class ChromeDriverCreator : IDriverCreator
{
    public IWebDriver GetLocalDriver()
    {
        var driver = new ChromeDriver(Environment.CurrentDirectory, GetOptions());

        return driver;
    }

    public IWebDriver GetRemoteDriver(string hubUri)
    {
        var driver = new RemoteWebDriver(new Uri(hubUri), GetOptions().ToCapabilities(), TimeSpan.FromSeconds(30));

        return driver;
    }

    private ChromeOptions GetOptions()
    {
        var options = new ChromeOptions();

        options.AddUserProfilePreference("intl.accept_languages", "en");
        options.AddUserProfilePreference("disable-popup-blocking", "true");
        options.AddArguments("--allow-no-sandbox-job", "--ignore-certificate-errors");
        options.AddArguments("use-fake-ui-for-media-stream");
        options.AddExcludedArgument("enable-automation");
        options.AddAdditionalChromeOption("useAutomationExtension", false);

        return options;
    }
}