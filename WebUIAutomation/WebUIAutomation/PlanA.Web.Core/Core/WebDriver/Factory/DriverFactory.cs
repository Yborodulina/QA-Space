using System;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;

namespace PlanA.Web.Core.Core.WebDriver.Factory;

public class DriverFactory
{
    public static IDriverCreator GetDriverCreator(Type driverType)
    {
        if (driverType == typeof(ChromeDriver)) return new ChromeDriverCreator();

        if (driverType == typeof(EdgeDriver)) return new EdgeDriverCreator();

        throw new NotImplementedException($"{driverType.Name} doesn't supported.");
    }
}