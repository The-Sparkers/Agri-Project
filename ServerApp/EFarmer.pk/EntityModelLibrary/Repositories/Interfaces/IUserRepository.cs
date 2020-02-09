using EFarmer.Models;
using EFarmer.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFarmerPkModelLibrary.Repositories
{
    public interface IUserRepository : IDisposable
    {
        Task AddToFavoritesAsync(User seller, User buyer);
        long Create(User model);
        bool Delete(long id);
        Task<List<User>> GetBuyersAsync();
        Task<List<User>> GetFavoriteBuyersAsync(User seller);
        User GetSeller(long id);
        Task<List<User>> GetSellersAsync();
        User GetUser(ContactNumberFormat contact);
        Task<List<User>> GetUsersAsync(City model);
        void MakeBuyer(User user);
        void MakeSeller(User user);
        User Read(long id);
        Task<List<User>> ReadAllAsync();
        bool Update(User model);
        void UpdateLocation(User user, GeoLocation location);
    }
}