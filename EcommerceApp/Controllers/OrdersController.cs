using Microsoft.AspNetCore.Mvc;
using EcommerceApp.Models;
using Microsoft.AspNetCore.Identity;
using EcommerceApp.Models.ViewModels;

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
            return View();
        }

        [HttpPost]
public IActionResult CheckOut(CheckoutViewModel model)
{
        int userid = _userManager.GetUserId(User) == null ? 0 : int.Parse(_userManager.GetUserId(User));
        if (User.Identity.IsAuthenticated && userid != 0)
                return RedirectToAction("Login", "Account", new { returnUrl = "/Orders/CheckOut" });
            // Create the Order
            var order = new Order
            {
                UserId = userid,
                CreatedDate = DateTime.Now,
                StatusId = 1, // Assuming 1 is the default status for new orders
                AddressId = model.SelectedAddressId
            };

        // Add Order Details from Cart Items
        foreach (var item in model.CartItems)
        {
            order.OrderDetails.Add(new OrderDetail
            {
                ProductId = item.ProductId,
                Price = item.Product.Price,
                Quantity = item.Quantity
            });
        }

        _context.Orders.Add(order);
        _context.SaveChanges();

        // Redirect to a confirmation page or order details
       

    // If we got this far, something failed, redisplay form
    return View(model);
    }

    }
}
