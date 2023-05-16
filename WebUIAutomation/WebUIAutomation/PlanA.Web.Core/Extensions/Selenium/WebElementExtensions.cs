using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using PlanA.Web.Core.Core.WebDriver;
using PlanA.Web.Core.Logger;

namespace PlanA.Web.Core.Extensions.Selenium;

public static class WebElementExtensions
{
    public static IWebDriver ToDriver(this ISearchContext context)
    {
        if (context is IWrapsDriver iWrappedElement) return iWrappedElement.WrappedDriver;

        var fieldInfo = context.GetType().GetField("underlyingElement", BindingFlags.NonPublic | BindingFlags.Instance);

        if (fieldInfo == null) return null;

        iWrappedElement = fieldInfo.GetValue(context) as IWrapsDriver;

        if (iWrappedElement == null) throw new ArgumentException("Element must wrap a web driver", nameof(context));

        return iWrappedElement.WrappedDriver;
    }

    public static IWebElement ScrollToElement(this IWebElement element)
    {
        var actions = new Actions(Driver.Instance);
        actions.MoveToElement(element);
        actions.Perform();

        return element;
    }

    public static string GetValue(this IWebElement element)
    {
        return element.GetAttribute("value");
    }

    public static IWebElement TypeInto(this IWebElement element, string text)
    {
        element.ClearBackspace();

        if (!string.IsNullOrEmpty(text)) element.SendKeys(text);

        return element;
    }

    public static IWebElement TypeDate(this IWebElement element, string text)
    {
        element.ClearBackspace();

        if (!string.IsNullOrEmpty(text))
            foreach (var c in text)
            {
                if (c == 'P')
                {
                    element.SendKeys(Keys.Backspace);
                    element.SendKeys(Keys.Backspace);
                }

                element.SendKeys(c.ToString());
            }

        return element;
    }

    public static IWebElement TypeIntoAndSubmit(this IWebElement element, string text) //TODO Replace with SendKeyAndSubmit
    {
        TypeInto(element, text).SendKeys(Keys.Enter);

        return element;
    }

    public static bool HasAttribute(this IWebElement e, string attributeName)
    {
        return !e.GetAttribute(attributeName).Equals(string.Empty);
    }

    public static string ToStringElement(this IWebElement e)
    {
        return string.Format(
            CultureInfo.CurrentCulture,
            "{0}{{{1}{2}{3}{4}{5}{6}{7}{8}}}",
            e.TagName,
            AppendAttribute(e, "id"),
            AppendAttribute(e, "name"),
            AppendAttribute(e, "value"),
            AppendAttribute(e, "class"),
            AppendAttribute(e, "type"),
            AppendAttribute(e, "role"),
            AppendAttribute(e, "text"),
            AppendAttribute(e, "href"));
    }

    private static string AppendAttribute(this IWebElement e, string attribute)
    {
        var attrValue = attribute == "text" ? e.Text : e.GetAttribute(attribute);

        return string.IsNullOrEmpty(attrValue)
            ? string.Empty
            : string.Format(CultureInfo.CurrentCulture, $" {attribute}='{attrValue}' ");
    }

    public static string TextClear(this IWebElement element)
    {
        return element.Text.Trim('\r', '\n');
    }

    public static void TryClick(this IWebElement element)
    {
        try
        {
            element.Click();
        }
        catch (Exception e)
        {
            LoggingHelper.LogDebug($"WARN: Unable to click on element - {e}");
            LoggingHelper.LogDebug(element.ToStringElement());
            LoggingHelper.LogDebug("WARN: Going to perform JsClick");
            element.JsClick();
        }
    }

    public static void ClickOn(this IWebElement elem)
    {
        var executor = (IJavaScriptExecutor)Driver.Instance;
        executor.ExecuteScript("arguments[0].click();", elem);
    }

    public static void ClearBackspace(this IWebElement elem)
    {
        var length = elem.GetValue().Length;

        while (length != 0)
        {
            elem.SendKeys(Keys.Backspace);
            length--;
        }
    }

    public static void ClickElement(this IWebElement elem)
    {
        try
        {
            LoggingHelper.LogDebug("Try to click element via general click");
            elem.Click();
        }
        catch (ElementNotInteractableException)
        {
            LoggingHelper.LogDebug("GeneralClickFailedd. Try to click element via JS");
            elem.ClickOn();
        }
    }

    public static List<IWebElement> GetVisibleElements(this IEnumerable<IWebElement> collection)
    {
        return collection.Where(el => el.Displayed).ToList();
    }

    public static List<string> GetElementsText(this IEnumerable<IWebElement> collection)
    {
        return collection.Select(el => el.TextClear()).ToList();
    }

    public static void HighlightPerformRelease(this IWebElement element, Action action)
    {
        element.JsHighlight();
        action.Invoke();
        element.JsDehighlight();
    }

    public static IWebElement GetParent(this IWebElement element)
    {
        return element.FindElement(By.XPath("./parent::*"));
    }

    public static IWebElement GetChild(this IWebElement element)
    {
        return element.FindElement(By.XPath("./child::*"));
    }

    public static void DoubleClick(this IWebElement element)
    {
        var actionsBuilder = new Actions(element.ToDriver());
        var action = actionsBuilder.DoubleClick(element).Build();
        action.Perform();
    }

    public static void RightClick(this IWebElement element)
    {
        var actionsBuilder = new Actions(element.ToDriver());
        var action = actionsBuilder.ContextClick(element).Build();
        action.Perform();
    }

    public static void ClickAndHold(this IWebElement element)
    {
        var actionsBuilder = new Actions(element.ToDriver());
        var action = actionsBuilder.ClickAndHold(element).Build();
        action.Perform();
    }

    public static void DragAndDrop(this IWebElement element, IWebElement targetElement)
    {
        var actionsBuilder = new Actions(element.ToDriver());
        var action = actionsBuilder.DragAndDrop(element, targetElement).Build();
        action.Perform();
    }

    public static By GetBy(this IWebElement element)
    {
        return By.XPath(GetXPathAsString(element));
    }

    public static string GetXPathAsString(this IWebElement element)
    {
        var attributesDict = element.JsGetAttributes();
        var sb = new StringBuilder();
        sb.Append("//*[");

        foreach (var el in element.JsGetAttributes())
        {
            sb.Append($"@{el.Key}='{el.Value}'");

            if (!el.Equals(attributesDict.Last())) sb.Append(" and ");
        }

        sb.Append("]");

        return sb.ToString();
    }

    public static void SendKeyAndSubmit(this IWebElement element, string key)
    {
        TypeInto(element, key).SendKeys(Keys.Enter);
    }
}