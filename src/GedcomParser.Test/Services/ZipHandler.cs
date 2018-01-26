using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using static System.IO.Compression.ZipStorer;


namespace GedcomParser.Test.Services
{
    public static class ZipHandler
    {
        public static void SaveToZipFile(object obj, string zipFilePath, string jsonFileName)
        {
            var settings = new JsonSerializerSettings { Formatting = Formatting.Indented, NullValueHandling = NullValueHandling.Ignore };
            var json = JsonConvert.SerializeObject(obj, settings);
            using (var zipStorer = Create(zipFilePath, string.Empty))
            {
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                {
                    zipStorer.AddStream(Compression.Deflate, jsonFileName, ms, DateTime.Now, string.Empty);
                }
            }
        }

        public static T ParseFromZipFile<T>(string zipFilePath, string jsonFileName) where T : class
        {
            using (var zipStorer = Open(zipFilePath, FileAccess.Read))
            {
                var zipDir = zipStorer.ReadCentralDir();
                foreach (var zipEntry in zipDir)
                {
                    if (Path.GetFileName(zipEntry.FilenameInZip) == jsonFileName)
                    {
                        using (var ms = new MemoryStream())
                        {
                            zipStorer.ExtractFile(zipEntry, ms);
                            var json = Encoding.UTF8.GetString(ms.ToArray());
                            return JsonConvert.DeserializeObject<T>(json);
                        }
                    }
                }
                return null;
            }
        }
    }
}