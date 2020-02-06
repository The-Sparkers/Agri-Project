using EFarmer.Models.Helpers;
using EntityGrabber;
using System.Runtime.Serialization;

namespace EFarmer.Models
{
    /// <summary>
    /// City on any geography from which a user and advertisment belongs
    /// </summary>
    [DataContract]
    public class City : IEntityModel<short>
    {
        /// <summary>
        /// Location of the city onto the Geo Map
        /// </summary>
        [DataMember]
        public GeoLocation GeoLocation
        {
            get; set;
        }
        /// <summary>
        /// Name of the city
        /// </summary>
        [DataMember]
        public string Name
        {
            get; set;
        }
        /// <summary>
        /// Primary Key
        /// </summary>
        [DataMember]
        public short Id { get; set; }
        public static City Convert(EFarmerPkModelLibrary.Entities.CITY city)
        {
            return new City
            {
                GeoLocation = new GeoLocation { Latitude = city.GLat, Longitude = city.GLng },
                Id = city.Id,
                Name = city.Name
            };
        }
    }
}