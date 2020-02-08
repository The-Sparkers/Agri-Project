using EntityGrabber;
using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace EFarmer.Connections
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public class SQLConnection : DbConnection
    {
        public SQLConnection(string connectionString) : base(connectionString)
        {

        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
