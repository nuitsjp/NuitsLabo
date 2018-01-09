using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperStudy.SimpleCRUD
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();
            }
        }

        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["AdventureWorks2017"].ConnectionString;
        }
    }
}
