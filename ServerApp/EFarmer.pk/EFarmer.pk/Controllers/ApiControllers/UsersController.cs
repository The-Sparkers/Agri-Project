using Autofac;
using EFarmer.Models;
using EFarmer.pk.ApiModels;
using EFarmer.pk.Models;
using EFarmerPkModelLibrary.Factories;
using EFarmerPkModelLibrary.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFarmer.pk.Controllers.ApiControllers
{
    /// <summary>
    /// Controls the requests and responses for users
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IContainer container;
        private RepositoryFactory repositoryFactory;
        /// <summary>
        /// Initializer
        /// </summary>
        public UsersController()
        {
            repositoryFactory = new ModelsFactory();
            container = repositoryFactory.Build();
        }
        /// <summary>
        /// Returns a list of registered buyers
        /// </summary>
        /// <returns></returns>
        [HttpGet("Buyers", Name = "GetBuyers")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<User>>> GetBuyers()
        {
            try
            {
                List<User> buyers = new List<User>();
                using (var scope = container.BeginLifetimeScope())
                {
                    using (var userFactory = scope.Resolve<IUserRepository>())
                    {
                        buyers = await userFactory.GetBuyersAsync();
                    }
                }
                return Ok(buyers);
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
        [HttpGet("Sellers", Name = "GetSellers")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<User>>> GetSellers()
        {
            try
            {
                List<User> sellers = new List<User>();
                using (var scope = container.BeginLifetimeScope())
                {
                    using (var userFactory = scope.Resolve<IUserRepository>())
                    {
                        sellers = await userFactory.GetSellersAsync();
                    }
                }
                return Ok(sellers);
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
                User user = null;
                using (var scope = container.BeginLifetimeScope())
                {
                    using (var userRepository = scope.Resolve<IUserRepository>())
                    {
                        user = userRepository.Read(id);
                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Returns a user by its contact number
        /// </summary>
        /// <param name="countryCode"></param>
        /// <param name="companyCode"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpGet("GetByContactNumber", Name = "GetUserByContactNumber")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<User> GetByContactNumber(string countryCode, string companyCode, string phone)
        {
            Common.ContactNumberFormat contactNumber = new Common.ContactNumberFormat(countryCode, companyCode, phone);
            try
            {
                User user = null;
                using (var scope = container.BeginLifetimeScope())
                {
                    using (var userRepository = scope.Resolve<IUserRepository>())
                    {
                        user = userRepository.GetUser(new EFarmer.Models.Helpers.ContactNumberFormat(contactNumber.CountryCode, contactNumber.CompanyCode, contactNumber.PhoneNumber));
                    }
                }
                return user;
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
        [HttpPost("", Name = "PostUser")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<User> PostUser([FromBody] UserApiModel user)
        {
            try
            {
                User _user = new User
                {
                    Address = user.Address,
                    City = new City
                    {
                        Id = user.CityId
                    },
                    ContactNumber = new EFarmer.Models.Helpers.ContactNumberFormat(user.CountryCode, user.CompanyCode, user.Phone),
                    Location = user.Location,
                    Name = user.Name
                };
                using (var scope = container.BeginLifetimeScope())
                {
                    using (var repository = scope.Resolve<IUserRepository>())
                    {
                        _user = repository.Read(repository.Create(_user));
                    }
                }
                return _user;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">User to be updated</param>
        // PUT: api/Users/5
        [HttpPut("Update", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put([FromBody] UserApiModel user)
        {
            try
            {
                User _user = new User
                {
                    Address = user.Address,
                    Name = user.Name,
                    Location = user.Location,
                    City = new City
                    {
                        Id = user.CityId
                    },
                    Id = user.Id,
                    ContactNumber = new EFarmer.Models.Helpers.ContactNumberFormat(user.CountryCode, user.CompanyCode, user.Phone)
                };
                using (var scope = container.BeginLifetimeScope())
                {
                    using (var repository = scope.Resolve<IUserRepository>())
                    {
                        repository.Update(_user);
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Deletes this user
        /// </summary>
        /// <param name="id"></param>
        // DELETE: api/Delete/5
        [HttpDelete("Delete/{id}", Name = "DeleteUser")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<bool> Delete(long id)
        {
            try
            {
                bool flag = false;
                using (var scope = container.BeginLifetimeScope())
                {
                    using (var repository = container.Resolve<IUserRepository>())
                    {
                        flag = repository.Delete(id);
                    }
                }
                return flag;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
