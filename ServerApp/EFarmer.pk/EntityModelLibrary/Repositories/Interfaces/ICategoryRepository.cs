using EFarmer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFarmerPkModelLibrary.Repositories
{
    public interface ICategoryRepository : IDisposable
    {
        short Create(Category model);
        bool Delete(short id);
        Task<List<AgroItem>> GetRelatedAgroItemsAsync(Category category);
        Category Read(short id);
        Task<List<Category>> ReadAllAsync();
        bool Update(Category model);
    }
}