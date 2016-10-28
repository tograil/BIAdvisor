using BIAdvisor.BL;
//using BIAdvisor.Web.Infrastructure.Notification;
using BIAdvisor.Web.Models;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BIAdvisor.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly string connString;
        protected int userRole = 0;

        // Constructor
        public BaseController()
        {
            connString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
        }

        #region Unused Methods

        //protected KeyValuePair<string, string> GetUrlControllerAction(string absUrl)
        //{
        //    absUrl = absUrl ?? "/"; // if null, replace with /
        //    var a = absUrl.Split('/');
        //    string controllerName = "home", actionName = "index";
        //    if (a.Length > 2)
        //    {
        //        controllerName = a[1].ToLower();
        //        actionName = a[2].ToLower();
        //    }
        //    return new KeyValuePair<string, string>(controllerName, actionName);
        //}
        //protected void ShowMessage(Controller controller, MessageType messageType, string message, bool showAfterRedirect = false)
        //{
        //    var messageTypeKey = messageType.ToString();
        //    if (showAfterRedirect)
        //    {
        //        controller.TempData[messageTypeKey] = message;
        //    }
        //    else
        //    {
        //        controller.ViewData[messageTypeKey] = message;
        //    }
        //}

        #endregion

        /// <summary>
        /// Set Alert Message and Alert Type
        /// </summary>
        /// <param name="message"></param>
        /// <param name="alertType"></param>
        protected void SetAlertViewBag(string message, AlertType alertType)
        {
            TempData["message"] = message;
            TempData["type"] = alertType;
        }


        // This will get executed before each actionmethod
        // This method contains the security code for checking user access level
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // to avoid infinite loop, check if the request is made for /home/unauthorized
            if (filterContext.RouteData.Values["controller"].ToString() == "Home"
                && filterContext.RouteData.Values["action"].ToString() == "Unauthorized")
            {
                return;
            }

            bool isQA = false;
            bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["isQA"], out isQA);
            if (isQA)
            {
                var dr = new UserMethods().GetUser("QA");
                if (dr != null && !string.IsNullOrWhiteSpace((dr["SecurityLevel"] ?? "").ToString()))
                {
                    Session["userRole"] = dr["SecurityLevel"].ToString();
                    Session["name"] = dr["FirstName"].ToString();
                }
            }
            else
            {
                // if the current Identity name doesnt match with un cookie, or the session role value is empty
                // re -assign name and role.
                var un = Request.Cookies["un"];
                if (un == null || string.IsNullOrEmpty(un.Value) || un.Value != User.Identity.Name
                        || Session["userRole"] == null || string.IsNullOrEmpty(Session["userRole"].ToString()))
                {
                    var name = ((ClaimsIdentity)ClaimsPrincipal.Current.Identity).Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                    if (name != null && !string.IsNullOrWhiteSpace(name.Value))
                    {
                        //remove role cookie
                        Response.Cookies.Remove("userRole");

                        //assign username to cookie
                        un = new HttpCookie("un", name.Value);
                        Response.Cookies.Add(un);
                        //If role not found, get name from request and check the db for the role
                        //Check user in the db.
                        var dr = new UserMethods().GetUser(name.Value);
                        if (dr != null && !string.IsNullOrWhiteSpace((dr["SecurityLevel"] ?? "").ToString()))
                        {
                            Session["userRole"] = dr["SecurityLevel"].ToString();
                            Session["name"] = dr["FirstName"].ToString();
                            //((ClaimsIdentity)ClaimsPrincipal.Current.Identity).AddClaim(new Claim(ClaimTypes.Role, dr["SecurityLevel"].ToString()));
                        }
                    }
                }
            }

            // If role is still is empty, redirect user to the unauthorized page
            if (Session["userRole"] == null || string.IsNullOrWhiteSpace(Session["userRole"].ToString()))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Unauthorized" }));
            }
            else
            {
                var ur = HttpContext.Request.Cookies["userRole"];
                switch (Session["userRole"].ToString().ToUpper())
                {
                    case "ADMINISTRATOR":
                        userRole = (ur != null && ur.Value != "" && int.Parse(ur.Value) == (int)UserRole.SuperUser)
                                        ? (int)UserRole.SuperUser : (int)UserRole.Admin;
                        break;
                    case "READONLY":
                        userRole = (int)UserRole.ReadOnly;
                        break;
                    case "READWRITE":
                        userRole = (int)UserRole.ReadWrite;
                        break;
                    default:
                        break;
                }
                HttpContext.Response.Cookies.Add(new HttpCookie("userRole", userRole.ToString()));
            }

            base.OnActionExecuting(filterContext);
        }

        // This will get executed after each actionmethod
        // This method sets the alert message which is then displayed using Toastr notifications
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (TempData["message"] != null)
            {
                ViewBag.Message = TempData["message"];
                AlertType alertType = TempData["type"] != null ? (AlertType)TempData["type"] : AlertType.Success;
                switch (alertType)
                {
                    case AlertType.Failed:
                        ViewBag.AlertType = "error";
                        break;
                    case AlertType.Success:
                        ViewBag.AlertType = "success";
                        break;
                    case AlertType.Warning:
                        ViewBag.AlertType = "warning";
                        break;
                    case AlertType.Info:
                        ViewBag.AlertType = "info";
                        break;
                    default:
                        break;
                };
            }

            //    ViewBag.userRole = HttpContext.Cache.Get(HttpContext.Request.AnonymousID + "_role");
            ViewBag.userRole = HttpContext.Response.Cookies["userRole"] != null ? HttpContext.Response.Cookies["userRole"].Value : "";
            ViewBag.userRole = ViewBag.userRole ?? (int)UserRole.ReadOnly;
            ViewBag.Username = Session["name"];
            base.OnActionExecuted(filterContext);
        }
    }
}