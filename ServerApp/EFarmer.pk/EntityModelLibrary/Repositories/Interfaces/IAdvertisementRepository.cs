using EFarmer.Models;
using EFarmer.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFarmerPkModelLibrary.Repositories
{
    internal interface IAdvertisementRepository : IDisposable
    {
        long Create(Advertisement model);
        bool Delete(long id);
        bool Favorite(Advertisement ad, Buyer buyer);
        List<Advertisement> GetNearbyAdvertisements(Advertisement advertisement, GeoLocation location, double radiusInKm);
        Advertisement Read(long id);
        Task<List<Advertisement>> ReadAllAsync();
        bool Update(Advertisement model);
    }
}