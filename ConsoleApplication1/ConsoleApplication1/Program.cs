using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Data.Common;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        public static void Write(DataTable dt, string filePath)
        {
            int i = 0;
            StreamWriter sw = null;
            sw = new StreamWriter(filePath, false);
            for (i = 0; i < dt.Columns.Count - 1; i++)
            {
                sw.Write(dt.Columns[i].ColumnName + " ");
            }
            sw.Write(dt.Columns[i].ColumnName);
            sw.WriteLine();
            foreach (DataRow row in dt.Rows)
            {
                object[] array = row.ItemArray;
                for (i = 0; i < array.Length - 1; i++)
                {
                    sw.Write(array[i] + " ");
                }
                sw.Write(array[i].ToString());
                sw.WriteLine();
            }
            sw.Close();
        }

        static void Main(string[] args)
        {
            try
            {
                Connection con = new Connection();
                //con.SSQL = "Select count(*) from  Customers;";
                con.SSQL = "Select * from  Customers;";
                con.Conn = new SqlConnection(@"Data Source=" + ConfigurationManager.ConnectionStrings["DV"].ConnectionString + ";Integrated Security=True;AttachDbFilename=" 
                    + ConfigurationManager.ConnectionStrings["Database"].ConnectionString);

                con.Conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con.Conn;
                cmd.CommandText = con.SSQL;

                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                
                foreach (DataRow row in dt.Rows) // Loop over the rows.
                {
                    Console.WriteLine("--- Row ---"); // Print separator.
                    foreach (var item in row.ItemArray) // Loop over the items.
                    {
                        Console.Write("Item: "); // Print label.
                        Console.WriteLine(item); // Invokes ToString abstract method.
                    }
                }
                Console.ReadLine();

                string filePath = ConfigurationManager.AppSettings["filePath"] + ConfigurationManager.AppSettings["fileName"];

                if (File.Exists(filePath))
                {
                    string e = "The file " + filePath + " already exists.";
                    throw new Exception(e);
                }
                
                Write(dt, filePath );
                con.Conn.Dispose();

                //// execute the command
                //using (SqlDataReader rdr = cmd.ExecuteReader())
                //{
                //    // iterate through results, printing each to console
                //    while (rdr.Read())
                //    {
                //        Console.WriteLine("CustomerID: {0} CustomerName: {1}", rdr["CustomerID"], rdr["CustomerName"]);
                //        Console.WriteLine("Address: {0} ", rdr["CustomerAddress"]);
                //        Console.ReadLine();
                //    }
                //}
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }
    }
}
