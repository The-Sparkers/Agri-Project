using EFarmer.pk.Common;
using EFarmer.pk.Exceptions;
using System;
using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace EFarmer.pk.Models
{
    /// <summary>
    /// User is the actor of the system who interacts with the system (maybe a Buyer or seller)
    /// </summary>
    [DataContract]
    public class User : SQLConnection
    {
        protected long id;
        private NameFormat name;
        private ContactNumberFormat contact;
        private string address;
        private readonly GeoLocation location;
        private City city;
        protected bool isBuyer, isSeller;
        /// <summary>
        /// Constructor to initialize values from DB by using primary key
        /// </summary>
        /// <param name="id">Primary Key</param>
        public User(long id)
        {
            this.id = id;
            StoredProcedureCommand.CommandText = "GetUser";
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.BigInt)).Value = id;
            Connection.Open();
            try
            {
                using (SqlDataReader reader = StoredProcedureCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        name = new NameFormat
                        {
                            FirstName = (string)reader["FName"],
                            LastName = (string)reader["LName"]
                        };
                        contact = new ContactNumberFormat((string)reader["CCountryCode"], (string)reader["CCompanyCode"], (string)reader["CPhone"]);
                        address = (string)reader["Address"];
                        location = new GeoLocation
                        {
                            Latitude = Convert.ToDecimal(reader["GLat"]),
                            Longitude = Convert.ToDecimal((decimal)reader["GLng"])
                        };
                        city = new City((short)reader["CityId"]);
                        isBuyer = (bool)reader["BuyerFlag"];
                        isSeller = (bool)reader["SellerFlag"];
                    }
                }
            }
            catch (SqlException ex)
            {
                Connection.Close();
                throw new DbQueryProcessingFailedException("User.Constructor(id)", ex);
            }
            Connection.Close();
        }

        /// <summary>
        /// Constructor to add new user into the database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="contact"></param>
        /// <param name="address"></param>
        /// <param name="city"></param>
        protected User(NameFormat name, ContactNumberFormat contact, string address, City city)
        {
            StoredProcedureCommand.CommandText = "AddNewUser";
            StoredProcedureCommand.Parameters.Add("@fName", System.Data.SqlDbType.VarChar).Value = name.FirstName;
            StoredProcedureCommand.Parameters.Add("@lName", System.Data.SqlDbType.VarChar).Value = name.LastName;
            StoredProcedureCommand.Parameters.Add("@countryCode", System.Data.SqlDbType.NChar).Value = contact.CountryCode;
            StoredProcedureCommand.Parameters.Add("@companyCode", System.Data.SqlDbType.NChar).Value = contact.CompanyCode;
            StoredProcedureCommand.Parameters.Add("@phone", System.Data.SqlDbType.NChar).Value = contact.PhoneNumber;
            StoredProcedureCommand.Parameters.Add("@address", System.Data.SqlDbType.VarChar).Value = address;
            StoredProcedureCommand.Parameters.Add("@cityId", System.Data.SqlDbType.SmallInt).Value = city.Id;
            Connection.Open();
            try
            {
                id = Convert.ToInt64(StoredProcedureCommand.ExecuteScalar());
                this.name = name;
                this.contact = contact;
                this.address = address;
                this.city = city;
            }
            catch (SqlException ex)
            {
                Connection.Close();
                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    //Unique key handler
                    //returns the already created driver in the System
                    var user = GetUser(contact);
                    Copy(user);
                    return;
                }
                throw new DbQueryProcessingFailedException("User->Constructor(add value)", ex);
            }
            Connection.Close();
        }


        /// <summary>
        /// City to which the user belongs
        /// </summary>
        [DataMember]
        public City City
        {
            get => city;
            set
            {
                const string _PATH = "User->City";
                ResetCommands();
                StoredProcedureCommand.CommandText = "UpdateUserCity";
                StoredProcedureCommand.Parameters.Add(new SqlParameter("@userId", System.Data.SqlDbType.BigInt)).Value = id;
                StoredProcedureCommand.Parameters.Add(new SqlParameter("@cityId", System.Data.SqlDbType.SmallInt)).Value = value.Id;
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
        /// Latest location based on the cellular GPS Information
        /// </summary>
        [DataMember]
        public GeoLocation Location
        {
            get => location;
            set
            {
                const string _PATH = "User->Location";
                ResetCommands();
                StoredProcedureCommand.CommandText = "UpdateUserLocation";
                StoredProcedureCommand.Parameters.Add(new SqlParameter("@userId", System.Data.SqlDbType.BigInt)).Value = id;
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
        /// Physical Address of the User
        /// </summary>
        [DataMember]
        public string Address
        {
            get => address;
            set
            {
                const string _PATH = "User->Address";
                ResetCommands();
                StoredProcedureCommand.CommandText = "UpdateUserAddress";
                StoredProcedureCommand.Parameters.Add(new SqlParameter("@userId", System.Data.SqlDbType.BigInt)).Value = id;
                StoredProcedureCommand.Parameters.Add(new SqlParameter("@address", System.Data.SqlDbType.VarChar)).Value = value;
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
        /// Contact Number of the user
        /// </summary>
        [DataMember]
        public ContactNumberFormat ContactNumber
        {
            get => contact;
            set
            {
                const string _PATH = "User->ContactNumber";
                ResetCommands();
                StoredProcedureCommand.CommandText = "UpdateUserContact";
                StoredProcedureCommand.Parameters.Add(new SqlParameter("@userId", System.Data.SqlDbType.BigInt)).Value = id;
                StoredProcedureCommand.Parameters.Add(new SqlParameter("@countryCode", System.Data.SqlDbType.NChar)).Value = value.CountryCode;
                StoredProcedureCommand.Parameters.Add(new SqlParameter("@comapnyCode", System.Data.SqlDbType.NChar)).Value = value.CompanyCode;
                StoredProcedureCommand.Parameters.Add(new SqlParameter("@phone", System.Data.SqlDbType.NChar)).Value = value.PhoneNumber;
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
        /// Full Name of the user
        /// </summary>
        [DataMember]
        public NameFormat Name
        {
            get => name;
            set
            {
                const string _PATH = "User->Name";
                ResetCommands();
                StoredProcedureCommand.CommandText = "UpdateUserName";
                StoredProcedureCommand.Parameters.Add(new SqlParameter("@userId", System.Data.SqlDbType.BigInt)).Value = id;
                StoredProcedureCommand.Parameters.Add(new SqlParameter("@fName", System.Data.SqlDbType.VarChar)).Value = value.FirstName;
                StoredProcedureCommand.Parameters.Add(new SqlParameter("@lName", System.Data.SqlDbType.VarChar)).Value = value.LastName;
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
        /// Method to Make this user a buyer
        /// </summary>
        public void MakeBuyer()
        {
            const string _PATH = "User->MakeBuyer";
            ResetCommands();
            StoredProcedureCommand.CommandText = "MakeBuyer";
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@userId", System.Data.SqlDbType.BigInt)).Value = id;
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
        /// Method to make this user a seller
        /// </summary>
        public void MakeSeller()
        {
            const string _PATH = "User->MakeSeller";
            ResetCommands();
            StoredProcedureCommand.CommandText = "MakeSeller";
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@userId", System.Data.SqlDbType.BigInt)).Value = id;
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
        /// Primary Key
        /// </summary>
        public long Id => id;
        /// <summary>
        /// Gets a user by finding it by using unique contact number
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public static User GetUser(ContactNumberFormat contact)
        {
            var con = GetConnection();
            var cmd = GetCommand(System.Data.CommandType.StoredProcedure);
            cmd.CommandText = "GetUserByContact";
            cmd.Parameters.Add(new SqlParameter("@countryCode", System.Data.SqlDbType.NChar)).Value = contact.CountryCode;
            cmd.Parameters.Add(new SqlParameter("@companyCode", System.Data.SqlDbType.NChar)).Value = contact.CompanyCode;
            cmd.Parameters.Add(new SqlParameter("@phone", System.Data.SqlDbType.NChar)).Value = contact.PhoneNumber;
            User user;
            con.Open();
            try
            {
                user = new User(Convert.ToInt64(cmd.ExecuteScalar()));
            }
            catch (SqlException ex)
            {
                con.Close();
                throw new DbQueryProcessingFailedException("User->GetUser(contact)", ex);
            }
            con.Close();
            return user;
        }
        /// <summary>
        /// Copies an object to this one
        /// </summary>
        /// <param name="o"></param>
        public override void Copy(SQLConnection o)
        {
            try
            {
                var user = (User)o;
                id = user.id;
                name = user.Name;
                contact = user.ContactNumber;
                address = user.Address;
                city = user.City;
                isBuyer = user.isBuyer;
                isSeller = user.isSeller;
            }
            catch (InvalidCastException)
            {
                id = 0;
                name = null;
                contact = null;
                address = null;
                city = null;
                isBuyer = false;
                isSeller = false;
            }
        }
    }
}
