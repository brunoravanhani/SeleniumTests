using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;


namespace Scrapper.Scrappers
{
    public class TabelaBrasileiroScrapper : IScrapper
    {

        private SeleniumConfigurations _configurations;

        private IWebDriver _driver;

        public TabelaBrasileiroScrapper(SeleniumConfigurations configurations)
        {
            _configurations = configurations;

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");

            _driver = new ChromeDriver(_configurations.CaminhoDriverChrome, options);
        }

        public void Load()
        {
            _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(_configurations.Timeout);

            _driver.Navigate().GoToUrl(_configurations.Url);
        }

        public string GetPageTitle() 
        {

            string title = _driver
                .FindElement(By.ClassName("header-navegacao"))
                .FindElement(By.ClassName("header-title"))
                .FindElement(By.TagName("a")).Text;

            return title;
        }

        public IWebElement GetContainerTabela()
        {
            return _driver
                .FindElement(By.Id("widget-classificacao"))
                .FindElement(By.ClassName("section-container"));
        }

        public IList<Equipe> GetEquipes()
        {

            var container = GetContainerTabela();

            var namesWrapper = container
                .FindElement(By.ClassName("tabela-times"))
                .FindElements(By.TagName("tbody"));

            var names = namesWrapper.Select(el => GetEquipeName(el)).ToArray();

            var statusWrapper = container
                .FindElement(By.ClassName("tabela-pontos"))
                .FindElement(By.TagName("tbody"))
                .FindElements(By.ClassName("tabela-body-linha")); 

            var equipes = statusWrapper.Select(el => GetStatusEquipe(el)).ToList();

            for (int i = 0; i < names.Length; i++) 
            {
                equipes[i].Name = names[i];
            }

            return equipes;
        }

        public string GetEquipeName(IWebElement element) 
        {
            return element.FindElement(By.ClassName("tabela-times-time-nome")).Text;
        }

        public Equipe GetStatusEquipe(IWebElement element) 
        {
            
            var status = element.FindElements(By.TagName("td"));

            Equipe equipe = new Equipe
            {
                Pontos = int.Parse(status[0].Text),
                Jogos = int.Parse(status[1].Text),
                Empates = int.Parse(status[2].Text),
                Derrotas = int.Parse(status[3].Text),
                GolsPro = int.Parse(status[4].Text),
                GolsContra = int.Parse(status[5].Text),
                SaldoGols = int.Parse(status[6].Text)
            };

            return equipe;
        }

    }

    public class Equipe
    {
        public string Name { get; set; }

        public int Position { get; set; }

        public int Pontos { get; set; }

        public int Jogos { get; set; }

        public int Vitorias { get; set; }

        public int Empates { get; set; }

        public int Derrotas { get; set; }

        public int GolsPro { get; set; }

        public int GolsContra { get; set; }

        public int SaldoGols { get; set; }

    }
}
