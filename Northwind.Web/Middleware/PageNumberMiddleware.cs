using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Middleware
{
    public class PageNumberService
    {
        public short pageSize;
        public PageNumberService(IConfiguration config)
        {
            pageSize = Int16.Parse(config.GetSection("section0:M").Value);
        }
    }
}
