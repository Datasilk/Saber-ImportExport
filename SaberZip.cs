using System;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Saber.Vendors.ImportExport
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
                    var files = Core.Website.AllFiles();
                    var root = App.MapPath("/") + (App.IsDocker ? "/" : "\\");
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
                    Console.WriteLine("entry: " + entry.FullName);
                    var path = entry.FullName.Replace(entry.Name, "").Replace("\\", "/");
                    var paths = path.Split("/", StringSplitOptions.RemoveEmptyEntries);
                    var exts = entry.Name.ToLower().Split(".");
                    var extension = exts[^1];
                    var copyTo = "";
                    var root = paths[0].ToLower();

                    //ignore restricted file extensions (potentially dangerous malicious files)
                    if (Malicious.FileExtensions.Contains(extension)) { continue; }

                    switch (root)
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
                            else
                            {
                                switch (entry.Name.ToLower())
                                {
                                    case "website.less":
                                    case "website.json":
                                        copyTo = "/Content/";
                                        break;
                                }
                            }
                            break;
                    }

                    if (copyTo != "")
                    {
                        Console.WriteLine("copy to: " + copyTo + entry.Name);
                        if (!Directory.Exists(App.MapPath(copyTo)))
                        {
                            Directory.CreateDirectory(App.MapPath(copyTo));
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

                            File.WriteAllBytes(App.MapPath(copyTo + entry.Name), bytes);
                            if (extension == "less")
                            {
                                //compile less file to public wwwroot folder
                                var lesspath = "";
                                switch (root)
                                {
                                    case "content":
                                        if (entry.Name.ToLower() == "website.less")
                                        {
                                            lesspath = "/wwwroot/css/";
                                        }
                                        else
                                        {
                                            lesspath = "/wwwroot/" + path.Replace("Content/", "content/");
                                        }
                                        break;
                                }
                                if (!string.IsNullOrEmpty(lesspath))
                                {
                                    Console.WriteLine("compiling LESS file: " + App.MapPath(lesspath + entry.Name.Replace(".less", ".css")));

                                    if (!Directory.Exists(App.MapPath(lesspath)))
                                    {
                                        Directory.CreateDirectory(App.MapPath(lesspath));
                                    }
                                    var data = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                                    //Website.SaveLessFile(data, lesspath + entry.Name.Replace(".less", ".css"), copyTo);
                                }
                                
                            }
                            else if (root == "content" && extension == "js")
                            {
                                //copy js file to public wwwroot folder
                                Console.WriteLine("copying JS file: " + App.MapPath("/wwwroot/" + path.Replace("Content/", "content/") + entry.Name));
                                File.WriteAllBytes(App.MapPath("/wwwroot/" + path.Replace("Content/", "content/") + entry.Name), bytes);
                            }
                        }
                    }
                }

                //finally, recompile website.css
                //Website.SaveLessFile(File.ReadAllText(App.MapPath("/Content/website.less")), "/wwwroot/css/website.css", "/Content");
            }
        }
    }
}
