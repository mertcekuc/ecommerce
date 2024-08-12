using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EcommerceApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace EcommerceApp.Controllers
{
    public class CartsController : Controller
    {
        private readonly EcommerceContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public CartsController(EcommerceContext context, UserManager<User> userManager, SignInManager<User> signInManager) 
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;

        }

        
        public async Task<IActionResult> Index()
        {
            int userId = _userManager.GetUserId(User) == null ? 0 : int.Parse(_userManager.GetUserId(User));
            if (userId == 0)
			{
				return RedirectToAction("Login", "Users");
			}
			var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
			cart.CartDetails = await _context.CartDetails
												 .Include(cd => cd.Product)
												 .Where(cd => cd.CartId == cart.Id)
												 .ToListAsync();
            return View( cart);
        }

     

 

        public IActionResult EmptyCart()
        {

            int userId = _userManager.GetUserId(User) == null ? 0 : int.Parse(_userManager.GetUserId(User));
			var cart = _context.Carts.Include(c => c.CartDetails).FirstOrDefault(c => c.UserId == userId);
            if (cart != null)
            {
                _context.CartDetails.RemoveRange(cart.CartDetails);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));

        }
        
        public IActionResult AddToCart(int productId)
        {

            //productId = Convert.ToInt32(Request.Form["productId"]);
            int userId = _userManager.GetUserId(User) == null ? 0 : int.Parse(_userManager.GetUserId(User));
            if (userId == 0)
                return RedirectToAction("Login", "Users");

            var cart = _context.Carts.Include(c => c.CartDetails).FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                return NotFound(); // Return a 404 if the cart is not found.
            }

            var product = _context.Products.Find(productId);
            if (product == null)
            {
                return NotFound(); // Return a 404 if the product is not found.
            }

            var cartDetail = cart.CartDetails.FirstOrDefault(cd => cd.ProductId == productId);
            if (cartDetail != null)
            {
                // If the product is already in the cart, increase the quantity
                cartDetail.Quantity++;
            }
            else
            {
                // If the product is not in the cart, add it
                cartDetail = new CartDetail
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = 1,
                    Price = product.Price
                };
                cart.CartDetails.Add(cartDetail);
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "Carts");

        }

        public IActionResult RemoveFromCart(int cartDetailId)
        {
            var cartDetail = _context.CartDetails.Find(cartDetailId);
            _context.CartDetails.Remove(cartDetail);
            _context.SaveChanges();
            return RedirectToAction("Index", "Carts");
        }

        public IActionResult UpdateQuantity(int cartDetailId, int quantity)
        {

            var cartDetail = _context.CartDetails.Find(cartDetailId);
            cartDetail.Quantity = quantity;
            _context.Update(cartDetail);
            _context.SaveChanges();
            return RedirectToAction("Index", "Carts");
        }
            

    }

        
}
