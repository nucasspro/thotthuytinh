using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace GetOrderConsole
{
    public class DbConnect
    {
        private readonly SQLiteConnection _con = new SQLiteConnection();
        private string _dbName = "OrderDatabase.db3";

        public void Init()
        {
            CreateTables();
        }

        public void CreateConection()
        {
            const string strConnect = "Data Source=OrderDatabase.db3;Version=3;";
            _con.ConnectionString = strConnect;
            _con.Open();
        }

        public void CloseConnection()
        {
            _con.Close();
        }

        private bool CheckDatabaseExists()
        {
            var path = Directory.GetCurrentDirectory() + "\\" + _dbName;
            if (!File.Exists(path))
                return false;
            Console.WriteLine("co file");
            return true;
        }

        private void CreateTables()
        {
            if (CheckDatabaseExists())
                return;
            Console.WriteLine("ko co file");
            SQLiteConnection.CreateFile(_dbName);
            List<string> query = new List<string>();

            const string accountsTable = "CREATE TABLE IF NOT EXISTS Accounts (" +
                                         "Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                                         "Username TEXT NOT NULL, " +
                                         "Password TEXT NOT NULL, " +
                                         "Type TEXT NOT NULL);";
            query.Add(accountsTable);

            const string customersTable = "CREATE TABLE IF NOT EXISTS Customers (" +
                                          "Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                                          "Name TEXT NOT NULL, " +
                                          "Phone TEXT, " +
                                          "Adress TEXT, " +
                                          "NumberOfPurchased INTEGER, " +
                                          "QuantityPurchased INTEGER, " +
                                          "Type TEXT NOT NULL);";
            query.Add(customersTable);

            const string ordersTable = "CREATE TABLE IF NOT EXISTS Orders (" +
                                       "Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                                       "OrderCode TEXT, " +
                                       "CreatedTime INTEGER NOT NULL, " +
                                       "UpdatedTime INTEGER NOT NULL, " +
                                       "ShipId INTEGER, " +
                                       "TotalPrice TEXT, " +
                                       "CustomerId INTEGER, " +
                                       "IsVerify TEXT, " +
                                       "VerifyBy INTEGER, " +
                                       "OrderFrom TEXT NOT NULL, " +
                                       "Type TEXT NOT NULL);";
            query.Add(ordersTable);

            const string productsTable = "CREATE TABLE IF NOT EXISTS Products (" +
                                         "Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                                         "Name TEXT NOT NULL, " +
                                         "Weight TEXT, " +
                                         "Width TEXT, " +
                                         "Height TEXT, " +
                                         "Length TEXT, " +
                                         "Price TEXT NOT NULL, " +
                                         "Image TEXT, " +
                                         "NumberOfStocks INTEGER, " +
                                         "CreatedBy INTEGER);";
            query.Add(productsTable);

            const string orderDetailTable = "CREATE TABLE IF NOT EXISTS OrderDetail (" +
                                            "Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                                            "OrderId INTEGER, " +
                                            "Quantity INTEGER, " +
                                            "DeliverCity TEXT, " +
                                            "DeliverDistrict TEXT, " +
                                            "DeliverAddress TEXT, " +
                                            "ProductId INTEGER);";
            query.Add(orderDetailTable);

            CreateTable(query);
            InitData();
        }

        public void CreateTable(List<string> query)
        {
            CreateConection();
            foreach (var item in query)
            {
                SQLiteCommand command = new SQLiteCommand(item, _con);
                command.ExecuteNonQuery();
            }
            CloseConnection();
        }

        private void InitData()
        {
            ExecuteQuery("insert into Accounts(Username, Password, Type) values ('admin', 'admin', 'admin');");
            ExecuteQuery("insert into Accounts(Username, Password, Type) values ('user', 'user', 'user');");
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