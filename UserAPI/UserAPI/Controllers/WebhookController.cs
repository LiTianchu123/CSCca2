using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using UserAPI.DynamoDb;
using UserAPI.Models;

namespace UserAPI.Controllers
{
    public class WebhookController : Controller
    {
        private readonly IAddUser _putItem;
        private readonly IGetUser _getItem;
        private readonly IUpdateUser _updateItem;
        //  private readonly IDeleteTable _deleteTable;

        public WebhookController(IAddUser putItem, IGetUser getItem, IUpdateUser updateItem)
        {
            // _createTable = createTable;
            _putItem = putItem;
            _getItem = getItem;
            _updateItem = updateItem;
            // _deleteTable = deleteTable;
        }
        [HttpPost("Webhook")]
        public async Task<IActionResult> Webhook()
        {

            //using (Stream iStream = Request.InputStream)
            //{
            //    using (StreamReader reader = new StreamReader(iStream, Encoding.UTF8))   //you should use   Request.ContentEncoding
            //    {
            //        json = await reader.ReadToEndAsync();
            //    }
            //}
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {

                var stripeEvent = EventUtility.ParseEvent(json);
                if (stripeEvent.Type == Events.CustomerCreated)
                {
                    Customer cus = stripeEvent.Data.Object as Customer;
                    var customerId = cus.Id;
                    var customerName = cus.Name;
                    var customerEmail = cus.Email;
                    try
                    {
                       await _putItem.AddNewEntry(customerId, customerEmail, customerName, "No Subscription");
                    }
                    catch (Exception e)
                    {
                       
                    }


                }
                else if (stripeEvent.Type == Events.CustomerSubscriptionUpdated)
                {
                    Subscription sub = stripeEvent.Data.Object as Subscription;
                    Price price = sub.Items.Data[0].Price;
                    int amount = (int)price.UnitAmount;
                    string subscriptionId = sub.Items.Data[0].Subscription;
                    string customerId = sub.CustomerId;
                    User response = new User();
                    try
                    {
                        response = await _updateItem.Update(customerId, subscriptionId);
                    }
                    catch (Exception e)
                    {

                    }
                }
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }


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
