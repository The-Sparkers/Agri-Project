using EFarmer.pk.Common;
using EFarmer.pk.Exceptions;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EFarmer.pk.Models
{
    /// <summary>
    /// User of the system which uses the system to post buying related advertisements
    /// </summary>
    public class Buyer : User
    {
        /// <summary>
        /// Initializes Values from Db
        /// </summary>
        /// <param name="id">Primary Key</param>
        public Buyer(long id) : base(id)
        {
            if (!isBuyer)
            {
                Copy(null);
            }
        }
        /// <summary>
        /// Adds a new Buyer to the db
        /// </summary>
        /// <param name="name"></param>
        /// <param name="contact"></param>
        /// <param name="address"></param>
        /// <param name="city"></param>
        public Buyer(NameFormat name, ContactNumberFormat contact, string address, City city) : base(name, contact, address, city)
        {
            MakeBuyer();
        }
        /// <summary>
        /// Buyer Flag
        /// </summary>
        public bool IsSeller
        {
            get => isSeller;
        }
        /// <summary>
        /// Returns a list for advertisments favorited by this user
        /// </summary>
        /// <returns></returns>
        public List<Advertisement> GetFavoriteAdvertisements()
        {
            List<Advertisement> advertisements = new List<Advertisement>();
            const string _PATH = "Buyer->GetFavoriteAdvertisements";
            ResetCommands();
            StoredProcedureCommand.CommandText = "GetFavAdvertisments";
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@buyerId", System.Data.SqlDbType.BigInt)).Value = id;
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
        /// Returns a list of items interested by this user
        /// </summary>
        /// <returns></returns>
        public List<AgroItem> GetInterestedItems()
        {
            List<AgroItem> agroItems = new List<AgroItem>();
            const string _PATH = "Buyer->GetInterestedItems";
            ResetCommands();
            StoredProcedureCommand.CommandText = "GetInterestedItems";
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@buyerId", System.Data.SqlDbType.BigInt)).Value = id;
            Connection.Open();
            try
            {
                using (var reader = StoredProcedureCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        agroItems.Add(new AgroItem((long)reader[0]));
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
        /// Static method to get buyers from database
        /// </summary>
        /// <returns></returns>
        public static List<Buyer> GetBuyers()
        {
            List<Buyer> buyers = new List<Buyer>();
            const string _PATH = "Buyer->GetBuyers";
            var StoredProcedureCommand = GetCommand(System.Data.CommandType.StoredProcedure);
            var Connection = GetConnection();
            StoredProcedureCommand.CommandText = "GetBuyers";
            Connection.Open();
            try
            {
                using (var reader = StoredProcedureCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        buyers.Add(new Buyer((long)reader[0]));
                    }
                }
            }
            catch (SqlException ex)
            {
                Connection.Close();
                throw new DbQueryProcessingFailedException(_PATH, ex);
            }
            Connection.Close();
            return buyers;
        }
    }
}
