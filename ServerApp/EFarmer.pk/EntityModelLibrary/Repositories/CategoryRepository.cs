using EFarmer.Exceptions;
using EFarmer.Models;
using EFarmerPkModelLibrary.Entities;
using EntityGrabber;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EFarmerPkModelLibrary.Repositories
{
    internal class CategoryRepository : ModelRepository<Category, short>, ICategoryRepository
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
            var result = categories.Add(new CATEGORY
            {
                Name = model.Name,
                UName = model.UrduName
            });

            try
            {
                return (dbContext.SaveChanges() > 0) ? result.Entity.Id : (short)0;
            }
            catch (DbUpdateException ex)
            {
                throw new DbQueryProcessingFailedException("Category->Create", (SqlException)ex.InnerException);
            }
        }

        public override bool Delete(short id)
        {
            try
            {
                categories.Remove(categories.Find(id));
                var result = dbContext.SaveChanges();
                return (result > 0);

            }
            catch (Exception ex)
            {
                if (ex.InnerException.GetType() == typeof(SqlException))
                {
                    throw new DbQueryProcessingFailedException("Catgory->delete", (SqlException)ex.InnerException);
                }
                throw;
            }
        }

        public void Dispose()
        {
            ((IDisposable)dbContext).Dispose();
        }

        public override Category Read(short id)
        {
            var category = categories.Find(id);
            return (category != null) ? Category.Convert(category) : null;
        }

        public override async Task<List<Category>> ReadAllAsync()
        {
            List<Category> lstCategories = new List<Category>();
            await categories.ForEachAsync(x => lstCategories.Add(Category.Convert(x)));
            return lstCategories;
        }

        public override bool Update(Category model)
        {
            try
            {
                var category = categories.Find(model.Id);
                category.Name = model.Name;
                category.UName = model.UrduName;
                categories.Update(category);
                var result = dbContext.SaveChanges();
                return (result > 0);
            }
            catch (Exception ex)
            {
                if (ex.InnerException.GetType() == typeof(SqlException))
                {
                    throw new DbQueryProcessingFailedException("Category->Update", (SqlException)ex.InnerException);
                }
                throw;
            }
        }
        
    }
}
