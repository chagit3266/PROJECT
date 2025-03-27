using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Service.Interfaces
{
    public interface IAlgorithem
    {
        //openstreetmap הבאת נקודות מ 
        Task<string> GetOsmData(double lat, double lon, double radius = 1000);

        //עיבוד נתונים
        
        //צמתים - נקודות
        public List<Node> ExtractNodes(string json);
        
        //קשתות - מסלולים
        public List<Way> ExtractWays(string json);

        //שנמצאים באזור בין 2 הנקודות SQL-החזרת הקשתות והצמתים מה
        //לכאורה רק קשתות


        //SQLעיבוד מידע שמור ב
        //SQLאם נחתכת עם קשת מ openstreetmap ופיצול קשתות מ


        

        // אלגוריתם למציאת מרחק בין 2 נקודות
        // 'ע"מ לבדוק אם צריך לשמור נק
        // Way וכן כדי לחשב אורך 
        public double CalculateDistance(Node start, Node end);
        
        //ישלח לפונקציה המועמסת כל 2 נקודות ויבצע חיבור בין כל המרחקים
        public double CalculateDistance(Way way,Dictionary<long, Node> nodes);
        
        //פונקציה לחישוב אמצע קטע
        public Node MidNode(Node start, Node end);

        //בדיקה אם נתיב קיים
        //נבדוק גם בשמורים שאושרו וגם באלו שעדיין לא אושרו ונבדוק אם צריך לשנות לו סטטוס לפעיל

        // שמירת נתיב אם לא קיים
        
        //יצירת רשימת סמיכויות
        public Dictionary<long, List<Way>> CreateAdjacencyList(List<Node> nodes, List<Way> ways);

        // אלגוריתם דייקסטרה 
        public Dictionary<long, long?> Dijkstra(Dictionary<long, List<Way>> adjacencyList, long source);
        //פונקציה לחישוב מסלול
        public Task<List<Node>> CalculateRoute(Node start,Node end);
    }
}
