using System;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using PlanA.Web.Core.Core.WebDriver;
using PlanA.Web.Core.Logger;

namespace PlanA.Web.Core.Extensions.Selenium;

public static class WebDriverExtensions
{
    public static IJavaScriptExecutor JsExecutor(this IWebDriver driver)
    {
        return (IJavaScriptExecutor)driver;
    }

    public static object ExecuteScript(this IWebDriver context, string script, params object[] args)
    {
        return context.JsExecutor().ExecuteScript(script, args);
    }

    public static IWebDriver Maximize(this IWebDriver driver)
    {
        driver.CheckNotNull(nameof(driver));
        driver.Manage().Window.Maximize();

        return driver;
    }

    public static void TryMaximize(this IWebDriver driver)
    {
        try
        {
            Maximize(driver);
        }
        catch (Exception)
        {
            //ignored
        }
    }

    public static IWebDriver SetSize(this IWebDriver driver, int width, int height)
    {
        driver.CheckNotNull(nameof(driver));
        driver.Manage().Window.Size = new Size(width, height);

        return driver;
    }

    public static IWebDriver Zoom(this IWebDriver driver, double number)
    {
        driver.ExecuteScript($"document.body.style.zoom = '{number}'");
        return driver;
    }

    public static IWebDriver SetPosition(this IWebDriver driver, int x, int y)
    {
        driver.CheckNotNull(nameof(driver));
        driver.Manage().Window.Position = new Point(x, y);

        return driver;
    }

    public static void NavigateTo(this IWebDriver driver, Uri uri)
    {
        driver.Url = uri.AbsolutePath;
    }

    public static void NavigateTo(this IWebDriver driver, string uri)
    {
        try
        {
            driver.Url = uri;
        }
        catch (WebDriverTimeoutException)
        {
            Driver.Instance.RefreshPage();
        }
    }

    public static bool TitleContains(this IWebDriver driver, string text)
    {
        text.CheckNotNullOrEmpty(nameof(text));

        return driver?.Title.Contains(text) ?? false;
    }

    public static IWebDriver Perform(this IWebDriver driver, Func<Actions, Actions> actions)
    {
        driver.CheckNotNull(nameof(driver));
        actions.CheckNotNull(nameof(actions));
        var act = new Actions(driver);
        act = actions(act);
        act.Perform();

        return driver;
    }

    public static void RefreshPage(this IWebDriver driver)
    {
        driver.Navigate().Refresh();

        Driver.Instance.WaitForPageToLoad();
    }
}