using EFarmer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFarmerPkModelLibrary.Repositories
{
    public interface IAgroItemRepository : IDisposable
    {
        int Create(AgroItem model);
        bool Delete(int id);
        Task<List<Advertisement>> GetAdvertisementsRelatedToItemsAsync(AgroItem item, int max = int.MaxValue);
        AgroItem Read(int id);
        Task<List<AgroItem>> ReadAllAsync();
        bool Update(AgroItem model);
    }
}