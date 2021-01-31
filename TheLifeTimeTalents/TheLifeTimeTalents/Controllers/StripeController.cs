using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.BillingPortal;

namespace TheLifeTimeTalents.Controllers
{
    public class StripeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CustomerPortal()
        {
            StripeConfiguration.ApiKey = "sk_test_51I2fLjHVInnZqCxx5mfcY7lp7tH89t76L6SUNcISWHiC94j65SejSjybi0FDcROhbxnGkgnatlpcrPEGBwfUoDgQ00DYHAjbZ2";
            Session session = new Session();
            try
            {
                var options = new SessionCreateOptions
                {
                    Customer = "cus_IhyVFRJRWlxfwh",
                    ReturnUrl = "https://localhost:44362/",
                };
                var service = new SessionService();
                session = service.Create(options);
            }
            catch
            {
                return BadRequest();
            }

            return Content(session.Url);
        }
    }
}
