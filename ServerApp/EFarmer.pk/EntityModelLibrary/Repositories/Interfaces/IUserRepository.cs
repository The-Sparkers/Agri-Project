using EFarmer.Models;
using EFarmer.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFarmerPkModelLibrary.Repositories
{
    public interface IUserRepository : IDisposable
    {
        Task AddToFavoritesAsync(Seller seller, Buyer buyer);
        long Create(User model);
        bool Delete(long id);
        Task<List<Buyer>> GetBuyersAsync();
        Task<List<Advertisement>> GetFavoriteAdvertisementsAsync(Buyer buyer);
        Task<List<Buyer>> GetFavoriteBuyersAsync(Seller seller);
        Task<List<AgroItem>> GetInterestedItemsAsync(Buyer buyer);
        Task<List<Advertisement>> GetPostedAdvertismentsAsync(DateTime startDate, DateTime endDate, Seller seller);
        User GetSeller(long id);
        Task<List<Seller>> GetSellersAsync();
        User GetUser(ContactNumberFormat contact);
        void MakeBuyer(User user);
        void MakeSeller(User user);
        Advertisement PostAdvertisement(Seller seller, short quality, short quantity, DateTime dateTime, decimal price, AgroItem item, City city, string picture = "");
        User Read(long id);
        Task<List<User>> ReadAllAsync();
        bool Update(User model);
    }
}