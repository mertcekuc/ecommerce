using EcommerceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using EcommerceApp.Models.ViewModels;

namespace EcommerceApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EcommerceContext _context;

        public HomeController(ILogger<HomeController> logger, EcommerceContext context)
        {
            _logger = logger;
            _context = context;

        }

        public IActionResult Index()
        {
            var products = _context.Products
                        .OrderBy(p => Guid.NewGuid()) // Random order
                        .Take(4) // Take 3 random products
                        .ToList();

            var categories = _context.Categories.Take(3).ToList();

            var viewModel = new HomeIndexViewModel
            {
                Products = products,
                Categories = categories
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
