using Saber.Common.ProcessInfo;

namespace Saber.Vendor.ImportExport
{
    public class SaberRecompile: Controller, IVendorController
    {
        public override string Render(string body = "")
        {
            Gulp.Task("default");
            Gulp.Task("default:website");
            return "";
        }
    }
}
