using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public  class Route
    {
        [Key]
        public int RouteId { get; set; }

        // מזהים של הנקודות
        public int FromPointId { get; set; }
        public int ToPointId { get; set; }

        // קשרים לנקודות
        [ForeignKey("FromPointId")]
        public virtual Point FromPoint { get; set; }

        [ForeignKey("ToPointId")]
        public virtual Point ToPoint { get; set; }
    }
}
