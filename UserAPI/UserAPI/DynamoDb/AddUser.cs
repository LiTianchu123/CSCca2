using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace UserAPI.DynamoDb
{
    public class AddUser : IAddUser
    {
	    private readonly IAmazonDynamoDB _dynamoClient;

	    public AddUser(IAmazonDynamoDB dynamoClient)
	    {
		    _dynamoClient = dynamoClient;
	    }

	    public async Task AddNewEntry(int id, string customerId, string email, string name, string subscription)
	    {
		    var queryRequest = RequestBuilder(id, customerId, email,name,subscription);

		    await AddUserAsync(queryRequest);
	    }

        private PutItemRequest RequestBuilder(int id, string customerId, string email, string name, string subscription)
	    {
		    var item = new Dictionary<string, AttributeValue>
		    {
			    {"id", new AttributeValue {N = id.ToString()}},
			    {"customerId", new AttributeValue {S = customerId}},
                {"customerEmail", new AttributeValue {S = email}},
				{"customerName", new AttributeValue {S = name}},
				{"subscription", new AttributeValue {S = subscription}}
			};

		    return new PutItemRequest
		    {
			    TableName = "Users",
			    Item = item
		    };
	    }

	    private async Task AddUserAsync(PutItemRequest request)
	    {
		    await _dynamoClient.PutItemAsync(request);
	    }
    }
}
