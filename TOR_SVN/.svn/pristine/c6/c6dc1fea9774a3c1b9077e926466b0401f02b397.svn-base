using System;
using System.ServiceModel.Activation;

namespace navision
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class NavisionService : INavisionService
    {
        public NavisionService()
        {
            Database = new Database();
        }
        public Database Database { get; set; }

        public ResponseModel NewPODocument(string ID, string VendorName, string POStatus, string Currency, string Amount, string AmountVAT, string OrderDate,
                                          string CreationDate, string BUYERID, string USERID, string Approval1, string Approval2,
                                          string PL_LineNo, string PL_ItemDesc, string PL_Qnt, string PL_UnitPrice, string PL_Amount, string PL_AmountVAT, string PL_VAT,
                                          string PL_WH, string PL_Delivery, string PL_Branch)
        {
            var response = new ResponseModel();
            try
            {
                response.IsSuccessfull = Database.NewPODocument(ID, VendorName, POStatus, Currency, Amount, AmountVAT, OrderDate,
                                                                CreationDate, BUYERID, USERID, Approval1, Approval2,
                                                                PL_LineNo, PL_ItemDesc, PL_Qnt, PL_UnitPrice, PL_Amount, PL_AmountVAT, PL_VAT,
                                                                PL_WH, PL_Delivery, PL_Branch);
            }
            catch (Exception exp)
            {
                response.ErrorMessage = (exp.Message);
                response.IsSuccessfull = false;
            }
            return response;
        }
    }
}
