using EFarmer.Exceptions;
using EFarmer.Models;
using EFarmerPkModelLibrary.Entities;
using EntityGrabber;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Linq;

namespace EFarmerPkModelLibrary.Repositories
{
    internal class AgroItemRepository : ModelRepository<AgroItem, int>, IAgroItemRepository
    {
        private Context.EFarmerDbModel dbContext;
        private readonly DbSet<AGROITEM> agroItems;
        public AgroItemRepository(IDbConnection connectionString) : base(connectionString)
        {
            dbContext = new Context.EFarmerDbModel(connectionString.GetConnectionString());
            agroItems = dbContext.AGROITEMs;
        }

        public override int Create(AgroItem model)
        {
            try
            {
                var result = agroItems.Add(new AGROITEM
                {
                    CATEGORY = dbContext.Find<CATEGORY>(model.Category.Id),
                    Name = model.Name,
                    Uname = model.UrduName,
                    UWeightScale = model.UrduWeightScale,
                    WeightScale = model.WeightScale
                });
                dbContext.SaveChanges();
                return result.Entity.Id;
            }
            catch (Exception ex)
            {
                if (ex.InnerException.GetType() == typeof(SqlException))
                {
                    throw new DbQueryProcessingFailedException("AgroItem->Create", (SqlException)ex.InnerException);
                }
                throw;
            }
        }

        public override bool Delete(int id)
        {
            agroItems.Remove(agroItems.Find(id));
            var result = dbContext.SaveChanges();
            return (result > 0);
        }

        public void Dispose()
        {
            ((IDisposable)dbContext).Dispose();
        }

        public override AgroItem Read(int id)
        {
            var item = agroItems.Find(id);
            return (item != null) ? AgroItem.Convert(item) : null;
        }

        public override async Task<List<AgroItem>> ReadAllAsync()
        {
            List<AgroItem> lstAgroItems = new List<AgroItem>();
            await agroItems.ForEachAsync(x => lstAgroItems.Add(AgroItem.Convert(x)));
            return lstAgroItems;
        }

        public override bool Update(AgroItem model)
        {
            agroItems.Update(new AGROITEM
            {
                Id = model.Id,
                Name = model.Name,
                Uname = model.UrduName,
                UWeightScale = model.UrduWeightScale,
                CATEGORY = dbContext.Find<CATEGORY>(model.Category.Id),
                WeightScale = model.WeightScale
            });
            var result = dbContext.SaveChanges();
            return (result > 0);
        }
        /// <summary>
        /// Returns the ads in which this item is used
        /// </summary>
        /// <returns></returns>
        public async Task<List<Advertisement>> GetAdvertisementsRelatedToItemsAsync(AgroItem item,int max = int.MaxValue)
        {
            List<Advertisement> advertisements = new List<Advertisement>();
            List<Task<Advertisement>> _tAdvertisements = new List<Task<Advertisement>>();
            agroItems.Find(item.Id).ADVERTISEMENTs_ItemId
                .ToList()
                .ForEach(x => _tAdvertisements.Add(Task.Run(() => Advertisement.Convert(x))));
            var _tResults = await Task.WhenAll(_tAdvertisements);
            advertisements = _tResults.ToList();
            return advertisements;
        }
    }
}
