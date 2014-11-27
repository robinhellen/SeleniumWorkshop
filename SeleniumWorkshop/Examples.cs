using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.PageObjects;

namespace SeleniumWorkshop
{
    [TestFixture]
    public class Examples
    {
        [Test]
        public void Test1()
        {
            IWebDriver driver = new FirefoxDriver();
            driver.Navigate().GoToUrl("http://rg-web01/website_data/sales_graph");
            IWebElement totalElement = driver.FindElement(By.Id("total"));
            StringAssert.StartsWith("$", totalElement.Text);
        }

        [Test]
        public void Test2()
        {
            IWebDriver driver = new FirefoxDriver();
            SalesGraphPage page = SalesGraphPage.Load(driver);
            string total = page.TotalSales;
            StringAssert.StartsWith("$", total);
        }


        [Test]
        public void Test3()
        {
            using(IWebDriver driver = new RemoteWebDriver
                (
                new Uri("http://selenium-hub1.testnet.red-gate.com:4444/wd/hub"),
                DesiredCapabilities.Firefox()
                ))
            {
                SalesGraphPage page = SalesGraphPage.Load(driver);
                string total = page.TotalSales;
                StringAssert.StartsWith("$", total);
            }
        }

#pragma warning disable 649 // Selenium assigns this by reflection

        public class SalesGraphPage
        {
            public static SalesGraphPage Load(IWebDriver driver)
            {
                driver.Navigate().GoToUrl("http://rg-web01/website_data/sales_graph");
                return new SalesGraphPage(driver);
            }

            private SalesGraphPage(IWebDriver driver)
            {
                PageFactory.InitElements(driver, this);
            }
            [FindsBy(How = How.Id, Using = "total")]
            private IWebElement totalSales;

            public string TotalSales
            {
                get { return totalSales.Text; }
            }
        }

#pragma warning restore 649
    }
}
