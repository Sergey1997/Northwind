using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.DataAccess.Context;
using Northwind.DataAccess.Models;
using Northwind.Web.Helpers;
using Northwind.Web.Middleware;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Northwind.Web.Controllers
{
    public class ProductController : Controller
    {
        protected readonly NorthwindContext context;
        public PageNumberService service;
        public ProductController(NorthwindContext context, PageNumberService service)
        {
            this.context = context;
            this.service = service;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index(int? page)
        {
            return View(await PaginatedList<Products>.CreateAsync(context.Products.AsNoTracking(), page ?? 1, service.pageSize));
        }
    }
}
