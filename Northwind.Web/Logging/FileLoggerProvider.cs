using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Logging
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private string _path;
        private LogLevel _logLevel;

        public FileLoggerProvider(string path, LogLevel logLevel)
        {
            _path = path;
            _logLevel = logLevel;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(_path, _logLevel, categoryName);
        }

        public void Dispose()
        {

        }
    }
}
