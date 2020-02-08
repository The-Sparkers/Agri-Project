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
        Category Read(short id);
        Task<List<Category>> ReadAllAsync();
        bool Update(Category model);
    }
}