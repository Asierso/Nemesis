using Nemesis.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemesis.Scrapper
{
    public class ScrapperEngine
    {
        private WebDriver driver;
        private IWebElement? elementBuffer;
        public string Logs = "";
        public ScrapperEngine()
        {
            var service = ChromeDriverService.CreateDefaultService();
            service.SuppressInitialDiagnosticInformation = true;
            service.EnableVerboseLogging = false;
            service.HideCommandPromptWindow = true;
            var options = new ChromeOptions();
            options.AddArgument("--disable-crash-reporter");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--disable-in-process-stack-traces");
            options.AddArgument("--disable-logging");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--log-level=3");
            options.AddArgument("--output=/dev/null");
            driver = new ChromeDriver(service,options);
            
        }
        public void LoadScrappingInstructions(string fname,User user)
        {
            var buffer = File.ReadAllText(fname).Split('\n');
            buffer.ToList().ForEach(line => Performer(line,user));
        }
        private void Performer(string line,User user)
        {
            if (line.Length == 0)
                return;
            var lineSegments = line.Split(' ');
            if (user is not null && lineSegments.Length > 1 && lineSegments[1].StartsWith("$"))
            {
                var prop = lineSegments[1].Trim('$');
                switch (prop)
                {
                    case "name": lineSegments[1] = user.Name;  break;
                    case "surname": lineSegments[1] = user.Surname; break;
                    case "bday": lineSegments[1] = user.Birth.Split("/")[0]; break;
                    case "bmonth": lineSegments[1] = user.Birth.Split("/")[1]; break;
                    case "byear": lineSegments[1] = user.Birth.Split("/")[2]; break;
                    case "email": lineSegments[1] = user.Email; break;
                    case "password": lineSegments[1] = user.Password; break;
                    case "alias": lineSegments[1] = user.Alias; break;
                }
            }
            switch(lineSegments[0])
            {
                case "goto":
                    driver.Navigate().GoToUrl(lineSegments[1]);break;
                case "find-css":
                    elementBuffer = driver.FindElement(By.CssSelector(lineSegments[1]));break;
                case "find-id":
                    elementBuffer = driver.FindElement(By.Id(lineSegments[1])); break;
                case "find-class":
                    elementBuffer = driver.FindElement(By.ClassName(lineSegments[1])); break;
                case "submit":
                    elementBuffer?.Submit(); break;
                case "click":
                    elementBuffer?.Click(); break;
                case "goto-href":
                    driver.Navigate().GoToUrl(elementBuffer?.GetAttribute("href")); break;
                case "write":
                    elementBuffer?.SendKeys(lineSegments[1]); break;
                case "select-ind":
                    new SelectElement(elementBuffer).SelectByIndex(int.Parse(lineSegments[1]));break;
                case "select-text":
                    new SelectElement(elementBuffer).SelectByText(lineSegments[1]); break;
                case "close":
                    driver.Close();break;
                case "flush":
                    elementBuffer = null;break;
                case "wait":
                    Thread.Sleep(int.Parse(lineSegments[1])); break;
                case "wss":
                    LoadScrappingInstructions(lineSegments[1],user);
                    break;
                case "log":
                    Logs += lineSegments[1];
                    break;
            }
        }
    }
}
