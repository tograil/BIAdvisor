using System;
using System.Data;

namespace BIAdvisor.BL
{
    public interface IPayee
    {
        /// <summary>
        /// Add New Payee 
        /// </summary>
        /// <param name="Payee">Payee Name</param>
        /// <param name="PayeeType">Payee Type</param>
        /// <param name="Role">Payee Role</param>
        /// <param name="EffectiveDate">Effective Date</param>
        /// <param name="ExpirationDate">Expiration Date (Should be greater than Effective Date)</param>
        /// <returns></returns>
        DataRow AddNewPayee(string Payee, string PayeeType, string Role, DateTime EffectiveDate, DateTime ExpirationDate);

        /// <summary>
        /// Copy Payee Details into a new Payee Record.
        /// </summary>
        /// <param name="copyPayeeId">PayeeID to be copied</param>
        /// <param name="copyName">Payee Name</param>
        /// <param name="copyEffectiveDate">Effective Date for the new record</param>
        /// <param name="copyExpirationDate">Expiration Date for the new record</param>
        /// <param name="copyDeals">True if deals need to be copied</param>
        /// <returns></returns>
        DataRow CopyPayee(int copyPayeeId, string copyName, DateTime copyEffectiveDate, DateTime copyExpirationDate, bool copyDeals);

        /// <summary>
        /// Delete Payee by the given PayeeKey
        /// </summary>
        /// <param name="PayeeKey"></param>
        /// <returns></returns>
        bool DeletePayee(int PayeeKey);

        /// <summary>
        /// Delete Payee Deal by the given PayeeDealKey
        /// </summary>
        /// <param name="payeeDealKey"></param>
        /// <returns></returns>
        object DeletePayeeDeal(int payeeDealKey);

        /// <summary>
        /// Get Lookup data for Blacksmith Code
        /// </summary>
        /// <returns></returns>
        DataTable GetBlacksmith();

        /// <summary>
        /// Get Lookup data for Deduction
        /// </summary>
        /// <param name="payeeType"></param>
        /// <returns></returns>
        DataTable GetLookupDeduction(string payeeType);

        /// <summary>
        /// Get Lookup data for Model
        /// </summary>
        /// <param name="payeeType"></param>
        /// <param name="payeeRole"></param>
        /// <returns></returns>
        DataTable GetLookupModel(string payeeType, string payeeRole);

        /// <summary>
        /// Get Lookup data for Payee Type
        /// </summary>
        /// <returns></returns>
        DataTable GetLookupPayeeType();

        /// <summary>
        /// Get Payee Details for the given PayeeKey
        /// </summary>
        /// <param name="payeeKey"></param>
        /// <returns></returns>
        DataSet GetPayeeDetailsByKey(int payeeKey);

        /// <summary>
        /// Get Lookup data for Payee Index
        /// </summary>
        /// <returns></returns>
        DataTable GetPayeeIndex(string PayeeType, string PayeeRole);

        /// <summary>
        /// Get Schema Details for the Payee list for the search page.
        /// </summary>
        /// <returns></returns>
        DataTable GetPayeesSchema();

        /// <summary>
        /// Get Payee List for the search page
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="role"></param>
        /// <param name="timeframe"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        DataTable GetPayeesBySearch(string name = "", string type = "", string role = "", string timeframe = "", bool? model = null);

        /// <summary>
        /// Get Lookup data for Payee Name dropdown
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="role"></param>
        /// <param name="timeframe"></param>
        /// <returns></returns>
        DataTable GetPayeesForLookup(string name, string type, string role, string timeframe);

        /// <summary>
        /// Get Lookup data for PayeeType Table dropdown
        /// </summary>
        /// <returns></returns>
        DataTable GetPayeeTypeRole();

        /// <summary>
        /// Save Payee details.
        /// </summary>
        /// <param name="PayeeKey"></param>
        /// <param name="Published"></param>
        /// <param name="EffectiveDate"></param>
        /// <param name="ExpirationDate"></param>
        /// <param name="PayeeModel"></param>
        /// <param name="PayToName"></param>
        /// <param name="COCPCT"></param>
        /// <param name="BlacksmithCode"></param>
        /// <param name="PayeeIndexKey"></param>
        /// <param name="DeductionModel"></param>
        /// <param name="MethodBCheck"></param>
        /// <param name="DoNotPay"></param>
        /// <param name="Model"></param>
        /// <param name="PayeeGroup"></param>
        /// <param name="TBD"></param>
        /// <param name="PayeeNotes"></param>
        /// <returns></returns>
        bool SavePayeeDetails(int PayeeKey, string Published, DateTime EffectiveDate, DateTime ExpirationDate, string PayeeModel, string PayToName, decimal? COCPCT, string BlacksmithCode, string PayeeIndexKey, string DeductionModel, string MethodBCheck, string DoNotPay, string Model, string PayeeGroup, string TBD, string PayeeNotes);

        /// <summary>
        /// Validate Payee Effective and Expiration Date for the given Payee, Type and Published flag.
        /// </summary>
        /// <param name="payeeName"></param>
        /// <param name="payeeType"></param>
        /// <param name="isPublished"></param>
        /// <param name="effectiveDate"></param>
        /// <param name="expirationDate"></param>
        /// <param name="payeeKey"></param>
        /// <returns></returns>
        bool ValidatePayeeOverlapDates(string payeeName, string payeeType, bool isPublished, DateTime effectiveDate, DateTime expirationDate, int payeeKey = 0);

        /// <summary>
        /// Get Lookup data for Payee Groups
        /// </summary>
        /// <param name="payeeType"></param>
        /// <param name="payeeRole"></param>
        /// <returns></returns>
        DataTable GetLookupPayeeGroups(string payeeType, string payeeRole);
    }
}