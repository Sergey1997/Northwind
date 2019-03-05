using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Northwind.DataAccess.Context;
using Northwind.DataAccess.Models;
using Northwind.Web.Helpers;
using Northwind.Web.Middleware;
using Northwind.Web.Models;

namespace Northwind.Web.Controllers
{
    public class CategoryController : Controller
    {

        protected readonly NorthwindContext _context;
        protected readonly ILogger _logger;
        protected SettingService service;

        public CategoryController(NorthwindContext context, SettingService service, ILoggerFactory logger)
        {
            _logger = logger.CreateLogger<CategoryController>();
            _context = context;
            this.service = service;
            if(service.pageSize == 0)
            {
                service.pageSize = context.Categories.Count();
            }
        }

        public async Task<IActionResult> Index(int? page)
        {
            _logger.LogInformation("Getting all categories on page {page}", page);

            var categories = await PaginatedList<Categories>
                .CreateAsync(_context.Categories
                .AsNoTracking().OrderBy(category => category.CategoryId), page ?? 1, service.pageSize);

            if(categories == null)
            {
                _logger.LogError("Index ({page}) not found", page);
                return NotFound();
            }

            return View(categories);
        }
        public IActionResult Details(int? categoryId)
        {
            _logger.LogInformation("Details of categoryId {categoryId} has been called", categoryId);
            var model = _context.Categories.SingleOrDefault(category => category.CategoryId == categoryId);

            if (model == null)
            {
                _logger.LogError("Details of categoryId ({categoryId}) not found", categoryId);
                return NotFound();
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
                _logger.LogError("Edit of categoryId({categoryId}) GET NOT FOUND", categoryId);
                return NotFound();
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
            _logger.LogInformation("Edit of caregory {viewModel.CategoryId} POST has been called", viewModel.CategoryId);
            if (ModelState.IsValid)
            {
                var model = _context.Categories.SingleOrDefault(category => category.CategoryId == viewModel.CategoryId);
                if (model != null)
                {
                    model.CategoryName = viewModel.CategoryName;
                    model.Description = viewModel.Description;
                    _context.SaveChanges();
                    _logger.LogInformation("Category of {model.CategoryId} edited", model.CategoryId);

                    return RedirectToAction("Index");
                }
            }
            _logger.LogError("Edit of category {viewModel.CategoryId} failed, model isn't valid", viewModel.CategoryId);
            return View(viewModel);
        }
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(CreateCategoryViewModel model)
        {
            _logger.LogInformation("Create post has been called");
            if (ModelState.IsValid)
            {
                Categories category = new Categories { CategoryName = model.CategoryName, Description = model.Description };
                _context.Categories.Add(category);
                _context.SaveChanges();
                _logger.LogInformation("Create category {category.CategoryId} successfully completed", category.CategoryId);
                return RedirectToAction("Index");
            }
            _logger.LogError("Create of {model.CategoryName} failed, model isn't valid", model.CategoryName);
            return View(model);
        }
    }
}
