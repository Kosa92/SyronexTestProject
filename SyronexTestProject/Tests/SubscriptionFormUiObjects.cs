using OpenQA.Selenium;
using System.Collections.ObjectModel;

namespace SyronexTestProject.Tests
{
    public class SubscriptionFormUiObjects
    {
        public IWebElement GetFirstNameInput(IWebDriver driver)
        {
            return driver.FindElement(By.CssSelector("input[placeholder='Please enter your first name.']"));
        }

        public IWebElement GetLastNameInput(IWebDriver driver)
        {
            return driver.FindElement(By.CssSelector("input[placeholder='Please enter your last name.']"));
        }

        public IWebElement GetEmailAddressInput(IWebDriver driver)
        {
            return driver.FindElement(By.CssSelector("input[placeholder='Please enter a valid email address.']"));
        }

        public IWebElement GetCountrySelect(IWebDriver driver)
        {
            return driver.FindElement(By.ClassName("country-w"));
        }

        public ReadOnlyCollection<IWebElement> GetSubscriptionLabels(IWebDriver driver)
        {
            return driver.FindElements(By.CssSelector("fieldset > label"));
        }

        public IWebElement GetFreeTrialInput(IWebDriver driver)
        {
            return driver.FindElement(By.CssSelector(".checkbox-w > input"));
        }
        public IWebElement GetFreeTrialText(IWebDriver driver)
        {
            return driver.FindElement(By.CssSelector(".checkbox-w > span"));
        }

        public IWebElement GetSubmitButton(IWebDriver driver)
        {
            return driver.FindElement(By.CssSelector(".button-primary[type='submit']"));
        }

        public IWebElement GetPageTitle(IWebDriver driver)
        {
            return driver.FindElement(By.CssSelector("h1"));
        }

        public IWebElement GetPageInformation(IWebDriver driver)
        {
            return driver.FindElement(By.CssSelector("p"));
        }
    }
}
