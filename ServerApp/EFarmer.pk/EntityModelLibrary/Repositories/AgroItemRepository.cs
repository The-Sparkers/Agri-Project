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
    internal class AgroItemRepository : ModelRepository<AgroItem, int>,IAgroItemRepository
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
        /// Returns a list of items interested by this user
        /// </summary>
        /// <returns></returns>
        public async Task<List<AgroItem>> GetInterestedItemsAsync(User buyer)
        {
            List<AgroItem> agroItems = new List<AgroItem>();
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
        public async Task<List<AgroItem>> GetAgroItemsByCategoryAsync(Category category)
        {
            List<AgroItem> lstAgroItems = new List<AgroItem>();
            List<Task<AgroItem>> _tAgroItems = new List<Task<AgroItem>>();
            foreach (var item in dbContext.CATEGORIES.Find(category.Id).AGROITEMS)
            {
                _tAgroItems.Add(Task.Run(() => AgroItem.Convert(item)));
            }
            var _tResults = await Task.WhenAll(_tAgroItems);
            lstAgroItems = _tResults.ToList();
            return lstAgroItems;
        }
    }
}
