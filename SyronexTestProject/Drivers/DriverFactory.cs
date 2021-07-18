using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using System;

namespace SyronexTestProject.Drivers
{
    public static class DriverFactory
    {
        public static IWebDriver StartDriver(DriverType driverType)
        {
            IWebDriver driver;
            switch (driverType)
            {
                case DriverType.Chrome:
                    ChromeOptions options = new ChromeOptions();
                    options.AddArgument("--window-size=1614,900");
                    options.AddArguments("--no-sandbox");
                    options.AddArguments("--disable-dev-shm-usage");
                    options.AddArgument("--fast-start");
                    options.AddArgument("--lang=en");
                    options.AddArgument("--disable-infobars");
                    options.AddArgument("--disable-notifications");
                    driver = new ChromeDriver(options);
                    break;
                case DriverType.Edge:
                    driver = new EdgeDriver();
                    break;
                case DriverType.Firefox:
                    driver = new FirefoxDriver();
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Driver type {driverType} does not exist.");
            }

            return driver;
        }

        public enum DriverType
        {
            Chrome,
            Firefox,
            Edge
        }
    }
}
