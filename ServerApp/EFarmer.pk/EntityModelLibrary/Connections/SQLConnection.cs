using EntityGrabber;
using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace EFarmer.Connections
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [DataContract]
    public abstract class SQLConnection
    {
        public SQLConnection(IDbConnection connection)
        {
            Connection = new SqlConnection(connection.GetConnectionString());
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

        protected SqlConnection Connection { get; private set; }
        protected SqlCommand StoredProcedureCommand { get; private set; }
        protected SqlCommand QueryCommand { get; private set; }
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
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
