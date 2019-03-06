using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Northwind.DataAccess.Context;
using Northwind.Web.Logging;
using Northwind.Web.Middleware;

namespace Northwind.Web
{
    public class Startup
    {
        protected ILogger _logger;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            var config = new SettingsConfiguration();
            Configuration.Bind("App", config);
            services.AddSingleton(config);

            services.AddDbContext<NorthwindContext>(options =>
            options.UseSqlServer(config.ConnectionStrings.NorthwindConnection));
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, SettingsConfiguration config)
        {
            ConfigureLogging(env, loggerFactory, config);
            _logger = loggerFactory.CreateLogger<Startup>();
            WriteAppSettings(env, _logger, config);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void ConfigureLogging(IHostingEnvironment env, ILoggerFactory loggerFactory, SettingsConfiguration config)
        {
            loggerFactory.AddFile(config.Logging.Path, config.Logging.LogLevel);
        }
        private void WriteAppSettings(IHostingEnvironment env, ILogger _logger,SettingsConfiguration config)
        {
            _logger.LogInformation("Directory " + env.ContentRootPath);
            _logger.LogInformation("PageSize:" + config.PageSize.M);
            _logger.LogInformation("Path to log:" + config.Logging.Path);
            _logger.LogInformation("LogLevel:" + config.Logging.LogLevel);
            _logger.LogInformation("NorthwindConnectionString:" + config.ConnectionStrings.NorthwindConnection);
        }
    }
}