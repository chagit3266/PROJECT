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
        private static readonly HttpClient client = new HttpClient();
        //openstreetmap הבאת נקודות מ 
        public async Task<string> GetOsmData(double lat, double lon, double radius = 1000)
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
        //עיבוד נתונים

        //צמתים - נקודות
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
        //קשתות - מסלולים
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
        // אלגוריתם למציאת מרחק בין 2 נקודות
        // 'ע"מ לבדוק אם צריך לשמור נק
        // Way וכן כדי לחשב אורך 
        public double CalculateDistance(Node start, Node end)
        {
            double d = 0;
            //d = 2R * arcsin( sqrt( sin²(Δφ/2) + cos(φ1) * cos(φ2) * sin²(Δλ/2) ) ) 
            const double R = 6371e3;//רדיוס במטרים
            double φ1 = start.Lat * Math.PI / 180;//...tan,cos,sin 'המרה ממעלות לרדיאנים ע"מ שנוכל להשתמש בפונק
            double φ2 = end.Lat * Math.PI / 180;
            double Δφ = (end.Lat - start.Lat) * Math.PI / 180;
            double Δλ = (end.Lon - start.Lon) * Math.PI / 180;
            d = 2 * R *
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
        //ישלח לפונקציה המועמסת כל 2 נקודות ויבצע חיבור בין כל המרחקים
        public double CalculateDistance(Way way, Dictionary<long, Node> nodes)
        {
            double d = 0;
            for (int i = 0; i < way.NodeIds.Count - 1; i++)
            {
                d += CalculateDistance(nodes[way.NodeIds[i]], nodes[way.NodeIds[i + 1]]);
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

        //פונקציה לחישוב אמצע קטע
        public Node MidNode (Node start, Node end)
        {
            //המרת קווי רוחב ואורך ממעלות לרדיאנים
            double φ1 = start.Lat * Math.PI / 180;//...tan,cos,sin 'המרה ממעלות לרדיאנים ע"מ שנוכל להשתמש בפונק
            double φ2 = end.Lat * Math.PI / 180;
            double λ1 = start.Lon * Math.PI / 180;
            double λ2 = end.Lon * Math.PI / 180;
            //חישוב קואורדינטות קרטזיות עבור כל נקודה
            double x1 = Math.Cos(φ1) * Math.Cos(λ1);
            double y1 = Math.Cos(φ1) * Math.Sin(λ1);
            double z1 = Math.Sin(φ1);

            double x2 = Math.Cos(φ2) * Math.Cos(λ2);
            double y2 = Math.Cos(φ2) * Math.Sin(λ2);
            double z2 = Math.Sin(φ2);
            //חשב את נקודת האמצע בקואורדינטות קרטזיות
            double x_m = (x1 + x2) / 2;
            double y_m = (y1 + y2) / 2;
            double z_m = (z1 + z2) / 2;
            //המר חזרה לקו רוחב ואורך
            double λm = Math.Atan2(y_m, x_m);
            double φm = Math.Atan2(z_m, Math.Sqrt(x_m * x_m + y_m * y_m));
            //המר את התוצאה בחזרה למעלות
            return new Node {
                Lat=φm * 180 / Math.PI, 
                Lon=λm * 180 / Math.PI 
            };
        }
        //יצירת רשימת סמיכויות
        public Dictionary<long, List<Way>> CreateAdjacencyList(List<Node> nodes, List<Way> ways)
        {
            Dictionary<long, List<Way>> adjacency = new Dictionary<long, List<Way>>();

            //מעבר על הנקודות והשמתן במערך סמיכויות
            //foreach (var node in nodes)//מכניס את כל הצמתים לטבלת גיבוב
            //{
            //    adjacency[node.Id]=new List<Way>();
            //}
            foreach (var way in ways)//מכניס כל דרך לטבלת גיבוב בצומת בה היא מתחילה או נגמרת
            {
                long startNodeId = way.NodeIds[0];
                long endNodeId = way.NodeIds[way.NodeIds.Count - 1];
                if (!adjacency.ContainsKey(startNodeId))
                {
                    adjacency[startNodeId] = new List<Way>();
                }
                if (!adjacency.ContainsKey(endNodeId))
                {
                    adjacency[endNodeId] = new List<Way>();
                }
                adjacency[startNodeId].Add(way);
                adjacency[endNodeId].Add(way);
            }
            return adjacency;
        }
        //דייקסטרה
        public Dictionary<long, long?> Dijkstra(Dictionary<long, List<Way>> adjacencyList, long source)
        {
            Dictionary<long, double> dist = adjacencyList.Keys.ToDictionary(node => node, node => double.MaxValue);
            Dictionary<long, long?> prev = adjacencyList.Keys.ToDictionary(node => node, node => (long?)null);
            // הגדרת צומת ההתחלה
            dist[source] = 0;
            
            // יצירת תור עדיפויות
            // ממיין לפי מרחקים
            //long nodeId, double currentDist;
            PriorityQueue< long, double > h = new PriorityQueue<long, double>();
            h.Enqueue(source, 0);
            while (h.Count > 0)
            {
                // מוציאים את הצומת הקרוב ביותר
                long u = h.Dequeue();

                // עוברים על כל השכנים של הצומת u
                if (adjacencyList.ContainsKey(u))
                {
                    foreach (var way in adjacencyList[u])
                    {
                        long v = way.NodeIds[way.NodeIds.Count-1];  // הצומת השכן
                        double weight = CalculateDistance(way,);  // משקל הדרך בין הצמתים

                        // אם נמצא מסלול קצר יותר לצומת v, מעדכנים את המרחק
                        if (dist[u] + weight < dist[v])
                        {
                            dist[v] = dist[u] + weight;
                            prev[v] = u;

                            // דוחפים את הצומת החדש לתור עם המרחק החדש
                            h.Enqueue(v, dist[v]);
                        }
                    }
                }
            }
            return prev;
        }
        //תכנון מסלול
        public async Task<List<Node>> CalculateRoute(Node start, Node end)
        {
            var mid =MidNode(start,end);//end ל start מציאת נקודת אמצע בין 
            double dist=CalculateDistance(start, end);//מציאת המרחק בין שתי הנקודות כדי לדעת איזה רדיוס לשלוח לפונקציה
            string data =await GetOsmData(mid.Lat,mid.Lon,dist);//באזור המבוקש waysו nodes עם json מקבל
            List<Node> nodes = ExtractNodes(data);
            List<Way> ways = ExtractWays(data);
            Dictionary<long, List<Way>> adjacencyList = CreateAdjacencyList(nodes, ways);
            //אם הדרך הקצרה ביותר ארוכה מהיקף חצי מהמעגל
            //נשלח שוב עם רדיוס גדול יותר
            //עדיף להוסיף לרשימת שכניות שכבר נבנתה
            //ולא לבנות מחדש
            List<Node> way = Dijkstra(adjacencyList,start.Id);
            return way;
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


    }
}
