using System;
using CSRFundDetails.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CSRFundDetails.Controllers
{
    public class CsrController : Controller
    {
        private readonly IMongoCollection<User> _userCollection;  // User collection
        private readonly IMongoCollection<CsrFund> _csrFundCollection;  // Excess CSR Fund
        private readonly IMongoCollection<DeficitCsrFund> _deficitCsrFundCollection;  // Deficit CSR Fund

        public CsrController(IMongoDatabase database)
        {
            _userCollection = database.GetCollection<User>("Users");  // User collection
            _csrFundCollection = database.GetCollection<CsrFund>("CsrFunds");  // Excess CSR Fund collection
            _deficitCsrFundCollection = database.GetCollection<DeficitCsrFund>("DeficitCsrFunds");  // Deficit CSR Fund collection
        }

         public IActionResult Index()
            {
                var userId = HttpContext.Session.GetString("UserId");
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }

                // Retrieve the user's OrganizationId from the Users collection
                var user = _userCollection.Find(u => u.UserId == userId).FirstOrDefault();
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var organizationId = user.OrganizationId;

                // Find the CsrFund by OrganizationId
                var csrFund = _csrFundCollection.Find(f => f.OrganizationId == organizationId).FirstOrDefault();
                if (csrFund == null)
                {
                    csrFund = new CsrFund
                    {
                        OrganizationId = organizationId,
                        OrganizationName = "New Organization",
                        OpeningBalance = 0,
                        CsrFundAlloted = 0,
                        ProjectedExpenditure = 0,
                        CsrFundBalance = 0
                    };
                    _csrFundCollection.InsertOne(csrFund);  // Insert default record for new organizations
                }

                // Find the DeficitCsrFund by OrganizationId
                var deficitCsrFund = _deficitCsrFundCollection.Find(f => f.OrganizationId == organizationId).FirstOrDefault();
                if (deficitCsrFund == null)
                {
                    deficitCsrFund = new DeficitCsrFund
                    {
                        OrganizationId = organizationId,
                        OrganizationName = "New Organization",
                        ProjectName = string.Empty,
                        ProjectReceivedFrom = string.Empty,
                        ImpactOutcome = string.Empty,
                        TargetBeneficiaries = string.Empty,
                        ProjectValue = 0
                    };
                    _deficitCsrFundCollection.InsertOne(deficitCsrFund);  // Insert default record for new organizations
                }

                // Combine the two models into a view model
                var combinedViewModel = new CombinedCsrFundsViewModel
                {
                    CsrFund = csrFund,
                    DeficitCsrFund = deficitCsrFund
                };

                return View(combinedViewModel);
            }
        

        [HttpPost]

        public IActionResult Save(CombinedCsrFundsViewModel model)
        {
            //Console.WriteLine("Model state is invalid.");
            //foreach (var key in ModelState.Keys)
            //{
            //    var state = ModelState[key];
            //    if (state != null)  // Check for null
            //    {
            //        foreach (var error in state.Errors)
            //        {
            //            Console.WriteLine($"Key: {key}, Error: {error.ErrorMessage}");
            //        }
            //    }
            //    else
            //    {
            //        Console.WriteLine($"Model state entry for key '{key}' is null.");
            //    }
            //}
            //return View("Index", model);


            // Log the incoming data for debugging
            Console.WriteLine("Received CsrFund:");
            Console.WriteLine($"OrganizationId: {model.CsrFund.OrganizationId}");
            Console.WriteLine($"OrganizationName: {model.CsrFund.OrganizationName}");
            Console.WriteLine($"OpeningBalance: {model.CsrFund.OpeningBalance}");
            Console.WriteLine($"Received DeficitCsrFund:");
            Console.WriteLine($"OrganizationId: {model.DeficitCsrFund.OrganizationId}");
            Console.WriteLine($"ProjectName: {model.DeficitCsrFund.ProjectName}");

            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model state is invalid.");
                return View("Index", model);
            }

            // Retrieve the original document from MongoDB for CsrFund
            var existingCsrFund = _csrFundCollection.Find(f => f.OrganizationId == model.CsrFund.OrganizationId).FirstOrDefault();
            if (existingCsrFund != null)
            {
                // Preserve the _id field and update other fields
                model.CsrFund.Id = existingCsrFund.Id;

                // Replace the existing document in the CsrFund collection
                var csrFundUpdateResult = _csrFundCollection.ReplaceOne(f => f.OrganizationId == model.CsrFund.OrganizationId, model.CsrFund);
                if (!csrFundUpdateResult.IsAcknowledged)
                {
                    Console.WriteLine("Error updating CsrFund in MongoDB.");
                }
            }
            else
            {
                // Insert a new CsrFund document if it doesn't exist
                _csrFundCollection.InsertOne(model.CsrFund);
            }

            // Retrieve the original document from MongoDB for DeficitCsrFund
            var existingDeficitCsrFund = _deficitCsrFundCollection.Find(f => f.OrganizationId == model.DeficitCsrFund.OrganizationId).FirstOrDefault();
            if (existingDeficitCsrFund != null)
            {
                // Preserve the _id field and update other fields
                model.DeficitCsrFund.Id = existingDeficitCsrFund.Id;

                // Replace the existing document in the DeficitCsrFund collection
                var deficitCsrFundUpdateResult = _deficitCsrFundCollection.ReplaceOne(f => f.OrganizationId == model.DeficitCsrFund.OrganizationId, model.DeficitCsrFund);
                if (!deficitCsrFundUpdateResult.IsAcknowledged)
                {
                    Console.WriteLine("Error updating DeficitCsrFund in MongoDB.");
                }
            }
            else
            {
                // Insert a new DeficitCsrFund document if it doesn't exist
                _deficitCsrFundCollection.InsertOne(model.DeficitCsrFund);
            }

            return RedirectToAction("Index");
        }




        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}