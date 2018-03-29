using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Scrapper;
using Scrapper.Scrappers;
using System;

namespace Scrapper.Test
{
    [TestClass]
    public class TabelaBrasileiroTest
    {
        private static TabelaBrasileiroScrapper scrapper;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            scrapper = new ScrapperFactory().GetTabelaBrasileiroScrapper() as TabelaBrasileiroScrapper;
            scrapper.Load();
        }

        [TestMethod]
        public void ReturnTitlePage()
        {
            Assert.AreEqual("BRASILEIRÃO SÉRIE A", scrapper.GetPageTitle());

        }

        [TestMethod]
        public void ReturnRightContainerTagName()
        {
            IWebElement element = scrapper.GetContainerTabela();

            Assert.AreEqual("section", element.TagName);
        }

        [TestMethod]
        public void ReturnEquipeName()
        {
            var container = scrapper.GetContainerTabela();

            var equipesWrapper = container
                .FindElement(By.ClassName("tabela-times"))
                .FindElements(By.TagName("tbody"));
            
            Assert.AreEqual("América-MG", scrapper.GetEquipeName(equipesWrapper[0]));
        }

        [TestMethod]
        public void ReturnEquipeStatus()
        {
            var container = scrapper.GetContainerTabela();

            var statusWrapper = container
                .FindElement(By.ClassName("tabela-pontos"))
                .FindElement(By.TagName("tbody"))
                .FindElements(By.ClassName("tabela-body-linha")); 
            
            Equipe equipe = new Equipe
            {
                Pontos = 0,
                Jogos = 0,
                Empates = 0,
                Derrotas = 0,
                GolsPro = 0,
                GolsContra = 0,
                SaldoGols = 0
            };

            Assert.AreEqual(equipe, scrapper.GetStatusEquipe(statusWrapper[0]));
        }
        [TestMethod]
        public void ReturnRightSizeEquipes()
        {
            var equipes = scrapper.GetEquipes();

            Assert.AreEqual(20, equipes.Count);
        }

        [TestMethod]
        public void ReturnRightFirstEquipe()
        {
            var equipes = scrapper.GetEquipes();

            Assert.AreEqual("América-MG", equipes[0].Name);
        }

        [TestMethod]
        public void ReturnRightLastEquipe()
        {
            var equipes = scrapper.GetEquipes();

            Assert.AreEqual("Vitória", equipes[19].Name);
        }
    }
}
