using System;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace SR_Test
{
    class SqlConnect
    {
        private static string sqlConn = "Server=localhost; Database=speech_db; Uid=root; Pwd=lpk~210;";
   
        MySqlConnection conn = new MySqlConnection(sqlConn);
        
        public string ReturnVoiceData(string input)
        {

            string sqlstr = "select * from words;";
            DataSet ds = new DataSet();
            MySqlDataAdapter adpt = new MySqlDataAdapter(sqlstr, conn);
            adpt.Fill(ds, "words");

            //DataTable dt = new DataTable();
            //dt = ds.Tables[0];

            Random rVal = new Random(); // value for random answer
            int spNum = rVal.Next(1, 3);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows) //foreach (DataRow r in ds.Tables[0].Rows)
                {
                    if(r["input"].ToString() == input)
                    {
                        if(r["type"].ToString() == "1")
                        {
                            switch (spNum)
                            {
                                case 1:
                                    return r["out_1"].ToString();

                                case 2:
                                    return r["out_2"].ToString();

                                case 3:
                                    return r["out_3"].ToString();
                            }


                        }

                        else if (r["type"].ToString() == "2")
                        {
                            return r["out_1"].ToString();
                        }
                    }
                    //Encoding.UTF8.GetString(Encoding.GetEncoding(r["input"].ToString()).getBytes(s))
                    

                }
            }
            
            return "error";
        }
    }
}
