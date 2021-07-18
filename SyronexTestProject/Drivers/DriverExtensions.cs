using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace SyronexTestProject.Drivers
{
    public static class DriverExtensions
    {
        private const int TimeOut = 45;

        public static void NavigateTo(this IWebDriver driver, string url)
        {
            driver.Navigate().GoToUrl(url);
            driver.Wait();
        }

        public static void Wait(this IWebDriver driver, int timeOut = TimeOut)
        {
            driver.WaitForAjaxRequestsToFinish(timeOut);
            driver.WaitForPageLoadDOM(timeOut);
        }

        public static void WaitForAjaxRequestsToFinish(this IWebDriver driver, int timeOut = 5)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut));
            wait.Until(x =>
                ((IJavaScriptExecutor)x).ExecuteScript("return window.jQuery != undefined && jQuery.active == 0")
                .Equals(true));
        }

        public static void WaitForPageLoadDOM(this IWebDriver driver, int timeOut = 30)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut));
            wait.Until(x => ((IJavaScriptExecutor)x).ExecuteScript("return document.readyState").Equals("complete"));
        }

        public static void WaitForElementDisplayed(this IWebDriver driver, By by, int timeOut = TimeOut)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut));
            try
            {
                wait.Until(x => driver.FindElement(by).Displayed);
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Fail("Element ({0}) was not displayed after {1} seconds.", by.ToString(), timeOut);
            }
        }

        public static void WaitForElementDisplayed(this IWebDriver driver, IWebElement element, int timeOut = TimeOut)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut));
            try
            {
                wait.Until(x => element.Displayed);
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Fail("Element ({0}) was not displayed after {1} seconds.", element.ToString(), timeOut);
            }
        }
    }
}
