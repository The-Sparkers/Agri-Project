using EntityGrabber;
using System.Runtime.Serialization;

namespace EFarmer.Models
{
    /// <summary>
    /// An items belongs to a category
    /// </summary>
    [DataContract]
    public class Category : IDataModel<short>
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

    }
}
