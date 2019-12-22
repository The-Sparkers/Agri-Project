using EFarmer.pk.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EFarmer.pk.Models
{
    /// <summary>
    /// Item for which the ads will be posted
    /// </summary>
    public class AgroItem : SQLConnection
    {
        private readonly int id;
        private string name;
        private string uName;
        private string scale;
        private Category category;
        private string uWeightScale;
        /// <summary>
        /// Initializes values from db by using primary key
        /// </summary>
        /// <param name="id">Primary Key</param>
        public AgroItem(int id)
        {
            this.id = id;
            StoredProcedureCommand.CommandText = "GetAgroItem";
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = id;
            Connection.Open();
            try
            {
                using (SqlDataReader reader = StoredProcedureCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        name = (string)reader["Name"];
                        uName = (string)reader["uName"];
                        WeightScale = (string)reader["WeightScale"];
                        uWeightScale = (string)reader["UWeightScale"];
                        category = new Category((short)reader["CategoryId"]);
                    }
                }
            }
            catch (SqlException ex)
            {
                Connection.Close();
                throw new DbQueryProcessingFailedException("AgroItem.Constructor(id)", ex);
            }
            Connection.Close();
        }
        /// <summary>
        /// Adds a new agro item into the database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="uName">Urdu Name</param>
        /// <param name="scale">Weight Scale</param>
        /// <param name="uScale">Urdu Weight Scale</param>
        /// <param name="category"></param>
        public AgroItem(string name, string uName, string scale, string uScale, Category category)
        {
            StoredProcedureCommand.CommandText = "AddNewAgroItem";
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.VarChar)).Value = name;
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@uName", System.Data.SqlDbType.VarChar)).Value = uName;
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@scale", System.Data.SqlDbType.NChar)).Value = name;
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@uScale", System.Data.SqlDbType.NChar)).Value = uName;
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@catId", System.Data.SqlDbType.SmallInt)).Value = category.Id;
            Connection.Open();
            try
            {
                id = Convert.ToInt32(StoredProcedureCommand.ExecuteScalar());
                this.name = name;
                this.uName = uName;
                this.scale = scale;
                uWeightScale = uScale;
                this.category = category;
            }
            catch (SqlException ex)
            {
                Connection.Close();
                throw new DbQueryProcessingFailedException("AgroItem->Constructor(add value)", ex);
            }
            Connection.Close();
        }
        /// <summary>
        /// Category to which this item belongs
        /// </summary>
        public Category Category
        {
            get => category;
            set
            {
                const string _PATH = "AgroItem->Category";
                ResetCommands();
                StoredProcedureCommand.CommandText = "UpdateItemCategory";
                StoredProcedureCommand.Parameters.Add(new SqlParameter("@itemId", System.Data.SqlDbType.Int)).Value = id;
                StoredProcedureCommand.Parameters.Add(new SqlParameter("@catId", System.Data.SqlDbType.SmallInt)).Value = value.Id;
                Connection.Open();
                try
                {
                    if (StoredProcedureCommand.ExecuteNonQuery() < 1)
                    {
                        throw new UpdateUnsuccessfulException(_PATH);
                    }
                    else
                    {
                        category = value;
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
        /// Weight Scale in urdu
        /// </summary>
        public string UrduWeightScale
        {
            get => uWeightScale;
            set
            {
                const string _PATH = "AgroItem->UrduWeightScale";
                ResetCommands();
                StoredProcedureCommand.CommandText = "UpdateItemUWeightScale";
                StoredProcedureCommand.Parameters.Add(new SqlParameter("@itemId", System.Data.SqlDbType.Int)).Value = id;
                StoredProcedureCommand.Parameters.Add(new SqlParameter("@uScale", System.Data.SqlDbType.NChar)).Value = value;
                Connection.Open();
                try
                {
                    if (StoredProcedureCommand.ExecuteNonQuery() < 1)
                    {
                        throw new UpdateUnsuccessfulException(_PATH);
                    }
                    else
                    {
                        uWeightScale = value;
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
        /// Weight scale from which this item's weight is denoted
        /// </summary>
        public string WeightScale
        {
            get => scale;
            set
            {
                const string _PATH = "AgroItem->WeightScale";
                ResetCommands();
                StoredProcedureCommand.CommandText = "UpdateItemWeightScale";
                StoredProcedureCommand.Parameters.Add(new SqlParameter("@itemId", System.Data.SqlDbType.Int)).Value = id;
                StoredProcedureCommand.Parameters.Add(new SqlParameter("@scale", System.Data.SqlDbType.NChar)).Value = value;
                Connection.Open();
                try
                {
                    if (StoredProcedureCommand.ExecuteNonQuery() < 1)
                    {
                        throw new UpdateUnsuccessfulException(_PATH);
                    }
                    else
                    {
                        scale = value;
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
        /// Name of this item in urdu
        /// </summary>
        public string UrduName
        {
            get => uName;
            set
            {
                const string _PATH = "AgroItem->UrduName";
                ResetCommands();
                StoredProcedureCommand.CommandText = "UpdateItemUName";
                StoredProcedureCommand.Parameters.Add(new SqlParameter("@itemId", System.Data.SqlDbType.Int)).Value = id;
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
        /// Name of the item
        /// </summary>
        public string Name
        {
            get => name;
            set
            {
                const string _PATH = "AgroItem->Name";
                ResetCommands();
                StoredProcedureCommand.CommandText = "UpdateItemName";
                StoredProcedureCommand.Parameters.Add(new SqlParameter("@itemId", System.Data.SqlDbType.Int)).Value = id;
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
        public int Id { get; }
        /// <summary>
        /// Deletes this agro item from the db
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            var flag = false;
            ResetCommands();
            StoredProcedureCommand.CommandText = "DeleteAgroItem";
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@itemId", System.Data.SqlDbType.Int)).Value = id;
            Connection.Open();
            try
            {
                flag = (StoredProcedureCommand.ExecuteNonQuery() > 0);
            }
            catch (SqlException)
            {
                Connection.Close();
            }
            Connection.Close();
            return flag;
        }
        /// <summary>
        /// Returns the ads in which this item is used
        /// </summary>
        /// <returns></returns>
        public List<Advertisement> GetAdvertisements(int max = int.MaxValue)
        {
            List<Advertisement> advertisements = new List<Advertisement>();
            const string _PATH = "AgroItem->GetAdvertisements";
            ResetCommands();
            StoredProcedureCommand.CommandText = "GetItemAds";
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@itemId", System.Data.SqlDbType.Int)).Value = id;
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@max", System.Data.SqlDbType.Int)).Value = max;
            Connection.Open();
            try
            {
                using (var reader = StoredProcedureCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        advertisements.Add(new Advertisement((long)reader[0]));
                    }
                }
            }
            catch (SqlException ex)
            {
                Connection.Close();
                throw new DbQueryProcessingFailedException(_PATH, ex);
            }
            Connection.Close();
            return advertisements;
        }
        /// <summary>
        /// Static method to get all agro items into the database
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public static List<AgroItem> GetAgroItems(int max = int.MaxValue)
        {
            List<AgroItem> agroItems = new List<AgroItem>();
            const string _PATH = "AgroItems->GetAgroItems";
            var StoredProcedureCommand = GetCommand(System.Data.CommandType.StoredProcedure);
            var Connection = GetConnection();
            StoredProcedureCommand.CommandText = "GetAgroItems";
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@max", System.Data.SqlDbType.Int)).Value = max;
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
        public override void Copy(SQLConnection o)
        {
            throw new NotImplementedException();
        }
    }
}