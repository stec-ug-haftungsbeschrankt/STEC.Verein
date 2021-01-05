using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Verein.Data;
using Verein.Models;
using Microsoft.EntityFrameworkCore;

namespace Verein
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            var environment = host.Services.GetRequiredService<IWebHostEnvironment>();

            PrintSystemInformation(environment, logger);
            CreateDbIfNotExists(host, environment);

            host.Run();
        }

        private static void PrintSystemInformation(IWebHostEnvironment environment, ILogger logger)
        {
            if (environment.IsDevelopment())
            {
                logger.LogInformation("Environment: Development");
            }
            else
            {
                logger.LogInformation("Environment: Production");
            }

            logger.LogInformation($"System Culture: {CultureInfo.CurrentCulture}");
        }

        private static void CreateDbIfNotExists(IWebHost host, IWebHostEnvironment environment)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<VereinDbContext>();
                    context.Database.Migrate();

                    if (environment.IsDevelopment())
                    {
                        var dbInitializer = new DbInitializer();
                        dbInitializer.Initialize(context);
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }


        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

    }
}
