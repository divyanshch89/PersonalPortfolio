using System.Web;
using System.Web.Optimization;

namespace Portfolio
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            

           

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
          

           

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/default.css",
                      "~/Content/layout.css",
                      "~/Content/media-queries.css",
                      "~/Content/magnific-popup.css"));
        }
    }
}
