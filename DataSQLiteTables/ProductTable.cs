using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppMobilaDaniela.DataSQLiteTables
{
    [Table("Product")]
    public class ProductTable
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        [MaxLength(8)]
        public string Name { get; set; }
        public string Cantity { get; set; }
        public override string ToString()
        {
            return Name;
        }
        public int magazinId { get; set; }

        public int userID { get; set; }

        [Ignore]
        public bool Checked { get; set; } = false; //folosit pentru stergere multipla
        
    }
    public class Product : ProductTable
    {
        [Ignore]
        public int Position { get; set; }
    }
}