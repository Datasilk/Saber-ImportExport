namespace Saber.Vendor.ImportExport.Import
{
    [ViewPath("/Views/AppSettings/appsettings.html")]
    public class ViewRenderer : IVendorViewRenderer
    {
        public string Render(Request request, View view)
        {
            var settingsView = new View("/Vendor/ImportExport/import.html");
            request.AddScript("/editor/js/vendor/importexport/importexport.js");
            return settingsView.Render();
        }
    }
}

namespace Saber.Vendor.ImportExport.Export
{
    [ViewPath("/Views/AppSettings/appsettings.html")]
    public class ViewRenderer : IVendorViewRenderer
    {
        public string Render(Request request, View view)
        {
            var settingsView = new View("/Vendor/ImportExport/export.html");
            return settingsView.Render();
        }
    }
}
