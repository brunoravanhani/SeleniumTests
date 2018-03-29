using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scrapper.Scrappers;

namespace Scrapper.Test
{
    public class ScrapperFactory
    {
        
        private SeleniumConfigurations _configurations;

        public ScrapperFactory() {

            // _configurations = new SeleniumConfigurations
            // {
            //     CaminhoDriverChrome = "C:\\Selenium\\",
            //     Timeout = 60,
            //     Url = "https://globoesporte.globo.com/futebol/brasileirao-serie-a/"

            // };

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _configurations = new SeleniumConfigurations();

            new ConfigureFromConfigurationOptions<SeleniumConfigurations>(
                configuration.GetSection("SeleniumConfigurations")
            ).Configure(_configurations);
        }

        public IScrapper GetTabelaBrasileiroScrapper()
        {
            return new TabelaBrasileiroScrapper(_configurations);
        }
    }
}