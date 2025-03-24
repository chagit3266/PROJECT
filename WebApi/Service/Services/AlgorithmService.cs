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
        public List<Node> ExtractNodes(string json)
        {
            JObject jsonResponse = JObject.Parse(json);
            List<Node> nodes = new List<Node>();

            foreach (var element in jsonResponse["elements"])
            {
                if (element["type"].ToString() == "node")
                {
                    nodes.Add(new Node
                    {
                        Lat = (double)element["lat"],
                        Lon = (double)element["lon"]
                    });
                }
            }
            return nodes;
        }
        public List<Way> ExtractWays(string json)
        {
            JObject jsonResponse = JObject.Parse(json);
            List<Way> ways = new List<Way>();
            foreach (var element in jsonResponse["elements"])
            {
                if (element["type"].ToString() == "way")
                {
                    ways.Add(new Way
                    {
                        //NodeIds = (List<long>)element["nodes"],
                        NodeIds = element["nodes"] != null ? element["nodes"].ToObject<List<long>>() : new List<long>(),
                        HighwayType = element["tags"] is JObject tags && tags.TryGetValue("highway", out JToken highwayValue)
                                       ? highwayValue.ToString() : "unknown",
                    });
                }
            }
            return ways;
        }
        //public List<Way> ExtractWays(string jsonResponse)
        //{
        //    var data=JsonSerializer.Deserialize<OsmResponse>(jsonResponse);
        //    List <Way> ways = new List<Way>();
        //    foreach (var element in data.elements)
        //    {
        //        if(element.type == "way")
        //        {
        //            ways.Add(new Way
        //            {
        //                Id= element.id,
        //                NodeIds = element.nodes,
        //                HighwayType = element.tags != null && element.tags.ContainsKey("highway") ? element.tags["highway"] : "unknown"
        //            });
        //        }
        //    }
        //    return ways;
        //}


    }
}
