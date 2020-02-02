using EntityGrabber;
using System;
using System.Runtime.Serialization;

namespace EFarmer.Models
{
    /// <summary>
    /// Posted by a seller for an agro item
    /// </summary>
    [DataContract]
    public class Advertisement : IDataModel<long>
    {
        public long Id { get; set; }
        /// <summary>
        /// Agro Item for which the Ad has been posted
        /// </summary>
        [DataMember]
        public AgroItem Item { get; set; }
        /// <summary>
        /// City for which the ad has been posted
        /// </summary>
        [DataMember]
        public City City { get; set; }
        /// <summary>
        /// Seller who posted the ad.
        /// </summary>
        [DataMember]
        public Seller Seller { get; set; }
        /// <summary>
        /// Picture of the Ad.
        /// </summary>
        [DataMember]
        public string Picture { get; set; }
        /// <summary>
        /// Price set by the seller at the time of posting
        /// </summary>
        [DataMember]
        public double Price { get; set; }

        /// <summary>
        /// Time Stamp
        /// </summary>
        [DataMember]
        public DateTime PostedDateTime { get; set; }
        /// <summary>
        /// Quantity offered by the seller according to the weigh scale
        /// </summary>
        [DataMember]
        public short Quantity { get; set; }
        /// <summary>
        /// Set as stars(1-3)
        /// </summary>
        [DataMember]
        public short Quality { get; set; }
    }
}