using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace navision
{
    [DataContract]
    public class POrderDetailModel
    {
        [DataMember]
        public string DocumentId { get; set; }
        [DataMember]
        public int LineNo { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public double Quantity { get; set; }
        [DataMember]
        public double UnitPrice { get; set; }
        [DataMember]
        public double Amount { get; set; }
        [DataMember]
        public double AmountVAT { get; set; }
        [DataMember]
        public int VAT { get; set; }
        [DataMember]
        public string Warehouse { get; set; }
        [DataMember]
        public string Branch { get; set; }
        [DataMember]
        public DateTime DeliveryDate { get; set; }
    }
}