using EntityGrabber;
using System.Runtime.Serialization;

namespace EFarmer.Models
{
    /// <summary>
    /// Item for which the ads will be posted
    /// </summary>
    [DataContract]
    public class AgroItem : IEntityModel<int>
    {
        private readonly int id;
        private readonly string name;
        private readonly string uName;
        private readonly string scale;
        private readonly Category category;
        private readonly string uWeightScale;

        /// <summary>
        /// Category to which this item belongs
        /// </summary>
        [DataMember]
        public Category Category
        {
            get; set;
        }
        /// <summary>
        /// Weight Scale in urdu
        /// </summary>
        [DataMember]
        public string UrduWeightScale
        {
            get; set;
        }
        /// <summary>
        /// Weight scale from which this item's weight is denoted
        /// </summary>
        [DataMember]
        public string WeightScale
        {
            get; set;
        }
        /// <summary>
        /// Name of this item in urdu
        /// </summary>
        [DataMember]
        public string UrduName
        {
            get; set;
        }
        /// <summary>
        /// Name of the item
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
        public int Id { get; set; }
        public static AgroItem Convert(EFarmerPkModelLibrary.Entities.AGROITEM agroItem)
        {
            return new AgroItem
            {
                Category = Category.Convert(agroItem.CATEGORY),
                Id = agroItem.Id,
                Name = agroItem.Name,
                UrduName = agroItem.Uname,
                UrduWeightScale = agroItem.UWeightScale,
                WeightScale = agroItem.WeightScale
            };
        }
    }
}