using BIAdvisor.BL;
using BIAdvisor.Web.Helpers;
using BIAdvisor.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BIAdvisor.Web.Controllers
{
    public class MaintenanceController : BaseController
    {
        private IPayee payee;
        private IDeals deal;

        // Instantiate Repositories (DB Read/Write Methods)
        public MaintenanceController()
        {
            payee = new Payee();
            deal = new Deals();
        }


        // GET: Maintenance
        // This is GET method for Payee Maintenance Page.
        [HttpGet]
        public ActionResult Index(int? id, bool? saved)
        {
            // Set no cache for browser back button
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();

            var dsPayee = payee.GetPayeeDetailsByKey(id ?? 1);
            var model = dsPayee.Tables[0].DataTableToList<PayeeDetailsViewModel>().FirstOrDefault();

            //Return to home page if payee not found
            if (model == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (dsPayee != null && dsPayee.Tables.Count > 0 && dsPayee.Tables[0].Rows.Count > 0)
            {
                model.EffectiveDate = Convert.ToDateTime(dsPayee.Tables[0].Rows[0]["EffectiveDate"]);
                model.ExpirationDate = Convert.ToDateTime(dsPayee.Tables[0].Rows[0]["ExpirationDate"]);
                //model.EffectiveDate = DateTime.ParseExact(dsPayee.Tables[0].Rows[0]["EffectiveDate"].ToString(), "d", CultureInfo.InvariantCulture);
                //model.ExpirationDate = DateTime.ParseExact(dsPayee.Tables[0].Rows[0]["ExpirationDate"].ToString(), "d", CultureInfo.InvariantCulture);

                //Boolean fields for Char for easy handling of checkbox.
                model.boolDoNotPay = model.DoNotPay.ToString() == "Y" ? true : false;
                model.boolModel = model.Model.ToString() == "Y" ? true : false;
                model.boolPublished = model.Published.ToString() == "Y" ? true : false;
                model.boolMethodBCheck = model.MethodBCheck.ToString() == "Y" ? true : false;
            }
            SetIndexViewBag(model);
            return View(model);
        }


        // POST: Maintenance
        // This is POST method for Payee Maintenance Page.
        // This is called when Save Changes button is clicked on Payee Page.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(PayeeDetailsViewModel payeeMod)
        {
            //Check for validation if published flag is true, else continue
            bool isModelValid = payee.ValidatePayeeOverlapDates(payeeMod.Payee, payeeMod.PayeeType, payeeMod.boolPublished,
                payeeMod.EffectiveDate, payeeMod.ExpirationDate, payeeMod.PayeeKey);

            if (isModelValid)
            {
                var result = payee.SavePayeeDetails(payeeMod.PayeeKey, payeeMod.boolPublished ? "Y" : "", payeeMod.EffectiveDate, payeeMod.ExpirationDate,
                            payeeMod.PayeeModel, null, payeeMod.COCPCT, payeeMod.BlacksmithCode, payeeMod.PayIndexKey, payeeMod.DeductionModel,
                            payeeMod.boolMethodBCheck ? "Y" : "", payeeMod.boolDoNotPay ? "Y" : "", payeeMod.boolModel ? "Y" : "",
                            payeeMod.PayeeGroup, null, payeeMod.PayeeNotes);

                if (result)
                {
                    SetAlertViewBag("Changes to Payee Details saved successfully.", AlertType.Success);
                }
                else
                {
                    SetAlertViewBag("Failed to save changes to Payee Details.", AlertType.Failed);
                }
            }
            else
            {
                SetAlertViewBag("Failed to update Payee Details. Payee already has a deal within these dates.", AlertType.Failed);
            }

            SetIndexViewBag(payeeMod);
            return View(payeeMod);
        }


        // POST: Copy Payee
        // This method is called when Copy Payee form is submitted
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CopyPayee(int CopyPayeeId, string CopyName, string CopyPayeeType, DateTime CopyEffectiveDate, DateTime CopyExpirationDate, bool CopyDeals)
        {
            if (ModelState.IsValid)
            {
                //Validation Check for overlapping dates.
                var valid = payee.ValidatePayeeOverlapDates(CopyName, CopyPayeeType, false, CopyEffectiveDate, CopyExpirationDate);
                if (!valid)
                {
                    SetAlertViewBag("Failed to add New Payee. Payee already has a deal within these dates.", AlertType.Failed);
                    return RedirectToAction("Index", "Maintenance", new { id = CopyPayeeId });
                }
                var dr = payee.CopyPayee(CopyPayeeId, CopyName, CopyEffectiveDate, CopyExpirationDate, CopyDeals);
                SetAlertViewBag("New Payee added successfully.", AlertType.Success);
                return RedirectToAction("Index", "Maintenance", new { id = dr["PayeeKey"] });
            }

            SetAlertViewBag("Failed to copy Payee. Please try again.", AlertType.Success);
            return RedirectToAction("Index", "Maintenance", new { id = CopyPayeeId });
        }


        // POST: Delete Deal
        // This method is called when delete button in the deal modal is clicked.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteDeal(DealMaintenanceViewModel payeeMod)
        {
            return Json(payee.DeletePayeeDeal(payeeMod.PayeeDealKey));
        }


        // GET: Deal Maintenance
        // This is GET method for page 3 i.e. Deal Maintenance
        [HttpGet]
        public PartialViewResult _DealMaintenance(int payeeDealKey = 0, int payeeKey = 0)
        {
            DataSet payeeDeals = deal.GetPayeeDealbyKey(payeeDealKey);
            DealMaintenanceViewModel model = new DealMaintenanceViewModel();
            var dsPayee = payee.GetPayeeDetailsByKey(payeeKey);
            if (dsPayee.Tables.Count < 1 || dsPayee.Tables[0].Rows.Count < 1)
            {
                return PartialView("Error");
            }

            DataRow payeeDetails = dsPayee.Tables[0].Rows[0];
            model.PayeeType = payeeDetails["PayeeType"].ToString();
            model.PayeeKey = payeeKey;

            if (payeeDeals != null && payeeDeals.Tables.Count > 0 && payeeDeals.Tables[0].Rows.Count > 0)
            {
                DataRow dr = payeeDeals.Tables[0].Rows[0];

                //Criteria Section Properties
                model.PayeeDealKey = dr["PayeeDealKey"] is DBNull ? 0 : Convert.ToInt32(dr["PayeeDealKey"]);
                model.PayeeKey = dr["PayeeKey"] is DBNull ? 0 : Convert.ToInt32(dr["PayeeKey"]);
                model.DealCriteria = dr["DealCriteria"] is DBNull ? null : dr["DealCriteria"].ToString();
                model.StatementDate = dr["StatementDate"] is DBNull ? (DateTime?)null : Convert.ToDateTime(dr["StatementDate"]);
                model.SubmitDate = dr["SubmitDate"] is DBNull ? (DateTime?)null : Convert.ToDateTime(dr["SubmitDate"]);
                model.CarrierID = dr["CarrierID"] is DBNull ? (int?)null : Convert.ToInt32(dr["CarrierID"]);
                model.Carrier = dr["Carrier"] is DBNull ? null : Convert.ToString(dr["Carrier"]);
                model.ProductLine = dr["ProductLine"] is DBNull ? (short?)null : Convert.ToInt16(dr["ProductLine"]);
                model.ProductID = dr["ProductID"] is DBNull ? (int?)null : Convert.ToInt32(dr["ProductID"]);
                model.Product = dr["Product"] is DBNull ? "" : dr["Product"].ToString();
                model.Wholesaler = dr["Wholesaler"] is DBNull ? null : dr["Wholesaler"].ToString();
                model.BrokerDealer = dr["BrokerDealer"] is DBNull ? null : dr["BrokerDealer"].ToString();
                model.Channel = dr["Channel"] is DBNull ? null : dr["Channel"].ToString();
                model.Internal = dr["Internal"] is DBNull ? null : dr["Internal"].ToString();
                model.ProducerID = dr["ProducerID"] is DBNull ? (int?)null : Convert.ToInt32(dr["ProducerID"]);
                model.Producer = dr["Producer"] is DBNull ? "" : dr["Producer"].ToString();
                model.Recruiter = dr["Recruiter"] is DBNull ? null : dr["Recruiter"].ToString();
                model.MPPartner = dr["MPPartner"] is DBNull ? null : dr["MPPartner"].ToString();
                model.WholesalerRole = dr["WholesalerRole"] is DBNull ? null : dr["WholesalerRole"].ToString();
                model.PayeeGroup = dr["PayeeGroup"] is DBNull ? null : dr["PayeeGroup"].ToString();
                model.CarrierAttribute = dr["CarrierAttributes"] is DBNull ? (int?)null : Convert.ToInt32(dr["CarrierAttributes"]);
                model.CarrierAttributeDescription = dr["CarrierAttributeDescription"] is DBNull ? "" : dr["CarrierAttributeDescription"].ToString();
                model.ProductAttribute = dr["ProductAttributes"] is DBNull ? (int?)null : Convert.ToInt32(dr["ProductAttributes"]);
                model.ProductAttributeDescription = dr["ProductAttributeDescription"] is DBNull ? "" : dr["ProductAttributeDescription"].ToString();
                model.RecruiterAttribute = dr["RecruiterAttributes"] is DBNull ? (int?)null : Convert.ToInt32(dr["RecruiterAttributes"]);
                model.RecruiterAttributeDescription = dr["RecruiterAttributeDescription"] is DBNull ? "" : dr["RecruiterAttributeDescription"].ToString();
                model.BrokerDealerAttribute = dr["BrokerDealerAttributes"] is DBNull ? (int?)null : Convert.ToInt32(dr["BrokerDealerAttributes"]);
                model.BDAttributeDescription = dr["BDAttributeDescription"] is DBNull ? "" : dr["BDAttributeDescription"].ToString();
                model.BrokerDealerInHierarchy = dr["BrokerDealerInHierarchy"] is DBNull ? null : Convert.ToString(dr["BrokerDealerInHierarchy"]);

                // Calculation Section Properties
                model.CalculationID = dr["CalculationID"] is DBNull ? null : Convert.ToString(dr["CalculationID"]);
                model.CalculationCode = dr["CalculationCode"] is DBNull ? null : Convert.ToString(dr["CalculationCode"]);
                model.PayIndexKey = dr["PayIndexKey"] is DBNull ? null : Convert.ToString(dr["PayIndexKey"]);
                model.DeductionModel = dr["DeductionModel"] is DBNull ? null : Convert.ToString(dr["DeductionModel"]);
                model.LOA = dr["LOA"] != null && Convert.ToString(dr["LOA"]) == "Y" ? true : false;
                model.Street = dr["Street"] != null && Convert.ToString(dr["Street"]) == "Y" ? true : false;
                model.ExpDelta = dr["ExpDelta"] is DBNull ? (decimal?)null : Convert.ToDecimal(dr["ExpDelta"]);
                model.Directive = dr["Directive"] is DBNull ? null : Convert.ToString(dr["Directive"]);
                model.DealCap = dr["DealCap"] is DBNull ? (decimal?)null : Convert.ToDecimal(dr["DealCap"]);
                model.CarrierLevel = dr["CarrierLevel"] is DBNull ? null : Convert.ToString(dr["CarrierLevel"]);
                model.VirtualCarrierLevel = dr["VirtualCarrierLevel"] is DBNull ? null : Convert.ToString(dr["VirtualCarrierLevel"]);
                model.PctGross = dr["PctGross"] is DBNull ? (decimal?)null : Convert.ToDecimal(dr["PctGross"]);
                model.PctPremium = dr["PctPremium"] is DBNull ? (decimal?)null : Convert.ToDecimal(dr["PctPremium"]);
                model.PctOverride = dr["PctOverride"] is DBNull ? (decimal?)null : Convert.ToDecimal(dr["PctOverride"]);
                model.PctCarrierLevel = dr["PctCarrierLevel"] is DBNull ? (decimal?)null : Convert.ToDecimal(dr["PctCarrierLevel"]);
                model.PctWholesalerNet = dr["PctWholesalerNet"] is DBNull ? (decimal?)null : Convert.ToDecimal(dr["PctWholesalerNet"]);
                model.RecruiterDebit = dr["RecruiterDebit"] is DBNull ? (decimal?)null : Convert.ToDecimal(dr["RecruiterDebit"]);
                model.RecruiterCredit = dr["RecruiterCredit"] is DBNull ? (decimal?)null : Convert.ToDecimal(dr["RecruiterCredit"]);
                model.ContingentBonus = dr["ContingentBonus"] is DBNull ? (decimal?)null : Convert.ToDecimal(dr["ContingentBonus"]);
                model.YearEndBonus = dr["YearEndBonus"] is DBNull ? (decimal?)null : Convert.ToDecimal(dr["YearEndBonus"]);
                model.Amount1 = dr["Amount1"] is DBNull ? (decimal?)null : Convert.ToDecimal(dr["Amount1"]);
                model.Amount2 = dr["Amount2"] is DBNull ? (decimal?)null : Convert.ToDecimal(dr["Amount2"]);
                model.Amount3 = dr["Amount3"] is DBNull ? (decimal?)null : Convert.ToDecimal(dr["Amount3"]);
                model.Rate1 = dr["Rate1"] is DBNull ? (decimal?)null : Convert.ToDecimal(dr["Rate1"]);
                model.Rate2 = dr["Rate2"] is DBNull ? (decimal?)null : Convert.ToDecimal(dr["Rate2"]);
                model.Rate3 = dr["Rate3"] is DBNull ? (decimal?)null : Convert.ToDecimal(dr["Rate3"]);
                model.Notes = dr["PayeeDealNotes"] is DBNull ? null : Convert.ToString(dr["PayeeDealNotes"]);

                //Check whether payee is published, in that case all of the modal has to be read only
                string isPublished = payeeDetails["Published"].ToString();
                ViewBag.IsPayeePublished = isPublished;

                //Check if readonly view needs to be displayed
                //When user is readonly or payee is published and current user is not SuperUser
                if (userRole == (int)UserRole.ReadOnly || (isPublished == "Y" && userRole != (int)UserRole.SuperUser))
                {
                    return PartialView("_MaintainenceReadOnly", model);
                }
            }

            SetMaintainenceViewBag(model);

            return PartialView("_DealDetails", model);
        }


        // GET: Deal Maintenance
        // This is POST method for page 3 i.e. Deal Maintenance
        // This is called when Save Changes button is clicked on Deals Page.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _DealMaintenance(DealMaintenanceViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.DealCriteria))
                {
                    ModelState.AddModelError(string.Empty, "Criteria combination is required");
                    return PartialView(model);
                }
                var result = deal.SavePayeeDeal(model.PayeeDealKey, model.PayeeKey, model.DealCriteria, model.StatementDate, model.SubmitDate,
                    model.CarrierID, model.ProductLine, model.ProductID, model.Wholesaler, model.BrokerDealer, model.Channel, model.Internal,
                    model.ProducerID, model.MSA, model.Recruiter, model.MPPartner, model.WholesalerRole, model.PayeeGroup, model.CarrierAttribute,
                    model.ProductAttribute, model.RecruiterAttribute, model.BrokerDealerAttribute, model.BrokerDealerInHierarchy,
                    model.CalculationID, model.PayIndexKey, model.DeductionModel, model.LOA ? "Y" : "", model.Street ? "Y" : "", model.Directive,
                    model.DealCap, model.CarrierLevel, model.VirtualCarrierLevel, model.ExpDelta, model.PctGross, model.PctPremium, model.PctOverride,
                    model.PctCarrierLevel, model.PctWholesalerNet, model.RecruiterDebit, model.RecruiterCredit, model.ContingentBonus,
                    model.YearEndBonus, model.Amount1, model.Amount2, model.Amount3, model.Rate1, model.Rate2, model.Rate3, model.Notes);

                if (result.Rows.Count > 0)
                {
                    SetAlertViewBag(model.PayeeDealKey == 0 ? "Payee Deal added successfully." : "Changes to Payee Deal saved successfully.", AlertType.Success);
                    model.PayeeDealKey = model.PayeeDealKey == 0 ? (int)result.Rows[0]["PayeeDealKey"] : model.PayeeDealKey;
                }
                else
                {
                    SetAlertViewBag("Failed to save changes to Payee Deal.", AlertType.Failed);
                }

            }
            else
            {
                SetMaintainenceViewBag(model);
                return PartialView("_DealDetails", model);
            }
            return RedirectToAction("_DealMaintenance", new { payeeDealKey = model.PayeeDealKey, payeeKey = model.PayeeKey });
        }


        // This is the partial view for the deals grid on Payee Page.
        // It is reloaded partially in some scenarios, hence a partial is created.
        [HttpGet]
        public PartialViewResult _PayeeDeals(int id)
        {
            var model = new PayeeDealsViewModel();
            var dsPayee = payee.GetPayeeDetailsByKey(id);
            if (dsPayee == null || dsPayee.Tables.Count < 1 && dsPayee.Tables[0].Rows.Count < 1)
            {
                model.boolPublished = true;
                model.userRole = UserRole.ReadOnly;
                model.dtDeals = new DataTable();
            }
            //Deals Table
            ViewBag.schema = deal.GetPayeeDealsSchema(id);
            var deals = deal.GetPayeeDealsByKey(id);
            if (deals != null && deals.Tables.Count > 0)
            {
                ViewBag.userRole = HttpContext.Request.Cookies["userRole"] != null ? HttpContext.Request.Cookies["userRole"].Value : "";
                model.userRole = Enum.Parse(typeof(UserRole), ViewBag.userRole ?? "0");
                model.boolPublished = (dsPayee.Tables[0].Rows[0]["Published"] ?? "").ToString() == "Y" ? true : false;
                model.dtDeals = deals.Tables[0];
            }

            return PartialView(model);
        }

        #region Lookups & Validations

        // Ajax method to get Product Items for Product Dropdown list in the deal maintenance page.
        public JsonResult GetProductItems(int carrierID = 0, int productLine = 0)
        {
            return Json(deal.GetProductsForCarrierLine(carrierID, productLine)
                .AsEnumerable().Select(d => new
                {
                    id = d["ProductID"],
                    text = d["Product"]
                }), JsonRequestBehavior.AllowGet);
        }

        // Ajax method to get Carrier level Items for Carrier Dropdown list in the deal maintenance page.
        public JsonResult GetCarrierLevel(int carrierID = 0, int productLine = 0)
        {
            return Json(deal.GetDealCarrierLevel(carrierID, productLine).AsEnumerable().Select(p => new
            {
                id = p["CarrierLevel"],
                text = p["CarrierLevel"]
            }), JsonRequestBehavior.AllowGet);
        }

        // Ajax method to get Calc Code Items for CalcCode Dropdown list in the deal maintenance page.
        public JsonResult GetCalCodeItems(string payeeType, string dealCriteria)
        {
            return Json(deal.GetCalCodeByParams(payeeType, dealCriteria).AsEnumerable().Select(p => new
            {
                id = p["CalculationID"],
                text = p["CalculationCode"],
                desc = p["CalculationDescription"],
                caljson = p["CalculationJSON"]
            }), JsonRequestBehavior.AllowGet);
        }

        // Ajax method to get Wholesaler/AFMO Items for Wholesaler Dropdown list in the deal maintenance page.
        public JsonResult GetWholesalerItems(string category = "Wholesaler")
        {
            return Json(deal.GetDealWholeSaler(category).AsEnumerable().Select(i => new { id = i["Payee"], text = i["Payee"] }),
                JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Private Methods

        // Set ViewBag items for Payee Maintenance Method
        // These viewbags contain data for dropdown lists for Payee Maintenance Page.
        private void SetIndexViewBag(PayeeDetailsViewModel model)
        {
            //DropDown Lists
            ViewBag.Models = payee.GetLookupModel(model.PayeeType, model.PayeeRole).ToSelectList("Payee", "Payee");
            ViewBag.PayeeGroups = payee.GetLookupPayeeGroups(model.PayeeType, model.PayeeRole).ToSelectList("PayeeGroup", "PayeeGroup");
            ViewBag.Types = payee.GetPayeeTypeRole().ToSelectList("PayeeType", "PayeeType");

            //PayIndex
            var payIndex = new List<object>() { new { id = "PayIndex", text = "disabled option", disabled = true } };
            payIndex.AddRange(payee.GetPayeeIndex(model.PayeeType, model.PayeeRole).AsEnumerable().Select(d => new
            {
                id = d["PayIndexKey"] != null ? d["PayIndexKey"].ToString().Trim() : "",
                text = d["PayIndexKey"] != null ? d["PayIndexKey"].ToString().Trim() : "",
                blacksmith = d["Blacksmith"] != null ? d["Blacksmith"].ToString().Trim() : "",
                coc = d["COC"] != null ? d["COC"].ToString().Trim() : "",
                basis = d["Basis"] != null ? d["Basis"].ToString().Trim() : ""
            }).ToList());
            ViewBag.PayIndex = payIndex;

            //Blacksmith
            var blacksmith = new List<object>() { new { id = "Blacksmith", text = "disabled option", disabled = true } };
            blacksmith.AddRange(payee.GetBlacksmith().AsEnumerable().Select(d => new
            {
                id = d["Blacksmith"] != null ? d["Blacksmith"].ToString().Trim() : "",
                text = d["Blacksmith"] != null ? d["Blacksmith"].ToString().Trim() : "",
                coc = d["COC"] != null ? d["COC"].ToString().Trim() : ""
            }).ToList());
            ViewBag.Blacksmiths = blacksmith;

            //Deductions
            ViewBag.Deductions = payee.GetLookupDeduction(model.PayeeType).ToSelectList("DeductionModel", "DeductionModel", model.DeductionModel);

            //Deals Table
            ViewBag.schema = deal.GetPayeeDealsSchema(model.PayeeKey);
            var deals = deal.GetPayeeDealsByKey(model.PayeeKey);
            if (deals != null && deals.Tables.Count > 0)
            {
                ViewBag.Deals = deals.Tables[0];
            }
        }


        // Set ViewBag items for Deal Maintenance Method
        // These viewbags contain data for dropdown lists for Deal Maintenance Page.
        private void SetMaintainenceViewBag(DealMaintenanceViewModel model)
        {
            // SECTION 1 (Deal Criteria)
            var criteriaCombo = new List<object>() { new { id = "CriteriaCombo", text = "disabled option", priority = 0, disabled = true } };
            criteriaCombo.AddRange(deal.GetPayeeDealAttributes("criteriacombination").AsEnumerable().Select(c => new
            {
                id = c["DealCriteria"],
                text = c["DealCriteria"],
                priority = c["DealPriority"],
                active = c["Active"]
            }));
            ViewBag.dDealCriteria = criteriaCombo;
            //deal.GetPayeeDealAttributes("criteriacombination").ToSelectList("DealCriteria", "DealCriteria", model.DealCriteria);

            //Carrier Table Dropdown.
            var carrierCombo = new List<object>() { new { id = "CarrierCombo", text = "disabled option", disabled = true } };
            carrierCombo.AddRange(deal.GetPayeeDealAttributes("carrier").AsEnumerable().Select(d => new
            {
                id = d["CarrierID"] != null ? d["CarrierID"].ToString().Trim() : "",
                text = d["Carrier"] != null ? d["Carrier"].ToString().Trim() : "",
                line = d["Line"] != null ? d["Line"].ToString().Trim() : ""
            }).ToList());
            ViewBag.dCarrierCombination = carrierCombo;

            ViewBag.dCarrierAttribute = deal.GetPayeeDealAttributes("carrierattribute")
                .ToSelectList("AttributeID", "AttributeDescription", model.CarrierAttribute.ToString());

            ViewBag.dProduct = deal.GetProductsForCarrierLine(model.CarrierID.HasValue ? model.CarrierID.Value : 0
                , model.ProductLine.HasValue ? model.ProductLine.Value : 0).ToSelectList("ProductID", "Product", model.ProductID.ToString());

            ViewBag.dProductAttribute = deal.GetPayeeDealAttributes("productattribute")
                .ToSelectList("AttributeID", "AttributeDescription", model.ProductAttribute.ToString());

            ViewBag.dWholesaler = (model.DealCriteria ?? "").Contains("AFMO")
                ? deal.GetDealWholeSaler("AFMO").ToSelectList("Payee", "Payee", model.Wholesaler)
                : deal.GetDealWholeSaler("Wholesaler").ToSelectList("Payee", "Payee", model.Wholesaler);

            ViewBag.dBrokerDealer = deal.GetPayeeDealAttributes("brokerdealer").ToSelectList("Payee", "Payee", model.BrokerDealer);

            ViewBag.dBrokerDealerAttribute = deal.GetPayeeDealAttributes("brokerdealerattribute")
                .ToSelectList("AttributeID", "AttributeDescription", model.BrokerDealerAttribute.ToString());

            ViewBag.BDInHierarchy = new SelectList(new List<SelectListItem> { new SelectListItem { Text = "Yes", Value = "Y" } }, "Value", "Text");

            ViewBag.dChannel = deal.GetPayeeDealAttributes("channel").ToSelectList("Channel", "Channel", model.Channel);

            ViewBag.dInternal = deal.GetPayeeDealAttributes("internal").ToSelectList("Payee", "Payee", model.Internal);

            ViewBag.dProducer = deal.GetPayeeDealAttributes("producer").ToSelectList("ProducerID", "Producer", model.ProducerID.ToString());

            ViewBag.dRecruiter = deal.GetPayeeDealAttributes("recruiter").ToSelectList("Recruiter", "Recruiter", model.Recruiter);

            ViewBag.dRecruiterAttribute = deal.GetPayeeDealAttributes("recruiterattribute")
                .ToSelectList("AttributeID", "AttributeDescription", model.RecruiterAttribute.ToString());

            ViewBag.dMarketingProgram = deal.GetPayeeDealAttributes("MPPartner")
                .ToSelectList("MPPartner", "MPPartner", model.MPPartner);

            ViewBag.dPayeeGroup = deal.GetPayeeDealAttributes("payeegroup").ToSelectList("PayeeGroup", "PayeeGroup", model.PayeeGroup);


            //SECTION 2 (Deal Calculation)
            ViewBag.dCalculationCode = deal.GetCalculationCodeDetails(model.CalculationID).AsEnumerable().FirstOrDefault();

            //Used for both Carrier Level and Virtual Carrier Level
            ViewBag.dCarrierLevel = deal.GetDealCarrierLevel(model.CarrierID, model.ProductLine)
                .ToSelectList("CarrierLevel", "CarrierLevel", model.CarrierLevel);

            ViewBag.dPayIndex = deal.GetPayeeDealAttributes("payindex").ToSelectList("PayIndexKey", "PayIndexKey", model.PayIndexKey);

            ViewBag.dDeductions = payee.GetLookupDeduction(model.PayeeType).ToSelectList("DeductionModel", "DeductionModel", model.DeductionModel);

            ViewBag.dDirective = deal.GetPayeeDealAttributes("directive").ToSelectList("Directive", "Directive", model.Directive);

            //ViewBag.CalCodeDesc = deal.GetPayeeDealAttributes("calculationcode");
            ViewBag.CalCode = deal.GetCalCodeByParams(model.PayeeType ?? "", model.DealCriteria ?? "").AsEnumerable()
                .Select(p => new
                {
                    id = p["CalculationID"],
                    text = p["CalculationCode"],
                    desc = p["CalculationDescription"],
                    caljson = p["CalculationJSON"]
                });
        }
        #endregion
    }
}