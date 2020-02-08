using System.Collections.Generic;
using System.Threading.Tasks;
using EFarmer.Models;

namespace EFarmerPkModelLibrary.Repositories
{
    public interface ICityRepository
    {
        short Create(City model);
        bool Delete(short id);
        Task<List<Advertisement>> GetAdvertisementsAsync(City city);
        Task<List<User>> GetUsers(City model);
        City Read(short id);
        Task<List<City>> ReadAllAsync();
        bool Update(City model);
    }
}