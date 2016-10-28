using BIAdvisor.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BIAdvisor.BL
{
    public class Deals : IDeals
    {
        private string connectionString;

        public Deals()
        {
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
        }


        public DataSet GetPayeeDealsByKey(int payeeKey)
        {
            if (payeeKey == 0)
            {
                return null;
            }

            string storedProcName = "uspdsPayeeGetPayeeDeals";
            try
            {
                List<SqlParameter> iParam = new List<SqlParameter>();

                iParam.Add(new SqlParameter("PayeeKey", payeeKey.ToString()));

                return DBHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, storedProcName, iParam);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetPayeeDealsSchema(int payeeKey)
        {
            return DBHelper.GetSchemaTable(connectionString, CommandType.StoredProcedure, "uspdsPayeeGetPayeeDeals",
                new List<SqlParameter>() { new SqlParameter("PayeeKey", payeeKey.ToString()) });
        }


        public DataSet GetPayeeDealbyKey(int payeeDealKey = 0)
        {
            if (payeeDealKey == 0)
            {
                return null;
            }

            string commandText = "uspdsPayeeDealGetByKey";
            try
            {
                List<SqlParameter> iParams = new List<SqlParameter>();
                iParams.Add(new SqlParameter("PayeeDealKey", payeeDealKey));

                return DBHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, commandText, iParams);

            }
            catch (Exception)
            {
                throw;
            }
        }


        public DataTable GetPayeeDealAttributes(string fieldName)
        {
            string commandText = "";
            switch (fieldName.ToLower())
            {
                case "carrier":
                    commandText = "uspdsPayeeDealLookupCarrier";
                    break;
                case "carrierattribute":
                    commandText = "uspdsPayeeDealLookupCarrierAttribute";
                    break;
                case "criteriacombination":
                    commandText = "uspdsPayeeDealLookupCriteriaCombination";
                    break;
                case "directive":
                    commandText = "uspdsPayeeDealLookupDirective";
                    break;
                case "product":
                    commandText = "uspdsPayeeDealLookupProduct";
                    break;
                case "productattribute":
                    commandText = "uspdsPayeeDealLookupProductAttribute";
                    break;
                case "brokerdealer":
                    commandText = "uspdsPayeeDealLookupBrokerDealer";
                    break;
                case "brokerdealerattribute":
                    commandText = "uspdsPayeeDealLookupBrokerDealerAttribute";
                    break;
                case "channel":
                    commandText = "uspdsPayeeDealLookupChannel";
                    break;
                case "internal":
                    commandText = "uspdsPayeeDealLookupInternal";
                    break;
                case "producer":
                    commandText = "uspdsPayeeDealLookupProducer";
                    break;
                case "recruiter":
                    commandText = "uspdsPayeeDealLookupRecruiter";
                    break;
                case "recruiterattribute":
                    commandText = "uspdsPayeeDealLookupRecruiterAttribute";
                    break;
                case "mppartner":
                    commandText = "uspdsPayeeDealLookupMPPartner";
                    break;
                case "payeegroup":
                    commandText = "uspdsPayeeDealLookupPayeeGroup";
                    break;
                case "wholesaler":
                    commandText = "uspdsPayeeDealLookupWholesaler";
                    break;
                case "calculationcode":
                    commandText = "uspdsPayeeDealLookupCalculationCode";
                    break;
                case "payindex":
                    commandText = "uspdsPayeeDealLookupPayIndex";
                    break;
                default:
                    break;
            }
            try
            {
                DataSet resultSet = DBHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, commandText);
                if (resultSet.Tables.Count > 0)
                {
                    return resultSet.Tables[0];
                }
                else
                {
                    return new DataTable();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }


        public DataTable GetCalculationCodeDetails(string id)
        {
            try
            {
                string commandText = "uspdsPayeeDealCalculationCodeGetByKey";
                List<SqlParameter> iParams = new List<SqlParameter>();
                iParams.Add(new SqlParameter("CalculationID", id));
                DataSet resultSet = DBHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, commandText, iParams);

                return resultSet.Tables.Count > 0 && resultSet.Tables[0].Rows.Count > 0
                    ? resultSet.Tables[0] : new DataTable();
            }
            catch (Exception)
            {

                throw;
            }
        }


        public DataTable GetCalCodeByParams(string payeeType, string dealCarrier)
        {
            try
            {
                if (payeeType == null || dealCarrier == null)
                {
                    return null;
                }
                string commandText = "uspdsPayeeDealLookupCalculationCode";
                List<SqlParameter> iParams = new List<SqlParameter>();
                iParams.Add(new SqlParameter("PayeeType", payeeType));
                iParams.Add(new SqlParameter("DealCriteria", dealCarrier));
                DataSet resultSet = DBHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, commandText, iParams);

                if (resultSet.Tables.Count > 0)
                {
                    return resultSet.Tables[0];
                }
                else
                {
                    return new DataTable();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public DataTable GetDealCarrierLevel(int? carrierID, int? productLine)
        {
            try
            {
                if (carrierID == null && productLine == null)
                {
                    return null;
                }

                string commandText = "uspdsPayeeDealLookupCarrierLevel";
                List<SqlParameter> iParams = new List<SqlParameter>();
                iParams.Add(new SqlParameter("CarrierID", carrierID));
                iParams.Add(new SqlParameter("ProductLine", productLine));
                DataSet resultSet = DBHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, commandText, iParams);
                if (resultSet.Tables.Count > 0)
                {
                    return resultSet.Tables[0];
                }
                else
                {
                    return new DataTable();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public DataTable GetDealWholeSaler(string category = "Wholesaler")
        {
            try
            {
                string commandText = "uspdsPayeeDealLookupWholesaler";
                List<SqlParameter> iParams = new List<SqlParameter>();
                iParams.Add(new SqlParameter("Category", category));
                DataSet resultSet = DBHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, commandText, iParams);
                if (resultSet.Tables.Count > 0)
                {
                    return resultSet.Tables[0];
                }
                else
                {
                    return new DataTable();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public DataTable GetProductsForCarrierLine(int carrier, int productLine)
        {
            try
            {
                string commandText = "uspdsPayeeDealLookupProduct";
                List<SqlParameter> iParams = new List<SqlParameter>();
                iParams.Add(new SqlParameter("CarrierID", carrier));
                iParams.Add(new SqlParameter("ProductLine", productLine));
                DataSet resultSet = DBHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, commandText, iParams);
                if (resultSet.Tables.Count > 0)
                {
                    return resultSet.Tables[0];
                }
                else
                {
                    return new DataTable();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public DataTable SavePayeeDeal(int PayeeDealKey, int PayeeKey, string DealCriteria, DateTime? StatementDate, DateTime? SubmitDate,
            int? CarrierID, short? ProductLine, int? ProductID, string Wholesaler, string BrokerDealer, string Channel, string Internal,
            int? ProducerID, string MSA, string Recruiter, string MPPartner, string WholesalerRole, string PayeeGroup, int? CarrierAttributes,
            int? ProductAttributes, int? RecruiterAttributes, int? BrokerDealerAttributes, string BrokerDealerInHierarchy, string CalculationID,
            string PayIndexKey, string DeductionModel, string LOA, string Street, string Directive, decimal? DealCap, string CarrierLevel,
            string VirtualCarrierLevel, decimal? ExpDelta, decimal? PctGross, decimal? PctPremium, decimal? PctOverride, decimal? PctCarrierLevel,
            decimal? PctWholesalerNet, decimal? RecruiterDebit, decimal? RecruiterCredit, decimal? ContingentBonus, decimal? YearEndBonus,
            decimal? Amount1, decimal? Amount2, decimal? Amount3, decimal? Rate1, decimal? Rate2, decimal? Rate3, string Notes)
        {
            try
            {
                string storedProcName = "uspdsPayeeDealSaveChanges";

                List<SqlParameter> iParams = new List<SqlParameter>();
                iParams.Add(new SqlParameter("PayeeDealKey", PayeeDealKey));
                iParams.Add(new SqlParameter("PayeeKey", PayeeKey));
                iParams.Add(new SqlParameter("DealCriteria", DealCriteria));
                iParams.Add(new SqlParameter("StatementDate", StatementDate));
                iParams.Add(new SqlParameter("SubmitDate", SubmitDate));
                iParams.Add(new SqlParameter("CarrierID", CarrierID));
                iParams.Add(new SqlParameter("ProductLine", ProductLine));
                iParams.Add(new SqlParameter("ProductID", ProductID));
                iParams.Add(new SqlParameter("Wholesaler", Wholesaler));
                iParams.Add(new SqlParameter("BrokerDealer", BrokerDealer));
                iParams.Add(new SqlParameter("Channel", Channel));
                iParams.Add(new SqlParameter("Internal", Internal));
                iParams.Add(new SqlParameter("ProducerID", ProducerID));
                iParams.Add(new SqlParameter("MSA", MSA));
                iParams.Add(new SqlParameter("Recruiter", Recruiter));
                iParams.Add(new SqlParameter("MPPartner", MPPartner));
                iParams.Add(new SqlParameter("WholesalerRole", WholesalerRole));
                iParams.Add(new SqlParameter("PayeeGroup", PayeeGroup));
                iParams.Add(new SqlParameter("CarrierAttributes", CarrierAttributes));
                iParams.Add(new SqlParameter("ProductAttributes", ProductAttributes));
                iParams.Add(new SqlParameter("RecruiterAttributes", RecruiterAttributes));
                iParams.Add(new SqlParameter("BrokerDealerAttributes", BrokerDealerAttributes));
                iParams.Add(new SqlParameter("BrokerDealerInHierarchy", BrokerDealerInHierarchy));
                iParams.Add(new SqlParameter("Notes", Notes));

                iParams.Add(new SqlParameter("CalculationID", CalculationID));
                iParams.Add(new SqlParameter("PayIndexKey", PayIndexKey));
                iParams.Add(new SqlParameter("DeductionModel", DeductionModel));
                iParams.Add(new SqlParameter("LOA", LOA));
                iParams.Add(new SqlParameter("Street", Street));
                iParams.Add(new SqlParameter("Directive", Directive));
                iParams.Add(new SqlParameter("ExpDelta", ExpDelta));
                iParams.Add(new SqlParameter("DealCap", DealCap));
                iParams.Add(new SqlParameter("CarrierLevel", CarrierLevel));
                iParams.Add(new SqlParameter("VirtualCarrierLevel", VirtualCarrierLevel));
                iParams.Add(new SqlParameter("PctGross", PctGross));
                iParams.Add(new SqlParameter("PctPremium", PctPremium));
                iParams.Add(new SqlParameter("PctOverride", PctOverride));
                iParams.Add(new SqlParameter("PctCarrierLevel", PctCarrierLevel));
                iParams.Add(new SqlParameter("PctWholesalerNet", PctWholesalerNet));
                iParams.Add(new SqlParameter("RecruiterDebit", RecruiterDebit));
                iParams.Add(new SqlParameter("RecruiterCredit", RecruiterCredit));
                iParams.Add(new SqlParameter("ContingentBonus", ContingentBonus));
                iParams.Add(new SqlParameter("YearEndBonus", YearEndBonus));
                iParams.Add(new SqlParameter("Amount1", Amount1));
                iParams.Add(new SqlParameter("Amount2", Amount2));
                iParams.Add(new SqlParameter("Amount3", Amount3));
                iParams.Add(new SqlParameter("Rate1", Rate1));
                iParams.Add(new SqlParameter("Rate2", Rate2));
                iParams.Add(new SqlParameter("Rate3", Rate3));

                var ds = DBHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, storedProcName, iParams);
                return ds != null && ds.Tables.Count > 0 ? ds.Tables[0] : null;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
