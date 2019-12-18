using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace EFarmer.pk.Models
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [DataContract]
    public abstract class SQLConnection
    {
        public SQLConnection()
        {
            Connection = new SqlConnection(Common.CommonValues.CONNECTION_STRING);
            StoredProcedureCommand = new SqlCommand
            {
                Connection = Connection,
                CommandType = System.Data.CommandType.StoredProcedure
            };
            QueryCommand = new SqlCommand
            {
                Connection = Connection
            };
        }

        public SqlConnection Connection { get; private set; }
        public SqlCommand StoredProcedureCommand { get; private set; }
        public SqlCommand QueryCommand { get; private set; }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(Common.CommonValues.CONNECTION_STRING);
        }
        public void ResetCommands()
        {
            StoredProcedureCommand = new SqlCommand()
            {
                CommandType = System.Data.CommandType.StoredProcedure,
                Connection = Connection
            };
            QueryCommand = new SqlCommand()
            {
                Connection = Connection
            };
        }
        public abstract void Copy(SQLConnection o);
        public static SqlCommand GetCommand(System.Data.CommandType commandType)
        {
            SqlCommand command = new SqlCommand
            {
                Connection = GetConnection(),
                CommandType = commandType
            };
            return command;
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
