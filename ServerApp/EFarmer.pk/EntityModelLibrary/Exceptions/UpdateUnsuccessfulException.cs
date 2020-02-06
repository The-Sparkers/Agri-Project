using System;

namespace EFarmer.Exceptions
{
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