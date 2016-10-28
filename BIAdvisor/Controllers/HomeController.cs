using BIAdvisor.BL;
using BIAdvisor.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BIAdvisor.Web.Controllers
{
    public class HomeController : BaseController
    {
        private IPayee payee;

        // Initialise Repositories (DB Read/Write Methods)
        public HomeController()
        {
            payee = new Payee();
        }

        //Unauthorized Method to show unauthorized page if user is not in the db
        public ActionResult Unauthorized()
        {
            return View();
        }

        // GET: Home/Index
        // This is GET method for Home/Index
        // 1. Show last search, get last search parameters from cookies.
        // 2. If cookie were expired, don't show any results.
        [HttpGet]
        public ActionResult Index()
        {
            // Set no cache for browser back button
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();

            //Get referrer controller and action (key = controller, value = action)
            //var ac = GetUrlControllerAction(Request.UrlReferrer?.AbsolutePath);

            //Read Values from Cookies on initial load
            string name = HttpContext.Request.Cookies["s_name"] != null ? HttpContext.Request.Cookies["s_name"].Value : "";
            string type = HttpContext.Request.Cookies["s_type"] != null ? HttpContext.Request.Cookies["s_type"].Value : "";
            string role = HttpContext.Request.Cookies["s_role"] != null ? HttpContext.Request.Cookies["s_role"].Value : "";
            string timeframe = HttpContext.Request.Cookies["s_timeframe"] != null ? HttpContext.Request.Cookies["s_timeframe"].Value : "";
            string sModel = HttpContext.Request.Cookies["s_model"] != null ? HttpContext.Request.Cookies["s_model"].Value : "";

            //Assign the values to the ViewModel
            var model = new SearchViewModel() { Name = name, Type = type, Role = role, Timeframe = timeframe };

            // Check if any of the parameter from cookies are not null or empty
            // then get the results and show them
            if (!string.IsNullOrWhiteSpace(name) || !string.IsNullOrWhiteSpace(type) || !string.IsNullOrWhiteSpace(role)
                || !string.IsNullOrWhiteSpace(timeframe) || !string.IsNullOrWhiteSpace(sModel))
            {
                ViewBag.schema = payee.GetPayeesSchema();

                model.Payees = payee.GetPayeesBySearch(name,
                    (type ?? "").Split(':')[0],  //.Split(':')[0] to remove the role added to type for select2
                    role,
                    timeframe == "2" ? "All Deals" : "Current Deals Only",
                    sModel == "Y" ? true : false);
            }

            // If no payee is null (most probably because the search was not done)
            model.Payees = model.Payees ?? new DataTable();

            // Data for Type Dropdown (to display table in a dropdown)
            model.TypesSelectListItems = GetTypeRoleResultsList();

            //Reset search cookies
            SetSearchCookies(name, type, role, timeframe, sModel);

            return View(model);
        }

        // POST: Index
        // This method is called when search button is clicked 
        [HttpPost]
        public ActionResult Index(SearchViewModel model)
        {
            // Set no cache for browser back button
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();

            model.TypesSelectListItems = GetTypeRoleResultsList();

            //.Split(':')[0] to remove the role added to type for select2
            model.Payees = payee.GetPayeesBySearch(model.Name, (model.Type ?? "").Split(':')[0], model.Role,
                model.Timeframe == "2" ? "All Deals" : "Current Deals Only", model.boolModel) ?? new DataTable();

            ViewBag.schema = payee.GetPayeesSchema();
            //Reset search cookies
            SetSearchCookies(model.Name, model.Type, model.Role, model.Timeframe, model.boolModel == true ? "Y" : "");
            return View(model);
        }


        // Delete Payee Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePayee(int PayeeDel)
        {
            payee.DeletePayee(PayeeDel);
            SetAlertViewBag("Payee deleted successfully.", AlertType.Success);
            return RedirectToAction("Index");
        }


        // Add Payee Method
        // This method is called when Add Payee button is clicked from the modal
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPayee(PayeeAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                //To remove the role added to type for select2
                model.PayeeType = model.PayeeType.Split(':')[0];
                //Validation Check for overlapping dates.
                var valid = payee.ValidatePayeeOverlapDates(model.PayeeName, model.PayeeType, false, model.EffectiveDate, model.ExpirationDate);
                if (!valid)
                {
                    SetAlertViewBag("Failed to add New Payee. Payee already has a deal within these dates.", AlertType.Failed);
                    return RedirectToAction("Index");
                }

                //Add New Payee
                var dr = payee.AddNewPayee(model.PayeeName, model.PayeeType, model.PayeeRole, model.EffectiveDate, model.ExpirationDate);
                SetAlertViewBag("New Payee added successfully.", AlertType.Success);
                return RedirectToAction("Index", "Maintenance", new { id = dr["PayeeKey"] });
            }

            SetAlertViewBag("Failed to add New Payee. Please try again.", AlertType.Failed);
            return RedirectToAction("Index");
        }


        #region Lookups & Validations

        // Date Overlap validation method to check overlapping dates for the given Payee
        [HttpPost]
        public JsonResult CheckOverlapDates(PayeeAddViewModel model)
        {
            //To remove the role added to type for select2
            model.PayeeType = model.PayeeType.Split(':')[0];
            var valid = payee.ValidatePayeeOverlapDates(model.PayeeName, model.PayeeType, false, model.EffectiveDate, model.ExpirationDate);
            return Json(valid);
        }

        // Get Lookup Data for Payee Field on the search page.
        public JsonResult GetPayees(string term, string type, string role, string timeframe)
        {
            DataTable dt = payee.GetPayeesForLookup(term, type, role, timeframe == "2" ? "All Deals" : "Current Deals Only");
            if (dt.Rows.Count > 0)
            {
                return Json(dt.AsEnumerable().Take(30).Select(d => new { id = d["Payee"], text = d["Payee"] }), JsonRequestBehavior.AllowGet);
            }
            return Json(new object[0], JsonRequestBehavior.AllowGet);
        }


        #endregion


        #region Private Methods

        // Get Lookup Data for Type and Role Table Dropdown
        private List<TypeRoleResult> GetTypeRoleResultsList()
        {
            var res = new List<TypeRoleResult>() { new TypeRoleResult { id = "Type", role = "role", text = "disabled option", disabled = true } };
            res.AddRange(payee.GetLookupPayeeType().AsEnumerable().Select(d => new TypeRoleResult
            {
                id = (d["PayeeType"] != null ? d["PayeeType"].ToString().Trim() : "") + ":" + (d["PayeeRole"] != null ? d["PayeeRole"].ToString().Trim() : ""),
                text = d["PayeeType"] != null ? d["PayeeType"].ToString().Trim() : "",
                role = d["PayeeRole"] != null ? d["PayeeRole"].ToString().Trim() : ""
            }));
            return res;
        }


        //Create cookies if not exists, and assign the passed values to them.
        private void SetSearchCookies(string name, string type, string role, string timeframe, string model = "")
        {
            var nameCookie = HttpContext.Request.Cookies.Get("s_name") ?? new HttpCookie("s_name");
            var typeCookie = HttpContext.Request.Cookies.Get("s_type") ?? new HttpCookie("s_type");
            var roleCookie = HttpContext.Request.Cookies.Get("s_role") ?? new HttpCookie("s_role");
            var timeCookie = HttpContext.Request.Cookies.Get("s_timeframe") ?? new HttpCookie("s_timeframe");
            var modelCookie = HttpContext.Request.Cookies.Get("s_model") ?? new HttpCookie("s_model");

            nameCookie.Value = name;
            typeCookie.Value = type;
            roleCookie.Value = role;
            timeCookie.Value = timeframe;
            modelCookie.Value = model;

            HttpContext.Response.Cookies.Add(nameCookie);
            HttpContext.Response.Cookies.Add(typeCookie);
            HttpContext.Response.Cookies.Add(roleCookie);
            HttpContext.Response.Cookies.Add(timeCookie);
            HttpContext.Response.Cookies.Add(modelCookie);
        }


        #endregion
    }
}