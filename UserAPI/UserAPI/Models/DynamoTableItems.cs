using System.Collections.Generic;

namespace UserAPI.Models
{
    public class DynamoTableUsers
    {
        public IEnumerable<User> Users { get; set; }
    }
}