using System;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.IO;

//https://www.c-sharpcorner.com/blogs/access-sql-server-database-in-net-core-console-application
namespace FailoverTesting
{
    class Program
    {
        private static IConfiguration _iconfiguration;
        static void Main(string[] args)
        {
            int index = 0;
            int count = 1000;
            GetAppSettingsFile();

            while (index < count)
            {
                insertRows(index);
                index += 1;
            }
            
        }

        static void GetAppSettingsFile()
        {
            var builder = new ConfigurationBuilder()
                                 .SetBasePath(Directory.GetCurrentDirectory())
                                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _iconfiguration = builder.Build();
        }

        static void insertRows(int index)
        {
            try
            {
                string _connectionString = _iconfiguration.GetConnectionString("Default");
                var builder = new SqlConnectionStringBuilder(_connectionString);
                //builder.DataSource = "<server>.database.windows.net";
                //builder.UserID = "<username>";
                //builder.Password = "<password>";
                //builder.InitialCatalog = "<database>";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nQuery data example:");
                    Console.WriteLine("=========================================\n");

                    StringBuilder sb = new StringBuilder();
                    sb.Append("INSERT INTO Test (Id) VALUES (" + index.ToString() + ")");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }

                    Console.WriteLine(String.Format("Inserted data for index: {0}", index));
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(String.Format("Error Occurred {0}", e.ToString()));
            }
            finally
            {
                Thread.Sleep(1000);
            }
        }
    }

}
