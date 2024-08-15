using Microsoft.AspNetCore.Mvc;
using EcommerceApp.Models;
using Microsoft.AspNetCore.Identity;
using EcommerceApp.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly EcommerceContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;  


        public OrdersController(EcommerceContext context, UserManager<User> userManager, SignInManager<User> signInManager) {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

       

		public IActionResult CheckOut()
		{
			int userId = int.Parse(_userManager.GetUserId(User));
			if (userId == 0)
			{
				return RedirectToAction("Login", "Account");
			}

			var user = _context.AspNetUsers
				.Include(u => u.Carts)
					.ThenInclude(c => c.CartDetails)
						.ThenInclude(cd => cd.Product)
				.Include(u => u.Addresses).ThenInclude(a => a.City).ThenInclude(a => a.Country)
				.FirstOrDefault(u => u.Id == userId);

			if (user == null)
			{
				return NotFound();
			}

			return View(user);
		}

        [HttpPost]
        public IActionResult PlaceOrder()
        {
            // Retrieve form data
            int userId = Convert.ToInt32(Request.Form["UserId"]);
            int addressId = Convert.ToInt32(Request.Form["AddressId"]);
            
          

            // Retrieve the cart for the user
            var cart = _context.Carts.Include(c => c.CartDetails).ThenInclude(cd => cd.Product).FirstOrDefault(c => c.UserId == userId);
                                     
            if (cart == null)
            {
                return NotFound();
            }

            // Create and save the order
            var order = new Order
            {
                UserId = userId,
                AddressId = addressId,
                CreatedDate = DateTime.Now,
                StatusId = 1 // Example status
            };
            _context.Orders.Add(order);
            _context.SaveChanges();

            // Create order details for each cart item
            foreach (var cartDetail in cart.CartDetails)
            {
                OrderDetail orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = cartDetail.ProductId,
                    Price = cartDetail.Product.Price,
                    Quantity = cartDetail.Quantity
                };
                _context.OrderDetails.Add(orderDetail);
            }

            // Clear the cart
            _context.CartDetails.RemoveRange(cart.CartDetails);
            _context.SaveChanges();

            
            return RedirectToAction("Details","Users");
        }
    }
}
