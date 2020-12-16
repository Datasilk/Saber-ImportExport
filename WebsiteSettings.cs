using System.Text;
using Saber.Core;
using Saber.Vendor;

namespace Saber.Vendors.ImportExport
{
    public class WebsiteSettings : IVendorWebsiteSettings
    {
        public string Name { get; set; } = "Import/Export Website";

        public string Render(IRequest request)
        {
            var html = new StringBuilder();
            var access = false;
            if (request.CheckSecurity("import")) {
                html.Append(Cache.LoadFile(App.MapPath("/Vendors/ImportExport/import.html")));
                access = true;
            }
            if (request.CheckSecurity("export"))
            {
                html.Append(Cache.LoadFile(App.MapPath("/Vendors/ImportExport/export.html")));
                access = true;
            }
            if (access)
            {
                request.AddScript("/editor/vendors/importexport/importexport.js");
            }
            return html.ToString();
        }
    }
}
