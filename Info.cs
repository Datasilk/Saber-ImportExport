using Saber.Vendor;

namespace Saber.Vendors.ImportExport
{
    public class Info : IVendorInfo
    {
        public string Key { get; set; } = "ImportExport";
        public string Name { get; set; } = "Import/Export";
        public string Description { get; set; } = "Allows administrators to export their website content (HTML, LESS, CSS, JS, JSON, images, documents, etc) and import website content from a zip file.";
        public Version Version { get; set; } = "1.0.0.0";
    }
}
