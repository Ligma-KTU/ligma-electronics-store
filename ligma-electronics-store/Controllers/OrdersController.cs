using ligma_electronics_store.Models;
using ligma_electronics_store.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace ligma_electronics_store.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OrdersController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var orders = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToList();
            return View(orders);
        }

        public IActionResult Details(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .Where(o => o.Id == id)
                .FirstOrDefault();

            return View(order);
        }

        public IActionResult Delete(int id)
        {
            var order = _context.Orders.Find(id);
            _context.Orders.Remove(order);
            _context.SaveChanges();;

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(OrderViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Create", model);

            string userId = _userManager.GetUserId(User);

            var order = new Order
                {
                    UserId = userId,
                    Name = model.Name,
                    Surname = model.Surname,
                    Address = model.Address,
                    ZipCode = model.ZipCode,
                    Country = model.Country,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    OrderDate = DateTime.UtcNow,
                    OrderItems = new List<OrderItem>()
                };

            var shoppingCart = _context.ShoppingCarts
                .Include(sc => sc.ShoppingCartItems)
                .FirstOrDefault(sc => sc.UserId == userId);

            foreach (var cartItem in shoppingCart.ShoppingCartItems)
            {
                var orderItem = new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity
                };

                order.OrderItems.Add(orderItem);
            }

            _context.Orders.Add(order);

            shoppingCart.ShoppingCartItems.Clear();
            _context.SaveChanges();

            return View("OrderSuccess");
        }
    }
}