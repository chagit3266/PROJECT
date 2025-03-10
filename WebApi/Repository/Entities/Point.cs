using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class Point
    {
        [Key]
        public int PointsId { get; set; }
        public double X {  get; set; }
        public double Y { get; set; }
    }
}
