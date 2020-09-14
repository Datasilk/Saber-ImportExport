using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Threading;
using Saber.Common.ProcessInfo;

namespace Saber.Vendor.ImportExport
{
    public static class SaberZip
    {
        public static byte[] Export()
        {
            //generate zip archive in memory
            var fms = new MemoryStream();
            using (var ms = new MemoryStream())
            {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    var files = Common.Platform.Website.AllFiles();
                    var root = Server.MapPath("/") + (Server.IsDocker ? "/" : "\\");
                    foreach (var file in files)
                    {
                        archive.CreateEntryFromFile(file, file.Replace(root, ""), CompressionLevel.Fastest);
                    }
                }
                ms.Position = 0;
                var buffer = new byte[512];
                var bytesRead = 0;
                while ((bytesRead = ms.Read(buffer, 0, buffer.Length)) > 0)
                    fms.Write(buffer, 0, bytesRead);
            }
            return fms.ToArray();
        }

        public static void Import(Stream stream)
        {
            //read zip archive contents
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Read, true))
            {
                var contentFiles = new string[] { "json", "html", "less", "js" };
                var buffer = new byte[512];
                var bytesRead = default(int);

                foreach (var entry in archive.Entries)
                {
                    if(entry.Name == "") { continue; }
                    var path = entry.FullName.Replace(entry.Name, "").Replace("\\", "/");
                    var paths = path.Split("/");
                    var exts = entry.Name.ToLower().Split(".");
                    var extension = exts[^1];
                    var copyTo = "";

                    //ignore restricted file extensions (potentially dangerous malicious files)
                    if (Malicious.FileExtensions.Contains(extension)) { continue; }

                    switch (paths[0].ToLower())
                    {
                        case "wwwroot":
                            if (paths.Length > 1)
                            {
                                switch (paths[1].ToLower())
                                {
                                    case "content":
                                        if(extension != "js" && extension != "css")
                                        {
                                            copyTo = string.Join("/", paths);
                                        }
                                        break;
                                    case "editor":
                                        break;

                                    default:
                                        copyTo = string.Join("/", paths);
                                        break;
                                }
                            }
                            break;

                        case "content":
                            if (paths.Length > 1)
                            {
                                if (contentFiles.Contains(extension))
                                {
                                    switch (paths[1].ToLower())
                                    {
                                        case "pages":
                                            copyTo = path;
                                            break;

                                        case "partials":
                                            copyTo = path;
                                            break;
                                    }
                                }
                            }
                            break;
                        case "css":
                            if (entry.Name.ToLower() == "website.less")
                            {
                                copyTo = "/CSS/";
                            }
                            break;
                        case "scripts":
                            if (entry.Name.ToLower() == "website.js")
                            {
                                copyTo = "/wwwroot/js/";
                            }
                            break;
                    }

                    if (copyTo != "")
                    {
                        if (!Directory.Exists(Server.MapPath(copyTo)))
                        {
                            Directory.CreateDirectory(Server.MapPath(copyTo));
                        }
                        using (var file = entry.Open())
                        {
                            var fms = new MemoryStream();
                            buffer = new byte[512];
                            bytesRead = 0;
                            byte[] bytes;
                            while ((bytesRead = file.Read(buffer, 0, buffer.Length)) > 0)
                                fms.Write(buffer, 0, bytesRead);
                            bytes = fms.ToArray();

                            using (var sr = new StreamReader(file))
                            {
                                var data = sr.ReadToEnd();
                                File.WriteAllBytes(Server.MapPath(copyTo + entry.Name), bytes);
                            }
                        }
                    }
                }
            }
            Thread.Sleep(500);

            //run default gulp command to copy new website resources to wwwroot folder
            Gulp.Task("website");
            Thread.Sleep(500);
        }
    }
}
