using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AppMobilaDaniela.DataSQLiteTables;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AppMobilaDaniela.DataAccessDataBase
{
    public class DataBaseClass
    {
        SQLiteConnection db;

        public DataBaseClass()
        {
             string dbPath = Path.Combine(PathClass.RootDirectory, "HoneyManagementDataBase.db");
            db = new SQLiteConnection(dbPath);

            db.CreateTable<PUser>();
            db.CreateTable<Product>();
            db.CreateTable<Magazin>();
            db.CreateTable<Price>();
        }


        //pUser
        public PUser GetPUser(string email, string password)
        {
            Console.WriteLine("Reading data From Table");
            var table = db.Table<PUser>();
            try
            {
                foreach (var s in table)
                {
                    if (string.Equals(s.Email, email) && string.Equals(s.Password, password))
                        return s;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public PUser GetPUser(string email)
        {
            Console.WriteLine("Reading data From Table");
            var table = db.Table<PUser>();
            try
            {
                foreach (var s in table)
                {
                    if (string.Equals(s.Email, email))
                        return s;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        public void RegisterPUser(PUser newPUser)
        {
            db.Insert(newPUser);
        }

        public void UpdatePUser(PUser user)
        {
            db.Update(user);
        }

        public PUser GetPUser(int id)
        {
            Console.WriteLine("Reading data From Table");
            var table = db.Table<PUser>();
            try
            {
                foreach (var s in table)
                {
                    if (string.Equals(s.PId, id))
                        return s;
                }
                return null;
            }
            catch
            {
                return null;
            }

        }

        /*--------------------------------------------------------    */
        public void addProduct(Product product)
        {
            db.Insert(product);
        }
        public void addMagazin(Magazin magazin)
        {
            db.Insert(magazin);
        }

        public List<Product> getAllProducts()
        {
            return db.Query<Product>("select * from Product");
        }
        public List<Product> getAllProductsByUserID(int id)
        {
            return db.Query<Product>("select * from Product where userID='"+id+ "' ");
        }

        public List<Magazin> getAllMagazin()
        {
            return db.Query<Magazin>("select * from Magazin");
        }

        public void deleteProduct(Product product)
        {
            db.Delete(product);
        }

        public void insertProduct(Product product)
        {
            db.Insert(product);
        }



        //public string insertProducts(string _id, string Name, string Cantity)
        //{
        //    db.Query<Product>("Insert into Product([Id],[Name],[Cantity]) values ('"+_id+ "','" + Name + "','" + Cantity + "')");

        //    return
        //}

        public void updateProduct(Product product)
        {
            db.Update(product);

        }

        public List<Magazin> GetMagazins()
        {
            return db.Query<Magazin>("select * from Magazin");
        }

        public Magazin GetMagazinById(int id)
        {
            return db.Get<Magazin>(id);
        }
        /*   //RecyclerView
           public List<OUser> GetAllAvailableOUsersPark()
           {
               return db.Query<OUser>("select * from OUsers where SessionStatus=1");
           } */
    }
}