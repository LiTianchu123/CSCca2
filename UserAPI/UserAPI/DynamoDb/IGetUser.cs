using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserAPI.Models;

namespace UserAPI.DynamoDb
{
    public interface IGetUser
    {
        Task<DynamoTableUsers> GetUsers(string customerId);
    }
}
