using System.Web.Optimization;

namespace BIAdvisor.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/toastr.js",
                        "~/Scripts/custom/numeric.js",
                        "~/Scripts/custom/site.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/foolproof/mvcfoolproof.unobtrusive.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryvalidate").Include("~/Scripts/jquery.validate.js"));
            bundles.Add(new ScriptBundle("~/bundles/bpopup").Include("~/Scripts/jquery.bpopup.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/select2").Include("~/Scripts/select2.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/home").Include("~/Scripts/custom/search.js"));
            bundles.Add(new ScriptBundle("~/bundles/details").Include("~/Scripts/custom/details.js"));

            bundles.Add(new ScriptBundle("~/bundles/deal").Include(
                "~/Scripts/jquery.validate.js",
                "~/Scripts/select2.min.js",
                "~/Scripts/jquery.dirtyforms.min.js",
                "~/Scripts/custom/deal.js"));

            bundles.Add(new StyleBundle("~/Content/s2").Include("~/Content/Select2.css"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/toastr.css",
                      "~/Content/site.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
