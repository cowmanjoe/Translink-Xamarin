using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Xml.Linq;

namespace Translink.Services
{
    public class FavouritesDataService : IFavouritesDataService
    {
        
        #region IFavouritesDataService Implmentation

        public async Task<List<string>> GetFavouriteRouteNumbers()
        {
            throw new NotImplementedException(); 
        }

        public async Task<List<StopInfo>> GetFavouriteStopInfos()
        {
            ISaveAndLoadService fileService = DependencyService.Get<ISaveAndLoadService>();
            if (fileService.FileExists("Favourites.xml"))
            {
                string text = await fileService.LoadTextAsync("Favourites.xml");

                List<StopInfo> stopInfos;
                using (Stream stream = GenerateStreamFromString(text))
                {
                    stopInfos = ParseFavouriteStopInfos(stream);
                }

                return stopInfos;
            }
            else
            {
                await CreateNewFavouritesFile(fileService); 

                return await GetFavouriteStopInfos();   
            }
        }

        public async Task AddFavouriteRoute(string routeNumber)
        {
            throw new NotImplementedException();
        }

        public async Task AddFavouriteStop(StopInfo stopInfo)
        {
            ISaveAndLoadService fileService = DependencyService.Get<ISaveAndLoadService>();
            if (!fileService.FileExists("Favourites.xml"))
            {
                await CreateNewFavouritesFile(fileService); 
            }
            
            string text = await fileService.LoadTextAsync("Favourites.xml");
            using (Stream stream = GenerateStreamFromString(text))
            {
                XDocument xDoc = new XDocument(stream);
                XElement stops = xDoc.Root.Element("Stops");
                XElement newStop = new XElement("Stop");
                newStop.SetAttributeValue("Number", stopInfo.Number);
                newStop.SetAttributeValue("Name", stopInfo.Name);
                stops.Add(newStop); 
                await fileService.SaveTextAsync("Favourites.xml", xDoc.ToString());
            }
        }

        public Task RemoveFavouriteRoute(string routeNumber)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveFavouriteStop(int stopNumber)
        {
            ISaveAndLoadService fileService = DependencyService.Get<ISaveAndLoadService>();
            if (!fileService.FileExists("Favourites.xml"))
            {
                throw new ArgumentException("No Favourites.xml file exists yet, so there are no favourites to remove."); 
            }

            string text = await fileService.LoadTextAsync("Favourites.xml");
            using (Stream stream = GenerateStreamFromString(text))
            {
                XDocument xDoc = new XDocument(stream);
                XElement stops = xDoc.Root.Element("Stops");
                var stopContainer = xDoc.Descendants("Stop"); 

                foreach (var stop in stopContainer)
                {
                    if (Convert.ToInt32(stop.Attribute("Number").Value) == stopNumber)
                    {
                        stop.Remove();
                        await fileService.SaveTextAsync("Favourites.xml", xDoc.ToString()); 
                        return; 
                    }
                }
            }
            throw new ArgumentException("Stop #" + stopNumber + " not found in favourites.");
        }

        #endregion

        // TODO: make private (public for testing) 
        public List<StopInfo> ParseFavouriteStopInfos(Stream stream)
        {
            List<StopInfo> stopInfos = new List<StopInfo>();

            XDocument xDoc = XDocument.Load(stream);
            var stopsElement = xDoc.Root.Element("Stops");
            var stopContainer = stopsElement.Descendants("Stop");

            foreach (var stopElement in stopContainer)
            {
                int number = Convert.ToInt32(stopElement.Attribute("Number").Value);
                string name = stopElement.Attribute("Name").Value;
                StopInfo stopInfo = new StopInfo(number, name);
                stopInfos.Add(stopInfo);
            }

            return stopInfos;
        }

        async Task CreateNewFavouritesFile(ISaveAndLoadService fileService)
        {
            XElement favourites = new XElement("Favourites");
            XElement stops = new XElement("Stops");
            XElement routes = new XElement("Routes");

            favourites.Add(stops);
            favourites.Add(routes);

            await fileService.SaveTextAsync("Favourites.xml", favourites.ToString());
        }

        Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream; 
        }
    }
}
