using EFarmer.Exceptions;
using EFarmer.Models;
using EFarmerPkModelLibrary.Entities;
using EntityGrabber;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace EFarmerPkModelLibrary.Repositories
{
    public class CategoryRepository : ModelRepository<Category, short>, IDisposable
    {
        Context.EFarmerDbModel dbContext;
        readonly DbSet<CATEGORY> categories;
        public CategoryRepository(IDbConnection connectionString) : base(connectionString)
        {
            dbContext = new Context.EFarmerDbModel(connectionString.GetConnectionString());
            categories = dbContext.CATEGORIES;
        }

        public override short Create(Category model)
        {
            var result=categories.Add(new CATEGORY
            {
                Name = model.Name,
                UName = model.UrduName
            });

            try
            {
                return (dbContext.SaveChanges()>0)?result.Entity.Id:(short)0;
            }
            catch (DbUpdateException ex)
            {
                throw new DbQueryProcessingFailedException("Category->Create", (SqlException)ex.InnerException);
            }
        }

        public override bool Delete(short id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            ((IDisposable)dbContext).Dispose();
        }

        public override EFarmer.Models.Category Read(short id)
        {
            throw new NotImplementedException();
        }

        public override async Task<List<EFarmer.Models.Category>> ReadAllAsync()
        {
            throw new NotImplementedException();
        }

        public override bool Update(EFarmer.Models.Category model)
        {
            throw new NotImplementedException();
        }
    }
}
