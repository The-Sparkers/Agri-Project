using System;

namespace EFarmer.Exceptions
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class ValidationPatternNotMatchException : Exception
    {
        public ValidationPatternNotMatchException(string stringValue, string pattern, string exampleWord) : base("Your string: " + stringValue + " failed to matched with the Pattern: " + pattern + ". Try using a word like: " + exampleWord + ".")
        {

        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}