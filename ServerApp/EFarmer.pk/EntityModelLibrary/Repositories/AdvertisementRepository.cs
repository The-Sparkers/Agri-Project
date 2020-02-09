using EFarmer.Models;
using EFarmer.Models.Helpers;
using EntityGrabber;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public bool Favorite(Advertisement ad, User buyer)
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
        public List<Advertisement> GetNearbyAdvertisements(Advertisement advertisement, GeoLocation location, double radiusInKm)
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
        /// <summary>
        /// Returns a list for advertisments favorited by this user
        /// </summary>
        /// <returns></returns>
        public async Task<List<Advertisement>> GetFavoriteAdvertisementsAsync(User buyer)
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
                    Seller = User.Convert(x.Seller)
                })));
            return advertisements;
        }
        /// <summary>
        /// Returns a list of advertisements associated to this city
        /// </summary>
        /// <returns></returns>
        public async Task<List<Advertisement>> GetAdvertisementsAsync(City city)
        {
            var _city = dbContext.CITIES.Find(city.Id);
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
                        Seller = User.Convert(x.Seller),
                        City = City.Convert(x.City),
                        Item = AgroItem.Convert(x.AgroItem)
                    }));
            return advertisements;
        }
        /// <summary>
        /// Method to get a list of advertisments posted by this user
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<List<Advertisement>> GetPostedAdvertismentsAsync(DateTime startDate, DateTime endDate, User seller)
        {
            List<Advertisement> advertisements = new List<Advertisement>();
            await Task.Run(() => dbContext.ADVERTISEMENTs
                .Where(x => x.Seller.Id == seller.Id)
                .ForEachAsync(x => advertisements.Add(Advertisement.Convert(x))));
            return advertisements;
        }

        /// <summary>
        /// Returns the ads in which this item is used
        /// </summary>
        /// <returns></returns>
        public async Task<List<Advertisement>> GetAdvertisementsRelatedToItemsAsync(AgroItem item, int max = int.MaxValue)
        {
            List<Advertisement> advertisements = new List<Advertisement>();
            List<Task<Advertisement>> _tAdvertisements = new List<Task<Advertisement>>();
            dbContext.AGROITEMs.Find(item.Id).ADVERTISEMENTs_ItemId
                .ToList()
                .ForEach(x => _tAdvertisements.Add(Task.Run(() => Advertisement.Convert(x))));
            var _tResults = await Task.WhenAll(_tAdvertisements);
            advertisements = _tResults.ToList();
            return advertisements;
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
