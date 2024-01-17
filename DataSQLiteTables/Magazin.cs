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
    [Table("Magazin")]
    public class Magazin
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        [MaxLength(8), Unique]
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
        public string Locatie { get; set; }
    }
}