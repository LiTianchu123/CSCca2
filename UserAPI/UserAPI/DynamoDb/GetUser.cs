using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using UserAPI.Models;

namespace UserAPI.DynamoDb
{
    public class GetUser : IGetUser
    {
        private readonly IAmazonDynamoDB _dynamoClient;

        public GetUser(IAmazonDynamoDB dynamoClient)
        {
            _dynamoClient = dynamoClient;
        }

        public async Task<DynamoTableUsers> GetUsers(int? id)
        {
            var queryRequest = RequestBuilder(id);

            var result = await ScanAsync(queryRequest);

            return new DynamoTableUsers
            {
                Users = result.Items.Select(Map).ToList()
            };
        }

        private User Map(Dictionary<string, AttributeValue> result)
        {
            return new User
            {
                Id = Convert.ToInt32(result["id"].N),
                CustomerId = result["customerId"].S,
                CustomerEmail = result["customerEmail"].S,
                CustomerName = result["customerName"].S,
                Subscription = result["subscription"].S,
            };
        }

        private async Task<ScanResponse> ScanAsync(ScanRequest request)
        
        {
            ScanResponse response = new ScanResponse();
            //try
            //{
                response = await _dynamoClient.ScanAsync(request);
             
            //}
            //catch (Exception e) {
               
            //}
            return response;

        }

        private ScanRequest RequestBuilder(int? id)
        {
            if (id.HasValue == false)
            {
                return new ScanRequest
                {
                    TableName = "Users"
                };
            }

            return new ScanRequest
            {
                TableName = "Users",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {
                        ":v_Id", new AttributeValue { N = id.ToString()}}

                },
                FilterExpression = "id = :v_Id",
                ProjectionExpression = "id, customerId, customerEmail, customerName, subscription"
            };
        }

    }
}
