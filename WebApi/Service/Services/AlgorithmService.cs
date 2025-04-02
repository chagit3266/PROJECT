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
using System.Net.Http.Headers;


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
                        Id = -(long)element["id"],
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
                        Id = (string)element["id"],
                        NodeIds = element["nodes"] != null ? element["nodes"].ToObject<List<long>>().Select(x => -x).ToList() : new List<long>(),
                        HighwayType = element["tags"] is JObject tags && tags.TryGetValue("highway", out JToken highwayValue)
                                       ? highwayValue.ToString() : "unknown",
                    });
                }
            }
            return ways;
        }
        //אלגוריתם למציאת הנקודה הקרובה ביותר הרישמית
        //וכן צריך לחלק את כל הדרכים שהנקודה היא חלק מהן ל2 דרכים
        public Node FindClosestNode(Node node, List<Node> nodes)
        {
            double minDistance = double.MaxValue;
            Node closestNode = null;
            foreach (var n in nodes)
            {
                double distance = CalculateDistance(node, n);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestNode = n;
                }
            }
            return closestNode;
        }
        //פונקציה לפירוק קשת ל2
        public List<Way> SplitWayAtNode(Node node, List<Way> ways)
        {
            List<Way> newWays = new List<Way>();
            foreach (var way in ways)
            {
                int splitIndex = -1;
                if (way.NodeIds.Count!=0)
                    splitIndex = way.NodeIds.IndexOf(node.Id);
                if (splitIndex != -1)
                {
                    // הדרך הראשונה - עד לנקודה
                    var way1 = new Way
                    {
                        Id = way.Id,
                        NodeIds = way.NodeIds.Take(splitIndex + 1).ToList(), // עד לנקודה כולל
                        HighwayType = way.HighwayType
                    };
                    newWays.Add(way1);

                    // הדרך השנייה - אחרי הנקודה
                    var way2 = new Way
                    {
                        Id = way.Id + "-P", // מייחד ID חדש (אפשר לשמור את ה-ID המקורי אם זה הכרחי)
                        NodeIds = way.NodeIds.Skip(splitIndex).ToList(), // מתחיל מהנקודה
                        HighwayType = way.HighwayType
                    };
                    newWays.Add(way2);
                }
                else
                {
                    // אם הצומת לא נמצא בדרך, הוסף את הדרך כפי שהיא
                    newWays.Add(way);
                }
            }
            return newWays;
        }

        public List<Way> SplitWaysAtIntersections(List<Way> ways)
        {
            List<Way> newWays = new List<Way>();

            // יצירת מבנה נתונים לאיתור צמתים שמופיעים כנקודת התחלה של דרך אחרת
            HashSet<long> keyNodes = new HashSet<long>(
             ways.Select(w => w.NodeIds.First()).Concat(ways.Select(w => w.NodeIds.Last()))
);
            foreach (var way in ways)
            {
                int prev = 0;
                for (int i = 1; i < way.NodeIds.Count - 1; i++) // מתחילים מ-1 כדי להימנע מראש הרשימה
                {
                    if (keyNodes.Contains(way.NodeIds[i]))
                    {
                        // מחלקים את הדרך
                        var way1 = new Way
                        {
                            Id = way.Id + "-" + i,
                            NodeIds = way.NodeIds.Skip(prev).Take(i + 1 - prev).ToList(), // עד הנקודה כולל
                            HighwayType = way.HighwayType
                        };
                        newWays.Add(way1);
                        prev = i;

                    }
                }
                if (prev < way.NodeIds.Count - 1) { 
                    var way2 = new Way
                {
                    Id = way.Id , // מזהה ייחודי לחלק החדש
                    NodeIds = way.NodeIds.Skip(prev).ToList(), // מהנקודה והלאה
                    HighwayType = way.HighwayType
                };
                newWays.Add(way2);
                }
            }

            return newWays;
        }


        // אלגוריתם למציאת מרחק בין 2 נקודות
        // 'ע"מ לבדוק אם צריך לשמור נק
        // Way וכן כדי לחשב אורך 
        public double CalculateDistance(Node start, Node end)
        {
            #region MyRegion

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

            #endregion
            #region MyRegion

            //const double a = 6378137; // רדיוס האליפסואיד
            //const double f = 1 / 298.257223563; // Flattening של כדור הארץ
            //const double b = (1 - f) * a; // רדיוס האליפסואיד בלאה התואם

            //double φ1 = start.Lat * Math.PI / 180; // המרה לרדיאנים
            //double φ2 = end.Lat * Math.PI / 180;
            //double Δλ = (end.Lon - start.Lon) * Math.PI / 180; // הפרש בקווי אורך

            //double U1 = Math.Atan((1 - f) * Math.Tan(φ1));
            //double U2 = Math.Atan((1 - f) * Math.Tan(φ2));
            //double sinU1 = Math.Sin(U1), cosU1 = Math.Cos(U1);
            //double sinU2 = Math.Sin(U2), cosU2 = Math.Cos(U2);

            //double λ = Δλ, λʹ = 0;
            //double sinσ = 0, cosσ = 0, σ = 0, sinα = 0, cos2α = 0, cos2σm = 0, C = 0;

            //int iterationLimit = 100;
            //while (Math.Abs(λ - λʹ) > 1e-12 && iterationLimit-- > 0)
            //{
            //    sinσ = Math.Sqrt(Math.Pow(cosU2 * Math.Sin(λ), 2) +
            //                     Math.Pow(cosU1 * sinU2 - sinU1 * cosU2 * Math.Cos(λ), 2));
            //    cosσ = sinU1 * sinU2 + cosU1 * cosU2 * Math.Cos(λ);
            //    σ = Math.Atan2(sinσ, cosσ);
            //    sinα = cosU1 * cosU2 * Math.Sin(λ) / sinσ;
            //    cos2α = 1 - sinα * sinα;
            //    cos2σm = cosσ - 2 * sinU1 * sinU2 / cos2α;
            //    C = f / 16 * cos2α * (4 + f * (4 - 3 * cos2α));
            //    λʹ = λ;
            //    λ = Δλ + (1 - C) * f * sinα *
            //        (σ + C * sinσ * (cos2σm + C * cosσ * (-1 + 2 * cos2σm * cos2σm)));
            //}

            //double u2 = cos2α * (a * a - b * b) / (b * b);
            //double A = 1 + u2 / 16384 * (4096 + u2 * (-768 + u2 * (320 - 175 * u2)));
            //double B = u2 / 1024 * (256 + u2 * (-128 + u2 * (74 - 47 * u2)));
            //double δσ = B * sinσ * (cos2σm + B / 4 * (cosσ * (-1 + 2 * cos2σm * cos2σm) - B / 6 * cos2σm * (-3 + 4 * sinσ * sinσ) * (-3 + 4 * cos2σm * cos2σm)));

            //double distance = b * A * (σ - δσ); // מרחק
            //return distance; // במטרים


            #endregion
        }
        //ישלח לפונקציה המועמסת כל 2 נקודות ויבצע חיבור בין כל המרחקים
        public double CalculateDistance(Way way, Dictionary<long, Node> nodes)
        {
            double d = 0;
            for (int i = 0; i < way.NodeIds.Count - 1; i++)
            {
                if(nodes.ContainsKey(way.NodeIds[i])&&nodes.ContainsKey(way.NodeIds[i + 1]))
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
        public Node MidNode(Node start, Node end)
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
            return new Node
            {
                Lat = φm * 180 / Math.PI,
                Lon = λm * 180 / Math.PI
            };

        }
        //ID יצירת טבלת גיבוב לצמתים לפי
        public Dictionary<long, Node> CreateDictionaryByIdNode(List<Node> node)
        {
            Dictionary<long, Node> byIdNode = new Dictionary<long, Node>();
            foreach (var item in node)
            {
                byIdNode[item.Id] = item;
            }
            return byIdNode;
        }
        //Dictionary<long, long> prev מה nodes יצירת רשימת 
        //צריך לשים לב באיזה כיוון השתמשתי לדרך כדי לדעת לאיזה כיוון לשחזר
        public List<Node> CreateListNodes(Dictionary<long, List<Way>> adjacencyList, Dictionary<long, Node> nodes, Dictionary<long, long> prev, long end, long start)
        {
            List<Node> way = new List<Node>();
            long current = end;
            while (prev.ContainsKey(current) && prev[current] != start)
            {
                //או להפך prev[current]ונגמרת ב currentאני צריכה לגשת לקשת שמתחילה ב
                //ways איך שולחים את  
                //prev נשלח רשימת סמיכויות וכך נוכל לחזור ל
                if (true)//אם השתמשתי הפוך אז להפוך
                    way.Reverse();
                Way temp = adjacencyList[prev[current]].
                    FirstOrDefault(item =>
                    (item.NodeIds[0] == prev[current] &&
                    item.NodeIds[item.NodeIds.Count - 1] == current) ||
                    (item.NodeIds[0] == current &&
                    item.NodeIds[item.NodeIds.Count - 1] == prev[current]));
                if (temp.NodeIds[0] == current && temp.NodeIds[temp.NodeIds.Count - 1] == prev[current])
                {    
                    foreach (var nodeId in temp.NodeIds)
                    {
                    way.Add(nodes[nodeId]);
                     }
                }
                else
                {
                    for (int i = temp.NodeIds.Count - 1; i >= 0; i--)
                    {
                        way.Add(nodes[temp.NodeIds[i]]);
                    }
                }
                current = prev[current];
            }
            if (prev.ContainsKey(current))
            {
                Way temp2 = adjacencyList[prev[current]].
                        FirstOrDefault(item =>
                        (item.NodeIds[0] == prev[current] &&
                        item.NodeIds[item.NodeIds.Count - 1] == current) ||
                        (item.NodeIds[0] == current &&
                        item.NodeIds[item.NodeIds.Count - 1] == prev[current]));
                //הוספת נקודת ההתחלה
                if (temp2.NodeIds[0] == current && temp2.NodeIds[temp2.NodeIds.Count - 1] == prev[current])
                {
                    foreach (var nodeId in temp2.NodeIds)
                    {
                        way.Add(nodes[nodeId]);
                    }
                }
                else
                {
                    for (int i = temp2.NodeIds.Count - 1; i >= 0; i--)
                    {
                        way.Add(nodes[temp2.NodeIds[i]]);
                    }
                }
            }
            return way;
        }
        //יצירת רשימת סמיכויות
        public Dictionary<long, List<Way>> CreateAdjacencyList(List<Node> nodes, List<Way> ways)
        {
            Dictionary<long, List<Way>> adjacency = new Dictionary<long, List<Way>>();

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
        public Dictionary<long, long> Dijkstra(Dictionary<long, List<Way>> adjacencyList, long source, Dictionary<long, Node> byIdNode)
        {
            Dictionary<long, double> dist = new Dictionary<long, double>();
            Dictionary<long, long> prev = new Dictionary<long, long>();


            foreach (var node in byIdNode)
            {
                dist[node.Key] = double.MaxValue;  // אתחול כל הצמתים לאינסוף
            }
            // הגדרת צומת ההתחלה
            dist[source] = 0;

            // יצירת תור עדיפויות
            // ממיין לפי מרחקים
            //long nodeId, double currentDist;
            PriorityQueue<long, double> h = new PriorityQueue<long, double>();
            //הכנסת כל הצמתים לתור עדיפויות
            //foreach (var node in byIdNode)
            //{
            //    h.Enqueue(node.Key, dist[node.Key]);
            //}
            h.Enqueue(source, 0);
            while (h.Count > 0)
            {
                // מוציאים את הצומת הקרוב ביותר
                long u = h.Dequeue();
                //זה יפתור מצב שהוא מתחיל להחזיר צמתים שלא יכולים להיות במסלול
                if (dist[u] == double.MaxValue)
                    break;
                //u עוברים על כל השכנים של הצומת 
                if (adjacencyList.ContainsKey(u))
                {
                    foreach (var way in adjacencyList[u])
                    {
                        Console.WriteLine(way.NodeIds);
                        long v;// הצומת השכן
                        //משום ששמרנו את הקשתות לשני הכיוונים צריך לבדוק לשני הכיוונים
                        if (way.NodeIds[0] == u)
                            v = way.NodeIds[way.NodeIds.Count - 1];
                        else v = way.NodeIds[0];
                        double weight = CalculateDistance(way, byIdNode);  // משקל הדרך בין הצמתים
                        if (!dist.ContainsKey(v))
                            dist[v] = double.MaxValue;
                        // אם נמצא מסלול קצר יותר לצומת v, מעדכנים את המרחק
                        Console.WriteLine($"{dist[u]} + {weight} < {dist[v]}");
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

            var mid =MidNode(start, end);//end ל start מציאת נקודת אמצע בין 
            double dist =CalculateDistance(start, end);//מציאת המרחק בין שתי הנקודות כדי לדעת איזה רדיוס לשלוח לפונקציה
            string data = await GetOsmData(mid.Lat, mid.Lon, dist * 0.6);//באזור המבוקש waysו nodes עם json מקבל
            List<Node> nodes = ExtractNodes(data);
            List<Way> ways = ExtractWays(data);
            start = FindClosestNode(start, nodes);//נרצה להמיר את נקודת ההתחלה לצומת רשמית
            end = FindClosestNode(end, nodes);//נרצה להמיר את נקודת הסיום לצומת רשמית
            if (start == null || end == null)
                return new List<Node>();
            ways = SplitWayAtNode(start, ways);//אם הצומת התחלה היא חלק ממסלול ולא צומת חשובה נרצה לפצל את הדרכים שעוברות בה ל2 דרכים
            ways = SplitWayAtNode(end, ways);//אם הצומת סיום היא חלק ממסלול ולא צומת חשובה נרצה לפצל את הדרכים שעוברות בה ל2 דרכים
            ways =  SplitWaysAtIntersections(ways);
            Dictionary<long, List<Way>> adjacencyList =CreateAdjacencyList(nodes, ways);
            //אם הדרך הקצרה ביותר ארוכה מהיקף חצי מהמעגל
            //נשלח שוב עם רדיוס גדול יותר
            //עדיף להוסיף לרשימת שכניות שכבר נבנתה
            //ולא לבנות מחדש
            Dictionary<long, Node> byIdNode = CreateDictionaryByIdNode(nodes);
            Dictionary<long, long> prev = Dijkstra(adjacencyList, start.Id, byIdNode);
            List<Node> way = CreateListNodes(adjacencyList, byIdNode, prev, end.Id, start.Id);//צריך לבדוק כל קשת מה מקשר בינה לבין הצומת כי השתמשנו לשתי הכיוונים
            way.Reverse();//לכן נהפוך prevמשחזר ע"פ ה
            return way;
        }

    }
}
