using BIAdvisor.Web.Models;
using System;
using System.Web;
using System.Web.Mvc;
using static BIAdvisor.Web.Models.EnumHelper;

namespace BIAdvisor.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        [AllowAnonymous]
        public ActionResult SetUserContext(string p)
        {
            var role = HttpContext.Session["userRole"];
            if (Enum.IsDefined(typeof(UserRole), p) && role != null && role.ToString() == "Administrator")
            {
                var roleValue = p.ToEnumValue<UserRole>();
                var cookie = HttpContext.Request.Cookies.Get("userRole") ?? new HttpCookie("userRole");
                cookie.Value = roleValue.ToString();
                HttpContext.Response.Cookies.Add(cookie);
                //HttpContext.Cache.Add(HttpContext.Request.AnonymousID + "_role", roleValue, null, DateTime.MaxValue, new TimeSpan(0, 30, 0), CacheItemPriority.Normal, null);
            }
            var returnPath = Request.UrlReferrer != null ? Request.UrlReferrer.AbsolutePath : "/";
            return RedirectToLocal(returnPath);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

    }
}