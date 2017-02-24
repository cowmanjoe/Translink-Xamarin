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
        #region IFavouritesDataService Implementation

        public async Task<List<string>> GetFavouriteRouteNumbers()
        {
            ISaveAndLoadService fileService = DependencyService.Get<ISaveAndLoadService>();

            if (fileService.FileExists("Favourites.xml"))
            {
                string text = await fileService.LoadTextAsync("Favourites.xml");

                List<string> routeNumbers;
                using (Stream stream = GenerateStreamFromString(text))
                {
                    routeNumbers = ParseFavouriteRouteNumbers(stream);
                }

                return routeNumbers;
            }
            else
            {
                await CreateNewFavouritesFile(fileService);

                return await GetFavouriteRouteNumbers();
            }
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
            ISaveAndLoadService fileService = DependencyService.Get<ISaveAndLoadService>();
            if (!fileService.FileExists("Favourites.xml"))
            {
                await CreateNewFavouritesFile(fileService);
            }

            string text = await fileService.LoadTextAsync("Favourites.xml");
            using (Stream stream = GenerateStreamFromString(text))
            {
                XDocument xDoc = XDocument.Load(stream);
                XElement routesElement = xDoc.Root.Element("Routes");
                var routes = routesElement.Descendants("Route");

                foreach (var r in routes)
                {
                    if (r.Attribute("Number").Value == routeNumber)
                        return;
                }
                XElement newRoute = new XElement("Route");
                newRoute.SetAttributeValue("Number", routeNumber);

                routesElement.Add(newRoute);
                await fileService.SaveTextAsync("Favourites.xml", xDoc.ToString());
            }
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
                XDocument xDoc = XDocument.Load(stream); 
                XElement stopsElement = xDoc.Root.Element("Stops");
                var stops = stopsElement.Descendants();
                
                foreach (var s in stops)
                {
                    if (Convert.ToInt32(s.Attribute("Number").Value) == stopInfo.Number)
                        return; 
                }
                XElement newStop = new XElement("Stop");
                newStop.SetAttributeValue("Number", stopInfo.Number);
                newStop.SetAttributeValue("Name", stopInfo.Name);

                stopsElement.Add(newStop); 
                await fileService.SaveTextAsync("Favourites.xml", xDoc.ToString());
                
            }
        }

        public async Task RemoveFavouriteRoute(string routeNumber)
        {
            ISaveAndLoadService fileService = DependencyService.Get<ISaveAndLoadService>(); 
            if (!fileService.FileExists("Favourites.xml"))
            {
                throw new ArgumentException("No Favourites.xml file exits yet, so there are no favourites to remove.");
            }

            string text = await fileService.LoadTextAsync("Favourites.xml");
            using (Stream stream = GenerateStreamFromString(text))
            {
                XDocument xDoc = XDocument.Load(stream);
                XElement routesElement = xDoc.Root.Element("Routes");
                var routes = routesElement.Descendants("Route"); 

                foreach(var r in routes)
                {
                    if (Util.RouteEquals(r.Attribute("Number").Value, routeNumber))
                    {
                        r.Remove();
                        await fileService.SaveTextAsync("Favourites.xml", xDoc.ToString());
                        return; 
                    }
                }
            }
            throw new ArgumentException("Route " + routeNumber + " not found in favourites."); 
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
                XDocument xDoc = XDocument.Load(stream); 
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

        public async Task ClearFavouriteStops()
        {
            ISaveAndLoadService fileService = DependencyService.Get<ISaveAndLoadService>();
            if (fileService.FileExists("Favourites.xml"))
            {
                string text = await fileService.LoadTextAsync("Favourites.xml");
                using (Stream stream = GenerateStreamFromString(text))
                {
                    XDocument xDoc = XDocument.Load(stream);
                    XElement stopsElement = xDoc.Root.Element("Stops");
                    stopsElement.Remove();
                    XElement newStopsElement = new XElement("Stops");
                    xDoc.Root.Add(newStopsElement);

                    await fileService.SaveTextAsync("Favourites.xml", xDoc.ToString()); 
                }
            }
        }

        public async Task ClearFavouriteRoutes()
        {
            ISaveAndLoadService fileService = DependencyService.Get<ISaveAndLoadService>();
            if (fileService.FileExists("Favourites.xml"))
            {
                string text = await fileService.LoadTextAsync("Favourites.xml");
                using (Stream stream = GenerateStreamFromString(text))
                {
                    XDocument xDoc = XDocument.Load(stream);
                    XElement routesElement = xDoc.Root.Element("Routes");
                    routesElement.Remove();
                    XElement newRoutesElement = new XElement("Routes");
                    xDoc.Root.Add(newRoutesElement);

                    await fileService.SaveTextAsync("Favourites.xml", xDoc.ToString());
                }
            }
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

        // TODO: make private (public for testing)
        public List<string> ParseFavouriteRouteNumbers(Stream stream)
        {
            List<string> routeNumbers = new List<string>();

            XDocument xDoc = XDocument.Load(stream);
            var routesElement = xDoc.Root.Element("Routes");
            var routeContainer = routesElement.Descendants("Route");

            foreach (var r in routeContainer)
            {
                string routeNumber = r.Attribute("Number").Value;
                routeNumbers.Add(routeNumber);
            }

            return routeNumbers;
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
