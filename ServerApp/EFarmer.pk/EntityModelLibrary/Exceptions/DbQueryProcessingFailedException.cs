using System;
using System.Data.SqlClient;

namespace EFarmer.Exceptions
{
    /// <summary>
    /// Exception will be thrown whenever Database query or stored procedure calling process fails.
    /// </summary>
    public sealed class DbQueryProcessingFailedException : Exception
    {
        public DbQueryProcessingFailedException(string path, SqlException sqlException) : base("Error occured while processing SQL Query or Stored Procedure. Path: " + path, sqlException)
        {
            InnerSQLException = sqlException;
        }
        /// <summary>
        /// Original SQL Exception caused the problem
        /// </summary>
        public SqlException InnerSQLException { get; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}