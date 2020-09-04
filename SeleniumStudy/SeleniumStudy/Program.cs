using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;

namespace SeleniumStudy
{
    class Program
    {
        static void Main(string[] args)
        {
            var driver = new InternetExplorerDriver();

            driver.Navigate().GoToUrl("https://www.google.co.jp/");

            var textBox = driver.FindElement(By.Name("q"));
            textBox.SendKeys("Selenium");

            var button = driver.FindElement(By.Name("btnK"));
            button.Click();
        }
    }
}
