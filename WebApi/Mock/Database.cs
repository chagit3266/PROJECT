using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock
{
    public class Database:DbContext,IContext
    {
        //המחלקה תכיל
        //DbSet<---> ---{get;set;} פונקציות שמחזירות  
        //save פונקצית
        //SQLבנוסף יכיל את הקריאה ל 
        public DbSet<User> Users { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<Route> Route { get; set; }
        public void Save()
        {
            SaveChangesAsync();
        }
        //צריך לתקין
        //SQLלכתוב פונ' חיבור ל
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //של המחשב עליו אני עובדת SQL צריך לכתוב את ה
            optionsBuilder.UseSqlServer("server=dc2016\\erasql; database=myshopDb; trusted_connection=true");
        }
    }
}
