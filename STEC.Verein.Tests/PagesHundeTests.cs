using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Verein.Pages.Hunde;
using Verein.Data;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Verein.Models;
using System.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Configuration;

namespace STEC.Verein.Tests
{
    public class PagesHundeTests
    {
        private readonly ILogger _logger;
        private readonly VereinDbContext _dbContext;


        public PagesHundeTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<PagesHundeTests>()
                .Build();

            var connectionString = configuration.GetValue<string>("PagesHundeTestsConnectionString");

            TestHelper testHelper = new TestHelper();
            _logger = testHelper.InitializeLogger();
            _dbContext = testHelper.InitializeDbContext(connectionString);

            DbInitializer initializer = new DbInitializer();
            initializer.Initialize(_dbContext);
        }

        [Fact]
        public async Task HundeIndexTest()
        {
            TestHelper testHelper = new TestHelper();
            var pageContext = testHelper.CreatePageContext();
            IDatabaseMediator databaseMediator = new DatabaseMediator(_dbContext, _logger);

            var indexModel = new IndexModel(databaseMediator)
            {
                PageContext = pageContext
            };

            await indexModel.OnGetAsync();

            Assert.Equal(1, indexModel.Hunde.Count);
        }

        [Fact]
        public async Task HundeCreateTest()
        {
            TestHelper testHelper = new TestHelper();
            var pageContext = testHelper.CreatePageContext();
            IDatabaseMediator databaseMediator = new DatabaseMediator(_dbContext, _logger);

            var indexModel = new IndexModel(databaseMediator)
            {
                PageContext = pageContext
            };

            await indexModel.OnGetAsync();
            Assert.Equal(1, indexModel.Hunde.Count);

            var createModel = new CreateModel(databaseMediator)
            {
                PageContext = pageContext
            };

            await createModel.OnGetAsync();

            createModel.Hund = new Hund()
            {
                Name = "Marley B.",
                Zwingername = "of extraordinary Snowdevils",
                Geburtsdatum = new DateTime(2011, 11, 5),
                Rasse = "Siberian Husky",
                Geimpft = true,
                Versichert = true
            };

            await createModel.OnPostAsync();

            await indexModel.OnGetAsync();
            Assert.Equal(2, indexModel.Hunde.Count);
        }

        [Fact]
        public async Task HundDeleteTest()
        {
            TestHelper testHelper = new TestHelper();
            var pageContext = testHelper.CreatePageContext();
            IDatabaseMediator databaseMediator = new DatabaseMediator(_dbContext, _logger);

            var indexModel = new IndexModel(databaseMediator)
            {
                PageContext = pageContext
            };

            await indexModel.OnGetAsync();
            Assert.Equal(1, indexModel.Hunde.Count);

            var hund = indexModel.Hunde.First();
            Assert.NotNull(hund);

            var deleteModel = new DeleteModel(databaseMediator)
            {
                PageContext = pageContext
            };

            await deleteModel.OnGetAsync(hund.Id);
            await deleteModel.OnPostAsync(hund.Id);

            await indexModel.OnGetAsync();
            Assert.Empty(indexModel.Hunde);
        }

        [Fact]
        public async Task HundEditTest()
        {
            TestHelper testHelper = new TestHelper();
            var pageContext = testHelper.CreatePageContext();
            IDatabaseMediator databaseMediator = new DatabaseMediator(_dbContext, _logger);

            var indexModel = new IndexModel(databaseMediator)
            {
                PageContext = pageContext
            };

            await indexModel.OnGetAsync();
            Assert.Equal(1, indexModel.Hunde.Count);

            var hund = indexModel.Hunde.First();
            Assert.NotNull(hund);


            var editModel = new EditModel(databaseMediator)
            {
                PageContext = pageContext
            };

            await editModel.OnGetAsync(hund.Id);

            Assert.NotNull(editModel.Hund);

            var name = editModel.Hund.Name;
            editModel.Hund.Name = "Rapunzel";

            await editModel.OnPostAsync();

            await indexModel.OnGetAsync();
            Assert.NotEmpty(indexModel.Hunde);
            Assert.Equal(1, indexModel.Hunde.Count);

            hund = indexModel.Hunde.First();
            Assert.NotEqual(name, hund.Name);
            Assert.Equal("Rapunzel", hund.Name);
        }

        [Fact]
        public async Task HundDetailsTest()
        {
            TestHelper testHelper = new TestHelper();
            var pageContext = testHelper.CreatePageContext();
            IDatabaseMediator databaseMediator = new DatabaseMediator(_dbContext, _logger);
            ILogger<DetailsModel> logger = new NullLoggerFactory().CreateLogger<DetailsModel>();

            var indexModel = new IndexModel(databaseMediator)
            {
                PageContext = pageContext
            };

            await indexModel.OnGetAsync();
            Assert.Equal(1, indexModel.Hunde.Count);

            var hund = indexModel.Hunde.First();
            Assert.NotNull(hund);

            var detailsModel = new DetailsModel(databaseMediator, logger)
            {
                PageContext = pageContext
            };

            await detailsModel.OnGetAsync(hund.Id);

            Assert.NotNull(detailsModel.Hund);

            // FIXME Test Mitglied verknüpfen/aufheben

            await indexModel.OnGetAsync();
            Assert.NotEmpty(indexModel.Hunde);
        }
    }
}
