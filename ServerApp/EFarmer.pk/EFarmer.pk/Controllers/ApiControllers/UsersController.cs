using EFarmer.pk.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EFarmer.pk.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET: api/Users
        [HttpGet(Name = "GetBuyers")]
        public IEnumerable<Buyer> GetBuyers()
        {
            return Buyer.GetBuyers();
        }
        [HttpGet(Name = "GetSellers")]
        public IEnumerable<Seller> GetSellers()
        {
            return Seller.GetSellers();
        }
        
        // GET: api/Users/5
        [HttpGet("{id}", Name = "Get")]
        
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Users
        [HttpPost]
        public void Post([FromBody] string value)
        {
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
