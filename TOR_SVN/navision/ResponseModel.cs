using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace navision
{
    [DataContract]
    public class ResponseModel
    {
        [DataMember]
        public bool IsSuccessfull { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
    }
}