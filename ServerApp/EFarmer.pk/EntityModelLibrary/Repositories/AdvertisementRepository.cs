using EFarmer.Models;
using EFarmer.Models.Helpers;
using EntityGrabber;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace EFarmerPkModelLibrary.Repositories
{
    internal class AdvertisementRepository : ModelRepository<Advertisement, long>, IAdvertisementRepository
    {
        private Context.EFarmerDbModel dbContext;
        private readonly DbSet<Entities.ADVERTISEMENT> advertisements;
        public AdvertisementRepository(IDbConnection connectionString) : base(connectionString)
        {
            dbContext = new Context.EFarmerDbModel(connectionString.GetConnectionString());
            advertisements = dbContext.ADVERTISEMENTs;
        }

        public override long Create(Advertisement model)
        {
            var result = advertisements.Add(new Entities.ADVERTISEMENT
            {
                AgroItem = dbContext.AGROITEMs.Find(model.Item.Id),
                City = dbContext.CITIES.Find(model.City.Id),
                Id = model.Id,
                Picture = model.Picture,
                PostedDateTime = model.PostedDateTime,
                Price = model.Price,
                Quality = model.Quality,
                Quantity = model.Quantity,
                Seller = dbContext.USERs.Find(model.Seller.Id)
            });
            dbContext.SaveChanges();
            return result.Entity.Id;
        }

        public override bool Delete(long id)
        {
            advertisements.Remove(advertisements.Find(id));
            var result = dbContext.SaveChanges();
            return (result > 0);
        }

        public override Advertisement Read(long id)
        {
            var advertisement = advertisements.Find(id);
            return (advertisement != null) ? Advertisement.Convert(advertisement) : null;
        }

        public override async Task<List<Advertisement>> ReadAllAsync()
        {
            List<Advertisement> lstAdvertisments = new List<Advertisement>();
            await advertisements.ForEachAsync(x => lstAdvertisments.Add(Advertisement.Convert(x)));
            return lstAdvertisments;
        }

        public override bool Update(Advertisement model)
        {
            return false;
        }
        /// <summary>
        /// Adds this advertsement to buyers favorites list
        /// </summary>
        /// <param name="buyer"></param>
        public bool Favorite(Advertisement ad, Buyer buyer)
        {
            try
            {
                dbContext.BUYERADDSDIFFERENTADSTOFAVs.Add(new Entities.BUYERADDSDIFFERENTADSTOFAV
                {
                    ADVERTISEMENT = advertisements.Find(ad.Id),
                    Buyer = dbContext.USERs.Find(buyer.Id)
                });
                var result = dbContext.SaveChanges();
                return (result > 0);
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Returns a list of advertisements near a certain location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public List<Advertisement> GetNearbyAdvertisements(Advertisement advertisement,GeoLocation location, double radiusInKm)
        {
            List<Advertisement> nearbyAdvertisements = new List<Advertisement>();
            List<GeoLocationDistanceAd> temp = new List<GeoLocationDistanceAd>();
            foreach (var item in advertisements.Take(advertisements.Count()))
            {
                var ad = Advertisement.Convert(item);
                var city = City.Convert(item.City);
                var distance = city.GeoLocation.DistanceFromAPoint(location); //in km
                if (distance < radiusInKm)
                {
                    temp.Add(new GeoLocationDistanceAd { DistanceFromOrigion = distance, Advertisement = ad });
                }
            }
            if (temp.Count > 0)
            {
                temp = temp.OrderBy(x => x.DistanceFromOrigion).ToList();
                foreach (var item in temp)
                {
                    nearbyAdvertisements.Add(item.Advertisement);
                }
            }
            return nearbyAdvertisements;
        }
        public void Dispose()
        {
            ((IDisposable)dbContext).Dispose();
        }

        private class GeoLocationDistanceAd
        {
            public double DistanceFromOrigion { get; internal set; }
            public Advertisement Advertisement { get; internal set; }
        }
    }
}
