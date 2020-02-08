using EFarmer.Models;
using EFarmer.pk.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace EFarmer.pk.Controllers.ApiControllers
{
    /// <summary>
    /// Controls the requests and responses for users
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        /// <summary>
        /// Returns a list of registered buyers
        /// </summary>
        /// <returns></returns>
        [HttpGet("Buyers", Name ="GetBuyers")]
        [ProducesResponseType(typeof(Buyer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<Buyer>> GetBuyers()
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Returns a list of sellers in the system
        /// </summary>
        /// <returns></returns>
        [HttpGet("Sellers",Name ="GetSellers")]
        [ProducesResponseType(typeof(Seller), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<Seller>> GetSellers()
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Returns a user
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<User> Get(long id)
        {
            try
            {
                //var user = new User(id);
                return null;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="user">Data for User</param>
        [HttpPost("Buyer", Name ="PostBuyer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PostBuyer([FromBody] string user)
        {
            //try
            //{
            //    return Ok(new Buyer(,user.ContactNumber, user.Address, user.City))
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(ex.Message);
            //}
            return Ok();
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
