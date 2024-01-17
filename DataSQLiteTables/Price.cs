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
    [Table("Price")]
    public class Price
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        [MaxLength(8)]
        public double priceH { get; set; }

        public int productID { get; set; }
    }
}