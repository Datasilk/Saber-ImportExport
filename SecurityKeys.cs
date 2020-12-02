using Saber.Core;
using Saber.Vendor;

namespace Saber.Vendors.ImportExport
{
    public class SecurityKeys : IVendorKeys
    {
        public string Vendor { get; set; } = "Import Export";
        public SecurityKey[] Keys { get; set; } = new SecurityKey[]
        {
            new SecurityKey(){Value = "import", Label = "Import", Description = "Able to import zip files to update content on the website"},
            new SecurityKey(){Value = "export", Label = "Export", Description = "Able to export the existing website to zip format"}
        };
    }
}
