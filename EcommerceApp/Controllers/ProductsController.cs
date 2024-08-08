using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EcommerceApp.Models;

namespace EcommerceApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly EcommerceContext _context;


        public ProductsController(EcommerceContext context)
        {
            _context = context;

        }

        [HttpGet]
        public IActionResult Index()
        {
            var products = _context.Products.Include(p => p.Category).ToList();
            return View(products);
        }

        public IActionResult Details(int id) {
            var product = _context.Products.Find(id);
            return View(product);
        }
        
        public IActionResult Create() {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
            
        }

        [HttpPost]
        public IActionResult Create([Bind("Name,Description")] Product product)
        {
            product.Price = Convert.ToDecimal(Request.Form["Price"]);
            product.CreateDate = DateTime.Now;
            product.CreateUserId = 1;
            product.CategoryId = Convert.ToInt32(Request.Form["CategoryId"]);

            var file = Request.Form.Files["img"];

            _context.Products.Add(product);
            _context.SaveChanges();

            if (file != null && file.Length > 0)
            {
                // Ensure the directory exists
                var directoryPath = Path.Combine("wwwroot", "img", "Products");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Add a file extension (like .jpg, .png) to the file name
                var fileExtension = Path.GetExtension(file.FileName);
                var filePath = Path.Combine(directoryPath, $"{product.Id}{fileExtension}");

                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }

            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id) {
            return View(_context.Products.Find(id));
        }

        [HttpPost]
        public IActionResult Delete() {
            int id = Convert.ToInt32(Request.Form["Id"]);
            var product = _context.Products.Find(id);
            _context.Products.Remove(product);
            var cartDetails = _context.CartDetails.Where(cd => cd.ProductId == id);
            _context.CartDetails.RemoveRange(cartDetails);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id) {
            return View(_context.Products.Find(id));
        }
        [HttpPost]
        public IActionResult Edit()
        {   
            int id = Convert.ToInt32(Request.Form["Id"]);
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = Request.Form["Name"];
            product.Price = Convert.ToDecimal(Request.Form["Price"]);
            product.Description = Request.Form["Description"];
            product.ModifiedDate = DateTime.Now;

            if (int.TryParse(Request.Form["CategoryId"], out int categoryId))
            {
                product.CategoryId = categoryId;
            }

            if (int.TryParse(Request.Form["ModifiedUserId"], out int modifiedUserId))
            {
                product.ModifiedUserId = modifiedUserId;
            }

            _context.Products.Update(product);
            _context.SaveChanges();
            return RedirectToAction("Edit", new { id = product.Id });
        }

    }
}
