using EFarmer.pk.Common;
using EFarmer.pk.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EFarmer.pk.Models
{
    /// <summary>
    /// User of the system who posts different Ads
    /// </summary>
    public class Seller : User
    {
        /// <summary>
        /// Initialize the value from DB
        /// </summary>
        /// <param name="id">PrimaryKey</param>
        public Seller(long id) : base(id)
        {
            if (!isSeller)
            {
                Copy(null);
            }
        }
        /// <summary>
        /// Adds new data for the seller to the DB
        /// </summary>
        /// <param name="name"></param>
        /// <param name="contact"></param>
        /// <param name="address"></param>
        /// <param name="city"></param>
        public Seller(NameFormat name, ContactNumberFormat contact, string address, City city) : base(name, contact, address, city)
        {
            MakeSeller();
        }
        /// <summary>
        /// Seller Flag
        /// </summary>
        public bool IsBuyer => isBuyer;
        /// <summary>
        /// Method which adds a buyer to the interest list of this user
        /// </summary>
        /// <param name="buyer"></param>
        public void AddToFavorites(Buyer buyer)
        {
            const string _PATH = "Seller->AddToInterest(buyer)";
            ResetCommands();
            StoredProcedureCommand.CommandText = "AddBuyerToInterest";
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@sellerId", System.Data.SqlDbType.BigInt)).Value = id;
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@buyerId", System.Data.SqlDbType.BigInt)).Value = buyer.Id;
            Connection.Open();
            try
            {
                if (StoredProcedureCommand.ExecuteNonQuery() < 1)
                {
                    throw new UpdateUnsuccessfulException(_PATH);
                }
            }
            catch (SqlException ex)
            {
                Connection.Close();
                throw new DbQueryProcessingFailedException(_PATH, ex);
            }
            Connection.Close();
        }
        /// <summary>
        /// Mehtod to get list of buyers favorited by this user
        /// </summary>
        /// <returns></returns>
        public List<Buyer> GetFavoriteBuyers()
        {
            List<Buyer> buyers = new List<Buyer>();
            const string _PATH = "Seller->GetFavoriteBuyers";
            ResetCommands();
            StoredProcedureCommand.CommandText = "GetFavBuyers";
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@sellerId", System.Data.SqlDbType.BigInt)).Value = id;
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
        /// <summary>
        /// Method which seller uses to post a new advertisement
        /// </summary>
        /// <param name="quality">mainly from 1-3</param>
        /// <param name="quantity"></param>
        /// <param name="dateTime"></param>
        /// <param name="price"></param>
        /// <param name="item"></param>
        /// <param name="city"></param>
        /// <param name="picture"></param>
        /// <returns></returns>
        public Advertisement PostAdvertisement(short quality, short quantity, DateTime dateTime, double price, AgroItem item, City city, string picture = "")
        {
            return new Advertisement(quality, quantity, dateTime, price, picture, this, item, city);
        }
        /// <summary>
        /// Method to get a list of advertisments posted by this user
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<Advertisement> GetPostedAdvertisments(DateTime startDate, DateTime endDate)
        {
            List<Advertisement> advertisements = new List<Advertisement>();
            const string _PATH = "Seller->GetPostedAdvertisments";
            ResetCommands();
            StoredProcedureCommand.CommandText = "GetPostedAdvertisments";
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@sellerId", System.Data.SqlDbType.BigInt)).Value = id;
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@startDate", System.Data.SqlDbType.DateTime)).Value = startDate;
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@endDate", System.Data.SqlDbType.DateTime)).Value = endDate;
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
        /// Static Method to get all sellers present into the database
        /// </summary>
        /// <returns></returns>
        public static List<Seller> GetSellers()
        {
            List<Seller> sellers = new List<Seller>();
            const string _PATH = "Seller->GetSellers";
            var StoredProcedureCommand = GetCommand(System.Data.CommandType.StoredProcedure);
            var Connection = GetConnection();
            StoredProcedureCommand.CommandText = "GetSellers";
            Connection.Open();
            try
            {
                using (var reader = StoredProcedureCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sellers.Add(new Seller((long)reader[0]));
                    }
                }
            }
            catch (SqlException ex)
            {
                Connection.Close();
                throw new DbQueryProcessingFailedException(_PATH, ex);
            }
            Connection.Close();
            return sellers;
        }
    }
}
