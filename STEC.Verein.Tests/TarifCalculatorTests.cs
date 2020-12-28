using System;
using Xunit;
using Verein;
using Microsoft.Extensions.Logging;
using Verein.Models;
using System.Threading.Tasks;
using Verein.Data;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace STEC.Verein.Tests
{
    public class TarifCalculatorTests
    {
        private readonly ILogger _logger;
        private readonly VereinDbContext _dbContext;


        public TarifCalculatorTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<TarifCalculatorTests>()
                .Build();

            var connectionString = configuration.GetValue<string>("TarifCalculatorTestsConnectionString");

            TestHelper testHelper = new TestHelper();
            _logger = testHelper.InitializeLogger();
            _dbContext = testHelper.InitializeDbContext(connectionString);
        }


        [Fact]
        public async Task EhrenmitgliedFeeTest()
        {
            var tarifCalculator = new TarifCalculator(_dbContext, _logger);

            var mitglied = new Mitglied()
            {
                Typ = MitgliederTyp.Ehrenmitglied
            };
            TarifResult result = await tarifCalculator.CalculateBeitrag(mitglied);

            Assert.NotNull(result);
            Assert.Equal(0.0m, result.Beitrag);
        }

        [Fact]
        public async Task WelpenFeeTest()
        {
            var tarifCalculator = new TarifCalculator(_dbContext, _logger);

            var mitglied = new Mitglied()
            {
                Typ = MitgliederTyp.WelpenLernSpielstunde
            };
            TarifResult result = await tarifCalculator.CalculateBeitrag(mitglied);

            Assert.NotNull(result);
            Assert.Equal(80.0m, result.Beitrag);
        }

        [Fact]
        public async Task JahresteilnehmerFeeTest()
        {
            var tarifCalculator = new TarifCalculator(_dbContext, _logger);

            var mitglied = new Mitglied()
            {
                Typ = MitgliederTyp.Jahresteilnahme,
                Eintrittsdatum = new DateTime(2020, 3, 5),
                Hunde = new List<Hund>()
                {
                    new Hund()
                    {
                        
                    }
                }
            };
            TarifResult result = await tarifCalculator.CalculateBeitrag(mitglied);

            Assert.NotNull(result);
            // 40€ pro Quartal
            Assert.Equal(160.0m, result.Beitrag);
        }


        [Fact]
        public async Task MitgliedFeeTest()
        {
            var tarifCalculator = new TarifCalculator(_dbContext, _logger);

            var mitglied = new Mitglied()
            {
                Typ = MitgliederTyp.Mitglied,
                Eintrittsdatum = new DateTime(2018, 3, 5),
                Hunde = new List<Hund>()
                {
                    new Hund()
                    {

                    }
                }
            };
            TarifResult result = await tarifCalculator.CalculateBeitrag(mitglied);

            Assert.NotNull(result);
            // Einzelmitgliedschaft + Platzpauschale 1. Hund + Nicht geleistete Arbetisstunden (10h)
            // 30€ + 15€ + 100€
            Assert.Equal(145.0m, result.Beitrag); 
        }


        [Fact]
        public async Task NeuesMitgliedFeeTest()
        {
            var tarifCalculator = new TarifCalculator(_dbContext, _logger);

            var mitglied = new Mitglied()
            {
                Typ = MitgliederTyp.Mitglied,
                Eintrittsdatum = new DateTime(DateTime.Now.Year, 1, 1),
                Hunde = new List<Hund>()
                {
                    new Hund()
                    {

                    }
                }
            };
            TarifResult result = await tarifCalculator.CalculateBeitrag(mitglied);

            Assert.NotNull(result);
            // Einzelmitgliedschaft + Platzpauschale 1. Hund + Beitrittsgebühr
            // 30€ + 15€ + 50€
            Assert.Equal(95.0m, result.Beitrag);
        }




    }
}
