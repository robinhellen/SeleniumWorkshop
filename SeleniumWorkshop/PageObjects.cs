using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.PageObjects;

namespace SeleniumWorkshop
{
    [TestFixture]
    public class PageObjectSearchTest
    {
        public void RedGateIsOrganicResultForSqlCompare(DesiredCapabilities capabilities)
        {
            using (var driver = new FirefoxDriver())
            {
                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));

                var page = SearchPage.Load(driver);
                var resultsPage = page.SearchFor("SQL Compare");

                CollectionAssert.Contains(resultsPage.ResultUrls, "www.red-gate.com/products/sql-development/sql-compare/");
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
