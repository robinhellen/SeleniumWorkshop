using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.PageObjects;

namespace SeleniumWorkshop
{
    [TestFixture]
    public class RemoteDriverSearchTest
    {
        [TestCaseSource("Drivers")]
        public void RedGateIsOrganicResultForSqlCompare(DesiredCapabilities capabilities)
        {
            using (var driver = new RemoteWebDriver(new Uri("http://selenium-hub1.testnet.red-gate.com:4444/wd/hub"), capabilities))
            {
                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));

                var page = SearchPage.Load(driver);
                var resultsPage = page.SearchFor("SQL Compare");

                CollectionAssert.Contains(resultsPage.ResultUrls, "www.red-gate.com/products/sql-development/sql-compare/");
            }
        }

        public IEnumerable<TestCaseData> Drivers
        {
            get
            {
                yield return new TestCaseData(DesiredCapabilities.Firefox()).SetName("Firefox");
                yield return new TestCaseData(DesiredCapabilities.Chrome()).SetName("Chrome");
                var ie8Caps = DesiredCapabilities.InternetExplorer();
                ie8Caps.SetCapability("version", "8");
                yield return new TestCaseData(ie8Caps).SetName("IE8");
                var ie9Caps = DesiredCapabilities.InternetExplorer();
                ie8Caps.SetCapability("version", "9");
                yield return new TestCaseData(ie9Caps).SetName("IE9");
                var ie10Caps = DesiredCapabilities.InternetExplorer();
                ie8Caps.SetCapability("version", "10");
                yield return new TestCaseData(ie10Caps).SetName("IE10");
                var ie11Caps = DesiredCapabilities.InternetExplorer();
                ie8Caps.SetCapability("version", "11");
                yield return new TestCaseData(ie11Caps).SetName("IE11");
            }
        }

        public class SearchPage
        {
            [FindsBy(How = How.Id, Using = "gbqfq")]
            private IWebElement searchBox;
            private readonly IWebDriver driver;

            public static SearchPage Load(IWebDriver driver)
            {
                driver.Navigate().GoToUrl("http://www.google.co.uk/");
                return new SearchPage(driver);
            }

            private SearchPage(IWebDriver driver)
            {
                PageFactory.InitElements(driver, this);
                this.driver = driver;
            }

            public SearchResultsPage SearchFor(string searchTerm)
            {
                searchBox.SendKeys("SQL Compare");
                searchBox.SendKeys(Keys.Return);

                return new SearchResultsPage(driver);
            }
        }

        public class SearchResultsPage
        {
            [FindsBy(How = How.Id, Using = "rso")]
            private IWebElement organicSearchList;

            public SearchResultsPage(IWebDriver driver)
            {
                PageFactory.InitElements(driver, this);
            }

            public IEnumerable<string> ResultUrls
            {
                get
                {
                    var resultUrlElements = organicSearchList.FindElements(By.TagName("cite"));
                    return resultUrlElements.Select(x => x.Text);
                }
            }
        }
    }
}
