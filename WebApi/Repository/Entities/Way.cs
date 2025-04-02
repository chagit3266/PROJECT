using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class Way
    {
        [Key]
        public string Id { get; set; }
        public List<long> NodeIds { get; set; } = new List<long>();
        public string HighwayType { get; set; }//OSM שדה לסוג הדרך לפי

        public Way()
        {
            //GUID יצירת מזהה אוטומטי 
            Id = Guid.NewGuid().ToString();
        }
    }
}
