using System;
using Translink.Services;
using Xamarin.Forms;
using Translink.Droid;
using System.Threading.Tasks;
using System.IO;

[assembly: Dependency (typeof(SaveAndLoadServiceAndroid))]

namespace Translink.Droid
{
    public class SaveAndLoadServiceAndroid : ISaveAndLoadService
    {
        #region ISaveAndLoad implementation

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

        string CreatePathToFile(string fileName)
        {
            var docsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(docsPath, fileName); 
        }
    }
}