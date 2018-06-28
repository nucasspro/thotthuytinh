using System;
using System.Data;
using System.Data.SQLite;

namespace OMS.Model
{
    public class DBConnect
    {
        private readonly SQLiteConnection _con = new SQLiteConnection();
        private string _path = @"D:\HK8\ThietKeHeThongTMDT\Do An\thotthuytinh\2. Source Code\GetOrderConsole\GetOrderConsole\GetOrderConsole\bin\Debug\OrderDatabase.db3";

        public void Init(string path)
        {
            _path = path;
        }

        public void CreateConection()
        {
            string strConnect = "Data Source=" + _path + ";Version=3; Pooling=true";
            _con.ConnectionString = strConnect;
            _con.Open();
        }

        public void CloseConnection()
        {
            _con.Close();
        }

        public void ExecuteQuery(string query)
        {
            CreateConection();
            SQLiteCommand cmd = new SQLiteCommand(query, _con);
            cmd.ExecuteNonQuery();
            CloseConnection();
        }

        public int ExecuteQueryToGetIdAndCount(string query)
        {
            CreateConection();
            SQLiteCommand cmd = new SQLiteCommand(query, _con);
            var reader = cmd.ExecuteReader();
            string a;
            using (DataTable dt = new DataTable())
            {
                dt.Load(reader);
                try
                {
                    DataRow row = dt.Rows[0];
                    a = row[0].ToString();
                }
                catch (Exception)
                {
                    a = "0";
                }
            }
            CloseConnection();
            return int.Parse(a);
        }

        public DataTable SelectQuery(string query)
        {
            CreateConection();
            DataTable dataTable = new DataTable();
            SQLiteCommand command = new SQLiteCommand(query, _con)
            {
                CommandText = query
            };
            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);
            dataAdapter.Fill(dataTable);
            CloseConnection();
            return dataTable;
        }

        public void UpdateData()
        {
            //string strInsert = string.Format("UPDATE tbl_students set fullname='{0}', birthday='{1}', email='{2}', address='{3}', phone='{4}' where id='{5}'", fullname, birthday, email, address, phone, id);

            //CreateConection();
            //SQLiteCommand cmd = new SQLiteCommand(strInsert, _con);
            //cmd.ExecuteNonQuery();
            CloseConnection();
        }
    }
}