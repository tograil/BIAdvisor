using System;
using System.Data;

namespace BIAdvisor.BL
{
    public interface IDeals
    {
        /// <summary>
        /// Get Lookup data for Carrier Level
        /// </summary>
        /// <param name="carrierID"></param>
        /// <param name="productLine"></param>
        /// <returns></returns>
        DataTable GetDealCarrierLevel(int? carrierID, int? productLine);

        /// <summary>
        /// Get Lookup data for Wholesaler
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        DataTable GetDealWholeSaler(string category = "Wholesaler");

        /// <summary>
        /// Get lookup data for given fields
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        DataTable GetPayeeDealAttributes(string fieldName);


        /// <summary>
        /// Get Payee Deal details for the given PayeeDealKey
        /// </summary>
        /// <param name="payeeDealKey"></param>
        /// <returns></returns>
        DataSet GetPayeeDealbyKey(int payeeDealKey = 0);


        /// <summary>
        /// Get list of Payee Deals for the given PayeeKey
        /// </summary>
        /// <param name="payeeKey"></param>
        /// <returns></returns>
        DataSet GetPayeeDealsByKey(int payeeKey);

        /// <summary>
        /// Get Schema for Payee Deals table
        /// </summary>
        /// <param name="payeeKey"></param>
        /// <returns></returns>
        DataTable GetPayeeDealsSchema(int payeeKey);

        /// <summary>
        /// Get Lookup data for Products by given Carrier and ProductLine
        /// </summary>
        /// <param name="carrier"></param>
        /// <param name="productLine"></param>
        /// <returns></returns>
        DataTable GetProductsForCarrierLine(int carrier, int productLine);

        /// <summary>
        /// Save Payee Deal
        /// </summary>
        /// <param name="PayeeDealKey"></param>
        /// <param name="PayeeKey"></param>
        /// <param name="DealCriteria"></param>
        /// <param name="StatementDate"></param>
        /// <param name="SubmitDate"></param>
        /// <param name="CarrierID"></param>
        /// <param name="ProductLine"></param>
        /// <param name="ProductID"></param>
        /// <param name="Wholesaler"></param>
        /// <param name="BrokerDealer"></param>
        /// <param name="Channel"></param>
        /// <param name="Internal"></param>
        /// <param name="ProducerID"></param>
        /// <param name="MSA"></param>
        /// <param name="Recruiter"></param>
        /// <param name="MPPartner"></param>
        /// <param name="WholesalerRole"></param>
        /// <param name="PayeeGroup"></param>
        /// <param name="CarrierAttributes"></param>
        /// <param name="ProductAttributes"></param>
        /// <param name="RecruiterAttributes"></param>
        /// <param name="BrokerDealerAttributes"></param>
        /// <param name="BrokerDealerInHierarchy"></param>
        /// <param name="CalculationCode"></param>
        /// <param name="PayIndexKey"></param>
        /// <param name="DeductionModel"></param>
        /// <param name="LOA"></param>
        /// <param name="Street"></param>
        /// <param name="Directive"></param>
        /// <param name="DealCap"></param>
        /// <param name="CarrierLevel"></param>
        /// <param name="VirtualCarrierLevel"></param>
        /// <param name="ExpDelta"></param>
        /// <param name="PctGross"></param>
        /// <param name="PctPremium"></param>
        /// <param name="PctOverride"></param>
        /// <param name="PctCarrierLevel"></param>
        /// <param name="PctWholesalerNet"></param>
        /// <param name="RecruiterDebit"></param>
        /// <param name="RecruiterCredit"></param>
        /// <param name="ContingentBonus"></param>
        /// <param name="YearEndBonus"></param>
        /// <param name="Amount1"></param>
        /// <param name="Amount2"></param>
        /// <param name="Amount3"></param>
        /// <param name="Rate1"></param>
        /// <param name="Rate2"></param>
        /// <param name="Rate3"></param>
        /// <param name="PayeeDealNotes"></param>
        /// <returns></returns>
        DataTable SavePayeeDeal(int PayeeDealKey, int PayeeKey, string DealCriteria, DateTime? StatementDate, DateTime? SubmitDate,
            int? CarrierID, short? ProductLine, int? ProductID, string Wholesaler, string BrokerDealer, string Channel, string Internal,
            int? ProducerID, string MSA, string Recruiter, string MPPartner, string WholesalerRole, string PayeeGroup, int? CarrierAttributes,
            int? ProductAttributes, int? RecruiterAttributes, int? BrokerDealerAttributes, string BrokerDealerInHierarchy, string CalculationCode,
            string PayIndexKey, string DeductionModel, string LOA, string Street, string Directive, decimal? DealCap, string CarrierLevel,
            string VirtualCarrierLevel, decimal? ExpDelta, decimal? PctGross, decimal? PctPremium, decimal? PctOverride, decimal? PctCarrierLevel,
            decimal? PctWholesalerNet, decimal? RecruiterDebit, decimal? RecruiterCredit, decimal? ContingentBonus, decimal? YearEndBonus,
            decimal? Amount1, decimal? Amount2, decimal? Amount3, decimal? Rate1, decimal? Rate2, decimal? Rate3, string PayeeDealNotes);

        /// <summary>
        /// Get Calculation Code Lookup data by Payee Type and Criteria
        /// </summary>
        /// <param name="payeeType"></param>
        /// <param name="dealCriteria"></param>
        /// <returns></returns>
        DataTable GetCalCodeByParams(string payeeType, string dealCriteria);

        /// <summary>
        /// Get Calculation Code details for the given Calculation Code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        DataTable GetCalculationCodeDetails(string code);
    }
}