using EntityGrabber;
using System.Runtime.Serialization;

namespace EFarmer.Models
{
    /// <summary>
    /// An items belongs to a category
    /// </summary>
    [DataContract]
    public class Category : IEntityModel<short>
    {
        /// <summary>
        /// Urdu Name of the category
        /// </summary>
        [DataMember]
        public string UrduName
        {
            get; set;
        }

        /// <summary>
        /// Name of the category
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
        public short Id
        {
            get; set;
        }
        public static Category Convert(EFarmerPkModelLibrary.Entities.CATEGORY category)
        {
            return new Category
            {
                Id = category.Id,
                Name = category.Name,
                UrduName = category.UName
            };
        }
    }
}
