using System;
using System.Data.SQLite;

namespace KITAB.Products.Infra
{
    public class Repository
    {
        public static string DbFile
        {
            get
            {
                var curDir = Environment.CurrentDirectory;

                if (curDir.IndexOf("KITAB.Products.Web", 0) > 0)
                {
                    curDir = curDir.Substring(0, (curDir.Length - (curDir.Length - curDir.IndexOf("KITAB.Products.Web", 0))));
                }
                else
                {
                    curDir += "\\";
                }

                return (curDir + "Database\\stock.db");
            }
        }

        public static SQLiteConnection SimpleDbConnection()
        {
            return new SQLiteConnection("Data Source=" + DbFile);
        }
    }
}
