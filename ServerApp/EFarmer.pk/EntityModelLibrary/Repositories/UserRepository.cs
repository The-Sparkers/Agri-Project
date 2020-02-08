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
            await Task.Run(() => users.ForEachAsync(x => lstUsers.Add(
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
                 })));
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
        /// Returns a list for advertisments favorited by this user
        /// </summary>
        /// <returns></returns>
        public async Task<List<Advertisement>> GetFavoriteAdvertisementsAsync(Buyer buyer)
        {
            List<Advertisement> advertisements = new List<Advertisement>();
            await Task.Run(() => dbContext.BUYERADDSDIFFERENTADSTOFAVs
                .Where(x => x.Buyer.Id == buyer.Id)
                .Select(x => x.ADVERTISEMENT)
                .ForEachAsync(x => advertisements.Add(new Advertisement
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
                })));
            return advertisements;
        }
        /// <summary>
        /// Returns a list of items interested by this user
        /// </summary>
        /// <returns></returns>
        public async Task<List<AgroItem>> GetInterestedItemsAsync(Buyer buyer)
        {
            List<AgroItem> agroItems = new List<EFarmer.Models.AgroItem>();
            await Task.Run(() => dbContext.BUYERSADDAGROITEMTOINTERESTs
                    .Where(x => x.User.Id == buyer.Id)
                    .Select(x => x.AGROITEM)
                    .ForEachAsync(x => agroItems.Add(new AgroItem
                    {
                        Category = Category.Convert(x.CATEGORY),
                        Id = x.Id,
                        Name = x.Name,
                        UrduName = x.Uname,
                        UrduWeightScale = x.UWeightScale,
                        WeightScale = x.WeightScale
                    })));
            return agroItems;
        }
        /// <summary>
        /// Static method to get buyers from database
        /// </summary>
        /// <returns></returns>
        public async Task<List<Buyer>> GetBuyersAsync()
        {
            List<Buyer> buyers = new List<Buyer>();
            await Task.Run(() => users.Where(x => x.BuyerFlag)
            .ForEachAsync(x => buyers.Add((Buyer)User.Convert(x))));
            return buyers;
        }
        /// <summary>
        /// Method which adds a buyer to the interest list of this user
        /// </summary>
        /// <param name="buyer"></param>

        public async Task AddToFavoritesAsync(EFarmer.Models.Seller seller, EFarmer.Models.Buyer buyer)
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
        public async Task<List<Buyer>> GetFavoriteBuyersAsync(Seller seller)
        {
            List<Buyer> buyers = new List<Buyer>();
            await dbContext.SELLERSFAVORITESBUYERs
                .Where(x => x.USER_SellerId.Id == seller.Id)
                .Select(x => x.USER_BuyerId)
                .ForEachAsync(x => buyers.Add((Buyer)User.Convert(x)));
            return buyers;
        }
        /// <summary>
        /// Method which seller uses to post a new advertisement
        /// </summary>
        /// <param name="quality">mainly from 1-3</param>
        /// <param name="quantity"></param>
        /// <param name="dateTime"></param>
        /// <param name="price"></param>
        /// <param name="item"></param>
        /// <param name="city"></param>
        /// <param name="picture"></param>
        /// <returns></returns>
        public Advertisement PostAdvertisement(Seller seller, short quality, short quantity, DateTime dateTime, decimal price, EFarmer.Models.AgroItem item, EFarmer.Models.City city, string picture = "")
        {
            try
            {
                var advertisement = dbContext.ADVERTISEMENTs.Find((long)dbContext.AddNewAdvertisement(quality, quality, dateTime, price, picture, seller.Id, item.Id, city.Id).FirstOrDefault().Column0.Value);
                return new Advertisement
                {
                    City = City.Convert(advertisement.City),
                    Id = advertisement.Id,
                    Item = AgroItem.Convert(advertisement.AgroItem),
                    Picture = advertisement.Picture,
                    PostedDateTime = advertisement.PostedDateTime,
                    Price = advertisement.Price,
                    Quality = advertisement.Quality,
                    Quantity = advertisement.Quantity,
                    Seller = (Seller)User.Convert(advertisement.Seller)
                };
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(SqlException))
                {
                    throw new DbQueryProcessingFailedException("User->PostAdvertisement", (SqlException)ex.InnerException);
                }
                throw;
            }
        }
        /// <summary>
        /// Method to get a list of advertisments posted by this user
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<List<Advertisement>> GetPostedAdvertismentsAsync(DateTime startDate, DateTime endDate, Seller seller)
        {
            List<Advertisement> advertisements = new List<Advertisement>();
            await Task.Run(()=>dbContext.ADVERTISEMENTs
                .Where(x => x.Seller.Id == seller.Id)
                .ForEachAsync(x => advertisements.Add(Advertisement.Convert(x))));
            return advertisements;
        }
        /// <summary>
        /// Static Method to get all sellers present into the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<Seller>> GetSellersAsync()
        {
            List<Seller> sellers = new List<Seller>();
            List<Task<Seller>> _tSellers = new List<Task<Seller>>();
            await users.Where(x => x.SellerFlag)
                .ForEachAsync(x => sellers.Add((Seller)User.Convert(x)));
            return sellers;
        }
        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
