using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;
// Assume User model is defined in this namespace
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using CSRFundDetails.Models;

namespace CSRDetails.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<User> _userCollection;

        public AccountController(IMongoDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _userCollection = _database.GetCollection<User>("Users")
                ?? throw new Exception("User collection is not initialized.");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (string.IsNullOrEmpty(model.UserId) || string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError(string.Empty, "User ID and Password cannot be empty.");
                return View(model);
            }

            Console.WriteLine("Attempting to log in with UserId: " + model.UserId);

            // Fetch user from MongoDB
            var user = _userCollection.Find(u => u.UserId == model.UserId && u.Password == model.Password).FirstOrDefault();

            if (user == null)
            {
                // ModelState.AddModelError(string.Empty, "Invalid credentials.");
                ViewBag.ErrorMessage = "Invalid User ID or Password.";
                return View(model);
            }

            Console.WriteLine("User Role: " + user.Role);

            var adminUser = _userCollection.Find(u => u.UserId == "admin_user_id" && u.Password == "admin_password").FirstOrDefault();
            if (adminUser != null)
            {
                Console.WriteLine("Admin user found and the role is: " + adminUser.Role);
            }
            else
            {
                Console.WriteLine("Admin user not found.");
            }

            // Set session variables
            HttpContext.Session.SetString("UserId", user.UserId);
            HttpContext.Session.SetString("UserRole", user.Role);

            // Redirect based on role
            if (user.Role == "csr-admin")
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (user.Role == "csr-user")
            {
                return RedirectToAction("Index", "Csr");
            }

            //ModelState.AddModelError(string.Empty, "Unknown role.");
            ViewBag.ErrorMessage = "Unknown role.";
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}