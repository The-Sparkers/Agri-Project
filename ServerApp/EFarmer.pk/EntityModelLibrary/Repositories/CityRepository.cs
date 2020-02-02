using System.Collections.Generic;
using EFarmer.Connections;
using EFarmer.Models;
using EntityGrabber;

namespace EFarmerPkModelLibrary.Repositories
{
    internal class CityRepository:SQLConnection, IModelRepository<City,short>
    {
        private IDbConnection dbConnection;

        public CityRepository(IDbConnection dbConnection):base(dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public short Create(IDataModel<short> model)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(short id)
        {
            throw new System.NotImplementedException();
        }

        public City Read(short id)
        {
            throw new System.NotImplementedException();
        }

        public List<City> ReadAll()
        {
            throw new System.NotImplementedException();
        }

        public bool Update(IDataModel<short> model)
        {
            throw new System.NotImplementedException();
        }
    }
}