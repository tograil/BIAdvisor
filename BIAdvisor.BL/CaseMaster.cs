using BIAdvisor.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BIAdvisor.BL
{
    public class CaseMaster
    {
        private string connectionString;
        public CaseMaster()
        {
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
        }

        public DataTable GetCaseMasterDDAttributes(string fieldName)
        {
            string commandText = "";
            switch (fieldName.ToLower())
            {
                
                case "brokerdealer":
                    commandText = "uspdsCaseMasterLookupBrokerDealer";
                    break;                
                case "channel":
                    commandText = "uspdsCaseMasterLookupChannel";
                    break;
                case "plchannel":
                    commandText = "uspdsCaseMasterLookupPlChannel";
                    break;
                case "internal":
                    commandText = "uspdsCaseMasterLookupInternal";
                    break;
                case "recruiter":
                    commandText = "uspdsCaseMasterLookupRecruiter";
                    break;
                case "wholesaler":
                    commandText = "uspdsCaseMasterLookupWholesaler";
                    break;
                case "paytowholesaler":
                    commandText = "uspdsCaseMasterLookupPaytowholesaler";
                    break;
                case "nsm":
                    commandText = "uspdsCaseMasterLookupNSM";
                    break;
                case "thrubd":
                    commandText = "uspdsCaseMasterLookupThruBD";
                    break;
                case "agency":
                    commandText = "uspdsCaseMasterLookupAgency";
                    break;
                case "agencymp":
                    commandText = "uspdsCaseMasterLookupAgencyMP";
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

        public DataSet GetCaseMasterResults(long CaseID, string PolicyNo, string Agent, string Wholesaler, string Paytowholesaler)
        {
            string storedProc = "uspdsCaseMasterSearch";
            try
            {
                List<SqlParameter> iParam = new List<SqlParameter>();

                iParam.Add(CaseID == 0 ? new SqlParameter("CaseID", null) : new SqlParameter("CaseID", CaseID));
                iParam.Add(string.IsNullOrEmpty(PolicyNo) ? new SqlParameter("PolNo", null) : new SqlParameter("PolNo", PolicyNo));
                iParam.Add(string.IsNullOrEmpty(Agent) ? new SqlParameter("Agent", null) : new SqlParameter("Agent", Agent));
                iParam.Add(string.IsNullOrEmpty(Wholesaler) ? new SqlParameter("Wholesaler", null) : new SqlParameter("Wholesaler", Wholesaler));
                iParam.Add(string.IsNullOrEmpty(Paytowholesaler) ? new SqlParameter("PayToWholesaler", null) : new SqlParameter("PayToWholesaler", Paytowholesaler));

                return DBHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, storedProc, iParam);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet GetCaseMasterArchiveResults(long CaseID, string PolicyNo, string Agent, string Wholesaler, string Paytowholesaler)
        {
            string storedProc = "uspdsCaseMasterArchiveSearch";
            try
            {
                List<SqlParameter> iParam = new List<SqlParameter>();

                iParam.Add(CaseID == 0 ? new SqlParameter("CaseID", null) : new SqlParameter("CaseID", CaseID));
                iParam.Add(string.IsNullOrEmpty(PolicyNo) ? new SqlParameter("PolNo", null) : new SqlParameter("PolNo", PolicyNo));
                iParam.Add(string.IsNullOrEmpty(Agent) ? new SqlParameter("Agent", null) : new SqlParameter("Agent", Agent));
                iParam.Add(string.IsNullOrEmpty(Wholesaler) ? new SqlParameter("Wholesaler", null) : new SqlParameter("Wholesaler", Wholesaler));
                iParam.Add(string.IsNullOrEmpty(Paytowholesaler) ? new SqlParameter("PayToWholesaler", null) : new SqlParameter("PayToWholesaler", Paytowholesaler));

                return DBHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, storedProc, iParam);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet GetCaseMasterResultByKey(long AGPCOMID)
        {
            string storedProc = "uspdsCaseMasterSearchByKey";
            if (AGPCOMID == 0)
            {
                return null;
            }
            try
            {
                List<SqlParameter> iParam = new List<SqlParameter>();
                iParam.Add(new SqlParameter("AGPCOMID", AGPCOMID));

                return DBHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, storedProc, iParam);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet GetCaseMasterArchiveResultByKey(long AGPCOMID)
        {
            string storedProc = "uspdsCaseMasterArchiveSearchByKey";
            if (AGPCOMID == 0)
            {
                return null;
            }
            try
            {
                List<SqlParameter> iParam = new List<SqlParameter>();
                iParam.Add(new SqlParameter("AGPCOMID", AGPCOMID));

                return DBHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, storedProc, iParam);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable SaveEditCaseMasterRecord(bool IsEdit, long AGPCOMID, 
             string Wholesaler, string Internal, string Recruiter, string Channel, string Brokerdealer, string PayToWholesaler,
            string PlChannel, string ThruBD, string Agency, string AgencyMP, string NSM, DateTime ArchiveDate, string ArchiveReason)
        {
            string storedProc = "uspdsCaseMasterSaveEditedRecord";
            try
            {
                List<SqlParameter> iParam = new List<SqlParameter>();

                iParam.Add(new SqlParameter("IsEdit", IsEdit));
                iParam.Add(new SqlParameter("AGPCOMID", AGPCOMID));
                iParam.Add(new SqlParameter("Wholesaler", Wholesaler));
                iParam.Add(new SqlParameter("Internal", Internal));
                iParam.Add(new SqlParameter("Recruiter", Recruiter));
                iParam.Add(new SqlParameter("Channel", Channel));
                iParam.Add(new SqlParameter("Brokerdealer", Brokerdealer));
                iParam.Add(new SqlParameter("PayToWholesaler", PayToWholesaler));
                iParam.Add(new SqlParameter("PlChannel", PlChannel));
                iParam.Add(new SqlParameter("ThruBD", ThruBD));
                iParam.Add(new SqlParameter("Agency", Agency));
                iParam.Add(new SqlParameter("AgencyMP", AgencyMP));
                iParam.Add(new SqlParameter("NSM", NSM));
                iParam.Add(new SqlParameter("ArchiveDate", ArchiveDate));
                iParam.Add(new SqlParameter("ArchiveReason", ArchiveReason));

                var ds = DBHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, storedProc, iParam);
                return ds != null && ds.Tables.Count > 0 ? ds.Tables[0] : null;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
