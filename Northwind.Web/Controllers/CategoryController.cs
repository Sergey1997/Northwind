using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Northwind.DataAccess.Context;
using Northwind.DataAccess.Models;
using Northwind.Web.Helpers;
using Northwind.Web.Configuration;
using Northwind.Web.Models;
using System;

namespace Northwind.Web.Controllers
{
    public class CategoryController : Controller
    {

        protected readonly NorthwindContext _context;
        protected readonly ILogger _logger;
        protected SettingsConfiguration _config { get; }

        public CategoryController(NorthwindContext context, SettingsConfiguration config, ILogger<CategoryController> logger)
        {
            _logger = logger;
            _context = context;
            _config = config;

            if(_config.PageSize.M == 0)
            {
                _config.PageSize.M = context.Categories.Count();
            }
        }

        public async Task<IActionResult> Index(int? page)
        {
            _logger.LogInformation("Getting all categories on page {page}", page);

            var categories = await PaginatedList<Categories>
                .CreateAsync(_context.Categories
                .AsNoTracking().OrderBy(category => category.CategoryId), page ?? 1, _config.PageSize.M);

            if(categories == null)
            {
                throw new NullReferenceException("Exception while fetching all the categories from the storage.");
            }

            return View(categories);
        }
        public IActionResult Details(int? categoryId)
        {
                _logger.LogInformation("Details of categoryId {categoryId} has been called", categoryId);

                var model = _context.Categories.SingleOrDefault(category => category.CategoryId == categoryId);

                if (model == null)
                {
                    throw new NullReferenceException("Model " + model.ToString() +" unable to be a null");
                }

                return View(model);
            
        }

        [HttpGet]
        public IActionResult Edit(int categoryId)
        {
            _logger.LogInformation("Edit of categoryId {categoryId} GET has been called", categoryId);
            var model = _context.Categories.SingleOrDefault(category => category.CategoryId == categoryId);

            if (model == null)
            {
                throw new NullReferenceException("Model " + model.ToString() + " unable to be a null");
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
            _logger.LogInformation("Editing of category {viewModel.CategoryId} has been called", viewModel.CategoryId);

            var model = _context.Categories.SingleOrDefault(category => category.CategoryId == viewModel.CategoryId);

            if (model == null)
            {
                throw new NullReferenceException("Edit of category {viewModel.CategoryId} failed, model isn't valid");
            }

            model.CategoryName = viewModel.CategoryName;
            model.Description = viewModel.Description;
            _context.SaveChanges();
            _logger.LogInformation("Category of {model.CategoryId} edited", model.CategoryId);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(CreateCategoryViewModel model)
        {
            _logger.LogInformation("Create post has been called");

            Categories category = new Categories { CategoryName = model.CategoryName, Description = model.Description };

            if(category == null)
            {
                throw new NullReferenceException("Create of {model} failed, reference are null");
            }

            _context.Categories.Add(category);
            _context.SaveChanges();

            _logger.LogInformation("Create category {category.CategoryId} successfully completed", category.CategoryId);

            return RedirectToAction("Index");
        }
    }
}
