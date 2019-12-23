using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;


namespace CrawllerRom
{
    class Program
    {
        static void Main(string[] args)
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl("https://www.planetemu.net/roms/mame-roms?page=0");

                var alfabeto = driver.FindElement(By.ClassName("menuroms")).FindElements(By.TagName("a"));
                var totalLetras = alfabeto.Count;
                for (int j = 0; j < totalLetras; j++)
                {
                    driver.Navigate().GoToUrl(driver.FindElement(By.ClassName("menuroms")).FindElements(By.TagName("a"))[j].GetAttribute("href"));
                    
                    var roms = driver.FindElements(By.ClassName("tlbr"))[2].FindElement(By.TagName("tbody"))
                        .FindElements(By.TagName("tr"));
                    string ultimoTitulo = "aaaaaaa";
                    var totalPagina = roms.Count;
                    
                    for (int i = 0; i < totalPagina; i++)
                    {
                        var titulo = roms[i].FindElements(By.TagName("td"))[0].Text;
                        var distancia =
                            JaroWinklerDistance.proximity(ultimoTitulo, titulo);
                        
                        Console.WriteLine(ultimoTitulo +" || "+titulo+" || "+distancia);
                        if (distancia < 0.70)
                        {
                            driver.Navigate().GoToUrl(roms[i].FindElements(By.TagName("td"))[0]
                                .FindElement(By.TagName("a")).GetAttribute("href"));
                            
                            driver.FindElement(By.Name("download")).Click();
                            
                            driver.Navigate().Back();
                            
                            roms = driver.FindElements(By.ClassName("tlbr"))[2].FindElement(By.TagName("tbody"))
                                .FindElements(By.TagName("tr"));
                            
                            ultimoTitulo = roms[i].FindElements(By.TagName("td"))[0].Text;
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("Baixado " + ultimoTitulo);
                            Console.ResetColor();

                        }
                    } 
                   
                }

                driver.Quit();
            }

            Console.ReadLine();
        }
    }
}