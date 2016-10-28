using BIAdvisor.Web.Infrastructure.Notification;
using System.Web.Mvc;

namespace BIAdvisor.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AjaxMessagesFilter());
        }
    }
}
