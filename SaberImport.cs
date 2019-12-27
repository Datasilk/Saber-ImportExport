using System.IO;

namespace Saber.Vendor.ImportExport
{
    public class SaberImport : Controller, IVendorController
    {
        public override string Render(string body = "")
        {
            if (Context.Request.Form.Files[0].ContentType != "application/x-zip-compressed")
            {
                return Error("Import file must be a compressed zip file.");
            }
            //create backup of website
            var copyTo = "Content/backups/";
            if (!Directory.Exists(Server.MapPath(copyTo)))
            {
                Directory.CreateDirectory(Server.MapPath(copyTo));
            }
            File.WriteAllBytes(copyTo + "latest.zip", SaberZip.Export());

            //open uploaded zip file
            using (var ms = Context.Request.Form.Files[0].OpenReadStream())
            {
                SaberZip.Import(ms);
            }

            return "latest.zip";
        }

        
    }
}
