using EFarmer.Models;
using EntityGrabber;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EFarmerPkModelLibrary.Repositories
{
    public class AgroItemRepository : ModelRepository<AgroItem, int>
    {
        public AgroItemRepository(IDbConnection connectionString) : base(connectionString)
        {
        }

        public override int Create(AgroItem model)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override AgroItem Read(int id)
        {
            throw new NotImplementedException();
        }

        public override Task<List<AgroItem>> ReadAllAsync()
        {
            throw new NotImplementedException();
        }

        public override bool Update(AgroItem model)
        {
            throw new NotImplementedException();
        }
    }
}
