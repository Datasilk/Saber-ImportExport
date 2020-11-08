using Saber.Core;

namespace Saber.Vendor.ImportExport.Import
{
    [ViewPath("/Views/AppSettings/appsettings.html")]
    public class ViewRenderer : IVendorViewRenderer
    {
        public string Render(IRequest request, View view)
        {
            if (!request.CheckSecurity("import")) { return ""; }
            var settingsView = new View("/Vendors/ImportExport/import.html");
            request.AddScript("/editor/js/vendors/importexport/importexport.js");
            return settingsView.Render();
        }
    }
}

namespace Saber.Vendor.ImportExport.Export
{
    [ViewPath("/Views/AppSettings/appsettings.html")]
    public class ViewRenderer : IVendorViewRenderer
    {
        public string Render(IRequest request, View view)
        {
            if (!request.CheckSecurity("export")) { return ""; }
            var settingsView = new View("/Vendors/ImportExport/export.html");
            return settingsView.Render();
        }
    }
}
