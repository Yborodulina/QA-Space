using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PlanA.Web.Core.Core.WebDriver;

namespace PlanA.Web.Core.Extensions.Selenium;

public static class WebElementConditions
{
    public static Func<ISearchContext, IReadOnlyCollection<IWebElement>> ElementsPresent(By locator)
    {
        return e => e.FindElements(locator);
    }
}

public static class WaitExtensions
{
    private const int MediumTimeout = 30;
    private const int LoadTimeout = 300;

    public static List<string> WaitForElementsWithText(this IWebDriver driver, By locator, int? timeoutInSec = null)
    {
        return driver.WaitFor(_ => _.FindElements(locator).Select(x => x.Text).ToList(),
            60, typeof(NullReferenceException), typeof(StaleElementReferenceException));
    }

    public static IWebElement WaitForElementPresent(this IWebDriver driver, By locator, int? timeoutInSec = null)
    {
        return driver.WaitFor(_ => _.FindElement(locator), null, typeof(NoSuchElementException));
    }

    public static bool WaitForTextPresentInElement(this IWebDriver driver, IWebElement element, string expectedText,
        int? timeoutInSec = null)
    {
        return driver.WaitFor(_ => element.Text.Equals(expectedText),
            exceptionTypes: new[]
            {
                typeof(NoSuchElementException), typeof(NullReferenceException), typeof(StaleElementReferenceException)
            });
    }

    public static bool WaitForValueIsNotEmpty(this IWebDriver driver, By locator, int? timeoutInSec = null)
    {
        
        return driver.WaitFor(_ => _.FindElement(locator).GetValue() != string.Empty, exceptionTypes: new[]
            {
                typeof(NoSuchElementException), typeof(NullReferenceException), typeof(StaleElementReferenceException)
            });
    }

    public static TResult WaitFor<TResult>(this IWebDriver driver, Func<ISearchContext, TResult> condition,
        int? timeoutInSec = null, params Type[] exceptionTypes)
    {
        var wait = new DefaultWait<ISearchContext>(driver)
        {
            Timeout = TimeSpan.FromSeconds(timeoutInSec ?? TestConfig.DefaultTimeout)
        };

        if (exceptionTypes != null) wait.IgnoreExceptionTypes(exceptionTypes);

        return wait.Until(condition);
    }

    public static void WaitForElementDisappeared(this IWebDriver driver, By locator, int timeout = MediumTimeout,
        string message = null)
    {
        var isDisplay = true;
        var tries = 0;
        while (isDisplay)
        {
            try
            {
                var wait = Wait(driver, timeout, message);
                wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
                Thread.Sleep(timeout);
                isDisplay = Driver.Instance.FindElement(locator).Displayed;
                tries++;
                if (tries == 1000)
                {
                    break;
                }
            }
            catch (WebDriverTimeoutException)
            {
                isDisplay = false;
            }
            catch (NoSuchElementException)
            {
                isDisplay = false;
            }
            catch (StaleElementReferenceException)
            {
                isDisplay = false;
            }
        }

        if (isDisplay)
        {
            throw new Exception("Element is not disappeared.");
        }
    }

    public static bool WaitForPageToLoad(this IWebDriver driver, int timeoutInSec = MediumTimeout)
    {
        return driver.Wait(timeoutInSec).Until(_ =>
            driver.JsExecutor().ExecuteScript("return document.readyState").Equals("complete"));
    }

    public static void WaitUrlContains(this IWebDriver driver, string url, int timeoutInSec)
    {
        Wait(driver, timeoutInSec).Until(ExpectedConditions.UrlContains(url));
    }

    public static void WaitUntilElementExists(this IWebDriver driver, By locator, int timeoutInSec = 4)
    {
        Wait(driver, timeoutInSec).Until(ExpectedConditions.ElementExists(locator));
    }

    public static void WaitElementToBeClickable(this IWebElement element, int timeoutInSec)
    {
        Wait(element.ToDriver(), timeoutInSec).Until(ExpectedConditions.ElementToBeClickable(element));
    }

    public static void WaitElementToBeSelected(this IWebElement element, int timeoutInSec)
    {
        Wait(element.ToDriver(), timeoutInSec).Until(ExpectedConditions.ElementToBeClickable(element));
    }

    public static void InvisibilityOfElementLocated(this IWebDriver driver, By locator, int timeoutInSec)
    {
        var wait = Wait(driver, timeoutInSec);
        wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
        wait.Until(ExpectedConditions.InvisibilityOfElementLocated(locator));
    }

    public static IWebElement WaitElementIsVisible(this IWebDriver driver, By locator, int timeoutInSec)
    {
        return Wait(driver, timeoutInSec).Until(ExpectedConditions.ElementIsVisible(locator));
    }

    public static void WaitElementToBeClickable(this IWebDriver driver, By locator, int timeoutInSec)
    {
        Wait(driver, timeoutInSec).Until(ExpectedConditions.ElementToBeClickable(locator));
    }

    public static void CriticalWait(this IWebDriver driver, int timeoutInSec = 500)
    {
        Thread.Sleep(timeoutInSec);
    }

    private static WebDriverWait Wait(this IWebDriver driver, int timeout = MediumTimeout, string message = null)
    {
        return new(driver, TimeSpan.FromSeconds(timeout)) { Message = message };
    }
}