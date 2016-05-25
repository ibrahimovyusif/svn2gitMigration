using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Transactions;

namespace navision
{
    public class Database
    {
        private readonly SqlConnection _conn;
        public Database()
        {
            string con_str = ConfigurationManager.AppSettings["connectionString"].ToString();
            _conn = new SqlConnection(con_str);
        }

        public bool NewPODocument(string ID, string VendorName, string POStatus, string Currency, string Amount, string AmountVAT, string OrderDate,
                                  string CreationDate, string BUYERID, string USERID, string Approval1, string Approval2,
                                  string PL_LineNo, string PL_ItemDesc, string PL_Qnt, string PL_UnitPrice, string PL_Amount, string PL_AmountVAT, string PL_VAT,
                                  string PL_WH, string PL_Delivery, string PL_Branch)
        {
            using (var t = new TransactionScope())
            {
                try
                {
                    OpenConnection();

                    if (POStatus == "1")
                    {
                        POrderInputModel model = new POrderInputModel();
                        model.Amount = double.Parse(Amount, CultureInfo.InvariantCulture);
                        model.AmountVAT = double.Parse(AmountVAT, CultureInfo.InvariantCulture);
                        model.BuyerId = BUYERID;
                        model.CreateDate = ConvertStringToDateTime(CreationDate);
                        model.Currency = Currency;
                        model.DocumentId = ID;
                        model.OrderDate = ConvertStringToDateTime(OrderDate);
                        model.POStatus = Convert.ToInt32(POStatus);
                        model.Username = USERID;
                        model.Vendor = VendorName;
                        model.Approval1 = Approval1;
                        model.Approval2 = Approval2;

                        const string sql = "INSERT INTO NavisionPOrders (DocumentId, Vendor, Currency, OrderDate, Amount, AmountVAT, POStatus, Username, CreateDate, BuyerId, Approval1, Approval2) " +
                                                      "VALUES (@DocumentId, @Vendor, @Currency, @OrderDate, @Amount, @AmountVAT, @POStatus, @Username, @CreateDate, @BuyerId, @Approval1, @Approval2 ) ";

                        using (var cmd = new SqlCommand(sql, _conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@DocumentId", CheckForDbNull(model.DocumentId));
                            cmd.Parameters.AddWithValue("@Vendor", CheckForDbNull(model.Vendor));
                            cmd.Parameters.AddWithValue("@Currency", CheckForDbNull(model.Currency));
                            cmd.Parameters.AddWithValue("@OrderDate", CheckForDbNull(model.OrderDate));
                            cmd.Parameters.AddWithValue("@Amount", CheckForDbNull(model.Amount));
                            cmd.Parameters.AddWithValue("@AmountVAT", CheckForDbNull(model.AmountVAT));
                            cmd.Parameters.AddWithValue("@POStatus", model.POStatus);
                            cmd.Parameters.AddWithValue("@Username", CheckForDbNull(model.Username));
                            cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@BuyerId", CheckForDbNull(model.BuyerId));
                            cmd.Parameters.AddWithValue("@Approval1", CheckForDbNull(model.Approval1));
                            cmd.Parameters.AddWithValue("@Approval2", CheckForDbNull(model.Approval2));
                            cmd.ExecuteNonQuery();
                        }

                        //Detail
                        POrderDetailSetModel detailSet = new POrderDetailSetModel();
                        detailSet.AmountSet = PL_Amount;
                        detailSet.AmountVATSet = PL_AmountVAT;
                        detailSet.BranchSet = PL_Branch;
                        detailSet.DescriptionSet = PL_ItemDesc;
                        detailSet.LineNoSet = PL_LineNo;
                        detailSet.QuantitySet = PL_Qnt;
                        detailSet.UnitPriceSet = PL_UnitPrice;
                        detailSet.VATSet = PL_VAT;
                        detailSet.WarehouseSet = PL_WH;
                        detailSet.DeliveryDateSet = PL_Delivery;

                        List<int> lineNoList = ConvertStringToSet(detailSet.LineNoSet, '~', "int").Cast<int>().ToList();
                        List<string> descList = ConvertStringToSet(detailSet.DescriptionSet, '~', "string").Cast<string>().ToList();
                        List<double> quantityList = ConvertStringToSet(detailSet.QuantitySet, '~', "double").Cast<double>().ToList();
                        List<double> unitPriceList = ConvertStringToSet(detailSet.UnitPriceSet, '~', "double").Cast<double>().ToList();
                        List<double> amountList = ConvertStringToSet(detailSet.AmountSet, '~', "double").Cast<double>().ToList();
                        List<double> amountVatList = ConvertStringToSet(detailSet.AmountVATSet, '~', "double").Cast<double>().ToList();
                        List<int> vatList = ConvertStringToSet(detailSet.VATSet, '~', "int").Cast<int>().ToList();
                        List<string> warehouseList = ConvertStringToSet(detailSet.WarehouseSet, '~', "string").Cast<string>().ToList();
                        List<string> branchList = ConvertStringToSet(detailSet.BranchSet, '~', "string").Cast<string>().ToList();
                        List<DateTime> deliveryDateList = ConvertStringToSet(detailSet.DeliveryDateSet, '~', "date").Cast<DateTime>().ToList();

                        int count = lineNoList.Count;
                        List<POrderDetailModel> list = new List<POrderDetailModel>();

                        for (int i = 0; i < count; i++)
                        {
                            POrderDetailModel detail = new POrderDetailModel();
                            detail.DocumentId = ID;
                            detail.Amount = amountList[i];
                            detail.AmountVAT = amountVatList[i];
                            detail.Branch = branchList[i];
                            detail.DeliveryDate = deliveryDateList[i];
                            detail.Description = descList[i];
                            detail.LineNo = lineNoList[i];
                            detail.Quantity = quantityList[i];
                            detail.UnitPrice = unitPriceList[i];
                            detail.VAT = vatList[i];
                            detail.Warehouse = warehouseList[i];
                            list.Add(detail);
                        }

                        foreach (var item in list)
                        {
                            const string sqlDetail = "INSERT INTO NavisionPOrderDetails (DocumentId, [LineNo], [Description], Quantity, UnitPrice, Amount, AmountVAT, VAT, Warehouse, Branch, DeliveryDate ) " +
                                                                          "VALUES (@DocumentId, @LineNo, @Description, @Quantity, @UnitPrice, @Amount, @AmountVAT, @VAT, @Warehouse, @Branch, @DeliveryDate ) ";

                            using (var cmd = new SqlCommand(sqlDetail, _conn))
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@DocumentId", CheckForDbNull(item.DocumentId));
                                cmd.Parameters.AddWithValue("@LineNo", CheckForDbNull(item.LineNo));
                                cmd.Parameters.AddWithValue("@Description", CheckForDbNull(item.Description));
                                cmd.Parameters.AddWithValue("@Quantity", CheckForDbNull(item.Quantity));
                                cmd.Parameters.AddWithValue("@UnitPrice", CheckForDbNull(item.UnitPrice));
                                cmd.Parameters.AddWithValue("@Amount", CheckForDbNull(item.Amount));
                                cmd.Parameters.AddWithValue("@AmountVAT", CheckForDbNull(item.AmountVAT));
                                cmd.Parameters.AddWithValue("@VAT", CheckForDbNull(item.VAT));
                                cmd.Parameters.AddWithValue("@Warehouse", CheckForDbNull(item.Warehouse));
                                cmd.Parameters.AddWithValue("@Branch", CheckForDbNull(item.Branch));
                                cmd.Parameters.AddWithValue("@DeliveryDate", CheckForDbNull(item.DeliveryDate));
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    else if (POStatus == "0")
                    {
                        //delete order
                        const string sqlDelete = "DELETE FROM NavisionPOrders WHERE DocumentId = @DocumentId ";
                        using (var cmd = new SqlCommand(sqlDelete, _conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@DocumentId", CheckForDbNull(ID));
                            cmd.ExecuteNonQuery();
                        }

                        //delete order
                        const string sqlDetailDelete = "DELETE FROM NavisionPOrderDetails WHERE DocumentId = @DocumentId ";
                        using (var cmd = new SqlCommand(sqlDetailDelete, _conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@DocumentId", CheckForDbNull(ID));
                            cmd.ExecuteNonQuery();
                        }

                        //delete history
                        /////////////////////////////////////////////////////////////////
                    }
                    else
                    {
                        throw new Exception("POStatus value is not 0 or 1. Value is " + POStatus);
                    }

                    CloseConnection();
                    t.Complete();
                    return true;
                }
                catch (Exception ex)
                {
                    CloseConnection();
                    t.Dispose();
                    //var st = new StackTrace(ex, true);
                    //var frame = st.GetFrame(0);
                    //var line = frame.GetFileLineNumber();
                    //throw new Exception(ex.Message + " - " + line.ToString());
                    throw;
                }
            }
        }


        public void OpenConnection()
        {
            if (_conn.State != System.Data.ConnectionState.Open)
            {
                _conn.Open();
            }
        }

        public void CloseConnection()
        {
            if (_conn.State != System.Data.ConnectionState.Closed)
            {
                _conn.Close();
            }
        }

        public object CheckForDbNull(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            return value;
        }

        public static List<object> ConvertStringToSet(string str, char c, string type)
        {
            List<object> result = new List<object>();

            if (string.IsNullOrEmpty(str))
            {
                return result;
            }

            int length = str.Length;
            if (str[length - 1] == '~')
            {
                str = str.Remove(length - 1);
            }

            List<string> list = str.Split(c).ToList();

            foreach (var item in list)
            {
                if (type == "string")
                {
                    result.Add(item);
                }
                else if (type == "int")
                {
                    int parsed;
                    if (Int32.TryParse(item, out parsed) == true)
                    {
                        result.Add(parsed);
                    }
                }
                else if (type == "double")
                {
                    double parsed;
                    if (double.TryParse(item, NumberStyles.Number, CultureInfo.InvariantCulture, out parsed) == true)
                    {
                        result.Add(parsed);
                    }
                }
                else if (type == "date")
                {
                    string[] formats = new string[] { "yyyyMMdd" };
                    DateTime parsed;
                    if (DateTime.TryParseExact(item, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
                    {
                        result.Add(parsed);
                    }
                }
            }

            return result;
        }

        public static DateTime? ConvertStringToDateTime(string strDate)
        {
            string[] formats = new string[] { "yyyyMMdd" };

            DateTime result;
            if (DateTime.TryParseExact(strDate, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return result;
            }
            return null;
        }

        //public bool SavePOrder(POrderInputModel model)
        //{
        //    try
        //    {
        //        const string sql = "INSERT INTO NavisionPOrders (DocumentId, Vendor, CurrencyCode, OrderDate, Amount, AmountVAT, POStatus, Username, CreateDate, BuyerId, Approval1, Approval2) " +
        //                                            "VALUES (@DocumentId, @Vendor, @CurrencyCode, @OrderDate, @Amount, @AmountVAT, @POStatus, @Username, @CreateDate, @BuyerId, @Approval1, @Approval2 ) ";

        //        using (var cmd = new SqlCommand(sql, _conn))
        //        {
        //            cmd.CommandType = CommandType.Text;
        //            cmd.Parameters.AddWithValue("@DocumentId", CheckForDbNull(model.DocumentId));
        //            cmd.Parameters.AddWithValue("@Vendor", CheckForDbNull(model.Vendor));
        //            cmd.Parameters.AddWithValue("@CurrencyCode", CheckForDbNull(model.Currency));
        //            cmd.Parameters.AddWithValue("@OrderDate", CheckForDbNull(model.OrderDate));
        //            cmd.Parameters.AddWithValue("@Amount", CheckForDbNull(model.Amount));
        //            cmd.Parameters.AddWithValue("@AmountVAT", CheckForDbNull(model.AmountVAT));
        //            cmd.Parameters.AddWithValue("@POStatus", model.POStatus);
        //            cmd.Parameters.AddWithValue("@Username", CheckForDbNull(model.Username));
        //            cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
        //            cmd.Parameters.AddWithValue("@BuyerId", CheckForDbNull(model.BuyerId));
        //            cmd.Parameters.AddWithValue("@Approval1", CheckForDbNull(model.Approval1));
        //            cmd.Parameters.AddWithValue("@Approval2", CheckForDbNull(model.Approval2));
        //            cmd.ExecuteNonQuery();
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        var st = new StackTrace(ex, true);
        //        var frame = st.GetFrame(0);
        //        var line = frame.GetFileLineNumber();
        //        throw new Exception(ex.Message + " - " + line.ToString());
        //    }
        //}
        //public bool SavePOrderDetail(string documentId, POrderDetailSetModel model)
        //{
        //    try
        //    {
        //        List<int> lineNoList = ConvertStringToSet(model.LineNoSet, '~', "int").Cast<int>().ToList();
        //        List<string> descList = ConvertStringToSet(model.DescriptionSet, '~', "string").Cast<string>().ToList();
        //        List<double> quantityList = ConvertStringToSet(model.QuantitySet, '~', "double").Cast<double>().ToList();
        //        List<double> unitPriceList = ConvertStringToSet(model.UnitPriceSet, '~', "double").Cast<double>().ToList();
        //        List<double> amountList = ConvertStringToSet(model.AmountSet, '~', "double").Cast<double>().ToList();
        //        List<double> amountVatList = ConvertStringToSet(model.AmountVATSet, '~', "double").Cast<double>().ToList();
        //        List<int> vatList = ConvertStringToSet(model.VATSet, '~', "int").Cast<int>().ToList();
        //        List<string> warehouseList = ConvertStringToSet(model.WarehouseSet, '~', "string").Cast<string>().ToList();
        //        List<string> branchList = ConvertStringToSet(model.BranchSet, '~', "string").Cast<string>().ToList();
        //        List<DateTime> deliveryDateList = ConvertStringToSet(model.DeliveryDateSet, '~', "date").Cast<DateTime>().ToList();

        //        int count = lineNoList.Count;
        //        List<POrderDetailModel> list = new List<POrderDetailModel>();

        //        for (int i = 0; i < count; i++)
        //        {
        //            POrderDetailModel detail = new POrderDetailModel();
        //            detail.Amount = amountList[i];
        //            detail.AmountVAT = amountVatList[i];
        //            detail.Branch = branchList[i];
        //            detail.DeliveryDate = deliveryDateList[i];
        //            detail.Description = descList[i];
        //            detail.LineNo = lineNoList[i];
        //            detail.Quantity = quantityList[i];
        //            detail.UnitPrice = unitPriceList[i];
        //            detail.VAT = vatList[i];
        //            detail.Warehouse = warehouseList[i];
        //        }

        //        foreach (var item in list)
        //        {
        //            const string sql = "INSERT INTO NavisionPOrderDetails (DocumentId, Description, Quantity, UnitPrice, Amount, AmountVAT, VAT, Warehouse, Branch, DeliveryDate ) " +
        //                                            "VALUES (@DocumentId, @Description, @Quantity, @UnitPrice, @Amount, @AmountVAT, @VAT, @Warehouse, @Branch, @DeliveryDate ) ";

        //            using (var cmd = new SqlCommand(sql, _conn))
        //            {
        //                cmd.CommandType = CommandType.Text;
        //                cmd.Parameters.AddWithValue("@DocumentId", CheckForDbNull(documentId));
        //                cmd.Parameters.AddWithValue("@Description", CheckForDbNull(item.Description));
        //                cmd.Parameters.AddWithValue("@Quantity", CheckForDbNull(item.Quantity));
        //                cmd.Parameters.AddWithValue("@UnitPrice", CheckForDbNull(item.UnitPrice));
        //                cmd.Parameters.AddWithValue("@Amount", CheckForDbNull(item.Amount));
        //                cmd.Parameters.AddWithValue("@AmountVAT", CheckForDbNull(item.AmountVAT));
        //                cmd.Parameters.AddWithValue("@VAT", CheckForDbNull(item.VAT));
        //                cmd.Parameters.AddWithValue("@Warehouse", CheckForDbNull(item.Warehouse));
        //                cmd.Parameters.AddWithValue("@Branch", CheckForDbNull(item.Branch));
        //                cmd.Parameters.AddWithValue("@DeliveryDate", CheckForDbNull(item.DeliveryDate));
        //                cmd.ExecuteNonQuery();
        //            }
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        var st = new StackTrace(ex, true);
        //        var frame = st.GetFrame(0);
        //        var line = frame.GetFileLineNumber();
        //        throw new Exception(ex.Message + " - " + line.ToString());
        //    }
        //}

        // var result = ns.NewPODocument("020PO00011", "Tamilla Aliyeva Test PO 5", "1", "USD", "100.00", "101.00", "20160108", "20160309",
        //          "BUYERID", "USERID",
        //          "Approval1", "Approval2", "1000~2000~", "Test Item 1~Test Item2~", "10~20~", "10,00~15,00~", "100,00~300.00~", "200,00~400.00~", "0~0~", "Head Office WH~Head Office WH~",
        //           "20160309" + "~" + "20160309", "Head Office~Head office~");

        //if (result.IsSuccessfull)
        //{
        //    Console.WriteLine("yessss");
        //}
        //else
        //{
        //    Console.WriteLine(result.ErrorMessage);
        //}


    }
}