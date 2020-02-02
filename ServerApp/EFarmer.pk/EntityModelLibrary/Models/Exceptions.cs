using System;
using System.Data.SqlClient;

namespace EFarmer.Models.Exceptions
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class ValidationPatternNotMatchException : Exception
    {
        public ValidationPatternNotMatchException(string stringValue, string pattern, string exampleWord) : base("Your string: " + stringValue + " failed to matched with the Pattern: " + pattern + ". Try using a word like: " + exampleWord + ".")
        {

        }
    }
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
    /// <summary>
    /// This exception will be thrown whenever user violates the unique key constraint for the SQL data
    /// </summary>
    public sealed class UniqueKeyViolationException : Exception
    {
        public UniqueKeyViolationException(string message) : base(message)
        {

        }
    }

    /// <summary>
    /// Exception thrown whenever the update process remain unsuccessful
    /// </summary>
    public sealed class UpdateUnsuccessfulException : Exception
    {
        public UpdateUnsuccessfulException(string path) : base("Attempted update to the database not completed successfully. Path: " + path)
        {

        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}