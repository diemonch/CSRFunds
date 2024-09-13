using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using CSRFundDetails.Models;

namespace CSRFundDetails.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly IMongoCollection<CsrFund> _csrFundCollection;
        private readonly IMongoCollection<DeficitCsrFund> _deficitCsrFundCollection;

        public AdminController(IMongoDatabase database)
        {
            _userCollection = database.GetCollection<User>("Users");
            _csrFundCollection = database.GetCollection<CsrFund>("CsrFunds");
            _deficitCsrFundCollection = database.GetCollection<DeficitCsrFund>("DeficitCsrFunds");
        }

        // Admin dashboard
        public IActionResult Index()
        {
            return View();
        }

        // Add New User (Organization)
        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddUser(User model)
        {
            if (ModelState.IsValid)
            {
                _userCollection.InsertOne(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // View All CSR Funds
        public IActionResult AllCsrFunds()
        {
            var csrFunds = _csrFundCollection.Find(f => true).ToList();
            return View(csrFunds);
        }

        // View All Deficit CSR Funds
        public IActionResult AllDeficitFunds()
        {

            Console.WriteLine("Inside View Deficit");
            var deficitCsrFunds = _deficitCsrFundCollection.Find(f => true).ToList();

            if (deficitCsrFunds == null || !deficitCsrFunds.Any())
            {
                Console.WriteLine("No Data");
                ViewBag.Message = "No Deficit CSR Funds found.";
                return View(new List<DeficitCsrFund>());  // Return an empty list to avoid a null reference in the view
            }

            return View(deficitCsrFunds);
           // return View(deficitFunds);
        }

        // Logout
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}