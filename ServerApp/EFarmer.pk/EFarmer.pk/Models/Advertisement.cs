using EFarmer.pk.Common;
using EFarmer.pk.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;

namespace EFarmer.pk.Models
{
    /// <summary>
    /// Posted by a seller for an agro item
    /// </summary>
    [DataContract]
    public class Advertisement : SQLConnection
    {
        private readonly long id;
        private short quality;
        private short quantity;
        private DateTime dateTime;
        private double price;
        private string pictureName;
        private Seller seller;
        private City city;
        private AgroItem item;
        /// <summary>
        /// Constructor to initialize values from db by using primary key
        /// </summary>
        /// <param name="id">Primary Key</param>
        public Advertisement(long id)
        {
            this.id = id;
            StoredProcedureCommand.CommandText = "GetAdvertisement";
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.BigInt)).Value = id;
            Connection.Open();
            try
            {
                using (SqlDataReader reader = StoredProcedureCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        quality = (short)reader["Quality"];
                        quantity = (short)reader["Quantity"];
                        dateTime = (DateTime)reader["PostedDateTime"];
                        price = (double)reader["Price"];
                        pictureName = (string)reader["Picture"];
                        seller = new Seller((long)reader["SellerId"]);
                        item = new AgroItem((int)reader["ItemId"]);
                        city = new City((short)reader["CityId"]);
                    }
                }
            }
            catch (SqlException ex)
            {
                Connection.Close();
                throw new DbQueryProcessingFailedException("Advertisement.Constructor(id)", ex);
            }
            Connection.Close();
        }

        internal Advertisement(short quality, short quantity, DateTime dateTime, double price, string pictureName, Seller seller, AgroItem item, City city)
        {
            if (pictureName != "")
            {
                pictureName = Guid.NewGuid().ToString() + "_" + pictureName;
            }
            else
            {
                pictureName = string.Empty;
            }

            StoredProcedureCommand.CommandText = "AddNewAdvertisement";
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@quality", System.Data.SqlDbType.SmallInt)).Value = quality;
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@quantity", System.Data.SqlDbType.SmallInt)).Value = quantity;
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@dateTime", System.Data.SqlDbType.DateTime)).Value = dateTime;
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@price", System.Data.SqlDbType.Money)).Value = price;
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@picture", System.Data.SqlDbType.VarChar)).Value = pictureName;
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@sellerId", System.Data.SqlDbType.BigInt)).Value = seller.Id;
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@itemId", System.Data.SqlDbType.Int)).Value = item.Id;
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@cityId", System.Data.SqlDbType.SmallInt)).Value = city.Id;
            Connection.Open();
            try
            {
                id = Convert.ToInt64(StoredProcedureCommand.ExecuteScalar());
                this.quality = quality;
                this.quantity = quantity;
                this.dateTime = dateTime;
                this.price = price;
                this.pictureName = pictureName;
                this.item = item;
                this.city = city;
                this.seller = seller;
            }
            catch (SqlException ex)
            {
                Connection.Close();
                throw new DbQueryProcessingFailedException("Advertisement->Constructor(add value)", ex);
            }
            Connection.Close();
        }
        /// <summary>
        /// Agro Item for which the Ad has been posted
        /// </summary>
        [DataMember]
        public AgroItem Item => item;
        /// <summary>
        /// City for which the ad has been posted
        /// </summary>
        [DataMember]
        public City City => city;
        /// <summary>
        /// Seller who posted the ad.
        /// </summary>
        [DataMember]
        public Seller Seller => seller;
        /// <summary>
        /// Picture of the Ad.
        /// </summary>
        [DataMember]
        public string Picture => pictureName;
        /// <summary>
        /// Price set by the seller at the time of posting
        /// </summary>
        [DataMember]
        public double Price => price;

        /// <summary>
        /// Time Stamp
        /// </summary>
        [DataMember]
        public DateTime PostedDateTime => dateTime;
        /// <summary>
        /// Quantity offered by the seller according to the weigh scale
        /// </summary>
        [DataMember]
        public short Quantity => quantity;
        /// <summary>
        /// Set as stars(1-3)
        /// </summary>
        [DataMember]
        public short Quality => quality;
        /// <summary>
        /// Primary Key
        /// </summary>
        [DataMember]
        public long Id => id;
        /// <summary>
        /// Adds this advertsement to buyers favorites list
        /// </summary>
        /// <param name="buyer"></param>
        public void Favorite(Buyer buyer)
        {
            const string _PATH = "Advertisement->Favorite";
            ResetCommands();
            StoredProcedureCommand.CommandText = "FavAd";
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@buyerId", System.Data.SqlDbType.BigInt)).Value = id;
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@adId", System.Data.SqlDbType.BigInt)).Value = id;
            Connection.Open();
            try
            {
                StoredProcedureCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Connection.Close();
                throw new DbQueryProcessingFailedException(_PATH, ex);
            }
            Connection.Close();
        }
        /// <summary>
        /// Static method to get all advertisments present into the database
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public static List<Advertisement> GetAdvertisements(int max = int.MaxValue)
        {
            List<Advertisement> advertisements = new List<Advertisement>();
            const string _PATH = "Advertisement->GetAdvertisements";
            var StoredProcedureCommand = GetCommand(System.Data.CommandType.StoredProcedure);
            var Connection = GetConnection();
            StoredProcedureCommand.CommandText = "GetAdvertisements";
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
        /// Returns a list of advertisements near a certain location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static List<Advertisement> GetNearbyAdvertisements(GeoLocation location)
        {
            List<Advertisement> nearbyAdvertisements = new List<Advertisement>();
            List<GeoLocationDistanceAd> temp = new List<GeoLocationDistanceAd>();
            foreach (var item in GetAdvertisements())
            {
                var distance = item.City.GeoLocation.DistanceFromAPoint(location); //in km
                if (distance < CommonValues.RADIUS_IN_KM)
                {
                    temp.Add(new GeoLocationDistanceAd { DistanceFromOrigion = distance, Advertisement = item });
                }
            }
            if (temp.Count > 0)
            {
                temp = temp.OrderBy(x => x.DistanceFromOrigion).ToList();
                foreach (var item in temp)
                {
                    nearbyAdvertisements.Add(item.Advertisement);
                }
            }
            return nearbyAdvertisements;
        }
        /// <summary>
        /// Copies the content of an object to this object
        /// </summary>
        /// <param name="object"></param>
        public override void Copy(SQLConnection @object)
        {
            try
            {
                Advertisement advertisement = (Advertisement)@object;
                city = advertisement.city;
                item = advertisement.item;
                seller = advertisement.seller;
                quality = advertisement.quality;
                quantity = advertisement.quantity;
                dateTime = advertisement.dateTime;
                price = advertisement.price;
                pictureName = advertisement.pictureName;
            }
            catch (InvalidCastException)
            {
                city = null;
                item = null;
                seller = null;
                quality = 0;
                quantity = 0;
                dateTime = DateTime.MinValue;
                price = 0;
                pictureName = string.Empty;
            }
        }
        struct GeoLocationDistanceAd
        {
            public Advertisement Advertisement { get; set; }
            public double DistanceFromOrigion { get; set; }
        }
    }
}