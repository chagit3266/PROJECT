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
        Task<string> GetOsmData(double lat, double lon, int radius = 1000);

        //עיבוד נתונים
        
        //צמתים - נקודות
        public List<Node> ExtractPoint(string jsonResponse);
        
        //קשתות - מסלולים
        public List<Way> ExtractRoutes(string jsonResponse);

        // אלגוריתם דייקסטרה 


        // אלגוריתם למציאת מרחק בין 2 נקודות  
        // 'ע"מ לבדוק אם צריך לשמור נק


        // בדיקה אם נתיב קיים


        // שמירת נתיב אם לא קיים


        // 
    }
}
