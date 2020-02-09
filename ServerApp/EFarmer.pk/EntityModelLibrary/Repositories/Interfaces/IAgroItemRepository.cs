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
        void Dispose();
        Task<List<AgroItem>> GetAgroItemsByCategoryAsync(Category category);
        Task<List<AgroItem>> GetInterestedItemsAsync(User buyer);
        AgroItem Read(int id);
        Task<List<AgroItem>> ReadAllAsync();
        bool Update(AgroItem model);
    }
}