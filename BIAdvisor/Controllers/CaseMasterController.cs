using BIAdvisor.BL;
using BIAdvisor.Web.Helpers;
using BIAdvisor.Web.Models.CaseMaster;
using System;
using System.Data;
using System.Web.Mvc;

namespace BIAdvisor.Web.Controllers
{
    public class CaseMasterController : BaseController
    {

        private CaseMaster caseMaster;

        //Initialize Repositories (DB Read/Write Methods)
        public CaseMasterController()
        {
            caseMaster = new CaseMaster();
        }


        /// <summary>
        /// Get the search results for Case Master Search Page
        /// </summary>
        /// <returns></returns>
        /// 
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get the search results for Case Master Archive page
        /// </summary>
        /// <returns></returns>
        public ActionResult Archives()
        {
            return View();
        }

        public PartialViewResult _CaseMasterArchiveResults(string SortOrder, string SortColumn, long CaseID = 0, string PolicyNo = "", string Agent = "", string WholeSaler = "", string PayToWholeSaler = "")
        {
            SearchCMAViewModel model = new SearchCMAViewModel
            {
                CaseID = CaseID.ToString(),
                Agent = Agent,
                PayToWholeSaler = PayToWholeSaler,
                PolicyNo = PolicyNo,
                WholeSaler = WholeSaler,
            };

            SortColumn = (string.IsNullOrEmpty(SortColumn)) ? "Name" : SortColumn;
            SortOrder = (!string.IsNullOrEmpty(SortOrder) && SortOrder.ToLower().Equals("asc")) ? "DESC" : "ASC";

            model.SortOrder = SortOrder;
            model.SortColumn = SortColumn;
            var results = caseMaster.GetCaseMasterArchiveResults(CaseID, PolicyNo, Agent, WholeSaler, PayToWholeSaler).Tables[0];
            DataView dv = new DataView(results, null, SortColumn + " " + SortOrder, DataViewRowState.CurrentRows);
            model.Results = dv.ToTable();
            return PartialView(model.Results);
        }

        public PartialViewResult _ViewCaseMasterArchiveRecord(string AGPCOMID)
        {
            if (string.IsNullOrEmpty(AGPCOMID))
            {
                return PartialView(new CaseMasterArchiveViewModel());
            }
            else
            {
                long key = Convert.ToInt64(AGPCOMID);
                var result = caseMaster.GetCaseMasterArchiveResultByKey(key).Tables[0].Rows[0];
                CaseMasterArchiveViewModel model = new CaseMasterArchiveViewModel
                {
                    AGPCOMID = AGPCOMID,
                    CaseID = Convert.ToInt64(result["CaseID"]),
                    PolNo = Convert.ToString(result["PolNo"]),
                    CrtDate = Convert.ToDateTime(result["CrtDate"]),
                    AgtNo = Convert.ToInt64(result["AgtNo"]),
                    AgtName = Convert.ToString(result["Name"]),
                    AgtPct = Convert.ToDecimal(result["AgtPct"]),
                    ComLvl = Convert.ToInt64(result["ComLvl"]),
                    Wholesaler = Convert.ToString(result["Wholesaler"]),
                    PayToWholesaler = Convert.ToString(result["PayToWholesaler"]),
                    Internal = Convert.ToString(result["Internal"]),
                    Recruiter = Convert.ToString(result["Recruiter"]),
                    Channel = Convert.ToString(result["Channel"]),
                    PlChannel = Convert.ToString(result["PlChannel"]),
                    NSM = Convert.ToString(result["NSM"]),
                    BrokeDealer = Convert.ToString(result["BrokerDealer"]),
                    ThruBD = Convert.ToString(result["ThruBD"]),
                    Agency = Convert.ToString(result["Agency"]),
                    AgencyMP = Convert.ToString(result["AgencyMP"]),
                    ArchiveDateTime = Convert.ToDateTime(result["ArchiveDate"]),
                    ArchiveReason = Convert.ToString(result["ArchiveReason"])
                };
                return PartialView(model);

            }
        }
        public PartialViewResult _CaseMasterResults(string SortOrder, string SortColumn, long CaseID = 0, string PolicyNo = "", string Agent = "", string WholeSaler = "", string PayToWholeSaler = "")
        {
            SearchCMViewModel model = new SearchCMViewModel
            {
                CaseID = CaseID.ToString(),
                Agent = Agent,
                PayToWholeSaler = PayToWholeSaler,
                PolicyNo = PolicyNo,
                WholeSaler = WholeSaler,
            };

            SortColumn = (string.IsNullOrEmpty(SortColumn)) ? "Name" : SortColumn;
            SortOrder = (!string.IsNullOrEmpty(SortOrder) && SortOrder.ToLower().Equals("asc")) ? "DESC" : "ASC";

            model.SortOrder = SortOrder;
            model.SortColumn = SortColumn;
            var results = caseMaster.GetCaseMasterResults(CaseID, PolicyNo, Agent, WholeSaler, PayToWholeSaler).Tables[0];
            DataView dv = new DataView(results, null, SortColumn + " " + SortOrder, DataViewRowState.CurrentRows);
            model.Results = dv.ToTable();
            return PartialView(model.Results);
        }

        [HttpGet]
        public PartialViewResult _EditCaseMasterRecord(string AGPCOMID = "")
        {
            if (string.IsNullOrEmpty(AGPCOMID))
            {
                return PartialView(new CaseMasterEditViewModel());
            }
            else
            {
                long key = Convert.ToInt64(AGPCOMID);
                var result = caseMaster.GetCaseMasterResultByKey(key).Tables[0].Rows[0];
                CaseMasterEditViewModel model = new CaseMasterEditViewModel
                {
                    AGPCOMID = AGPCOMID,
                    CaseID = Convert.ToInt64(result["CaseID"]),
                    PolNo = Convert.ToString(result["PolNo"]),
                    CrtDate = Convert.ToDateTime(result["CrtDate"]),
                    AgtNo = Convert.ToInt64(result["AgtNo"]),
                    AgtName = Convert.ToString(result["Name"]),
                    AgtPct = Convert.ToDecimal(result["AgtPct"]),
                    ComLvl = Convert.ToInt64(result["ComLvl"]),
                    Wholesaler = Convert.ToString(result["Wholesaler"]),
                    PayToWholesaler = Convert.ToString(result["PayToWholesaler"]),
                    Internal = Convert.ToString(result["Internal"]),
                    Recruiter = Convert.ToString(result["Recruiter"]),
                    Channel = Convert.ToString(result["Channel"]),
                    PlChannel = Convert.ToString(result["PlChannel"]),
                    NSM = Convert.ToString(result["NSM"]),
                    BrokeDealer = Convert.ToString(result["BrokerDealer"]),
                    ThruBD = Convert.ToString(result["ThruBD"]),
                    Agency = Convert.ToString(result["Agency"]),
                    AgencyMP = Convert.ToString(result["AgencyMP"]),
                };
                SetCaseMasterModalViewBag(model);
                return PartialView(model);

            }
        }

        [HttpPost]
        public ActionResult _EditCaseMasterRecord(CaseMasterEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.ArchiveDateTime = DateTime.Now;
                //Server validations to be added here
                var result = caseMaster.SaveEditCaseMasterRecord(true, Convert.ToInt64(model.AGPCOMID),
                    model.Wholesaler, model.Internal, model.Recruiter, model.Channel, model.BrokeDealer, model.PayToWholesaler,
                    model.PlChannel, model.ThruBD, model.Agency, model.AgencyMP, model.NSM, model.ArchiveDateTime, model.ArchiveReason);

                //if (result.Rows.Count > 0)
                //{
                //    SetAlertViewBag("Casemaster record edited successfully.", AlertType.Success);
                //}
                //else
                //{
                //    SetAlertViewBag("Failed to save changes to Casemaster record", AlertType.Failed);
                //}
            }
            else
            {
                return PartialView("_EditCaseMasterRecord", model);
            }
            return RedirectToAction("_EditCaseMasterRecord", new { AGPCOMID = model.AGPCOMID });
        }

        private void SetCaseMasterModalViewBag(CaseMasterEditViewModel model)
        {
            ViewBag.dWholeSaler = caseMaster.GetCaseMasterDDAttributes("wholesaler")
                .ToSelectList("wholesaler", "wholesaler", model.Wholesaler);

            ViewBag.dBrokerDealer = caseMaster.GetCaseMasterDDAttributes("brokerdealer")
                .ToSelectList("brokerDealer", "brokerDealer", model.Wholesaler);

            ViewBag.dChannel = caseMaster.GetCaseMasterDDAttributes("channel")
                .ToSelectList("channel", "channel", model.Wholesaler);

            ViewBag.dPlChannel = caseMaster.GetCaseMasterDDAttributes("plchannel")
                .ToSelectList("PLChannel", "PLChannel", model.Wholesaler);

            ViewBag.dInternal = caseMaster.GetCaseMasterDDAttributes("internal")
                .ToSelectList("internal", "internal", model.Wholesaler);

            ViewBag.dRecruiter = caseMaster.GetCaseMasterDDAttributes("recruiter")
                .ToSelectList("recruiter", "recruiter", model.Wholesaler);

            ViewBag.dPayToWholesaler = caseMaster.GetCaseMasterDDAttributes("paytowholesaler")
                .ToSelectList("payToWholesaler", "payToWholesaler", model.Wholesaler);

            ViewBag.dNSM = caseMaster.GetCaseMasterDDAttributes("nsm")
                .ToSelectList("NSM", "NSM", model.Wholesaler);

            ViewBag.dThruBD = caseMaster.GetCaseMasterDDAttributes("thrubd")
                .ToSelectList("ThruBD", "ThruBD", model.Wholesaler);

            ViewBag.dAgency = caseMaster.GetCaseMasterDDAttributes("agency")
                .ToSelectList("Agency", "Agency", model.Wholesaler);

            ViewBag.dAgencyMP = caseMaster.GetCaseMasterDDAttributes("agencymp")
                .ToSelectList("AgencyMP", "AgencyMP", model.Wholesaler);
        }
    }
}