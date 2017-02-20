using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translink.Services
{
    public interface ISaveAndLoadService
    {
        Task SaveTextAsync(string fileName, string text);
        Task<string> LoadTextAsync(string fileName);
        bool FileExists(string fileName);  
    }
}
