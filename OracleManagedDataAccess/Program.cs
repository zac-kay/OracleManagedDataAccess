using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleManagedDataAccess
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World");
            Console.Read();
        }

        public DataTable GetDataTable(string id)
        {
            DataTable dt = new DataTable();
            using (OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString))
            {
                string sql = string.Format(@"SELECT * 
                                             FROM table 
                                             WHERE id = :Id");

                OracleDataAdapter da = new OracleDataAdapter(sql, conn);
                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.BindByName = true;
                da.SelectCommand.Parameters.Add(":Id", id);
                da.Fill(dt);
            }
            return dt;
        }

        public string ExecuteScalar(string id)
        {
            string result = "";
            using (OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString))
            {
                conn.Open();

                try
                {
                    string sql = string.Format(@"SELECT * 
                                                 FROM table 
                                                 WHERE id = :Id");

                    using (OracleCommand cmd = new OracleCommand(sql, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.BindByName = true;
                        cmd.Parameters.Add(":Id", id);

                        object obj = cmd.ExecuteScalar();

                        if (obj != null && obj != DBNull.Value)
                        {
                            result = Convert.ToString(obj);
                        }
                    }
                }
                catch
                {
                    throw;
                }
            }
            return result;
        }

        public int ExecuteNonQuery(string id)
        {
            int result = 0;
            using (OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString))
            {
                conn.Open();
                OracleTransaction txn = conn.BeginTransaction();

                try
                {
                    string sql = string.Format(@"SELECT * 
                                                 FROM table 
                                                 WHERE id = :Id");

                    using (OracleCommand cmd = new OracleCommand(sql, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.BindByName = true;
                        cmd.Parameters.Add(":Id", id);

                        result = cmd.ExecuteNonQuery();
                    }
                    txn.Commit();
                }
                catch
                {
                    txn.Rollback();
                    throw;
                }
            }
            return result;
        }

        public class Person
        {
            public string Name { get; set; }
            public string Age { get; set; }
            public string City { get; set; }
        }

        public Person ExecuteReader(string id)
        {
            Person result = new Person();
            using (OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString))
            {
                conn.Open();

                try
                {
                    string sql = string.Format(@"SELECT * 
                                                 FROM table 
                                                 WHERE id = :Id");

                    using (OracleCommand cmd = new OracleCommand(sql, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.BindByName = true;
                        cmd.Parameters.Add(":Id", id);

                        OracleDataReader rdr = cmd.ExecuteReader();

                        while (rdr.Read())
                        {
                            result.Name = rdr["Name"].ToString();
                            result.Age = rdr["Age"].ToString();
                            result.City = rdr["City"].ToString();
                        }
                    }
                }
                catch
                {
                    throw;
                }
            }
            return result;
        }

    }
}
