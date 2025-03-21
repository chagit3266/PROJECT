using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class Node
    {
        [Key]
        public long Id { get; set; }
        public double Lat {  get; set; }
        public double Lon { get; set; }

    }
}
