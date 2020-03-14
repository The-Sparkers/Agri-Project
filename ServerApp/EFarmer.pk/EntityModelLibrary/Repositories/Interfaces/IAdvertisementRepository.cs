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
        bool Favorite(Advertisement ad, User buyer);
        Task<List<Advertisement>> GetAdvertisementsAsync(City city, int startRow = 1, int endRow = int.MaxValue);
        Task<List<Advertisement>> GetAdvertisementsRelatedToItemsAsync(AgroItem item, int max = int.MaxValue);
        Task<List<Advertisement>> GetFavoriteAdvertisementsAsync(User buyer);
        List<Advertisement> GetNearbyAdvertisements(GeoLocation location, double radiusInKm);
        Task<List<Advertisement>> GetPostedAdvertismentsAsync(DateTime startDate, DateTime endDate, User seller, int startRow = 1, int endRow = int.MaxValue);
        Advertisement Read(long id);
        Task<List<Advertisement>> ReadAllAsync();
        bool Update(Advertisement model);
        Task<List<Advertisement>> ReadRowsAsync(int startRow, int endRow = int.MaxValue);
        List<Advertisement> GetNearbyAdvertisements(GeoLocation location, double radiusInKm, int startRow, int endRow = int.MaxValue);
        Task<List<Advertisement>> GetAdvertisementsByCategoryAsync(Category category, int startRow = 1, int endRow = int.MaxValue);
    }
}