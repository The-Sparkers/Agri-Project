using EFarmer.pk.Common;
using EFarmer.pk.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace EFarmer.pk.Models
{
    /// <summary>
    /// City on any geography from which a user and advertisment belongs
    /// </summary>
    public class City : SQLConnection
    {
        private readonly short id;
        private string name;
        private readonly GeoLocation geoLocation;
        /// <summary>
        /// Initialize values from Db by using primary key
        /// </summary>
        /// <param name="id"></param>
        public City(short id)
        {
            this.id = id;
            StoredProcedureCommand.CommandText = "GetCity";
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.SmallInt)).Value = id;
            Connection.Open();
            try
            {
                using (SqlDataReader reader = StoredProcedureCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        name = (string)reader["Name"];
                        geoLocation = new GeoLocation
                        {
                            Latitude = Convert.ToDecimal(reader["GLat"]),
                            Longitude = Convert.ToDecimal(reader["GLng"])
                        };
                    }
                }
            }
            catch (SqlException ex)
            {
                Connection.Close();
                throw new DbQueryProcessingFailedException("City.Constructor(id)", ex);
            }
            Connection.Close();
        }
        /// <summary>
        /// Adds a new city into the database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="geoLocation"></param>
        public City(string name, GeoLocation geoLocation)
        {
            if (name.Length > 50)
            {
                throw new ArgumentOutOfRangeException("name");
            }
            StoredProcedureCommand.CommandText = "AddNewCity";
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.VarChar)).Value = name;
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@lat", System.Data.SqlDbType.Decimal)).Value = geoLocation.Latitude;
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@lng", System.Data.SqlDbType.Decimal)).Value = geoLocation.Longitude;
            Connection.Open();
            try
            {
                id = Convert.ToInt16(StoredProcedureCommand.ExecuteScalar());
                this.name = name;
                this.geoLocation = geoLocation;
            }
            catch (SqlException ex)
            {
                Connection.Close();
                throw new DbQueryProcessingFailedException("City->Constructor(add value)", ex);
            }
            Connection.Close();
        }
        /// <summary>
        /// Location of the city onto the Geo Map
        /// </summary>
        public GeoLocation GeoLocation
        {
            get => geoLocation;
            set
            {
                const string _PATH = "City->GeoLocation";
                ResetCommands();
                StoredProcedureCommand.CommandText = "UpdateCityGeoLocation";
                StoredProcedureCommand.Parameters.Add(new SqlParameter("@cityId", System.Data.SqlDbType.SmallInt)).Value = id;
                StoredProcedureCommand.Parameters.Add(new SqlParameter("@lat", System.Data.SqlDbType.Decimal)).Value = value.Latitude;
                StoredProcedureCommand.Parameters.Add(new SqlParameter("@lng", System.Data.SqlDbType.Decimal)).Value = value.Longitude;
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
        }
        /// <summary>
        /// Name of the city
        /// </summary>
        public string Name
        {
            get => name;
            set
            {
                if (name.Length > 50)
                {
                    throw new IndexOutOfRangeException("name");
                }
                const string _PATH = "City->Name";
                ResetCommands();
                StoredProcedureCommand.CommandText = "UpdateCityName";
                StoredProcedureCommand.Parameters.Add(new SqlParameter("@cityId", System.Data.SqlDbType.SmallInt)).Value = id;
                StoredProcedureCommand.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.VarChar)).Value = value;
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
        }
        /// <summary>
        /// Primary Key
        /// </summary>
        public short Id => id;
        /// <summary>
        /// Returns a list of users belong to this city
        /// </summary>
        /// <returns></returns>
        public List<User> GetUsers()
        {
            List<User> users = new List<User>();
            const string _PATH = "City->GetUsers";
            ResetCommands();
            StoredProcedureCommand.CommandText = "GetUsersCity";
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@cityId", System.Data.SqlDbType.SmallInt)).Value = id;
            Connection.Open();
            try
            {
                using (var reader = StoredProcedureCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User((long)reader[0]));
                    }
                }
            }
            catch (SqlException ex)
            {
                Connection.Close();
                throw new DbQueryProcessingFailedException(_PATH, ex);
            }
            Connection.Close();
            return users;
        }
        /// <summary>
        /// Returns a list of advertisements associated to this city
        /// </summary>
        /// <returns></returns>
        public List<Advertisement> GetAdvertisements()
        {
            List<Advertisement> advertisements = new List<Advertisement>();
            const string _PATH = "City->GetUsers";
            ResetCommands();
            StoredProcedureCommand.CommandText = "GetAdsCity";
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@cityId", System.Data.SqlDbType.SmallInt)).Value = id;
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
        /// Returns a list of buyers nearby this city geographically
        /// </summary>
        /// <returns></returns>
        public List<Buyer> GetNearbyBuyers()
        {
            List<Buyer> nearbyBuyers = new List<Buyer>();
            List<GeoLocationDistanceBuyer> temp = new List<GeoLocationDistanceBuyer>();
            foreach (var item in Buyer.GetBuyers())
            {
                var distance = item.Location.DistanceFromAPoint(geoLocation); //in km
                if (distance < CommonValues.RADIUS_IN_KM)
                {
                    temp.Add(new GeoLocationDistanceBuyer { DistanceFromOrigion = distance, Buyer = item });
                }
            }
            if (temp.Count > 0)
            {
                temp = temp.OrderBy(x => x.DistanceFromOrigion).ToList();
                foreach (var item in temp)
                {
                    nearbyBuyers.Add(item.Buyer);
                }
            }
            return nearbyBuyers;
        }
        /// <summary>
        /// Returns a list of seller nearby this city geographically
        /// </summary>
        /// <returns></returns>
        public List<Seller> GetNearbySeller()
        {
            List<Seller> nearbySellers = new List<Seller>();
            List<GeoLocationDistanceSeller> temp = new List<GeoLocationDistanceSeller>();
            foreach (var item in Seller.GetSellers())
            {
                var distance = item.Location.DistanceFromAPoint(geoLocation); //in km
                if (distance < CommonValues.RADIUS_IN_KM)
                {
                    temp.Add(new GeoLocationDistanceSeller { DistanceFromOrigion = distance, Seller = item });
                }
            }
            if (temp.Count > 0)
            {
                temp = temp.OrderBy(x => x.DistanceFromOrigion).ToList();
                foreach (var item in temp)
                {
                    nearbySellers.Add(item.Seller);
                }
            }
            return nearbySellers;
        }
        /// <summary>
        /// Static method to get all cities present into the database
        /// </summary>
        /// <param name="max">Maximum number of results</param>
        /// <returns></returns>
        public static List<City> GetCities(int max = int.MaxValue)
        {
            List<City> cities = new List<City>();
            const string _PATH = "City->GetCities";
            var StoredProcedureCommand = GetCommand(System.Data.CommandType.StoredProcedure);
            var Connection = GetConnection();
            StoredProcedureCommand.CommandText = "GetCities";
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@max", System.Data.SqlDbType.Int)).Value = max;
            Connection.Open();
            try
            {
                using (var reader = StoredProcedureCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cities.Add(new City((short)reader[0]));
                    }
                }
            }
            catch (SqlException ex)
            {
                Connection.Close();
                throw new DbQueryProcessingFailedException(_PATH, ex);
            }
            Connection.Close();
            return cities;
        }
        /// <summary>
        /// Copies the values of other object to this one
        /// </summary>
        /// <param name="o"></param>
        public override void Copy(SQLConnection o)
        {
            throw new NotImplementedException();
        }
        struct GeoLocationDistanceBuyer
        {
            public Buyer Buyer { get; set; }
            public double DistanceFromOrigion { get; set; }
        }
        struct GeoLocationDistanceSeller
        {
            public Seller Seller { get; set; }
            public double DistanceFromOrigion { get; set; }
        }
    }
}