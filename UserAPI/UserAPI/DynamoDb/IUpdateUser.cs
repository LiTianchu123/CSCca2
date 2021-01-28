using System.Threading.Tasks;
using UserAPI.Models;

namespace UserAPI.DynamoDb
{
    public interface IUpdateUser
    {
        Task<User> Update(int id, string subscription);
    }
}