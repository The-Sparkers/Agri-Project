using EFarmer.Exceptions;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace EFarmer.Models.Helpers
{
    /// <summary>
    /// Class to store user's contact details.
    /// </summary>
    [DataContract]
    public class ContactNumberFormat
    {
        readonly string phoneNumber, countryCode, companyCode;
        /// <summary>
        /// Constructor to initiate contact number class values
        /// </summary>
        public ContactNumberFormat(string countryCode, string companyCode, string phoneNumber)
        {
            Regex regexForCountryCode = new Regex(@"[+][1-9][1-9]");
            if (!regexForCountryCode.IsMatch(countryCode))
            {
                throw new ValidationPatternNotMatchException(countryCode, regexForCountryCode.ToString(), "+92");
            }
            Regex regexForCompanyCode = new Regex(@"[3][0-9][0-9]");
            if (!regexForCompanyCode.IsMatch(companyCode))
            {
                throw new ValidationPatternNotMatchException(companyCode, regexForCompanyCode.ToString(), "301");
            }
            Regex regexForPhoneNumber = new Regex(@"\b\d{7,7}\b");
            if (!regexForPhoneNumber.IsMatch(phoneNumber))
            {
                throw new ValidationPatternNotMatchException(phoneNumber, regexForPhoneNumber.ToString(), "1234567");
            }
            this.companyCode = companyCode;
            this.countryCode = countryCode;
            this.phoneNumber = phoneNumber;
        }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public string CountryCode => countryCode;
        public string CompanyCode => companyCode;
        public string PhoneNumber => phoneNumber;
        /// <summary>
        /// Method which will return the full phone number.
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string PhoneNumberFormat => countryCode + companyCode + phoneNumber;

        /// <summary>
        /// Method to get phone number in (+xx-xxx-xxxxxxx) format
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string InternationalFormatedPhoneNumber => countryCode + "-" + companyCode + "-" + phoneNumber;

        /// <summary>
        /// Method to get phone number in (0xxx-xxxxxxx)
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string LocalFormatedPhoneNumber => "0" + companyCode + "-" + phoneNumber;
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
