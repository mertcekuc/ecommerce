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
    public class CategoriesController : Controller
    {
        private readonly EcommerceContext _context;
        private readonly UserManager<User> _userManager;

        public CategoriesController(EcommerceContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


     

            [HttpGet]
            public IActionResult GetCategories()
            {
                var categories = _context.Categories.ToList();
                return Json(categories);
            }



        public IActionResult Index()
        {
            return View(_context.Categories.ToList());
        }

        public IActionResult CategoryPage(int id)
        {
            var category = _context.Categories.Where(c => c.Id == id).Include(c => c.Products).FirstOrDefault();
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
      
        public IActionResult Details(int id) {
            var category = _context.Categories.Find(id);
            return View(category);
        }

        public IActionResult Edit(int id) {
            return View(_context.Categories.Find(id));
        }
        [HttpPost]
        public IActionResult Edit()
        {
            if (_userManager.GetUserId(User) == null)
                return RedirectToAction("Login", "Users");

            int userId = Convert.ToInt32(_userManager.GetUserId(User));
            int id = Convert.ToInt32(Request.Form["Id"]);
            var category = _context.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            category.Name = Request.Form["Name"];
            category.Description = Request.Form["Description"];
            category.ModifiedDate = DateTime.Now;
            category.ModifiedUserId = userId;
            category.Active = Request.Form["Active"].Contains("true");

            _context.Categories.Update(category);
            _context.SaveChanges();

            var file = Request.Form.Files["img"];
            if (file != null && file.Length > 0)
            {
                var directoryPath = Path.Combine("wwwroot", "img", "Categories");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Optionally delete the old image if you're replacing it
                var oldFilePath = Directory.GetFiles(directoryPath, $"{category.Id}.*").FirstOrDefault();
                if (oldFilePath != null)
                {
                    System.IO.File.Delete(oldFilePath);
                }

                var fileExtension = Path.GetExtension(file.FileName);
                var filePath = Path.Combine(directoryPath, $"{category.Id}{fileExtension}");

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id) {
            return View(_context.Categories.Find(id));
        }
        [Authorize("Admin")]
        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        public IActionResult Create([Bind("Name,Description")] Category category)
        {
            int userId = _userManager.GetUserId(User) == null ? 1 : Convert.ToInt32(_userManager.GetUserId(User));
            category.CreateDate = DateTime.Now;
            category.CreateUserId = userId;
            category.Active = Request.Form["Active"].Contains("true");
            var file = Request.Form.Files["img"];

            _context.Categories.Add(category);
            _context.SaveChanges();

            if (file != null && file.Length > 0)
            {

                var directoryPath = Path.Combine("wwwroot", "img", "Categorıes");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Add a file extension (like .jpg, .png) to the file name
                var fileExtension = Path.GetExtension(file.FileName);
                var filePath = Path.Combine(directoryPath, $"{category.Id}{fileExtension}");

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }

            return RedirectToAction("Index");
        }
    }
}
