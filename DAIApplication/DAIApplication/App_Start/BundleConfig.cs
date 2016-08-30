using System.Web;
using System.Web.Optimization;

namespace DAIApplication
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region AngularJS
            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                      "~/Scripts/Core/AngularJS/angular.min.js",
                      "~/Scripts/Core/AngularJS/angular-animate.min.js",
                      "~/Scripts/Core/AngularJS/angular-cookies.min.js",
                      "~/Scripts/Core/AngularJS/angular-resource.min.js",
                      "~/Scripts/Core/AngularJS/angular-route.min.js",
                      "~/Scripts/Core/AngularJS/angular-sanitize.min.js",
                      "~/Scripts/Core/AngularJS/angular-simple-logger.min.js",
                      "~/Scripts/Core/AngularJS/angular-loader.min.js"));
            #endregion

            #region Bootstrap
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/Core/Bootstrap/bootstrap.min.js",
                      "~/Scripts/Core/Bootstrap/bootstrap-datepicker.min.js"));

            bundles.Add(new StyleBundle("~/Content/bootstrap-css").Include(
                      "~/Content/Core/Bootstrap/css/bootstrap.min.css",
                      "~/Content/Core/Bootstrap/css/bootstrap-theme.min.css",
                      "~/Content/Core/Bootstrap/css/bootstrap-datepicker.min.css"));
            #endregion

            #region Bootstrap
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-ui").Include(
                      "~/Scripts/Core/Bootstrap/ui-bootstrap-1.3.1.min.js"));
            #endregion

            #region FontAwesome
            bundles.Add(new StyleBundle("~/Content/font-awesome-css").Include(
                      "~/Content/Core/FontAwesome/css/font-awesome.min.css"));
            #endregion

            #region JQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/Core/JQuery/jquery-1.12.3.min.js"));
            #endregion

            #region JQuery-UI
            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                        "~/Scripts/Core/JQuery/jquery-ui.min.js"));

            bundles.Add(new StyleBundle("~/Content/jquery-ui-css").Include(
                     "~/Content/Core/JQuery-UI/jquery-ui.min.css",
                     "~/Content/Core/JQuery-UI/jquery-ui.theme.min.css"));
            #endregion

            #region Moment
            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                     "~/Scripts/Core/Moment/moment.js"));
            #endregion

            #region Toastr
            bundles.Add(new ScriptBundle("~/bundles/toastr").Include(
                      "~/Scripts/Core/Toastr/toastr.js"));

            bundles.Add(new StyleBundle("~/Content/toastr-css").Include(
                      "~/Content/Core/Toastr/toastr.css"));
            #endregion

            #region Underscore
            bundles.Add(new ScriptBundle("~/bundles/underscore").Include(
                     "~/Scripts/Core/Underscore/underscore.min.js"));
            #endregion

            #region Theme
            bundles.Add(new StyleBundle("~/Content/theme-css").Include(
                     "~/Content/Theme/theme-default.css"));
            #endregion

            #region Application
            bundles.Add(new StyleBundle("~/Content/app-css").Include(
                     "~/Content/Application/Shared/style.css"));

            bundles.Add(new ScriptBundle("~/bundles/app-common").Include(
                     "~/Scripts/Application/Common/Directives/directives.js",
                     "~/Scripts/Application/Common/Filters/filters.js",
                     "~/Scripts/Theme/custom.js",
                     "~/Scripts/Theme/mcustomscrollbar/jquery.mCustomScrollbar.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                    "~/Scripts/Application/app.js",
                    "~/Scripts/Application/services.js",
                    "~/Scripts/Application/config.js",
                    "~/Scripts/Application/Admin/Dashboard/DashboardController.js",
                    "~/Scripts/Application/Admin/Test/TestController.js"
                     ));
            #endregion
        }
    }
}
