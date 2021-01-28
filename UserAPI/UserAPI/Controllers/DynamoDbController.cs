using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAPI.DynamoDb;
using Microsoft.AspNetCore.Mvc;
using UserAPI.Models;

namespace UserAPI.Controllers
{
    [Route("api/DynamoDb")]
    public class DynamoDbController : Controller
    {
      //  private readonly ICreateTable _createTable;
        private readonly IAddUser _putItem;
        private readonly IGetUser _getItem;
        private readonly IUpdateUser _updateItem;
      //  private readonly IDeleteTable _deleteTable;

        public DynamoDbController(IAddUser putItem, IGetUser getItem, IUpdateUser updateItem)
        {
           // _createTable = createTable;
            _putItem = putItem;
            _getItem = getItem;
            _updateItem = updateItem;
           // _deleteTable = deleteTable;
        }
     
        [HttpPost]
        [Route("AddUser")]
        public IActionResult AddUser([FromQuery] int id, string customerId, string email, string name, string subscription)
        {
            //try
            //{
                _putItem.AddNewEntry(id, customerId, email, name, subscription);
            //}
            //catch(Exception e) {
            //    BadRequest(e.Message);
            //}

            return Ok();
        }

        [HttpGet]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser([FromQuery] int? id)
        {
            DynamoTableUsers response = new DynamoTableUsers();
            try
            {
                response = await _getItem.GetUsers(id);


            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(response);
        }

        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromQuery] int id, string subscription)
        {
            User response = new User();
            //try
            //{
                 response = await _updateItem.Update(id, subscription);
            //}
            //catch (Exception e) {
            //    BadRequest(e.Message);
            //}
          

            return Ok(response);
        }

        //[HttpDelete]
        //[Route("delete")]
        //public async Task<IActionResult> DeleteTable([FromQuery] string tableName)
        //{
        //    var response = await _deleteTable.ExecuteTableDelete(tableName);

        //    return Ok(response);
        //}
    }
}
