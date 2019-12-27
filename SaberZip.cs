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
                                    case "css":
                                    case "editor":
                                    case "js":
                                    case "themes":
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
                                            copyTo = "/Content/pages/";
                                            break;

                                        case "partials":
                                            copyTo = "/Content/partials/";
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
                                copyTo = "/Scripts/";
                            }
                            break;
                    }

                    if (copyTo != "")
                    {
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
                                if (!Directory.Exists(Server.MapPath(copyTo)))
                                {
                                    Directory.CreateDirectory(Server.MapPath(copyTo));
                                }
                                File.WriteAllBytes(Server.MapPath(copyTo + entry.Name), bytes);
                            }
                        }
                    }
                }
            }
            Thread.Sleep(500);

            //run default gulp command to copy new website resources to wwwroot folder
            Gulp.Task("default");
            Gulp.Task("default:website");
            Thread.Sleep(500);
        }

        private static void AddDirectoryToArchive(ZipArchive archive, DirectoryInfo parent)
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
