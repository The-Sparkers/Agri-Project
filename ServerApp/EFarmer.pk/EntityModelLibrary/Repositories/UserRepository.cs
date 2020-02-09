using EFarmer.Exceptions;
using EFarmer.Models;
using EFarmer.Models.Helpers;
using EFarmerPkModelLibrary.Entities;
using EntityGrabber;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EFarmerPkModelLibrary.Repositories
{
    internal class UserRepository : ModelRepository<User, long>, IUserRepository
    {
        readonly IDbConnection dbConnection;
        readonly Context.EFarmerDbModel dbContext;
        readonly DbSet<USER> users;
        public UserRepository(IDbConnection connectionString) : base(connectionString)
        {
            dbConnection = connectionString;
            dbContext = new Context.EFarmerDbModel(connectionString.GetConnectionString());
            users = dbContext.USERs;
        }

        public User GetUser(ContactNumberFormat contact)
        {
            var user = users
                .Where(x => x.CCountryCode == contact.CountryCode
                && x.CCompanyCode == contact.CompanyCode
                && x.CPhone == contact.PhoneNumber).First() ?? null;
            return (user != null) ? new EFarmer.Models.User
            {
                Address = user.Address,
                City = EFarmer.Models.City.Convert(user.City),
                ContactNumber = new ContactNumberFormat(user.CCountryCode, user.CCompanyCode, user.CPhone),
                IsBuyer = user.BuyerFlag,
                IsSeller = user.SellerFlag,
                Location = new GeoLocation { Latitude = user.GLat, Longitude = user.GLng },
                Name = new NameFormat { FirstName = user.FName, LastName = user.LName },
                Id = user.Id
            } : null;
        }
        public override long Create(User model)
        {
            var result = users.Add(new USER
            {
                Address = model.Address,
                BuyerFlag = model.IsBuyer,
                CCompanyCode = model.ContactNumber.CompanyCode,
                CCountryCode = model.ContactNumber.CountryCode,
                CPhone = model.ContactNumber.PhoneNumber,
                City = dbContext.CITIES.Find(model.City.Id),
                LName = model.Name.LastName,
                SellerFlag = model.IsSeller,
                FName = model.Name.FirstName,
                GLat = (model.Location != null) ? model.Location.Latitude : 0,
                GLng = (model.Location != null) ? model.Location.Longitude : 0
            });
            try
            {
                dbContext.SaveChanges();
                return result.Entity.Id;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.GetType() == typeof(SqlException))
                {
                    var sqlException = (SqlException)ex.InnerException;
                    if (sqlException.Number == 2601 || sqlException.Number == 2627)
                    {
                        //Unique key handler
                        //returns the already created driver in the System
                        var user = GetUser(model.ContactNumber);
                        return user.Id;
                    }
                }
                throw;
            }
        }

        public override bool Delete(long id)
        {
            return false;
        }

        public override User Read(long id)
        {
            var user = users.Find(id);
            return (user != null) ? new EFarmer.Models.User
            {
                Address = user.Address,
                City = EFarmer.Models.City.Convert(user.City),
                ContactNumber = new ContactNumberFormat(user.CCountryCode, user.CCompanyCode, user.CPhone),
                IsBuyer = user.BuyerFlag,
                IsSeller = user.SellerFlag,
                Location = new GeoLocation { Latitude = user.GLat, Longitude = user.GLng },
                Name = new NameFormat { FirstName = user.FName, LastName = user.LName },
                Id = user.Id
            } : null;
        }

        public override async Task<List<User>> ReadAllAsync()
        {
            List<User> lstUsers = new List<User>();
            await users.ForEachAsync(x => lstUsers.Add(
                 new User
                 {
                     Address = x.Address,
                     City = EFarmer.Models.City.Convert(x.City),
                     ContactNumber = new ContactNumberFormat(x.CCountryCode, x.CCompanyCode, x.CPhone),
                     IsBuyer = x.BuyerFlag,
                     IsSeller = x.SellerFlag,
                     Location = new GeoLocation { Latitude = x.GLat, Longitude = x.GLng },
                     Name = new NameFormat { FirstName = x.FName, LastName = x.LName },
                     Id = x.Id
                 }));
            return lstUsers;
        }

        public override bool Update(User model)
        {
            try
            {
                var result = users.Update(new USER
                {
                    Address = model.Address,
                    BuyerFlag = model.IsBuyer,
                    SellerFlag = model.IsSeller,
                    CCompanyCode = model.ContactNumber.CompanyCode,
                    CCountryCode = model.ContactNumber.CountryCode,
                    CPhone = model.ContactNumber.PhoneNumber,
                    City = dbContext.CITIES.Find(model.City.Id),
                    LName = model.Name.LastName,
                    FName = model.Name.FirstName,
                    GLat = model.Location.Latitude,
                    GLng = model.Location.Longitude,
                    Id = model.Id
                });
                dbContext.SaveChanges();
                return (result.State == EntityState.Modified) ? true : false;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.GetType() == typeof(SqlException))
                {
                    var sqlException = (SqlException)ex.InnerException;
                    if (sqlException.Number == 2601 || sqlException.Number == 2627)
                    {
                        //Unique key handler
                        //returns the already created driver in the System
                        throw new UniqueKeyViolationException("The contact number is already added to the database");
                    }
                }
                throw;
            }
        }
        /// <summary>
        /// Method to Make this user a buyer
        /// </summary>
        public void MakeBuyer(User user)
        {
            var u = users.Find(user.Id);
            u.BuyerFlag = true;
            users.Update(u);
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                new UpdateUnsuccessfulException("User->MakeBuyer");
            }
        }
        /// <summary>
        /// Method to make this user a seller
        /// </summary>
        public void MakeSeller(User user)
        {
            var u = users.Find(user.Id);
            u.SellerFlag = true;
            users.Update(u);
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                new UpdateUnsuccessfulException("User->MakeSeller");
            }
        }
        public User GetSeller(long id)
        {
            var seller = users.FirstOrDefault(x => x.Id == id && x.SellerFlag);
            return new User
            {
                Address = seller.Address,
                City = City.Convert(seller.City),
                ContactNumber =
                new ContactNumberFormat(seller.CCountryCode, seller.CCompanyCode, seller.CPhone),
                Id = seller.Id,
                IsBuyer = seller.BuyerFlag,
                IsSeller = seller.SellerFlag,
                Location = new GeoLocation { Latitude = seller.GLat, Longitude = seller.GLng },
                Name = new NameFormat { FirstName = seller.FName, LastName = seller.LName }
            };
        }


        /// <summary>
        /// Static method to get buyers from database
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetBuyersAsync()
        {
            List<User> buyers = new List<User>();
            await Task.Run(() => users.Where(x => x.BuyerFlag)
            .ForEachAsync(x => buyers.Add((User)User.Convert(x))));
            return buyers;
        }
        /// <summary>
        /// Method which adds a buyer to the interest list of this user
        /// </summary>
        /// <param name="buyer"></param>

        public async Task AddToFavoritesAsync(User seller, User buyer)
        {
            const string _PATH = "Seller->AddToInterest(buyer)";
            try
            {
                await dbContext.AddBuyerToInterestAsync(seller.Id, buyer.Id);
            }
            catch (Exception ex)
            {
                if (ex.InnerException.GetType() == typeof(SqlException))
                {
                    throw new DbQueryProcessingFailedException(_PATH, (SqlException)ex.InnerException);
                }

                throw;
            }
        }
        /// <summary>
        /// Mehtod to get list of buyers favorited by this user
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetFavoriteBuyersAsync(User seller)
        {
            List<User> buyers = new List<User>();
            await dbContext.SELLERSFAVORITESBUYERs
                .Where(x => x.USER_SellerId.Id == seller.Id)
                .Select(x => x.USER_BuyerId)
                .ForEachAsync(x => buyers.Add((User)User.Convert(x)));
            return buyers;
        }

        /// <summary>
        /// Static Method to get all sellers present into the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetSellersAsync()
        {
            List<User> sellers = new List<User>();
            List<Task<User>> _tSellers = new List<Task<User>>();
            await users.Where(x => x.SellerFlag)
                .ForEachAsync(x => sellers.Add((User)User.Convert(x)));
            return sellers;
        }
        /// <summary>
        /// Returns a list of Users belong to this city
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<List<User>> GetUsersAsync(City model)
        {
            var city = dbContext.CITIES.Find(model.Id);
            List<User> lstUsers = new List<User>();
            List<Task<User>> _tUsers = new List<Task<User>>();
            var users = city.Users;
            foreach (var item in users.Where(x => x.City.Id == model.Id).ToList())
            {
                _tUsers.Add(Task.Run(() => new User
                {
                    Address = item.Address,
                    City = City.Convert(city),
                    ContactNumber = new ContactNumberFormat(item.CCountryCode, item.CCompanyCode, item.CPhone),
                    Name = new NameFormat { FirstName = item.FName, LastName = item.LName },
                    Id = item.Id,
                    IsBuyer = item.BuyerFlag,
                    IsSeller = item.SellerFlag,
                    Location = new GeoLocation { Latitude = item.GLat, Longitude = item.GLng }

                }));
            }
            var _tResults = await Task.WhenAll(_tUsers);
            lstUsers = _tResults.ToList();
            return lstUsers;
        }
        public void UpdateLocation(User user, GeoLocation location)
        {
            dbContext.UpdateUserLocation(user.Id, location.Latitude, location.Longitude);
        }
        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
