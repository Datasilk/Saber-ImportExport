using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Saber.Vendor.ImportExport
{
    public class SaberExport : Controller, IVendorController
    {
        public override string Render(string body = "")
        {
            if (!CheckSecurity()) { return AccessDenied<Controllers.Login>(); }
            try
            {
                //generate zip archive in memory
                byte[] zip = null;
                using (var ms = new MemoryStream())
                {
                    using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                    {
                        //add content to zip archive
                        var parent = new DirectoryInfo(Server.MapPath("/Content/pages"));
                        AddDirectoryToArchive(archive, parent);
                        parent = new DirectoryInfo(Server.MapPath("/Content/partials"));
                        AddDirectoryToArchive(archive, parent);
                        archive.CreateEntryFromFile(Server.MapPath("/CSS/website.less"), "CSS/website.less", CompressionLevel.Fastest);
                        archive.CreateEntryFromFile(Server.MapPath("/Scripts/website.js"), "Scripts/website.js", CompressionLevel.Fastest);
                        parent = new DirectoryInfo(Server.MapPath("/wwwroot"));
                        foreach (var dir in parent.GetDirectories())
                        {
                            switch (dir.Name)
                            {
                                case "content":
                                case "css":
                                case "editor":
                                case "js":
                                case "themes":
                                    //ignore platform-specific wwwroot directories
                                    break;
                                default:
                                    AddDirectoryToArchive(archive, dir);
                                    break;
                            }
                        }
                    }
                    ms.Position = 0;

                    var content = new StreamContent(ms);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");
                    content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    content.Headers.ContentDisposition.FileName = "SaberExport.zip";
                    Context.Response.ContentLength = content.Headers.ContentLength;
                    Context.Response.ContentType = "application/zip";
                    Context.Response.StatusCode = 200;
                    content.CopyToAsync(Context.Response.Body);
                }
            }
            catch (Exception)
            {

            }
            return "";
        }

        private void AddDirectoryToArchive(ZipArchive archive, DirectoryInfo parent)
        {
            foreach (var file in parent.GetFiles())
            {
                archive.CreateEntryFromFile(file.FullName, file.FullName.Replace(Server.MapPath("/") + "\\", ""), CompressionLevel.Fastest);
            }
            foreach (var dir in parent.GetDirectories())
            {
                AddDirectoryToArchive(archive, dir);
            }
        }
    }
}
