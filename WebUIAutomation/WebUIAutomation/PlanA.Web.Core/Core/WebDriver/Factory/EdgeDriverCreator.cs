using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace PlanA.Web.Core.Core.WebDriver.Factory;

public class EdgeDriverCreator : IDriverCreator

{
    public IWebDriver GetLocalDriver()
    {
        return new EdgeDriver(Environment.CurrentDirectory, GetOptions());
    }

    public IWebDriver GetRemoteDriver(string remoteUri)
    {
        throw new NotImplementedException();
    }

    private EdgeOptions GetOptions()
    {
        var options = new EdgeOptions { UseChromium = true, UseInPrivateBrowsing = true, StartPage = "" };

        options.AddUserProfilePreference("intl.accept_languages", "en");

        return options;
    }
}