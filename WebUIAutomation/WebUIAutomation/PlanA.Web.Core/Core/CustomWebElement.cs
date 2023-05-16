using System.Collections.Generic;
using OpenQA.Selenium;
using PlanA.Web.Core.Core.WebDriver;
using PlanA.Web.Core.Extensions.Selenium;
using PlanA.Web.Core.Logger;

namespace PlanA.Web.Core.Core;

public class CustomWebElement
{
    public CustomWebElement(By locator)
    {
        Locator = locator;
    }

    private By Locator { get; }

    public IReadOnlyCollection<IWebElement> Elements
    {
        get
        {
            try
            {
                return GetElements();
            }
            catch (StaleElementReferenceException)
            {
                LoggingHelper.LogDebug("Re-initialize web elements since DOM has been refreshed");

                return GetElements();
            }
        }
    }

    public IWebElement Element
    {
        get
        {
            try
            {
                return GetElement();
            }
            catch (StaleElementReferenceException)
            {
                LoggingHelper.LogDebug("Re-initialize web element since DOM has been refreshed");

                return GetElement();
            }
        }
    }

    private IWebElement GetElement()
    {
        return Driver.Instance.WaitForElementPresent(Locator, TestConfig.DefaultTimeout);
    }

    private IReadOnlyCollection<IWebElement> GetElements()
    {
        return Driver.Instance.WaitFor(WebElementConditions.ElementsPresent(Locator));
    }
}