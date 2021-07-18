using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SyronexTestProject.Drivers;
using SyronexTestProject.TestHooks;
using System.Collections.ObjectModel;
using System.Linq;

namespace SyronexTestProject.Tests
{
    [TestFixture]
    public class Tests : TestSetup
    {
        private readonly SubscriptionFormUiObjects _form = new SubscriptionFormUiObjects();

        public class ExpectedData
        {
            public (string Label, string Input) FirstName { get; set; }
            public (string Label, string Input) LastName { get; set; }
            public (string Label, string Input) Email { get; set; }
            public (string Label, string Input) Country { get; set; }
            public (string Label, string Input) Subscription { get; set; }
            public (string Label, string Input) FreeTrial { get; set; }

            public ExpectedData((string, string) firstName, (string, string) lastName, (string, string) email, (string, string) country, (string, string) subscription, (string, string) freeTrial)
            {
                FirstName = firstName;
                LastName = lastName;
                Email = email;
                Country = country;
                Subscription = subscription;
                FreeTrial = freeTrial;
            }

            public ExpectedData()
            {

            }
        }

        [Test]
        public void FillInTheFormAndProceedToPayment()
        {
            const string urlDataInput = "https://formsmarts.com/_form/mfe";
            const string urlSubmit = "https://formsmarts.com/_form/mfe/submit";
            const string urlConfirm = "https://formsmarts.com/_form/mfe/confirm";

            ExpectedData expectedData = new ExpectedData()
            {
                FirstName = ("First Name:", "Conan"),
                LastName = ("Last Name:", "Barbarian"),
                Email = ("Email Address:", "conan.barbarian@cimmeria.com"),
                Country = ("Country:", "United States"),
                Subscription = ("Choose a Subscription:", "1-Month Subscription ($9.99 USD/Month)"),
                FreeTrial = ("Claim a FREE 7-day Trial Subscription ($0 USD/7 Days)", "yes"),
            };

            driver.NavigateTo(urlDataInput);

            AssertDataInputFormStep(urlDataInput);
            SetUserSubscriptionSettingsAndSubmit(expectedData);
            AssertDataValidationFormStep(urlSubmit, expectedData);
            AssertPaymentConfirmationFormStep(urlConfirm);
            SubmitAndGetRedirectedToPayPal();
        }

        private void AssertDataInputFormStep(string expectedUrl)
        {
            Assert.AreEqual(expectedUrl, driver.Url, "Wrong page url.");

            ReadOnlyCollection<IWebElement> subscriptionLabels = _form.GetSubscriptionLabels(driver);

            Assert.AreEqual(_form.GetPageTitle(driver).Text, "Subscription Form Demo");

            Assert.True(subscriptionLabels[0].FindElement(By.CssSelector("input")).Selected);
            Assert.AreEqual(subscriptionLabels[0].FindElement(By.CssSelector("span")).Text,
                "1-Month Subscription ($9.99 USD/Month)");
            Assert.False(subscriptionLabels[1].FindElement(By.CssSelector("input")).Selected);
            Assert.AreEqual(subscriptionLabels[1].FindElement(By.CssSelector("span")).Text,
                "6-Month Subscription Save $7.94 ($52 USD/6 Months)");
            Assert.False(subscriptionLabels[2].FindElement(By.CssSelector("input")).Selected);
            Assert.AreEqual(subscriptionLabels[2].FindElement(By.CssSelector("span")).Text,
                "1-Year Subscription Save $20.88 ($99 USD/Year)");

            Assert.False(_form.GetFreeTrialInput(driver).Selected);
            _form.GetFreeTrialInput(driver).Click();
            Assert.AreEqual(_form.GetFreeTrialText(driver).Text,
                "Claim a FREE 7-day Trial Subscription ($0 USD/7 Days)");
        }

        private void SetUserSubscriptionSettingsAndSubmit(ExpectedData expectedData)
        {
            _form.GetFirstNameInput(driver).SendKeys(expectedData.FirstName.Input);
            _form.GetLastNameInput(driver).SendKeys(expectedData.LastName.Input);
            _form.GetEmailAddressInput(driver).SendKeys(expectedData.Email.Input);

            SelectElement selectCountry = new SelectElement(_form.GetCountrySelect(driver));
            selectCountry.SelectByText(expectedData.Country.Input);

            _form.GetSubmitButton(driver).Click();
            driver.Wait();
        }

        private void AssertDataValidationFormStep(string expectedUrl, ExpectedData expectedDataData)
        {
            Assert.AreEqual(driver.Url, expectedUrl);

            Assert.AreEqual(_form.GetPageTitle(driver).Text, "Subscription Form Demo");
            Assert.AreEqual(_form.GetPageInformation(driver).Text,
                "If the information below is correct, press Confirm to complete your _form submission. Otherwise, press Modify.");
            
            ReadOnlyCollection<IWebElement> summary = driver.FindElements(By.CssSelector("tbody tr"));
            Assert.AreEqual(summary[0].FindElement(By.CssSelector("th")).Text,
                expectedDataData.FirstName.Label);
            Assert.AreEqual(summary[0].FindElement(By.CssSelector("td")).Text,
                expectedDataData.FirstName.Input);
            Assert.AreEqual(summary[1].FindElement(By.CssSelector("th")).Text,
                expectedDataData.LastName.Label);
            Assert.AreEqual(summary[1].FindElement(By.CssSelector("td")).Text,
                expectedDataData.LastName.Input);
            Assert.AreEqual(summary[2].FindElement(By.CssSelector("th")).Text,
                expectedDataData.Email.Label);
            Assert.AreEqual(summary[2].FindElement(By.CssSelector("td")).Text,
                expectedDataData.Email.Input);
            Assert.AreEqual(summary[3].FindElement(By.CssSelector("th")).Text,
                expectedDataData.FirstName.Label);
            Assert.AreEqual(summary[3].FindElement(By.CssSelector("td")).Text,
                expectedDataData.FirstName.Input);

            _form.GetSubmitButton(driver).Click();
            driver.Wait();
        }

        private void AssertPaymentConfirmationFormStep(string expectedUrl)
        {
            Assert.AreEqual(driver.Url, expectedUrl);

            Assert.AreEqual(_form.GetPageTitle(driver).Text, "Subscription Form Demo");
            Assert.AreEqual(_form.GetPageInformation(driver).Text,
                "Please now press the button below to checkout. If you've chosen to take advantage of a free trial " + 
                     "subscription, no money will be changed on you credit card or PayPal account before the end of you trial " + 
                     "period. (Note: if you have any questions about this demo or FormSmarts Payment Integration, email formsmarts-sales@syronex.com)");
        }

        private void SubmitAndGetRedirectedToPayPal()
        {
            _form.GetSubmitButton(driver).Click();
            driver.Wait();
            
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            Assert.True(driver.Url.Contains("https://www.paypal.com/webapps/hermes?token="));
            Assert.AreEqual(driver.Title, "Log in to your PayPal account");
        }
    }
}