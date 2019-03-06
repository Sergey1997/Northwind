using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Middleware
{
    public class SettingsConfiguration
    {
        public LoggingConfiguration Logging { get; set; } = new LoggingConfiguration();
        public PageSizeConfiguration PageSize { get; set; } = new PageSizeConfiguration();
        public ConnectionStringConfgiration ConnectionStrings { get; set; } = new ConnectionStringConfgiration();
    }
    public class LoggingConfiguration
    {
        public string Path { get; set; }
        public LogLevel LogLevel { get; set; }
    }
    public class PageSizeConfiguration
    {
        public int M { get; set; }
    }
    public class ConnectionStringConfgiration
    {
        public string NorthwindConnection { get; set; }
    }
}
