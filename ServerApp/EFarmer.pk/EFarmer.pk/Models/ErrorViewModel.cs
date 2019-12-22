using System;
using System.Runtime.Serialization;

namespace EFarmer.pk.Models
{
    [DataContract]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class ErrorViewModel
    {
        [DataMember]
        public string RequestId { get; set; }
        [DataMember]
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}