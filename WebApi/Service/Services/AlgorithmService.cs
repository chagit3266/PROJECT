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
using Repository.Interfaces;

namespace Service.Services
{
    public class AlgorithmService : IAlgorithem
    {
        private static readonly HttpClient client=new HttpClient();
        
        public async Task<string> GetOsmData(double lat, double lon, int radius = 1000)
        {
            string query = $"[out:json];(node(around:{radius},{lat},{lon});way(around:{radius},{lat},{lon}););out body;";//out skel qt;";
            string url = $"https://overpass-api.de/api/interpreter?data={Uri.EscapeDataString(query)}";

            HttpResponseMessage response = await client.GetAsync(url);

            //HttpResponseMessage response = await client.GetAsync($"https://overpass-api.de/api/interpreter?data=[out:json];way(around:1000,51.5,-0.1)[highway];out body;");
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

        #region public List<Way> ExtractWays(string jsonResponse)

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

        #endregion

        //יצירת רשימת סמיכויות
        public Dictionary<long, List<Way>> CreateAdjacencyList(List<Node> nodes, List<Way> ways)
        {
            Dictionary<long, List<Way>> adjacency=new Dictionary<long, List<Way>>();

            //מעבר על הנקודות והשמתן במערך סמיכויות
            foreach (var node in nodes)//מכניס את כל הצמתים לטבלת גיבוב
            {
                adjacency.Add(node.Id, new List<Way>());
            }
            foreach (var way in ways)//מכניס כל דרך לטבלת גיבוב בצומת בה היא מתחילה או נגמרת
            {
                adjacency[way.NodeIds[0]].Add(way);
                adjacency[way.NodeIds[way.NodeIds.Count]].Add(way);
            }
            return adjacency;
        }
        public List<Node> Dijkstra(Dictionary<long, List<Way>> adjacencyList)
        {
            Dictionary <long,double> dist = new Dictionary<long, double>();
            Dictionary<long, long> prev = new Dictionary<long, long>();

            return null;
        }
        public double CalculateDistance(Way way,Dictionary<long,Node> nodes)
        {
            double d = 0;
            for (int i = 0; i < way.NodeIds.Count-1; i++)
            {
                d += CalculateDistance(nodes[way.NodeIds[i]],nodes[way.NodeIds[i + 1]]);
            }
            return d;
        }
        /// <summary>
            ///d = 2R * arcsin( sqrt( sin²(Δφ/2) + cos(φ1) * cos(φ2) * sin²(Δλ/2) ) )
            ///φ קווי רוחב,
            ///Δφ = φ2 - φ1
            ///λ קווי אורך,
            ///Δλ = λ2 - λ1
            ///R בערך 6371 ק"מ ) רדיוס של העולם )
            /// </summary>
        public double CalculateDistance(Node start, Node end)
        {
            double d = 0;
            //d = 2R * arcsin( sqrt( sin²(Δφ/2) + cos(φ1) * cos(φ2) * sin²(Δλ/2) ) ) 
            const double R = 6371e3;//רדיוס במטרים
            double φ1 = start.Lat * Math.PI / 180;//...tan,cos,sin 'המרה ממעלות לרדיאנים ע"מ שנוכל להשתמש בפונק
            double φ2 = end.Lat * Math.PI / 180;
            double Δφ = (end.Lat - start.Lat) * Math.PI / 180;
            double Δλ = (end.Lon - start.Lon) * Math.PI / 180;
            d= 2 * R *
                Math.Asin(
                    Math.Sqrt(
                        Math.Pow(Math.Sin(Δφ / 2), 2) +
                        (Math.Cos(φ1) * 
                        Math.Cos(φ2) * 
                        Math.Pow(Math.Sin(Δλ / 2), 2))
                        )
                    );
            return d;
        }
    }
}
