using Newtonsoft.Json.Linq;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading.Tasks;
using Repository.Entities;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text.Json;

namespace Service.Services
{
    public class AlgorithmService:IAlgorithem
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<string> GetOsmData(double lat, double lon, int radius = 1000)
        {
            string query = $"[out:json];(node(around:{radius},{lat},{lon});way(around:{radius},{lat},{lon}););out body;>;out skel qt;";
            string url = $"https://overpass-api.de/api/interpreter?data={Uri.EscapeDataString(query)}";

            HttpResponseMessage response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API request failed: {response.StatusCode}");
            }

            return await response.Content.ReadAsStringAsync();
        }

        public List<Node> ExtractPoint(string jsonResponse)
        {
            var data = JsonSerializer.Deserialize<OsmResponse>(jsonResponse);
            List<Node> points = new List<Node>();

            foreach (var element in data.elements)
            {
                if (element.type == "node")
                {
                    points.Add(new Node
                    {
                        Id = element.id,
                        Lat = element.lat,
                        Lon = element.lon
                    });
                }
            }
            return points;
        }
        public List<Way> ExtractRoutes(string jsonResponse)
        {
            var data=JsonSerializer.Deserialize<OsmResponse>(jsonResponse);
            List <Way> ways = new List<Way>();
            foreach (var element in data.elements)
            {
                if(element.type == "way")
                {
                    ways.Add(new Way
                    {
                        Id= element.id,

                    });
                }
            }
            return ways;
        }
        
    }
}
