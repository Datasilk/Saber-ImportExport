namespace Saber.Vendor.ImportExport
{
    [ViewPath("/Views/AppSettings/appsettings.html")]
    public class ViewRenderer : IVendorViewRenderer
    {
        public void Render(Request request, View view)
        {
            var settingsView = new View("/Vendor/ImportExport/importexport.html");
            request.AddScript("/js/vendor/importexport/importexport.js");
            view["vendor"] += settingsView.Render();
        }
    }
}
