using ligma_electronics_store.Models;
using ligma_electronics_store.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ligma_electronics_store.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        private void PopulateCategoryList()
        {
            var categories = _context.Categories.ToList();
            SelectList categorySelectList = new SelectList(categories, "Id", "Name");
            ViewData["CategoryList"] = categorySelectList;
        }

        public IActionResult Index()
        {
            var products = _context.Products.Include(p => p.Category).ToList();
            return View(products);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            PopulateCategoryList();
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create([Bind("Name, Price, Description, CategoryId")] ProductViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                PopulateCategoryList();
                return View(viewModel);
            }

            var product = new Product
            {
                Name = viewModel.Name,
                Price = viewModel.Price,
                Description = viewModel.Description,
                CategoryId = viewModel.CategoryId
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            var viewModel = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                CategoryId = product.CategoryId
            };

            PopulateCategoryList();
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(int id, [Bind("Id, Name, Price, Description, CategoryId")] ProductViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                PopulateCategoryList();
                return View(viewModel);
            }

            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            product.Name = viewModel.Name;
            product.Price = viewModel.Price;
            product.Description = viewModel.Description;
            product.CategoryId = viewModel.CategoryId;

            _context.Products.Update(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }

}
