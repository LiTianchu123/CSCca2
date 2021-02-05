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
        private readonly string API_KEY = "stripeapikey";
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CustomerPortal(string customerId)
        {
            StripeConfiguration.ApiKey = API_KEY;
            Session session = new Session();
            try
            {
                var options = new SessionCreateOptions
                {
                    Customer = customerId,
                    ReturnUrl = "http://thelifetimetalents-dev.us-east-1.elasticbeanstalk.com/Home/Products",
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
            StripeConfiguration.ApiKey = API_KEY;
            List<object> resultList = new List<object>();
            try
            {

                var options = new ProductListOptions
                {
                    Active = true,
                };
                var service = new ProductService();
                StripeList<Product> products = service.List(
                  options
                );
                foreach (Product p in products)
                {

                    var priceOptions = new PriceListOptions { Currency = "sgd", Active = true, Product = p.Id };
                    var priceService = new PriceService();
                    StripeList<Price> prices = priceService.List(priceOptions);
                    float amount = 0.0F;
                    string priceId = "";
                    foreach (Price pr in prices)
                    {
                        priceId = pr.Id;
                        amount = (float)pr.UnitAmount / 100;
                    }
                    string id = p.Id;
                    string name = p.Name;

                    resultList.Add(new
                    {
                        id = id,
                        priceId = priceId,
                        name = name,
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
        public IActionResult GetSubscription([FromQuery] string customerId)
        {

            StripeConfiguration.ApiKey = API_KEY;
            List<object> resultList = new List<object>();
            try
            {
                var options = new SubscriptionListOptions
                {
                    Customer = customerId
                };
                var service = new SubscriptionService();
                StripeList<Stripe.Subscription> subscriptions = service.List(
                  options
                );

                foreach (Stripe.Subscription s in subscriptions)
                {

                    string id = s.Items.Data[0].Subscription;
                    string productId = s.Items.Data[0].Price.ProductId;
                    string priceId = s.Items.Data[0].Price.Id;
                    resultList.Add(new
                    {
                        id = id,
                        prodId = productId,
                        priceId = priceId
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
        public IActionResult GetCustomer([FromQuery] string email)
        {
            StripeConfiguration.ApiKey = API_KEY;
            List<object> resultList = new List<object>();
            var options = new CustomerListOptions
            {
                Limit = 1,
                Email = email,
            };
            var service = new CustomerService();
            StripeList<Customer> customers = service.List(
              options
            );
            foreach (Customer cus in customers)
            {
                string customerId = cus.Id;
                string customerName = cus.Name;

                var subOptions = new SubscriptionListOptions
                {
                    Limit = 1,
                    Customer = customerId
                };
                var subService = new SubscriptionService();
                StripeList<Stripe.Subscription> subscriptions = subService.List(
                  subOptions
                );
                Subscription sub = subscriptions.ElementAt(0);

                resultList.Add(new
                {
                    id = customerId,
                    name = customerName,
                    amount = (int)sub.Items.Data[0].Price.UnitAmount
                });
            }
            return Ok(new JsonResult(resultList));
        }

        [HttpPost]
        public IActionResult AddCustomer(string email, string name)
        {
            StripeConfiguration.ApiKey = API_KEY;

            var options = new CustomerCreateOptions
            {
                Email = email,
                Name = name,
            };
            var service = new CustomerService();
            Customer customer = service.Create(options);

            var subOptions = new SubscriptionCreateOptions
            {
                Customer = customer.Id,
                Items = new List<SubscriptionItemOptions>
                {
                new SubscriptionItemOptions
                  {
                   Price = "price_1IFcjaHVInnZqCxxolFGevVP",
                        },
                   },
               };
            var subService = new SubscriptionService();
            subService.Create(subOptions);
            return Ok();
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
