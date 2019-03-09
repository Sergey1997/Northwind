using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Northwind.DataAccess.Context;
using Northwind.DataAccess.Models;
using Northwind.Web.Helpers;
using Northwind.Web.Configuration;
using Northwind.Web.Models;
using Microsoft.Extensions.Logging;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Northwind.Web.Controllers
{
    public class ProductController : Controller
    {
        protected readonly NorthwindContext context;
        protected readonly ILogger _logger;
        protected SettingsConfiguration _config { get; }
        public ProductController(NorthwindContext context, SettingsConfiguration config, ILogger<ProductController> logger)
        {
            this.context = context;
            _config = config;
            _logger = logger;

            if (_config.PageSize.M == 0)
            {
                _config.PageSize.M = context.Products.Count();
            }
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index(int? page)
        {
            return View(await PaginatedList<Products>
                .CreateAsync(context.Products
                .Include(product=>product.Category)
                .Include(product => product.Supplier)
                .AsNoTracking(), page ?? 1, _config.PageSize.M));
        }
        public IActionResult Details(int? productId)
        {
            _logger.LogInformation("Details of {0} product has been started", productId);

            var model = context.Products.SingleOrDefault(product => product.ProductId == productId);

            if (model == null)
            {
                throw new NullReferenceException("Exception while fetching all the products from the storage.");
            }

            _logger.LogInformation("Details of {0} product has been finished", productId);
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int? productId)
        {
            _logger.LogInformation("Edit GET of {0} product has been started", productId);

            var model = context.Products.Find(productId);

            if (model == null)
            {
                throw new NullReferenceException("Edit of category {productId} failed, model unable to be a null");
            }

            SelectList categories = new SelectList(context.Categories, "CategoryId", "CategoryName", model.CategoryId);
            SelectList suppliers = new SelectList(context.Suppliers, "SupplierId", "CompanyName", model.SupplierId);
            ViewBag.Categories = categories;
            ViewBag.Suppliers = suppliers;

            EditProductViewModel viewModel = new EditProductViewModel
            {
                ProductId = model.ProductId,
                ProductName = model.ProductName,
                SupplierId = model.SupplierId,
                CategoryId = model.CategoryId,
                Discontinued = model.Discontinued
            };
            _logger.LogInformation("Edit GET of {0} product has been finished", productId);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(EditProductViewModel viewModel)
        {
            _logger.LogInformation("Details POST of {0} product has been started", viewModel.ProductId);

            var model = context.Products.SingleOrDefault(product => product.ProductId == viewModel.ProductId);

            if (model == null)
            {
                throw new NullReferenceException("Edit of category {model} failed, model unable to be a null");
            }

            model.ProductName = viewModel.ProductName;
            model.SupplierId = viewModel.SupplierId;
            model.CategoryId = viewModel.CategoryId;
            model.Discontinued = viewModel.Discontinued;
            context.SaveChanges();

            _logger.LogInformation("Details of {0} product has been finished", viewModel.ProductId);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Create()
        {
            _logger.LogInformation("Create GET of product has been started");
            SelectList categories = new SelectList(context.Categories, "CategoryId", "CategoryName");
            SelectList suppliers = new SelectList(context.Suppliers, "SupplierId", "CompanyName");
            ViewBag.Categories = categories ?? throw new NullReferenceException("Categories {categories} uanble to be a null");
            ViewBag.Suppliers = suppliers ?? throw new NullReferenceException("Suppliers {categories} uanble to be a null");
            _logger.LogInformation("Details GET of product has been finished");
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateProductViewModel model)
        {
            _logger.LogInformation("Trying to create product {0}", model.ProductName);
            Products product = new Products
            {
                ProductName = model.ProductName,
                SupplierId = model.SupplierId,
                CategoryId = model.CategoryId
            };

            if(product == null)
            {
                throw new NullReferenceException("Edit of category {product} failed, model unable to be a null");
            }

            context.Products.Add(product);
            context.SaveChanges();
            _logger.LogInformation("Product {0} has been created", model.ProductName);

            return RedirectToAction("Index");
        }
    }
}
