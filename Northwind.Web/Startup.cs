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
            string connection = "NorthwindConnection";
           
            services.AddScoped<SettingService>();
            services.AddDbContext<NorthwindContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString(connection)));
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            ConfigureLogging(env, loggerFactory);
            _logger = loggerFactory.CreateLogger<Startup>();
            _logger.LogError("Directory " + env.ContentRootPath);
            _logger.LogError(Configuration.GetChildren().ToString());

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

        private void ConfigureLogging(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var logConfig = Configuration.GetSection("Logging");

            var fileConfig = logConfig.GetSection("File");

            var path = fileConfig.GetValue<string>("path");
            var logLevel = Enum.Parse<LogLevel>(fileConfig.GetValue<string>("Loglevel"));

            loggerFactory.AddFile(path, logLevel);

        }
    }
}