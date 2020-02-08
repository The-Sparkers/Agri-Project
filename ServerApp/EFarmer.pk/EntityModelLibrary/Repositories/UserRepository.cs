using EFarmer.Exceptions;
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
    public class UserRepository : ModelRepository<EFarmer.Models.User, long>, IDisposable
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

        public EFarmer.Models.User GetUser(ContactNumberFormat contact)
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
        public override long Create(EFarmer.Models.User model)
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

        public override EFarmer.Models.User Read(long id)
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

        public override async Task<List<EFarmer.Models.User>> ReadAllAsync()
        {
            List<EFarmer.Models.User> lstUsers = new List<EFarmer.Models.User>();
            await Task.Run(() => users.ForEachAsync(x => lstUsers.Add(
                 new EFarmer.Models.User
                 {
                     Address = x.Address,
                     City = EFarmer.Models.City.Convert(x.City),
                     ContactNumber = new ContactNumberFormat(x.CCountryCode, x.CCompanyCode, x.CPhone),
                     IsBuyer = x.BuyerFlag,
                     IsSeller = x.SellerFlag,
                     Location = new GeoLocation { Latitude = x.GLat, Longitude = x.GLng },
                     Name = new NameFormat { FirstName = x.FName, LastName = x.LName },
                     Id = x.Id
                 })));
            return lstUsers;
        }

        public override bool Update(EFarmer.Models.User model)
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
        public void MakeBuyer(EFarmer.Models.User user)
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
        public void MakeSeller(EFarmer.Models.User user)
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
        public EFarmer.Models.User GetSeller(long id)
        {
            var seller = users.FirstOrDefault(x => x.Id == id && x.SellerFlag);
            return new EFarmer.Models.User
            {
                Address = seller.Address,
                City = EFarmer.Models.City.Convert(seller.City),
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
        /// Returns a list for advertisments favorited by this user
        /// </summary>
        /// <returns></returns>
        public async Task<List<EFarmer.Models.Advertisement>> GetFavoriteAdvertisementsAsync(EFarmer.Models.Buyer buyer)
        {
            List<EFarmer.Models.Advertisement> advertisements = new List<EFarmer.Models.Advertisement>();
            await dbContext.BUYERADDSDIFFERENTADSTOFAVs
                .Where(x => x.Buyer.Id == buyer.Id)
                .Select(x => x.ADVERTISEMENT)
                .ForEachAsync(x => advertisements.Add(new EFarmer.Models.Advertisement
                {
                    City = EFarmer.Models.City.Convert(x.City),
                    Id = x.Id,
                    Item = EFarmer.Models.AgroItem.Convert(x.AgroItem),
                    Picture = x.Picture,
                    PostedDateTime = x.PostedDateTime,
                    Price = x.Price,
                    Quality = x.Quality,
                    Quantity = x.Quantity,
                    Seller = (EFarmer.Models.Seller)EFarmer.Models.User.Convert(x.Seller)
                }));
            return advertisements;
        }
        /// <summary>
        /// Returns a list of items interested by this user
        /// </summary>
        /// <returns></returns>
        public async Task<List<EFarmer.Models.AgroItem>> GetInterestedItemsAsync(EFarmer.Models.Buyer buyer)
        {
            List<EFarmer.Models.AgroItem> agroItems = new List<EFarmer.Models.AgroItem>();
            await dbContext.BUYERSADDAGROITEMTOINTERESTs
                    .Where(x => x.User.Id == buyer.Id)
                    .Select(x => x.AGROITEM)
                    .ForEachAsync(x => agroItems.Add(new EFarmer.Models.AgroItem
                    {
                        Category = EFarmer.Models.Category.Convert(x.CATEGORY),
                        Id = x.Id,
                        Name = x.Name,
                        UrduName = x.Uname,
                        UrduWeightScale = x.UWeightScale,
                        WeightScale = x.WeightScale
                    }));
            return agroItems;
        }
        /// <summary>
        /// Static method to get buyers from database
        /// </summary>
        /// <returns></returns>
        public async Task<List<EFarmer.Models.Buyer>> GetBuyersAsync()
        {
            List<EFarmer.Models.Buyer> buyers = new List<EFarmer.Models.Buyer>();
            await users.Where(x => x.BuyerFlag).ForEachAsync(x => buyers.Add(new EFarmer.Models.Buyer
            {
                Address = x.Address,
                City = EFarmer.Models.City.Convert(x.City),
                ContactNumber = new ContactNumberFormat(x.CCountryCode, x.CCompanyCode, x.CPhone),
                Id = x.Id,
                IsBuyer = x.BuyerFlag,
                IsSeller = x.SellerFlag,
                Location = new GeoLocation { Latitude = x.GLat, Longitude = x.GLng },
                Name = new NameFormat { FirstName = x.FName, LastName = x.LName }
            }));
            return buyers;
        }
        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
