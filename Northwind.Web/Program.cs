﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Northwind.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHost(args).Build();
            host.Run();
        }

        public static IWebHostBuilder CreateWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddFilter("System", LogLevel.Critical);
                logging.AddFilter("Microsoft", LogLevel.Critical);
            });
    }
}
