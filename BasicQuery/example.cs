// Why ABL Example
// Authors: Bill Wood, Alan Estrada
// File Name: BasicQuery/example.cs
// Version 1.04
// 
// FOR EACH Customer WHERE SalesRep = repname AND Balance > CreditLimit:
//    Balance = Balance * creditFactor.
// END.

using System;

using MySql.Data;
using MySql.Data.MySqlClient;

// Note that if you DO not give this a data source, MySql namespaces will NOT 
// be resolved properly.
namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string connStr = "server=localhost;user=root;database=classicmodels;port=3306;password='';";
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlTransaction tr = null;

            string repName = "GPE";
            double creditFactor = 1.05;

            try
            {
                // Open the connection to the database
                conn.Open();

                String update = "UPDATE customers " +
                    "SET BALANCE = BALANCE * @creditFactor " +
                    "WHERE SALESREPEMPLOYEENUMBER = @repName AND BALANCE > CREDITLIMIT";

                // Begin a transaction
                tr = conn.BeginTransaction();

                // Build the query
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.Transaction = tr;

                cmd.CommandText = update; 
                cmd.Parameters.AddWithValue("@creditFactor", creditFactor);
                cmd.Parameters.AddWithValue("@repName", repName);

                // Execute the query and commit it if we don't throw anything
                cmd.ExecuteNonQuery();
                tr.Commit();
            }
            catch (MySqlException ex)
            {
                try
                {
                    tr.Rollback();
                }
                catch (MySqlException ex1)
                {
                    Console.WriteLine(ex1.ToString());
                }

                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
    }
}
