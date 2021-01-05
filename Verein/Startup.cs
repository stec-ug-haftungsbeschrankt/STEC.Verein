using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using STEC.Services.Mailing;
using Verein.Data;
using Verein.Models;
using Verein.Maps;


namespace Verein
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment, ILogger<Startup> logger)
        {
            Configuration = configuration;
            Environment = environment;
            Logger = logger;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        public ILogger Logger { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            if (Environment.IsDevelopment())
            {               
                services.AddDbContext<VereinDbContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("DevelopmentVereinContextPostgreSql")));
            }
            else
            {
                services.AddDbContext<VereinDbContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("VereinContextPostgreSql")));
            }

            services.AddDefaultIdentity<HundevereinUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<VereinDbContext>();


            services.AddScoped<IDatabaseMediator, DatabaseMediator>();

/*
            services.AddTransient<IEmailSender, SendgridEmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration.GetSection("AuthMessageSenderOptions"));
*/
            services.AddTransient<IEmailSender, SmtpEmailSender>();
            services.Configure<SmtpOptions>(Configuration.GetSection("SmtpOptions"));

            services.AddTransient<IGeoService, GeoMapService>();
            services.Configure<GeoMapOptions>(Configuration.GetSection("GeoMapOptions"));

            // Do not remove, needed for Authorization of Account Pages
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });

            services.AddControllers(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthorization();
            app.UseAuthentication();

            var cultureInfo = new CultureInfo("de-DE");
            cultureInfo.NumberFormat.CurrencySymbol = "€";

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(cultureInfo),
                SupportedCultures = new List<CultureInfo>
                {
                    cultureInfo,
                },
                SupportedUICultures = new List<CultureInfo>
                {
                    cultureInfo,
                }
            });

            app.UseMvc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });

        }
    }
}
