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
                //TODO: change to a blank favourites page
                XElement favourites = new XElement("Favourites");
                XElement stops = new XElement("Stops");
                XElement routes = new XElement("Routes");

                XElement stop = new XElement("Stop");
                stop.SetAttributeValue("Number", 55555);
                stop.SetAttributeValue("Name", "TEST STOP");

                stops.Add(stop); 

                favourites.Add(stops);
                favourites.Add(routes);

                await fileService.SaveTextAsync("Favourites.xml", favourites.ToString());

                return await GetFavouriteStopInfos();   
            }
        }

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
