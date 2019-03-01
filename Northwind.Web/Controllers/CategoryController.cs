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
using Northwind.Web.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Northwind.Web.Controllers
{
    public class CategoryController : Controller
    {
        protected readonly NorthwindContext context;
        protected SettingService service;
        public CategoryController(NorthwindContext context, SettingService service)
        {
            this.context = context;
            this.service = service;
            if(service.pageSize == 0)
            {
                service.pageSize = context.Categories.Count();
            }
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index(int? page)
        {
            return View(await PaginatedList<Categories>
                .CreateAsync(context.Categories
                .AsNoTracking(), page ?? 1, service.pageSize));
        }
        public IActionResult Details(int? categoryId)
        {
            var model = context.Categories.SingleOrDefault(category => category.CategoryId == categoryId);

            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int categoryId)
        {

            var model = context.Categories.SingleOrDefault(category => category.CategoryId == categoryId);

            if (model == null)
            {
                return RedirectToAction("Index");
            }
            EditCategoryViewModel viewModel = new EditCategoryViewModel {
                CategoryId = model.CategoryId,
                CategoryName = model.CategoryName,
                Description = model.Description };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(EditCategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                var model = context.Categories.SingleOrDefault(category => category.CategoryId == viewModel.CategoryId);
                if (model != null)
                {
                    model.CategoryName = viewModel.CategoryName;
                    model.Description = viewModel.Description;
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(viewModel);
        }
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(CreateCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                Categories category = new Categories { CategoryName = model.CategoryName, Description = model.Description };
                context.Categories.Add(category);
                context.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(model);
        }
    }
}
