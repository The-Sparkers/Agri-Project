using EFarmer.Models.Helpers;
using EntityGrabber;
using System.Runtime.Serialization;

namespace EFarmer.Models
{
    /// <summary>
    /// User is the actor of the system who interacts with the system (maybe a Buyer or seller)
    /// </summary>
    [DataContract]
    public class User : IEntityModel<long>
    {
        /// <summary>
        /// City to which the user belongs
        /// </summary>
        [DataMember]
        public City City
        {
            get; set;
        }
        /// <summary>
        /// Latest location based on the cellular GPS Information
        /// </summary>
        [DataMember]
        public GeoLocation Location
        {
            get; set;
        }
        /// <summary>
        /// Physical Address of the User
        /// </summary>
        [DataMember]
        public string Address
        {
            get; set;
        }
        /// <summary>
        /// Contact Number of the user
        /// </summary>
        [DataMember]
        public ContactNumberFormat ContactNumber
        {
            get; set;
        }
        /// <summary>
        /// Full Name of the user
        /// </summary>
        [DataMember]
        public NameFormat Name
        {
            get; set;
        }
        [DataMember]
        public bool IsSeller { get; set; }
        [DataMember]
        public bool IsBuyer { get; set; }
        [DataMember]
        public long Id { get; set; }

        public static User Convert(EFarmerPkModelLibrary.Entities.USER user)
        {

            return new User
            {
                Address = user.Address,
                City = City.Convert(user.City),
                ContactNumber = new ContactNumberFormat(user.CCountryCode, user.CCompanyCode, user.CPhone),
                Id = user.Id,
                IsBuyer = user.BuyerFlag,
                IsSeller = user.SellerFlag,
                Location = new GeoLocation { Latitude = user.GLat, Longitude = user.GLng },
                Name = new NameFormat { FirstName = user.FName, LastName = user.LName }
            };
        }
    }
}
