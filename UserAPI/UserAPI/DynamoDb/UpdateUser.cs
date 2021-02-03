using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using UserAPI.Models;

namespace UserAPI.DynamoDb
{
    public class UpdateUser : IUpdateUser
    {
        private readonly IGetUser _getUser;
        private static readonly string tableName = "Users";
        private readonly IAmazonDynamoDB _dynamoDbClient;

        public UpdateUser(IGetUser getItem, IAmazonDynamoDB dynamoDbClient)
        {
            _getUser = getItem;
            _dynamoDbClient = dynamoDbClient;
        }

        public async Task<User> Update(string customerId, string plan)
        {
            var response = await _getUser.GetUsers(customerId);

            var currentplan = response.Users.Select(p => p.Plan).FirstOrDefault();
           

            var request = RequestBuilder(customerId, plan, currentplan);

            var result = await UpdateItemAsync(request);

            return new User
            {
                Id = result.Attributes["id"].S,
                Plan = result.Attributes["customerPlan"].S,
            };
        }

        private UpdateItemRequest RequestBuilder(string customerId, string plan, string currentplan)
        {
            var request = new UpdateItemRequest
            {
                Key = new Dictionary<string, AttributeValue>
                {
                    {
                        "id", new AttributeValue
                        {
                            S = customerId
                        }
                    }
                  
                },
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    {"#S", "customerPlan"}
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {
                        ":newcustomerPlan", new AttributeValue
                        {
                            S = plan.ToString()
                        }
                    },
                    {
                        ":currcustomerPlan", new AttributeValue
                        {
                            S = currentplan.ToString()
                        }
                    }
                },

                UpdateExpression = "SET #S = :newcustomerPlan",
                ConditionExpression = "#S = :currcustomerPlan",

                TableName = tableName,
                ReturnValues = "ALL_NEW"
            };

            return request;
        }
        private async Task<UpdateItemResponse> UpdateItemAsync(UpdateItemRequest request)
        {
            var response = await _dynamoDbClient.UpdateItemAsync(request);

            return response;
        }
    }
}