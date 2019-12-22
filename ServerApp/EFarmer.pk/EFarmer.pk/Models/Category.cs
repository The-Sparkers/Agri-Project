using EFarmer.pk.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EFarmer.pk.Models
{
    /// <summary>
    /// An items belongs to a category
    /// </summary>
    public class Category : SQLConnection
    {
        private readonly short id;
        private string name;
        private string uName;
        /// <summary>
        /// Initializes values from db using primary key
        /// </summary>
        /// <param name="id">Primary Key</param>
        public Category(short id)
        {
            this.id = id;
            StoredProcedureCommand.CommandText = "GetCategory";
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.SmallInt)).Value = id;
            Connection.Open();
            try
            {
                using (SqlDataReader reader = StoredProcedureCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        name = (string)reader["Name"];
                        uName = (string)reader["UName"];
                    }
                }
            }
            catch (SqlException ex)
            {
                Connection.Close();
                throw new DbQueryProcessingFailedException("Category.Constructor(id)", ex);
            }
            Connection.Close();
        }
        /// <summary>
        /// Adds new category into the db
        /// </summary>
        /// <param name="name"></param>
        /// <param name="uName">Urdu Name of the category</param>
        public Category(string name, string uName)
        {

            StoredProcedureCommand.CommandText = "AddNewCategory";
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.VarChar)).Value = name;
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@uName", System.Data.SqlDbType.VarChar)).Value = uName;
            Connection.Open();
            try
            {
                id = Convert.ToInt16(StoredProcedureCommand.ExecuteScalar());
                this.name = name;
                this.uName = uName;
            }
            catch (SqlException ex)
            {
                Connection.Close();
                throw new DbQueryProcessingFailedException("Category->Constructor(add value)", ex);
            }
            Connection.Close();
        }
        /// <summary>
        /// Urdu Name of the category
        /// </summary>
        public string UrduName
        {
            get => uName;
            set
            {
                const string _PATH = "Category->UrduName";
                ResetCommands();
                StoredProcedureCommand.CommandText = "UpdateCategoryUName";
                StoredProcedureCommand.Parameters.Add(new SqlParameter("@catId", System.Data.SqlDbType.SmallInt)).Value = id;
                StoredProcedureCommand.Parameters.Add(new SqlParameter("@uName", System.Data.SqlDbType.VarChar)).Value = value;
                Connection.Open();
                try
                {
                    if (StoredProcedureCommand.ExecuteNonQuery() < 1)
                    {
                        throw new UpdateUnsuccessfulException(_PATH);
                    }
                    else
                    {
                        uName = value;
                    }
                }
                catch (SqlException ex)
                {
                    Connection.Close();
                    throw new DbQueryProcessingFailedException(_PATH, ex);
                }
                Connection.Close();
            }
        }

        /// <summary>
        /// Name of the category
        /// </summary>
        public string Name
        {
            get => name;
            set
            {
                const string _PATH = "Category->Name";
                ResetCommands();
                StoredProcedureCommand.CommandText = "UpdateCategoryName";
                StoredProcedureCommand.Parameters.Add(new SqlParameter("@catId", System.Data.SqlDbType.SmallInt)).Value = id;
                StoredProcedureCommand.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.VarChar)).Value = value;
                Connection.Open();
                try
                {
                    if (StoredProcedureCommand.ExecuteNonQuery() < 1)
                    {
                        throw new UpdateUnsuccessfulException(_PATH);
                        
                    }
                    else
                    {
                        name = value;
                    }
                }
                catch (SqlException ex)
                {
                    Connection.Close();
                    throw new DbQueryProcessingFailedException(_PATH, ex);
                }
                Connection.Close();
            }
        }
        /// <summary>
        /// Primary Key
        /// </summary>
        public short Id
        {
            get => id;
        }
        /// <summary>
        /// Returns all agro items belong to this category
        /// </summary>
        /// <returns></returns>
        public List<AgroItem> GetAgroItems()
        {
            List<AgroItem> agroItems = new List<AgroItem>();
            const string _PATH = "Category->GetAgroItems";
            ResetCommands();
            StoredProcedureCommand.CommandText = "GetItemsCat";
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@catId", System.Data.SqlDbType.SmallInt)).Value = id;
            Connection.Open();
            try
            {
                using (var reader = StoredProcedureCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        agroItems.Add(new AgroItem((int)reader[0]));
                    }
                }
            }
            catch (SqlException ex)
            {
                Connection.Close();
                throw new DbQueryProcessingFailedException(_PATH, ex);
            }
            Connection.Close();
            return agroItems;
        }
        /// <summary>
        /// Static method to get all categories present into the database
        /// </summary>
        /// <param name="max">maximum number of results</param>
        /// <returns></returns>
        public static List<Category> GetCategories(int max = int.MaxValue)
        {
            List<Category> categories = new List<Category>();
            const string _PATH = "Category->GetCategories";
            var StoredProcedureCommand = GetCommand(System.Data.CommandType.StoredProcedure);
            var Connection = GetConnection();
            StoredProcedureCommand.CommandText = "GetCategories";
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@max", System.Data.SqlDbType.Int)).Value = max;
            Connection.Open();
            try
            {
                using (var reader = StoredProcedureCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new Category((short)reader[0]));
                    }
                }
            }
            catch (SqlException ex)
            {
                Connection.Close();
                throw new DbQueryProcessingFailedException(_PATH, ex);
            }
            Connection.Close();
            return categories;
        }
        public override void Copy(SQLConnection o)
        {
            throw new NotImplementedException();
        }
    }
}
