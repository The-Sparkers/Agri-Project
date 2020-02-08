using EFarmer.Exceptions;
using EFarmer.Models;
using EntityGrabber;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EFarmerPkModelLibrary.Repositories
{
    internal class CityRepository : ModelRepository<City, short>, IDisposable, ICityRepository
    {
        Context.EFarmerDbModel dbContext;
        readonly DbSet<Entities.CITY> cities;
        public CityRepository(IDbConnection connectionString) : base(connectionString)
        {
            dbContext = new Context.EFarmerDbModel(connectionString.GetConnectionString());
            cities = dbContext.CITIES;
        }

        public override short Create(City model)
        {
            var city = cities.Add(new Entities.CITY
            {
                GLat = model.GeoLocation.Latitude ?? 0,
                GLng = model.GeoLocation.Longitude ?? 0,
                Name = model.Name
            });
            try
            {
                var result = dbContext.SaveChanges();
                if (result > 0 && city.IsKeySet)
                {
                    return city.Entity.Id;
                }
            }
            catch (Exception ex)
            {
                throw new DbQueryProcessingFailedException("City->Create", (SqlException)ex.InnerException);
            }
            return 0;
        }

        public override bool Delete(short id)
        {
            try
            {
                var city = cities.Find(id);
                cities.Remove(city);
                var result = dbContext.SaveChanges();
                return (result > 0) ? true : false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override City Read(short id)
        {
            var city = cities.Find(id);
            return (city != null) ? new City
            {
                Name = city.Name,
                GeoLocation = new EFarmer.Models.Helpers.GeoLocation { Latitude = city.GLat, Longitude = city.GLng },
                Id = city.Id
            } : null;
        }

        public override async Task<List<City>> ReadAllAsync()
        {
            List<City> lstCities = new List<City>();
            await cities.ForEachAsync(x => lstCities.Add(new City
            {
                GeoLocation = new EFarmer.Models.Helpers.GeoLocation { Latitude = x.GLat, Longitude = x.GLng },
                Id = x.Id,
                Name = x.Name
            }));
            return lstCities;
        }

        public override bool Update(City model)
        {
            cities.Update(new Entities.CITY
            {
                Id = model.Id,
                GLat = model.GeoLocation.Latitude ?? 0,
                GLng = model.GeoLocation.Longitude ?? 0,
                Name = model.Name
            });
            try
            {
                var result = dbContext.SaveChanges();
                return (result > 0) ? true : false;
            }
            catch (Exception)
            {
                throw new UpdateUnsuccessfulException("City->Update");
            }
        }
        /// <summary>
        /// Returns a list of Users belong to this city
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<List<User>> GetUsers(City model)
        {
            var city = cities.Find(model.Id);
            List<User> lstUsers = new List<User>();
            List<Task<User>> _tUsers = new List<Task<User>>();
            var users = city.Users;
            foreach (var item in users.Where(x => x.City.Id == model.Id).ToList())
            {
                _tUsers.Add(Task.Run(() => new User
                {
                    Address = item.Address,
                    City = City.Convert(city),
                    ContactNumber = new EFarmer.Models.Helpers.ContactNumberFormat(item.CCountryCode, item.CCompanyCode, item.CPhone),
                    Name = new EFarmer.Models.Helpers.NameFormat { FirstName = item.FName, LastName = item.LName },
                    Id = item.Id,
                    IsBuyer = item.BuyerFlag,
                    IsSeller = item.SellerFlag,
                    Location = new EFarmer.Models.Helpers.GeoLocation { Latitude = item.GLat, Longitude = item.GLng }

                }));
            }
            var _tResults = await Task.WhenAll(_tUsers);
            lstUsers = _tResults.ToList();
            return lstUsers;
        }
        /// <summary>
        /// Returns a list of advertisements associated to this city
        /// </summary>
        /// <returns></returns>
        public async Task<List<Advertisement>> GetAdvertisementsAsync(City city)
        {
            var _city = cities.Find(city.Id);
            List<Advertisement> advertisements = new List<Advertisement>();
            var ads = dbContext.ADVERTISEMENTs;
            await ads.Where(x => x.City.Id == city.Id).ForEachAsync(x => advertisements.Add(
                    new Advertisement
                    {
                        Id = x.Id,
                        Picture = x.Picture,
                        PostedDateTime = x.PostedDateTime,
                        Price = x.Price,
                        Quality = x.Quality,
                        Quantity = x.Quantity,
                        Seller = (Seller)User.Convert(x.Seller),
                        City = City.Convert(x.City),
                        Item = AgroItem.Convert(x.AgroItem)
                    }));
            return advertisements;
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}