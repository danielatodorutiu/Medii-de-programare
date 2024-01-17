using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using SQLite;



namespace AppMobilaDaniela.DataSQLiteTables
{
    [Table("Users")]
    public class PUser
    {
        [PrimaryKey, AutoIncrement, Column("_pid")]
        public int PId { get; set; }
        [MaxLength(10)]
        public string FullName { get; set; }
        [MaxLength(8)]
        public string Password { get; set; }
        [MaxLength(10)]
        public string Phone { get; set; }
        [MaxLength(10)]
        public string Email { get; set; }
    }
}