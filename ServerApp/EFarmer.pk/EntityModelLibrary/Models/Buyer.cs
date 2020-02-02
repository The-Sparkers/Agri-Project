using System.Runtime.Serialization;

namespace EFarmer.Models
{
    /// <summary>
    /// User of the system which uses the system to post buying related advertisements
    /// </summary>
    [DataContract]
    public class Buyer : User
    {
        /// <summary>
        /// Initializes Values from Db
        /// </summary>
        /// <param name="id">Primary Key</param>
        public Buyer(long id) : base(id)
        {
            if (!isBuyer)
            {
                Copy(null);
            }
        }
        /// <summary>
        /// Buyer Flag
        /// </summary>
        [DataMember]
        public bool IsSeller
        {
            get => isSeller;
        }
    }
}
