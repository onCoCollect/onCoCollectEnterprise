// class code created by Thiru on 25 April, 2015
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ePxCollectDataAccess;
using System.Data;
using System.Data.SqlClient;

namespace ePxCollectWeb
{
    public class OncoCollectLookup
    {
        private static SqlConnection GetCon()
        {
            SqlConnection con = new SqlConnection(GlobalValues.strConnString);
            return con;
        }

        public string sqlLookup(string table_name, string column_name)
        {
            SqlConnection con = GetCon();
            con.Open();
            string query = string.Format("select {0} from {1}", column_name, table_name);
            string val = "";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    val = Convert.ToString(dr[0]);
                }

            }
            con.Close();
            return val;
        }

        public string sqlLookup(string table_name, string column_name, string where_condition)
        {
            SqlConnection con = GetCon();
            con.Open();
            string query = string.Format("select {0} from {1} where {2}", column_name, table_name, where_condition);
            string val = "";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    val = Convert.ToString(dr[0]);
                }

            }
            con.Close();
            return val;
        }
    }
}