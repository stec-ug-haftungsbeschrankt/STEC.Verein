using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Verein.Data;

namespace STEC.Verein.Tests
{
    public class TestHelper
    {
        public VereinDbContext InitializeDbContext(string connectionString)
        {

            // FIXME InMemory does not work, because __EFMigrationHistory Table is not created
            // See https://github.com/dotnet/efcore/issues/4922
            var options = new DbContextOptionsBuilder<VereinDbContext>().UseNpgsql(connectionString).Options;
            var dbContext = new VereinDbContext(options);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.Migrate();

            DbInitializer initializer = new DbInitializer();
            initializer.Initialize(dbContext);

            return dbContext;
        }


        public ILogger InitializeLogger()
        {
            var loggerFactory = new NullLoggerFactory();
            ILogger logger = loggerFactory.CreateLogger("");
            return logger;
        }

        public PageContext CreatePageContext()
        {
            var httpContext = new DefaultHttpContext();
            var modelState = new ModelStateDictionary();
            var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
            var modelMetadataProvider = new EmptyModelMetadataProvider();
            var viewData = new ViewDataDictionary(modelMetadataProvider, modelState);

            var pageContext = new PageContext(actionContext)
            {
                ViewData = viewData
            };
            return pageContext;
        }
    }
}
