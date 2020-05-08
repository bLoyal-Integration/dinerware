using bLoyal;
using bLoyal.Connectors.Grid.Sales;
using bLoyal.Connectors.LoyaltyEngine;
using ConfigApp.DinerwareEngineService;
using ConfigApp.Provider;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigApp.Helpers
{
    public class DinerwareDBHelper : ConfigurationHelper
    {
        #region Public Member
     
        LoggerHelper logger = new LoggerHelper();

        #endregion

        #region Public Methods

        /// <summary>
        /// Get TenderType Id
        /// </summary>
        /// <param name="connectionStr"></param>
        /// <returns></returns>
        public int GetTenderTypeId(string connectionStr)
        {
            int tenderTypeId = 0;
            try
            {
                SqlConnection conn = new SqlConnection(connectionStr);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT i_tender_type_id FROM TenderTypes  where s_tender_type_name =@s_tender_type_name";
                cmd.Parameters.Add("@s_tender_type_name", SqlDbType.VarChar);
                cmd.Parameters["@s_tender_type_name"].Value = Constants.BLOYALLOYALTYTENDER;
                cmd.Connection = conn;
                conn.Open();
                tenderTypeId = int.Parse(cmd.ExecuteScalar().ToString());
                conn.Close();
                return tenderTypeId;
            }
            catch (Exception ex)
            {
                logger.WriteLogError("*** Error in GetTenderTypeId for DinerwareDBHelper= " + ex.Message);
            }
            return tenderTypeId;
        }

        /// <summary>
        /// Get bLoyal GiftCard Tender Id
        /// </summary>
        /// <returns></returns>
        public int GetbLoyalGiftCardTenderId(string databaseConnectionStr)
        {
            int tenderTypeId = 0;
            try
            {
                SqlConnection conn = new SqlConnection(databaseConnectionStr);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT i_tender_type_id FROM TenderTypes  where s_tender_type_name =@s_tender_type_name";
                cmd.Parameters.Add("@s_tender_type_name", SqlDbType.VarChar);
                cmd.Parameters["@s_tender_type_name"].Value = Constants.BLOYALGIFTCARDTENDER;
                cmd.Connection = conn;
                conn.Open();
                tenderTypeId = int.Parse(cmd.ExecuteScalar().ToString());
                conn.Close();
                return tenderTypeId;
            }
            catch (Exception)
            {
                //logger.WriteLogError("*** Error in GetTenderTypeId for DinerwareDBHelper= " + ex.Message);
            }
            return tenderTypeId;
        }
     

        /// <summary>
        /// Add TenderType
        /// </summary>
        /// <returns></returns>
        public int AddTenderType(string databaseConnectionStr)
        {
            int tenderTypeId = 0;
            try
            {
                string strSQL = "INSERT INTO TenderTypes (s_tender_type_name,i_tender_overage_behavior,i_tender_reference_behavior,i_tender_ordinal,i_tender_lookup_id,b_cancel_on_no_auth,b_tender_type_active, b_tender_type_allow_tip,b_tender_type_allow_change,b_tender_type_intrinsic,b_tender_type_visible, b_tender_type_is_tender, b_tender_type_cash_drawer, b_tender_type_is_foreign,f_tender_type_exchange_rate, b_customer_required, b_print_all_guest_checks, s_local_tender_type_name, b_use_rounding, i_receipt_copies) VALUES (@s_tender_type_name, @i_tender_overage_behavior, @i_tender_reference_behavior, @i_tender_ordinal, @i_tender_lookup_id,@b_cancel_on_no_auth,@b_tender_type_active, @b_tender_type_allow_tip, @b_tender_type_allow_change, @b_tender_type_intrinsic, @b_tender_type_visible, @b_tender_type_is_tender, @b_tender_type_cash_drawer, @b_tender_type_is_foreign, @f_tender_type_exchange_rate, @b_customer_required, @b_print_all_guest_checks, @s_local_tender_type_name,@b_use_rounding, @i_receipt_copies)";
                strSQL += "SELECT CAST(scope_identity() AS int)";
                SqlConnection conn = new SqlConnection(databaseConnectionStr);
                conn.Open();
                using (SqlCommand comd = new SqlCommand(strSQL, conn))
                {
                    comd.Parameters.Add("@s_tender_type_name", SqlDbType.VarChar);
                    //comd.Parameters["@s_tender_type_name"].Value = "bLoyal LoyaltyTender";
                    comd.Parameters["@s_tender_type_name"].Value = Constants.BLOYALLOYALTYTENDER;
                    comd.Parameters.Add("@i_tender_overage_behavior", SqlDbType.Int);
                    comd.Parameters["@i_tender_overage_behavior"].Value = 1;
                    comd.Parameters.Add("@i_tender_reference_behavior", SqlDbType.Int);
                    comd.Parameters["@i_tender_reference_behavior"].Value = 1;
                    comd.Parameters.Add("@i_tender_ordinal", SqlDbType.Int);
                    comd.Parameters["@i_tender_ordinal"].Value = 20;
                    comd.Parameters.Add("@i_tender_lookup_id", SqlDbType.Int);
                    comd.Parameters["@i_tender_lookup_id"].Value = 0;
                    comd.Parameters.Add("@b_cancel_on_no_auth", SqlDbType.Bit);
                    comd.Parameters["@b_cancel_on_no_auth"].Value = 0;
                    comd.Parameters.Add("@b_tender_type_active", SqlDbType.Bit);
                    comd.Parameters["@b_tender_type_active"].Value = 1;
                    comd.Parameters.Add("@b_tender_type_allow_tip", SqlDbType.Bit);
                    comd.Parameters["@b_tender_type_allow_tip"].Value = 0;
                    comd.Parameters.Add("@b_tender_type_allow_change", SqlDbType.Bit);
                    comd.Parameters["@b_tender_type_allow_change"].Value = 1;
                    comd.Parameters.Add("@b_tender_type_intrinsic", SqlDbType.Bit);
                    comd.Parameters["@b_tender_type_intrinsic"].Value = 0;
                    comd.Parameters.Add("@b_tender_type_visible", SqlDbType.Bit);
                    comd.Parameters["@b_tender_type_visible"].Value = 1;
                    comd.Parameters.Add("@b_tender_type_is_tender", SqlDbType.Bit);
                    comd.Parameters["@b_tender_type_is_tender"].Value = 1;
                    comd.Parameters.Add("@b_tender_type_cash_drawer", SqlDbType.Bit);
                    comd.Parameters["@b_tender_type_cash_drawer"].Value = 0;
                    comd.Parameters.Add("@b_tender_type_is_foreign", SqlDbType.Bit);
                    comd.Parameters["@b_tender_type_is_foreign"].Value = 0;
                    comd.Parameters.Add("@f_tender_type_exchange_rate", SqlDbType.Float);
                    comd.Parameters["@f_tender_type_exchange_rate"].Value = 1;
                    comd.Parameters.Add("@b_customer_required", SqlDbType.Bit);
                    comd.Parameters["@b_customer_required"].Value = 1;
                    comd.Parameters.Add("@b_print_all_guest_checks", SqlDbType.Bit);
                    comd.Parameters["@b_print_all_guest_checks"].Value = 0;
                    comd.Parameters.Add("@s_local_tender_type_name", SqlDbType.VarChar);
                    comd.Parameters["@s_local_tender_type_name"].Value = Constants.BLOYALLOYALTYTENDER;
                    //comd.Parameters["@s_local_tender_type_name"].Value = "bLoyal LoyaltyTender";
                    comd.Parameters.Add("@b_use_rounding", SqlDbType.Bit);
                    comd.Parameters["@b_use_rounding"].Value = 0;
                    comd.Parameters.Add("@i_receipt_copies", SqlDbType.Int);
                    comd.Parameters["@i_receipt_copies"].Value = 1;
                    tenderTypeId = (int)comd.ExecuteScalar();
                }
                conn.Close();
            }
            catch
            {
                //logger.WriteLogError("*** Error in AddTenderType for DinerwareDBHelper= " + ex.Message);
            }
            return tenderTypeId;
        }

        /// <summary>
        /// Add bLoyal Gift Card Tender
        /// </summary>
        /// <returns></returns>
        public int AddbLoyalGiftCardTender(string databaseConnectionStr)
        {
            int tenderTypeId = 0;
            try
            {
                string strSQL = "INSERT INTO TenderTypes (s_tender_type_name,i_tender_overage_behavior,i_tender_reference_behavior,i_tender_ordinal,i_tender_lookup_id,b_cancel_on_no_auth,b_tender_type_active, b_tender_type_allow_tip,b_tender_type_allow_change,b_tender_type_intrinsic,b_tender_type_visible, b_tender_type_is_tender, b_tender_type_cash_drawer, b_tender_type_is_foreign,f_tender_type_exchange_rate, b_customer_required, b_print_all_guest_checks, s_local_tender_type_name, b_use_rounding, i_receipt_copies) VALUES (@s_tender_type_name, @i_tender_overage_behavior, @i_tender_reference_behavior, @i_tender_ordinal, @i_tender_lookup_id,@b_cancel_on_no_auth,@b_tender_type_active, @b_tender_type_allow_tip, @b_tender_type_allow_change, @b_tender_type_intrinsic, @b_tender_type_visible, @b_tender_type_is_tender, @b_tender_type_cash_drawer, @b_tender_type_is_foreign, @f_tender_type_exchange_rate, @b_customer_required, @b_print_all_guest_checks, @s_local_tender_type_name,@b_use_rounding, @i_receipt_copies)";
                strSQL += "SELECT CAST(scope_identity() AS int)";
                SqlConnection conn = new SqlConnection(databaseConnectionStr);
                conn.Open();
                using (SqlCommand comd = new SqlCommand(strSQL, conn))
                {
                    comd.Parameters.Add("@s_tender_type_name", SqlDbType.VarChar);
                    comd.Parameters["@s_tender_type_name"].Value = Constants.BLOYALGIFTCARDTENDER;
                    comd.Parameters.Add("@i_tender_overage_behavior", SqlDbType.Int);
                    comd.Parameters["@i_tender_overage_behavior"].Value = 1;
                    comd.Parameters.Add("@i_tender_reference_behavior", SqlDbType.Int);
                    comd.Parameters["@i_tender_reference_behavior"].Value = 1;
                    comd.Parameters.Add("@i_tender_ordinal", SqlDbType.Int);
                    comd.Parameters["@i_tender_ordinal"].Value = 20;
                    comd.Parameters.Add("@i_tender_lookup_id", SqlDbType.Int);
                    comd.Parameters["@i_tender_lookup_id"].Value = 0;
                    comd.Parameters.Add("@b_cancel_on_no_auth", SqlDbType.Bit);
                    comd.Parameters["@b_cancel_on_no_auth"].Value = 0;
                    comd.Parameters.Add("@b_tender_type_active", SqlDbType.Bit);
                    comd.Parameters["@b_tender_type_active"].Value = 1;
                    comd.Parameters.Add("@b_tender_type_allow_tip", SqlDbType.Bit);
                    comd.Parameters["@b_tender_type_allow_tip"].Value = 0;
                    comd.Parameters.Add("@b_tender_type_allow_change", SqlDbType.Bit);
                    comd.Parameters["@b_tender_type_allow_change"].Value = 1;
                    comd.Parameters.Add("@b_tender_type_intrinsic", SqlDbType.Bit);
                    comd.Parameters["@b_tender_type_intrinsic"].Value = 0;
                    comd.Parameters.Add("@b_tender_type_visible", SqlDbType.Bit);
                    comd.Parameters["@b_tender_type_visible"].Value = 1;
                    comd.Parameters.Add("@b_tender_type_is_tender", SqlDbType.Bit);
                    comd.Parameters["@b_tender_type_is_tender"].Value = 1;
                    comd.Parameters.Add("@b_tender_type_cash_drawer", SqlDbType.Bit);
                    comd.Parameters["@b_tender_type_cash_drawer"].Value = 0;
                    comd.Parameters.Add("@b_tender_type_is_foreign", SqlDbType.Bit);
                    comd.Parameters["@b_tender_type_is_foreign"].Value = 0;
                    comd.Parameters.Add("@f_tender_type_exchange_rate", SqlDbType.Float);
                    comd.Parameters["@f_tender_type_exchange_rate"].Value = 1;
                    comd.Parameters.Add("@b_customer_required", SqlDbType.Bit);
                    comd.Parameters["@b_customer_required"].Value = 1;
                    comd.Parameters.Add("@b_print_all_guest_checks", SqlDbType.Bit);
                    comd.Parameters["@b_print_all_guest_checks"].Value = 0;
                    comd.Parameters.Add("@s_local_tender_type_name", SqlDbType.VarChar);
                    comd.Parameters["@s_local_tender_type_name"].Value = Constants.BLOYALGIFTCARDTENDER;
                    comd.Parameters.Add("@b_use_rounding", SqlDbType.Bit);
                    comd.Parameters["@b_use_rounding"].Value = 0;
                    comd.Parameters.Add("@i_receipt_copies", SqlDbType.Int);
                    comd.Parameters["@i_receipt_copies"].Value = 1;
                    tenderTypeId = (int)comd.ExecuteScalar();
                }
                conn.Close();
            }
            catch
            {
                //logger.WriteLogError("*** Error in AddTenderType for DinerwareDBHelper= " + ex.Message);
            }
            return tenderTypeId;
        }    
     

        /// <summary>
        /// Add Discount Rule
        /// </summary>
        /// <param name="discountAmount"></param>
        /// <returns></returns>
        public int AddDiscountRule(decimal discountAmount, string discountDescription, string discountName, string discountUid, string discountType, string databaseConnectionStr)
        {
            int discountId = 0;
            try
            {
                string strSQL = "INSERT INTO DiscountType ( [s_desc],[s_name],[c_amount],[c_amount_delta],[f_percent],[b_amount],[b_amount_delta],[b_percent],[b_monday],[b_tuesday],[b_wednesday],[b_thursday],[b_friday],[b_saturday],[b_sunday],[b_applies_tickets],[b_applies_items],[b_active],[b_auto_apply],[i_ordinal], [b_open_discount] ,[g_DiscountType_id]) VALUES(@s_desc,@s_name,@c_amount,@c_amount_delta,@f_percent,@b_amount,@b_amount_delta,@b_percent,@b_monday,@b_tuesday,@b_wednesday,@b_thursday,@b_friday,@b_saturday,@b_sunday,@b_applies_tickets,@b_applies_items,@b_active,@b_auto_apply,@i_ordinal,@b_open_discount, @g_DiscountType_id)";
                strSQL += "SELECT CAST(scope_identity() AS int)";
                SqlConnection conn = new SqlConnection(databaseConnectionStr);
                conn.Open();
                using (SqlCommand comd = new SqlCommand(strSQL, conn))
                {
                    comd.Parameters.Add("@s_desc", SqlDbType.VarChar);
                    comd.Parameters["@s_desc"].Value = discountDescription;
                    comd.Parameters.Add("@s_name", SqlDbType.VarChar);
                    //comd.Parameters["@s_name"].Value = "Discount";
                    comd.Parameters["@s_name"].Value = discountName;
                    comd.Parameters.Add("@c_amount", SqlDbType.Decimal);
                    comd.Parameters["@c_amount"].Value = 0.00;
                    comd.Parameters.Add("@b_amount_delta", SqlDbType.Bit);
                    comd.Parameters["@b_amount_delta"].Value = 1;
                    comd.Parameters.Add("@c_amount_delta", SqlDbType.Decimal);
                    comd.Parameters["@c_amount_delta"].Value = discountAmount;
                    comd.Parameters.Add("@f_percent", SqlDbType.Float);
                    comd.Parameters["@f_percent"].Value = 0;
                    comd.Parameters.Add("@b_amount", SqlDbType.Float);
                    comd.Parameters["@b_amount"].Value = 0;
                    comd.Parameters.Add("@b_percent", SqlDbType.Float);
                    comd.Parameters["@b_percent"].Value = 0;
                    comd.Parameters.Add("@b_monday", SqlDbType.Bit);
                    comd.Parameters["@b_monday"].Value = 1;
                    comd.Parameters.Add("@b_tuesday", SqlDbType.Bit);
                    comd.Parameters["@b_tuesday"].Value = 1;
                    comd.Parameters.Add("@b_wednesday", SqlDbType.Bit);
                    comd.Parameters["@b_wednesday"].Value = 1;
                    comd.Parameters.Add("@b_thursday", SqlDbType.Bit);
                    comd.Parameters["@b_thursday"].Value = 1;
                    comd.Parameters.Add("@b_friday", SqlDbType.Bit);
                    comd.Parameters["@b_friday"].Value = 1;
                    comd.Parameters.Add("@b_saturday", SqlDbType.Bit);
                    comd.Parameters["@b_saturday"].Value = 1;
                    comd.Parameters.Add("@b_sunday", SqlDbType.Bit);
                    comd.Parameters["@b_sunday"].Value = 1;
                    comd.Parameters.Add("@b_applies_tickets", SqlDbType.Bit);
                    comd.Parameters["@b_applies_tickets"].Value = !string.IsNullOrEmpty(discountType) && discountType.Equals(Constants.TICKET) ? 1 : 0;
                    comd.Parameters.Add("@b_applies_items", SqlDbType.Bit);
                    comd.Parameters["@b_applies_items"].Value = !string.IsNullOrEmpty(discountType) && discountType.Equals(Constants.ITEMS) ? 1 : 0;
                    comd.Parameters.Add("@b_active", SqlDbType.Bit);
                    comd.Parameters["@b_active"].Value = 1;
                    comd.Parameters.Add("@b_auto_apply", SqlDbType.Bit);
                    comd.Parameters["@b_auto_apply"].Value = 0;
                    comd.Parameters.Add("@b_open_discount", SqlDbType.Bit);
                    // Add discount for manual 
                    comd.Parameters["@b_open_discount"].Value = 1;
                    //comd.Parameters["@b_open_discount"].Value = 0;
                    comd.Parameters.Add("@i_ordinal", SqlDbType.Int);
                    comd.Parameters["@i_ordinal"].Value = 0;
                    //comd.Parameters.Add("@s_discounttype_export_alias", SqlDbType.VarChar);
                    //comd.Parameters["@s_discounttype_export_alias"].Value = transactionTokenStr;
                    comd.Parameters.Add("@g_DiscountType_id", SqlDbType.VarChar);
                    comd.Parameters["@g_DiscountType_id"].Value = !string.IsNullOrEmpty(discountUid) ? discountUid : Guid.NewGuid().ToString();
                    discountId = (int)comd.ExecuteScalar();
                }
                conn.Close();
                return discountId;
            }
            catch
            {
                //logger.WriteLogError("*** Error in AddDiscountRule for DinerwareDBHelper= " + ex.Message);
            }
            return discountId;

        }

    
        /// <summary>
        /// Get Discount Rule discountUid
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetDiscountRuleByName(string discountName, string databaseConnectionStr)
        {
            int discountId = 0;
            try
            {
                SqlConnection conn = new SqlConnection(databaseConnectionStr);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT i_discount_type_id FROM DiscountType where  s_name = @s_name";
                cmd.Parameters.Add("@s_name", SqlDbType.VarChar);
                cmd.Parameters["@s_name"].Value = discountName;
                cmd.Connection = conn;
                conn.Open();
                discountId = int.Parse(cmd.ExecuteScalar().ToString());
                conn.Close();
                return discountId;
            }
            catch (Exception)
            {
                //logger.WriteLogError("*** Error in GetDiscountRuleByName for DinerwareDBHelper= " + ex.Message);
            }
            return discountId;
        }

        
     
        /// <summary>
        /// Get all Screen Categories
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllScreenCategories()
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection sqlConnection = new SqlConnection(DATABASE_CONNECTION_STR);
                SqlCommand cmd = new SqlCommand("SELECT *FROM ScreenCategory");
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
                sqlConnection.Close();
                return ds;
            }
            catch (Exception exception)
            {
                //logger.WriteLogError("*** Error in GetAllScreenCategories for DinerwareDBHelper= " + exception.Message);
                //throw exception;
            }
            return null;
        }

        /// <summary>
        /// Get All Revenue Classes
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllRevenueClasses()
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection sqlConnection = new SqlConnection(DATABASE_CONNECTION_STR);
                SqlCommand cmd = new SqlCommand("SELECT *FROM RevenueClass");
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
                sqlConnection.Close();
                return ds;
            }
            catch (Exception exception)
            {
                //logger.WriteLogError("*** Error in GetAllRevenueClasses for DinerwareDBHelper= " + exception.Message);
                //throw exception;
            }
            return null;
        }

      
        #endregion

        #region Private Methods
       

        public void DinerwareLogging(string msg)
        {
            try
            {
                //using (StreamWriter writer = new StreamWriter("D:\\DinerwareErrorLogging.txt", true))
                //{
                //    writer.WriteLine(msg);
                //}
            }
            catch (Exception ex)
            {
            }
        }

        #endregion
    }
}
