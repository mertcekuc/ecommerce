using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EcommerceApp.Models;
using Microsoft.AspNetCore.Identity;
using EcommerceApp.Models.ViewModels.Auth;
using EcommerceApp.Models.ViewModels;
using System.Runtime.Loader;

namespace EcommerceApp.Controllers
{
    public class UsersController : Controller
    {
        readonly UserManager<User> _userManager;
        readonly SignInManager<User> _signInManager;

        private readonly EcommerceDbContext _context;
        private readonly EcommerceContext _ecommerceContext;

        public UsersController(EcommerceContext ecommerceContext, EcommerceDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _ecommerceContext = ecommerceContext;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public IActionResult Index()
        {
            return View(_context.Users.ToList());
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                User appUser = new User
                {
                    UserName = userViewModel.UserName,
                    Email = userViewModel.Email,
                    LastName = userViewModel.LastName,
                    FirstName = userViewModel.FirstName,
                    CreatedDate = DateTime.Now
                };

                IdentityResult result = await _userManager.CreateAsync(appUser, userViewModel.Password);
                if (result.Succeeded)
                {
                    var cart = new Cart()
					{
						UserId = appUser.Id
					};
                    _ecommerceContext.Carts.Add(cart);
                    _ecommerceContext.SaveChanges();
                    // Sign in the user
                    await _signInManager.SignInAsync(appUser, isPersistent: false);
                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Add errors to ModelState
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(userViewModel);
        }


        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(SignInViewModel signInViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(signInViewModel.UserName, signInViewModel.Password, signInViewModel.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

            return View(signInViewModel);
        }

        
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
            
        }


        // GET: Users/Details/5
        public IActionResult Details()
        {
            
            ProfileViewModel profileViewModel = new ProfileViewModel();
            int id = _userManager.GetUserId(User) != null ? int.Parse(_userManager.GetUserId(User)) : 0;
            if (id == 0)
            {
                return RedirectToAction("Login", "User");
            }

            AspNetUser user = _ecommerceContext.AspNetUsers.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            else
            profileViewModel.User = user;

            var address = _ecommerceContext.Addresses.Where(a => a.UserId == id).Include(a => a.City).Include(a => a.Country);
            profileViewModel.Addresses = address.ToList();


            return View(profileViewModel);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Create([Bind("Email,UserName,Name,LastName")] User user)
        {
            if (ModelState.IsValid)
            {   
                //user.CreatedDate = DateTime.Now;
                _context.Add(user);
                _context.SaveChanges();
                var cart = new Cart()
                {
                    //userId = user.Id,
				};
                _context.Add(cart);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _ecommerceContext.AspNetUsers.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        public IActionResult Edit()
        {
            var user = _ecommerceContext.AspNetUsers.Find(Convert.ToInt32(Request.Form["Id"]));

            if (user == null)
            {
                return NotFound();
            }

            user.LastName = Request.Form["LastName"];
            user.FirstName = Request.Form["FirstName"];
            user.UserName = Request.Form["UserName"];
            user.Email = Request.Form["Email"];

            _ecommerceContext.Update(user);
            _ecommerceContext.SaveChanges();

            return RedirectToAction("Details");
        }


        // GET: Users/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _context.Users
                .Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult  Delete(int id)
        {
            
                if (id == null)
                {
                    return NotFound();
                }

                User user;
                try
                {
                    user = _context.Users
                       .Find(id);
                }
                catch
                {
                    return NotFound();
                }

                _context.Users.Remove(user);
                _context.SaveChanges();

                
            
            return RedirectToAction(nameof(Index));
            
        }

        [HttpGet]
        public IActionResult AddAddress()
        {
            ViewBag.Countries = _ecommerceContext.Countries.ToList();
            return View(new Address());
        }

        [HttpPost]
        public IActionResult AddAddress([Bind("Street","CountryId","CityId", "PostalCode")]Address address)
        {
            
            address.UserId = int.Parse(_userManager.GetUserId(User));
            address.CreatedDate = DateTime.Now;
            _ecommerceContext.Addresses.Add(address);
            _ecommerceContext.SaveChanges();

           
            return RedirectToAction("Details");
        }

        [HttpGet]
        public JsonResult GetCitiesByCountry(int id)
        {
            var cities = _ecommerceContext.Cities
                                 .Where(c => c.CountryId == id)
                                 .Select(c => new { c.Id, c.Name })
                                 .ToList();

            return Json(cities);
        }

        public IActionResult EditAddress(int id)
        {
            ViewBag.Countries = _ecommerceContext.Countries.ToList();

            var address = _ecommerceContext.Addresses
                .Include(a => a.City)
                .Include(a => a.Country)
                .FirstOrDefault(a => a.Id == id);

            if (address == null)
            {
                return NotFound();
            }

            //var model = new EditAddressViewModel
            //{
            //    Id = address.Id,
            //    Street = address.Street,
            //    CityId = address.CityId,
            //    CountryId = address.CountryId,
            //    PostalCode = address.PostalCode,
            //    AvailableCountries = _ecommerceContext.Countries.ToList(),
            //    AvailableCities = _ecommerceContext.Cities.Where(c => c.CountryId == address.CountryId).ToList()
            //};

            return View(address);
        }

        [HttpPost]
        public IActionResult EditAddress()
        {
            
                var address = _ecommerceContext.Addresses.Find(Convert.ToInt32(Request.Form["Id"]));

                if (address == null)
                {
                    return NotFound();
                }

                address.Street = Request.Form["Street"];
                address.CityId = Convert.ToInt32(Request.Form["CityId"]);
                address.CountryId = Convert.ToInt32(Request.Form["CountryId"]);
                address.PostalCode = Convert.ToInt32(Request.Form["PostalCode"]);

            _ecommerceContext.Update(address);
            _ecommerceContext.SaveChanges();

                return RedirectToAction("Details", new { id = address.UserId });
           
        }

    }
}
