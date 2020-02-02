using EFarmer.Connections;
using EFarmer.Models;
using EFarmer.Models.Exceptions;
using EFarmer.Models.Helpers;
using EntityGrabber;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EFarmerPkModelLibrary.Repositories
{
    internal class UserRepository : SQLConnection, IModelRepository<User, long>
    {
        readonly IDbConnection dbConnection;
        public UserRepository(IDbConnection connectionString) : base(connectionString)
        {
        }

        public long Create(IDataModel<long> model)
        {
            ResetCommands();
            try
            {
                User user = (User)model;
                long id = 0;
                StoredProcedureCommand.CommandText = "AddNewUser";
                StoredProcedureCommand.Parameters.Add("@fName", System.Data.SqlDbType.VarChar).Value = user.Name.FirstName;
                StoredProcedureCommand.Parameters.Add("@lName", System.Data.SqlDbType.VarChar).Value = user.Name.LastName;
                StoredProcedureCommand.Parameters.Add("@countryCode", System.Data.SqlDbType.NChar).Value = user.ContactNumber.CountryCode;
                StoredProcedureCommand.Parameters.Add("@companyCode", System.Data.SqlDbType.NChar).Value = user.ContactNumber.CompanyCode;
                StoredProcedureCommand.Parameters.Add("@phone", System.Data.SqlDbType.NChar).Value = user.ContactNumber.PhoneNumber;
                StoredProcedureCommand.Parameters.Add("@address", System.Data.SqlDbType.VarChar).Value = user.Address;
                StoredProcedureCommand.Parameters.Add("@cityId", System.Data.SqlDbType.SmallInt).Value = user.City.Id;
                Connection.Open();
                try
                {
                    id = Convert.ToInt64(StoredProcedureCommand.ExecuteScalar());
                }
                catch (SqlException ex)
                {
                    Connection.Close();
                    if (ex.Number == 2601 || ex.Number == 2627)
                    {
                        //Unique key handler
                        //returns the already created driver in the System
                        var u = GetUser(user.ContactNumber);
                        id = u.Id;
                        return id;
                    }
                    throw new DbQueryProcessingFailedException("User->Create", ex);
                }
                Connection.Close();
                return id;
            }
            catch (Exception)
            {
                throw new InvalidCastException("The passed aurgument is not able to cast into User Model");
            }
        }

        public User GetUser(ContactNumberFormat contact)
        {
            ResetCommands();
            StoredProcedureCommand.CommandText = "GetUserByContact";
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@countryCode", System.Data.SqlDbType.NChar)).Value = contact.CountryCode;
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@companyCode", System.Data.SqlDbType.NChar)).Value = contact.CompanyCode;
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@phone", System.Data.SqlDbType.NChar)).Value = contact.PhoneNumber;
            User user;
            Connection.Open();
            try
            {
                user = Read(Convert.ToInt64(StoredProcedureCommand.ExecuteScalar()));
            }
            catch (SqlException ex)
            {
                Connection.Close();
                throw new DbQueryProcessingFailedException("User->GetUser(contact)", ex);
            }
            Connection.Close();
            return user;
        }

        public bool Delete(long id)
        {
            return false;
        }

        public User Read(long id)
        {
            User user = null;
            StoredProcedureCommand.CommandText = "GetUser";
            StoredProcedureCommand.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.BigInt)).Value = id;
            Connection.Open();
            try
            {
                using (SqlDataReader reader = StoredProcedureCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user = new User
                        {
                            Name = new NameFormat
                            {
                                FirstName = (string)reader["FName"],
                                LastName = (string)reader["LName"]
                            },
                            ContactNumber = new ContactNumberFormat((string)reader["CCountryCode"], (string)reader["CCompanyCode"], (string)reader["CPhone"]),
                            Address = (string)reader["Address"],
                            Location = new GeoLocation
                            {
                                Latitude = Convert.ToDecimal(reader["GLat"]),
                                Longitude = Convert.ToDecimal((decimal)reader["GLng"])
                            },
                            City = new CityRepository(dbConnection).Read((short)reader["CityId"]),
                            IsBuyer = (bool)reader["BuyerFlag"],
                            IsSeller = (bool)reader["SellerFlag"]
                        };
                    }
                }
            }
            catch (SqlException ex)
            {
                Connection.Close();
                throw new DbQueryProcessingFailedException("User.Constructor(id)", ex);
            }
            Connection.Close();
            return user;
        }

        public List<User> ReadAll()
        {
            ResetCommands();
            List<User> users = new List<User>();
            QueryCommand.CommandText = "SELECT * FROM USERS";
            Connection.Open();
            try
            {
                using (var reader = QueryCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = (long)reader[0],
                            Name = new NameFormat { FirstName = (string)reader[1], LastName = (string)reader[2] },
                            ContactNumber = new ContactNumberFormat((string)reader[3], (string)reader[4], (string)reader[5]),
                            Address = (string)reader[6],
                            Location = new GeoLocation
                            {
                                Latitude = (reader[7] != null) ? (decimal)reader[7] : 0
                                 ,
                                Longitude = (reader[8] != null) ? (decimal)reader[8] : 0
                            },
                            IsBuyer = (bool)reader[9],
                            IsSeller = (bool)reader[10],
                            City = new CityRepository(dbConnection).Read((short)reader[11])
                        });
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DbQueryProcessingFailedException("User->ReadAll", ex);
            }
            Connection.Close();
            return users;
        }

        public bool Update(IDataModel<long> model)
        {
            throw new NotImplementedException();
        }
    }
}
