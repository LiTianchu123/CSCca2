using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UserAPI.DynamoDb
{
    public interface IAddUser
    {
        Task AddNewEntry(string customerId, string email, string name, string subscription);
    }
}
