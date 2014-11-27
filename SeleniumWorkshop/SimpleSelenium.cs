using System;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace SeleniumWorkshop
{
    [TestFixture]
    public class SimpleSearchTest
    {
        [Test]
        public void RedGateIsOrganicResultForSqlCompare()
        {
            using(var driver = new FirefoxDriver())
            {
                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));

                driver.Navigate().GoToUrl("http://www.google.co.uk/");
                var searchBox = driver.FindElement(By.Id("gbqfq"));
                searchBox.SendKeys("SQL Compare");
                searchBox.SendKeys(Keys.Return);

                var organicSearchList = driver.FindElement(By.Id("rso"));
                var resultUrlElements = organicSearchList.FindElements(By.TagName("cite"));
                var resultUrls = resultUrlElements.Select(x => x.Text);
                CollectionAssert.Contains(resultUrls, "www.red-gate.com/products/sql-development/sql-compare/");
            }
        }
    }
}
