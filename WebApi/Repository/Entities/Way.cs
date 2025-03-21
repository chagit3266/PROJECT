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
        public long Id { get; set; }
        public List<long> NodeIds { get; set; } = new List<long>();
        public string HighwayType { get; set; }//OSM שדה לסוג הדרך לפי
        public virtual List<Node> Nodes { get; set; } = new List<Node>();
        
    }
}
