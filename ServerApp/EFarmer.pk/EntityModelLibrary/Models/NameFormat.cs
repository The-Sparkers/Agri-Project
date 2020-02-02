using EFarmer.Models.Exceptions;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace EFarmer.Models.Helpers
{
    [DataContract]
    public class NameFormat
    {
        string firstName, lastName;
        /// <summary>
        /// Person's First Name.
        /// </summary>
        [DataMember]
        public string FirstName
        {
            get
            {
                string firstLetterCapital = firstName.Substring(0, 1).ToUpper();
                string restWordSmall = firstName.Substring(1).ToLower();
                return firstLetterCapital + restWordSmall;
            }
            set
            {
                Regex regexForFirstName = new Regex(@"([A-Z]|[a-z])+");
                if (regexForFirstName.IsMatch(value))
                {
                    firstName = value;
                }
                else
                {
                    throw new ValidationPatternNotMatchException(firstName, regexForFirstName.ToString(), "Ahmed or ahmed");
                }
            }
        }
        /// <summary>
        /// Person's Last Name.
        /// </summary>
        [DataMember]
        public string LastName
        {
            get
            {
                string firstLetterCapital = lastName.Substring(0, 1).ToUpper();
                string restWordSmall = lastName.Substring(1).ToLower();
                return firstLetterCapital + restWordSmall;
            }
            set
            {
                Regex regexForLastName = new Regex(@"([A-Z]|[a-z])+");
                if (regexForLastName.IsMatch(value))
                {
                    lastName = value;
                }
                else
                {
                    throw new ValidationPatternNotMatchException(lastName, regexForLastName.ToString(), "Azam or azam");
                }
            }
        }

        public override string ToString()
        {
            return firstName + " " + lastName;
        }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
