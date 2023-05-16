using OpenQA.Selenium;

namespace PlanA.Web.Core.Core.WebDriver.Factory;

public interface IDriverCreator
{
    IWebDriver GetLocalDriver();
    IWebDriver GetRemoteDriver(string remoteUri);
}