using EFarmer.Models;
using EFarmer.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFarmerPkModelLibrary.Repositories
{
    public interface IAdvertisementRepository : IDisposable
    {
        long Create(Advertisement model);
        bool Delete(long id);
        bool Favorite(Advertisement ad, Buyer buyer);
        Task<List<Advertisement>> GetAdvertisementsAsync(City city);
        Task<List<Advertisement>> GetAdvertisementsRelatedToItemsAsync(AgroItem item, int max = int.MaxValue);
        Task<List<Advertisement>> GetFavoriteAdvertisementsAsync(Buyer buyer);
        List<Advertisement> GetNearbyAdvertisements(Advertisement advertisement, GeoLocation location, double radiusInKm);
        Task<List<Advertisement>> GetPostedAdvertismentsAsync(DateTime startDate, DateTime endDate, Seller seller);
        Advertisement Read(long id);
        Task<List<Advertisement>> ReadAllAsync();
        bool Update(Advertisement model);
    }
}