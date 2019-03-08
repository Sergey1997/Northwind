using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Logging
{
    public static class FileLoggerExtensions
    {
        public static ILoggerFactory AddFileLogger(this ILoggerFactory loggerFactory, string path, LogLevel logLevel = LogLevel.Information)
        {
            loggerFactory.AddProvider(new FileLoggerProvider(path, logLevel));

            return loggerFactory;
        }
    }
}
