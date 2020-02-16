using EFarmer.Models.Helpers;

namespace EFarmer.pk.ApiModels
{
    /// <summary>
    /// Data of user
    /// </summary>
    public class UserApiModel
    {
        /// <summary>
        /// Name of the user
        /// </summary>
        public NameFormat Name { get; set; }
        /// <summary>
        /// Country Code of the phone number
        /// </summary>
        public string CountryCode { get; set; }
        /// <summary>
        /// Comapny Code of the phone number
        /// </summary>
        public string CompanyCode { get; set; }
        /// <summary>
        /// Rest phone number
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Address of the user
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        ///Id of the city to which the user belongs
        /// </summary>
        public short CityId { get; set; }
        /// <summary>
        /// Location of the user
        /// </summary>
        public GeoLocation Location { get; set; }
        /// <summary>
        /// Primary Key
        /// </summary>
        public long Id { get; set; }
    }
}