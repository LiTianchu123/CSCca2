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

        public async Task<User> Update(string customerId, string subscription)
        {
            var response = await _getUser.GetUsers(customerId);

            var currentSubscription = response.Users.Select(p => p.Subscription).FirstOrDefault();
           

            var request = RequestBuilder(customerId, subscription, currentSubscription);

            var result = await UpdateItemAsync(request);

            return new User
            {
                Id = result.Attributes["id"].S,
                Subscription = result.Attributes["subscription"].S,
            };
        }

        private UpdateItemRequest RequestBuilder(string customerId, string subscription, string currentSubscription)
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
                    {"#S", "subscription"}
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {
                        ":newsubscription", new AttributeValue
                        {
                            S = subscription.ToString()
                        }
                    },
                    {
                        ":currsubscription", new AttributeValue
                        {
                            S = currentSubscription.ToString()
                        }
                    }
                },

                UpdateExpression = "SET #S = :newsubscription",
                ConditionExpression = "#S = :currsubscription",

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