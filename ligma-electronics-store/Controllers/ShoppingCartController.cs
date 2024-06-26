﻿using ligma_electronics_store.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ligma_electronics_store.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public ShoppingCartController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);

            var shoppingCartItems = _context.ShoppingCartItems
                .Where(sci => sci.ShoppingCart.UserId == userId)
                .Include(sci => sci.Product)
                .ToList();

            return View(shoppingCartItems);
        }

        public IActionResult AddItem(int productId, int quantity)
        {
            if (quantity < 1)
            {
                return View("QuantityError");
            }

            string userId = _userManager.GetUserId(User);

            var shoppingCart = _context.ShoppingCarts
             .Include(sc => sc.ShoppingCartItems)
             .FirstOrDefault(sc => sc.UserId == userId);

            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart
                {
                    UserId = userId,
                    ShoppingCartItems = new List<ShoppingCartItem>()
                };
                _context.ShoppingCarts.Add(shoppingCart);
            }

            var shoppingCartItem = new ShoppingCartItem
            {
                ProductId = productId,
                Quantity = quantity
            };

            shoppingCart.ShoppingCartItems.Add(shoppingCartItem);
            _context.SaveChanges();

            var product = _context.Products.Find(productId);

            int categoryId = product.CategoryId;

            return RedirectToAction("ProductsInCategory", "Home", new { id = categoryId });
        }

        public IActionResult RemoveItem(int id)
        {
            var shoppingCartItem = _context.ShoppingCartItems.Find(id);

            _context.ShoppingCartItems.Remove(shoppingCartItem);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
