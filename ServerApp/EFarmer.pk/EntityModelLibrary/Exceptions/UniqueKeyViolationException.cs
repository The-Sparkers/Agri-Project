using System;

namespace EFarmer.Exceptions
{
    /// <summary>
    /// This exception will be thrown whenever user violates the unique key constraint for the SQL data
    /// </summary>
    public sealed class UniqueKeyViolationException : Exception
    {
        public UniqueKeyViolationException(string message) : base(message)
        {

        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}