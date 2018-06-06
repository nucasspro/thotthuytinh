using System;
using System.Data.SQLite;

namespace ConsoleGetOrder
{
    public class DbConnect
    {
        private readonly SQLiteConnection _con = new SQLiteConnection();

        public void CreateConection()
        {
            string _strConnect = "Data Source=OrderDatabase.db3;Version=3;";
            _con.ConnectionString = _strConnect;
            _con.Open();
        }

        public void CloseConnection()
        {
            _con.Close();
        }

        public void CreateTable()
        {
            try
            {
                string _ordersTable = "CREATE TABLE IF NOT EXISTS Orders (" +
                                      "Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                                      "OrderCode TEXT, " +
                                      "Price TEXT, " +
                                      "CreatedTime TEXT, " +
                                      "UpdatedTime TEXT, " +
                                      "ProductName TEXT, " +
                                      "ProductImage TEXT, " +
                                      "NumberItem INTEGER, " +
                                      "DeliverCity TEXT, " +
                                      "DeliverDistrict TEXT, " +
                                      "ShippingInfo TEXT, " +
                                      "IsVerify INTEGER, " +
                                      "OrderFrom TEXT);";
                SQLiteConnection.CreateFile("OrderDatabase.db3");
                CreateConection();
                SQLiteCommand command = new SQLiteCommand(_ordersTable, _con);
                command.ExecuteNonQuery();
                CloseConnection();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void InsertData(string query)
        {
            CreateConection();
            SQLiteCommand cmd = new SQLiteCommand(query, _con);
            cmd.ExecuteNonQuery();
            CloseConnection();
        }

        private void UpdateData()
        {
            //string strInsert = string.Format("UPDATE tbl_students set fullname='{0}', birthday='{1}', email='{2}', address='{3}', phone='{4}' where id='{5}'", fullname, birthday, email, address, phone, id);

            //CreateConection();
            //SQLiteCommand cmd = new SQLiteCommand(strInsert, _con);
            //cmd.ExecuteNonQuery();
            CloseConnection();
        }

        
    }
}