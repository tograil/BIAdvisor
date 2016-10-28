using BIAdvisor.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BIAdvisor.BL
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class Payee : IPayee
    {
        private readonly string connectionString;

        public Payee()
        {
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
        }

        public DataSet GetPayeeDetailsByKey(int payeeKey)
        {
            if (payeeKey == 0)
            {
                return null;
            }

            string storedProcName = "uspdsPayeeDetailsGetByKey";
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

        public DataTable GetPayeeTypeRole()
        {
            string commandText = "Select * From vdsPayeeType";
            try
            {
                return DBHelper.ExecuteDataset(connectionString, CommandType.Text, commandText).Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetLookupPayeeType()
        {
            string commandText = "uspdsPayeeSearchLookupPayeeType";
            try
            {
                return DBHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, commandText).Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetLookupModel(string payeeType, string payeeRole)
        {
            string commandText = "uspdsPayeeLookupModel";
            try
            {
                List<SqlParameter> iParams = new List<SqlParameter>();
                iParams.Add(new SqlParameter("PayeeType", payeeType));
                iParams.Add(new SqlParameter("PayeeRole", payeeRole));
                var ds = DBHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, commandText, iParams);
                return ds.Tables.Count > 0 ? ds.Tables[0] : null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Search for published records matching payeeName, payeeType which are marked as published 
        /// and fall overlap these passed dates.
        /// </summary>
        /// <param name="payeeName"></param>
        /// <param name="payeeType"></param>
        /// <param name="effectiveDate"></param>
        /// <param name="expirationDate"></param>
        /// <returns></returns>
        public bool ValidatePayeeOverlapDates(string payeeName, string payeeType, bool isPublished, DateTime effectiveDate,
            DateTime expirationDate, int payeeKey = 0)
        {
            string commandText = "uspdsPayeeSearchActiveContract";
            try
            {
                List<SqlParameter> iParams = new List<SqlParameter>();
                iParams.Add(new SqlParameter("Payee", payeeName));
                iParams.Add(new SqlParameter("PayeeType", payeeType));
                iParams.Add(new SqlParameter("IsPublished", isPublished));
                iParams.Add(new SqlParameter("EffectiveDate", effectiveDate));
                iParams.Add(new SqlParameter("ExpirationDate", expirationDate));
                iParams.Add(new SqlParameter("PayeeKey", payeeKey));
                var ds = DBHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, commandText, iParams);
                var dt = ds.Tables.Count > 0 ? ds.Tables[0] : null;
                return dt != null && dt.Rows.Count > 0 ? false : true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetLookupDeduction(string payeeType)
        {
            string commandText = "uspdsPayeeLookupDeductionModel";
            try
            {
                List<SqlParameter> iParams = new List<SqlParameter>();
                iParams.Add(new SqlParameter("PayeeType", payeeType));
                return DBHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, commandText, iParams).Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataRow CopyPayee(int copyPayeeId, string copyName, DateTime copyEffectiveDate, DateTime copyExpirationDate, bool copyDeals)
        {
            string commandText = "uspdsPayeeCopy";
            try
            {
                List<SqlParameter> iParams = new List<SqlParameter>();
                iParams.Add(new SqlParameter("PayeeKey", copyPayeeId));
                iParams.Add(new SqlParameter("CopyName", string.IsNullOrEmpty(copyName) ? null : copyName));
                iParams.Add(new SqlParameter("CopyEffectiveDate", copyEffectiveDate));
                iParams.Add(new SqlParameter("CopyExpirationDate", copyExpirationDate));
                iParams.Add(new SqlParameter("CopyDeals", copyDeals));

                var ds = DBHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, commandText, iParams);
                return ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0] : null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public object DeletePayeeDeal(int payeeDealKey)
        {
            try
            {
                string storedProcName = "uspdsPayeeDealDelete";

                List<SqlParameter> iParams = new List<SqlParameter>();
                iParams.Add(new SqlParameter("PayeeDealKey", payeeDealKey));

                DBHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, storedProcName, iParams);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetPayeeIndex(string PayeeType = "", string PayeeRole = "")
        {
            string commandText = "uspdsPayeeLookupPayIndex";
            try
            {
                List<SqlParameter> iParams = new List<SqlParameter>();
                iParams.Add(new SqlParameter("PayeeType", PayeeType));
                iParams.Add(new SqlParameter("PayeeRole", PayeeRole));

                return DBHelper.ExecuteDataset(connectionString, CommandType.Text, commandText).Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetBlacksmith()
        {
            string commandText = "uspdsPayeeLookupBlacksmith";
            try
            {
                return DBHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, commandText).Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetPayeesSchema()
        {
            return DBHelper.GetSchemaTable(connectionString, CommandType.StoredProcedure, "uspdsPayeeSearch", new List<SqlParameter>());
        }

        public DataTable GetPayeesBySearch(string name = "", string type = "", string role = "", string timeframe = "", bool? model = null)
        {
            string commandText = "uspdsPayeeSearch";
            try
            {
                //name = name.Length > 0 ? name + "%" : name;
                List<SqlParameter> iParams = new List<SqlParameter>();
                iParams.Add(new SqlParameter("Payee", string.IsNullOrEmpty(name) ? null : name + "%"));
                iParams.Add(new SqlParameter("PayeeType", string.IsNullOrEmpty(type) ? null : type.Split(':')[0]));
                iParams.Add(new SqlParameter("PayeeRole", string.IsNullOrEmpty(role) ? null : role));
                iParams.Add(new SqlParameter("Timeframe", string.IsNullOrEmpty(timeframe) ? null : timeframe));
                if (model == true)
                {
                    iParams.Add(new SqlParameter("Model", "Y"));
                }
                var ds = DBHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, commandText, iParams);
                return ds.Tables.Count > 0 ? ds.Tables[0] : null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetPayeesForLookup(string name, string type, string role, string timeframe)
        {
            string commandText = "uspdsPayeeSearchLookupPayee";
            try
            {
                //name = name.Length > 0 ? name + "%" : name;
                List<SqlParameter> iParams = new List<SqlParameter>();
                iParams.Add(new SqlParameter("Payee", string.IsNullOrEmpty(name) ? null : name + "%"));
                iParams.Add(new SqlParameter("PayeeType", string.IsNullOrEmpty(type) ? null : type));
                iParams.Add(new SqlParameter("PayeeRole", string.IsNullOrEmpty(role) ? null : role));
                iParams.Add(new SqlParameter("Timeframe", string.IsNullOrEmpty(timeframe) ? null : timeframe));

                var ds = DBHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, commandText, iParams);
                return ds.Tables.Count > 0 ? ds.Tables[0] : null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SavePayeeDetails(int PayeeKey, string Published, DateTime EffectiveDate, DateTime ExpirationDate, string PayeeModel,
            string PayToName, decimal? COCPCT, string BlacksmithCode, string PayeeIndexKey, string DeductionModel, string MethodBCheck,
            string DoNotPay, string Model, string PayeeGroup, string TBD, string PayeeNotes)
        {
            try
            {
                string storedProcName = "uspdsPayeeSaveChanges";

                List<SqlParameter> iParams = new List<SqlParameter>();
                iParams.Add(new SqlParameter("PayeeKey", PayeeKey));
                iParams.Add(new SqlParameter("Published", Published));
                iParams.Add(new SqlParameter("EffectiveDate", EffectiveDate));
                iParams.Add(new SqlParameter("ExpirationDate", ExpirationDate));
                iParams.Add(new SqlParameter("PayeeModel", PayeeModel));
                iParams.Add(new SqlParameter("PayToName", PayToName));
                iParams.Add(new SqlParameter("COCPCT", COCPCT));
                iParams.Add(new SqlParameter("BlacksmithCode", BlacksmithCode));
                iParams.Add(new SqlParameter("PayeeIndexKey", PayeeIndexKey));
                iParams.Add(new SqlParameter("DeductionModel", DeductionModel));
                iParams.Add(new SqlParameter("MethodBCheck", MethodBCheck));
                iParams.Add(new SqlParameter("DoNotPay", DoNotPay));
                iParams.Add(new SqlParameter("Model", Model));
                iParams.Add(new SqlParameter("PayeeGroup", PayeeGroup));
                iParams.Add(new SqlParameter("TBD", TBD));
                iParams.Add(new SqlParameter("PayeeNotes", PayeeNotes));

                DBHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, storedProcName, iParams);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool DeletePayee(int PayeeKey)
        {
            try
            {
                string storedProcName = "uspdsPayeeDelete";

                List<SqlParameter> iParams = new List<SqlParameter>();
                iParams.Add(new SqlParameter("PayeeKey", PayeeKey));

                DBHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, storedProcName, iParams);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataRow AddNewPayee(string Payee, string PayeeType, string Role, DateTime EffectiveDate, DateTime ExpirationDate)
        {
            try
            {
                string storedProcName = "uspdsPayeeAddNew";

                List<SqlParameter> iParams = new List<SqlParameter>();
                iParams.Add(new SqlParameter("Payee", Payee));
                iParams.Add(new SqlParameter("PayeeType", PayeeType));
                iParams.Add(new SqlParameter("Role", Role));
                iParams.Add(new SqlParameter("EffectiveDate", EffectiveDate));
                iParams.Add(new SqlParameter("ExpirationDate", ExpirationDate));

                var ds = DBHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, storedProcName, iParams);
                return ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0] : null;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public DataTable GetLookupPayeeGroups(string payeeType, string payeeRole)
        {
            string commandText = "uspdsPayeeLookupPayeeGroup";
            try
            {
                List<SqlParameter> iParams = new List<SqlParameter>();
                iParams.Add(new SqlParameter("PayeeType", payeeType));
                iParams.Add(new SqlParameter("PayeeRole", payeeRole));
                var ds = DBHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, commandText, iParams);

                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
