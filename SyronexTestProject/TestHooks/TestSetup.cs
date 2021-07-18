using System;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using SyronexTestProject.Drivers;

namespace SyronexTestProject.TestHooks
{
    [TestFixture]
    public abstract class TestSetup
    {
        private readonly string _currentTestAssembly = TestContext.CurrentContext.TestDirectory;
        protected IWebDriver driver;

        [SetUp]
        public void BeforeScenario()
        {
            driver = DriverFactory.StartDriver(DriverFactory.DriverType.Chrome);
        }

        [TearDown]
        public void AfterScenario()
        {
            ResultState testResult = TestContext.CurrentContext.Result.Outcome;
            Console.WriteLine("Test: {0} finished at: {1} with score: {2}", TestContext.CurrentContext.Test.Name, DateTime.Now, testResult);
            if (Equals(testResult, ResultState.Failure) || Equals(testResult, ResultState.Error))
            {
                LogError();
            }

            try
            {
                driver.Close();
                driver.Dispose();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Unexpected exception happened during browser closing: {0}", exception.ToString());
            }
        }

        private void CreateOrUseDirectory(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    return;
                }

                DirectoryInfo directory = Directory.CreateDirectory(path);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Unexpected exception happened during folder creation: {0}", exception.ToString());
                throw exception;
            }
        }

        private void LogError()
        {
            string path = Path.Combine(_currentTestAssembly + "\\error\\");
            CreateOrUseDirectory(path);
            string file = $"{path}{TestContext.CurrentContext.Test.Name}_{DateTime.Now:yyyy-MM-dd_HHmmss}.txt";
            using (TextWriter textWriter = new StreamWriter(file))
            {
                textWriter.WriteLine("driver.Title: {0}", driver.Title);
                textWriter.WriteLine("driver.Url: {0}", driver.Url);
                textWriter.WriteLine("driver.CurrentWindowHandle: {0}", driver.CurrentWindowHandle);
                textWriter.WriteLine("driver.WindowHandles: {0}", driver.WindowHandles);
                textWriter.WriteLine("driver.PageSource: {0}", driver.PageSource);
                textWriter.Flush();
                textWriter.Close();
            }

            string screen = $"{path}{TestContext.CurrentContext.Test.Name}_{DateTime.Now:yyyy-MM-dd_HHmmss}.png";
            driver.TakeScreenshot().SaveAsFile(screen, ScreenshotImageFormat.Png);
        }
    }
}
