using System.ServiceModel;

namespace navision
{
    [ServiceContract]
    public interface INavisionService
    {
        [OperationContract]
        ResponseModel NewPODocument(string ID, string VendorName, string POStatus, string Currency, string Amount, string AmountVAT, string OrderDate,
                                    string CreationDate, string BUYERID, string USERID, string Approval1, string Approval2,
                                    string PL_LineNo, string PL_ItemDesc, string PL_Qnt, string PL_UnitPrice, string PL_Amount, string PL_AmountVAT, string PL_VAT,
                                    string PL_WH, string PL_Delivery, string PL_Branch);
    }
}
