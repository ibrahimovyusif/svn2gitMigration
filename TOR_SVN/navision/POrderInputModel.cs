using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace navision
{
    [DataContract]
    public class POrderInputModel
    {
        [DataMember]
        public string DocumentId { get; set; }
        [DataMember]
        public string Vendor { get; set; }
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public DateTime? OrderDate { get; set; }
        [DataMember]
        public double Amount { get; set; }
        [DataMember]
        public double AmountVAT { get; set; }
        [DataMember]
        public int POStatus { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public DateTime? CreateDate { get; set; }
        [DataMember]
        public string BuyerId { get; set; }
        [DataMember]
        public string Approval1 { get; set; }
        [DataMember]
        public string Approval2 { get; set; }
    }
}