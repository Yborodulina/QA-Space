using System;
using System.IO;
using System.Text;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Tracing;

namespace PlanA.Web.Core.Helper;

public static class ScreenshotMaker
{
    public static string TakeScreenshot(IWebDriver driver, IFeatureContext featureContext, ScenarioInfo scenarioInfo)
    {
        var filePath = new string(string.Empty);
        try
        {
            var fileNameBase = $"{scenarioInfo.Title.ToIdentifier()}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}";

            var takesScreenshot = driver as ITakesScreenshot;
            if (takesScreenshot != null)
            {
                var screenshot = takesScreenshot.GetScreenshot();
                var screenshotFilePath = Path.Combine(FileHelper.CreateFolder("Screenshots", featureContext.FeatureInfo.Title.ToIdentifier()), fileNameBase + ".png");
                screenshot.SaveAsFile(screenshotFilePath, ScreenshotImageFormat.Png);
                Console.WriteLine("Screenshot: {0}", new Uri(screenshotFilePath));
                filePath = Path.Combine("Screenshots", featureContext.FeatureInfo.Title.ToIdentifier(), $"{fileNameBase}.png");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error while taking screenshot: {0}", ex);
        }

        return filePath;
    }

    public static void TakePageSource(IWebDriver driver, IFeatureContext featureContext, ScenarioInfo scenarioInfo)
    {
        try
        {
            var fileNameBase = $"{scenarioInfo.Title.ToIdentifier()}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}";

            var pageSource = driver.PageSource;
            var sourceFilePath = Path.Combine(FileHelper.CreateFolder("PageSource", featureContext.FeatureInfo.Title.ToIdentifier()), fileNameBase + ".html");
            File.WriteAllText(sourceFilePath, pageSource, Encoding.UTF8);
            Console.WriteLine("Page source: {0}", new Uri(sourceFilePath));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error while saving page source: {0}", ex);
        }
    }
}