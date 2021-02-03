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

        public async Task<DynamoTableUsers> GetUsers(string customerId)
        {
            var queryRequest = RequestBuilder(customerId);

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
                Id = result["id"].S,
                CustomerEmail = result["customerEmail"].S,
                CustomerName = result["customerName"].S,
                Plan = result["customerPlan"].S,
            };
        }

        private async Task<ScanResponse> ScanAsync(ScanRequest request)
        
        {
            ScanResponse response = new ScanResponse();
         
                response = await _dynamoClient.ScanAsync(request);
             
          
            return response;

        }

        private ScanRequest RequestBuilder(string id)
        {
            if (id == null || id=="")
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
                        ":v_Id", new AttributeValue { S = id}}

                },
                FilterExpression = "id = :v_Id",
                ProjectionExpression = "id, customerEmail, customerName, customerPlan"
            };
        }

    }
}
