using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.InteropServices;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;
using WebDriverManager.Helpers;

namespace WebDriverManager.Services.Impl
{
    public class BinaryService : IBinaryService
    {
        static readonly object _object = new object();

        public string SetupBinary(string url, string zipDestination, string binDestination, string binaryName)
        {
            if (File.Exists(binDestination)) return binDestination;
            FileHelper.CreateDestinationDirectory(zipDestination);
            zipDestination = DownloadZip(url, zipDestination);
            FileHelper.CreateDestinationDirectory(binDestination);

            if (zipDestination.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            {
                File.Copy(zipDestination, binDestination);
            }
            else if (zipDestination.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            {
                UnZip(zipDestination, binDestination, binaryName);
            }
            else if (zipDestination.EndsWith(".tar.gz", StringComparison.OrdinalIgnoreCase))
            {
                ExtractGZipFile(zipDestination, binDestination);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
                RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var chmod = Process.Start("chmod", $"+x {binDestination}");
                chmod.WaitForExit();
            }

            RemoveZip(zipDestination);
            return binDestination;
        }

        protected string DownloadZip(string url, string destination)
        {
            if (File.Exists(destination)) return destination;
            using (var webClient = new WebClient())
            {
                webClient.DownloadFile(new Uri(url), destination);
            }

            return destination;
        }

        protected string UnZip(string path, string destination, string name)
        {
            lock (_object)
            {
                var zipName = Path.GetFileName(path);
                if (zipName != null && zipName.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    File.Copy(path, destination);
                    return destination;
                }

                using (var zip = ZipFile.Open(path, ZipArchiveMode.Read))
                {
                    foreach (var entry in zip.Entries)
                    {
                        if (entry.Name == name)
                        {
                            entry.ExtractToFile(destination, true);
                        }
                    }
                }

                return destination;
            }
        }

        protected string ExtractGZipFile(string gzipFileName, string targetDir)
        {

            // Use a 4K buffer. Any larger is a waste.    
            var dataBuffer = new byte[4096];

            using (Stream fs = new FileStream(gzipFileName, FileMode.Open, FileAccess.Read))
            {
                using (var gzipStream = new GZipInputStream(fs))
                {

                    // Change this to your needs
                    var fnOut = Path.Combine(targetDir, Path.GetFileNameWithoutExtension(gzipFileName));

                    using (var fsOut = File.Create(fnOut))
                    {
                        StreamUtils.Copy(gzipStream, fsOut, dataBuffer);
                    }

                    return fnOut;
                }
            }
        }

        protected void RemoveZip(string path)
        {
            File.Delete(path);
        }
    }
}
