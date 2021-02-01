using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.BillingPortal;
using System.Net.Http.Formatting;
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

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            StripeConfiguration.ApiKey = "sk_test_51I2fLjHVInnZqCxx5mfcY7lp7tH89t76L6SUNcISWHiC94j65SejSjybi0FDcROhbxnGkgnatlpcrPEGBwfUoDgQ00DYHAjbZ2";
            List<object> resultList = new List<object>();
            try
            {
                var options = new PriceListOptions { Currency = "sgd" };
                var service = new PriceService();
                StripeList<Price> prices = service.List(options);

                foreach (Price p in prices)
                {

                    float amount = (float)p.UnitAmount / 100;
                    string id = p.Id;
                    resultList.Add(new
                    {
                        id = id,
                        amount = amount,
                    });
                }
            }
            catch
            {
                return BadRequest();
            }
            return Ok(new JsonResult(resultList));
        }

        [HttpGet]
        public IActionResult GetSubscriptionList()
        {

            StripeConfiguration.ApiKey = "sk_test_51I2fLjHVInnZqCxx5mfcY7lp7tH89t76L6SUNcISWHiC94j65SejSjybi0FDcROhbxnGkgnatlpcrPEGBwfUoDgQ00DYHAjbZ2";
            List<object> resultList = new List<object>();
            try
            {
                var options = new SubscriptionListOptions
                {
                };
                var service = new SubscriptionService();
                StripeList<Stripe.Subscription> subscriptions = service.List(
                  options
                );

                foreach (Stripe.Subscription s in subscriptions)
                {

                    string id = s.Items.Data[0].Subscription;
                    float amount = (float)s.Items.Data[0].Plan.Amount / 100;
                    resultList.Add(new
                    {
                        id = id,
                        amount = amount,
                    });
                }
            }
            catch
            {
                return BadRequest();
            }
            return Ok(new JsonResult(resultList));

        }

        //[HttpPost("Webhook")]
        //public async Task<IActionResult> Webhook()
        //{

        //    //using (Stream iStream = Request.InputStream)
        //    //{
        //    //    using (StreamReader reader = new StreamReader(iStream, Encoding.UTF8))   //you should use   Request.ContentEncoding
        //    //    {
        //    //        json = await reader.ReadToEndAsync();
        //    //    }
        //    //}
        //    var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        //    try
        //    {

        //        var stripeEvent = EventUtility.ParseEvent(json);
        //        if (stripeEvent.Type == Events.CustomerCreated)
        //        {
        //            Customer cus = stripeEvent.Data.Object as Customer;
        //            var customerId = cus.Id;
        //            var customerName = cus.Name;
        //            var customerEmail = cus.Email;

        //        }
        //        else if (stripeEvent.Type == Events.CustomerSubscriptionCreated)
        //        {
        //            Subscription sub = stripeEvent.Data.Object as Subscription;
        //            Price price = sub.Items.Data[0].Price;
        //            int amount = (int)price.UnitAmount;
        //            string subscriptionId = sub.Items.Data[0].Subscription;

        //        }
        //        else if (stripeEvent.Type == Events.CustomerSubscriptionUpdated)
        //        {
        //            Subscription sub = stripeEvent.Data.Object as Subscription;
        //            Price price = sub.Items.Data[0].Price;
        //            int amount = (int)price.UnitAmount;
        //            string subscriptionId = sub.Items.Data[0].Subscription;

        //        }

               
        //        else
        //        {
        //            Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
        //        }
        //        return Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest();
        //    }
        //}


        //public void UpdateUserSubscription(int id, string subscription)
        //{
        //    string URL = "https://p6aplnpyw8.execute-api.us-east-1.amazonaws.com/Prod/api/DynamoDb/UpdateUser";
        //    string urlParameters = "?id="+id+"&subscription="+subscription;
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri(URL);

        //    // Add an Accept header for JSON format.
        //    client.DefaultRequestHeaders.Accept.Add(
        //    new MediaTypeWithQualityHeaderValue("application/json"));

        //    // List data response.
        //    HttpResponseMessage response = client.PutAsync(URL,urlParameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
        //    if (response.IsSuccessStatusCode)
        //    {
        //        // Parse the response body.
        //        var dataObjects = response.Content.ReadAsAsync<IEnumerable<object>>();  //Make sure to add a reference to System.Net.Http.Formatting.dll
        //        foreach (var d in dataObjects)
        //        {
        //            Console.WriteLine("{0}", d.Name);
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
        //    }

        //    // Make any other calls using HttpClient here.

        //    // Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
        //    client.Dispose();
        //}
    }
}
