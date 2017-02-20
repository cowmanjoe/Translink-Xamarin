using System;
using Xamarin.Forms;
using Translink.iOS;
using System.IO;
using Translink.Services;
using System.Threading.Tasks;
using Foundation;
using System.Linq;

[assembly: Dependency (typeof(SaveAndLoadIOS))]

namespace Translink.iOS
{

    public class SaveAndLoadIOS : ISaveAndLoadService
    {
        public static string DocumentsPath
        {
            get
            {
                var documentsDirUrl = NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User).Last();
                return documentsDirUrl.Path; 
            }
        }

        #region ISaveAndLoadService Implementation

        public bool FileExists(string fileName)
        {
            return File.Exists(CreatePathToFile(fileName));
        }

        public async Task<string> LoadTextAsync(string fileName)
        {
            var path = CreatePathToFile(fileName);
            using (StreamReader sr = File.OpenText(path))
                return await sr.ReadToEndAsync();
        }

        public async Task SaveTextAsync(string fileName, string text)
        {
            var path = CreatePathToFile(fileName);
            using (StreamWriter sw = File.CreateText(path))
                await sw.WriteAsync(text);
        }

        #endregion

        static string CreatePathToFile(string fileName)
        {
            return Path.Combine(DocumentsPath, fileName);
        }
    }
}
