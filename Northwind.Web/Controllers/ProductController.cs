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


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Northwind.Web.Controllers
{
    public class ProductController : Controller
    {
        protected readonly NorthwindContext context;
        public ProductController(NorthwindContext context)
        {
            this.context = context;
            //this.service = service;
            //if (service.pageSize == 0)
            //{
            //    service.pageSize = context.Products.Count();
            //}
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index(int? page)
        {
            return View(await PaginatedList<Products>
                .CreateAsync(context.Products
                .Include(product=>product.Category)
                .Include(product => product.Supplier)
                .AsNoTracking(), page ?? 1, 5));
        }
        public IActionResult Details(int? productId)
        {
            var model = context.Products.SingleOrDefault(product => product.ProductId == productId);

            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int? productId)
        {

            var model = context.Products.Find(productId);

            if (model == null)
            {
                return RedirectToAction("Index");
            }

            if (model != null)
            {
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
                return View(viewModel);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(EditProductViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                var model = context.Products.SingleOrDefault(product => product.ProductId == viewModel.ProductId);
                if (model != null)
                {
                    model.ProductName = viewModel.ProductName;
                    model.SupplierId = viewModel.SupplierId;
                    model.CategoryId = viewModel.CategoryId;
                    model.Discontinued = viewModel.Discontinued;

                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(viewModel);
        }
        [HttpGet]
        public ActionResult Create()
        {
            SelectList categories = new SelectList(context.Categories, "CategoryId", "CategoryName");
            SelectList suppliers = new SelectList(context.Suppliers, "SupplierId", "CompanyName");
            ViewBag.Categories = categories;
            ViewBag.Suppliers = suppliers;

            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                Products product = new Products
                {
                    ProductName = model.ProductName,
                    SupplierId = model.SupplierId,
                    CategoryId = model.CategoryId
                };
                context.Products.Add(product);
                context.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(model);
        }
    }
}
