using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace GetOrderConsole
{
    public class DbConnect
    {
        private readonly SQLiteConnection _con = new SQLiteConnection();

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

        public void CreateTables()
        {
            SQLiteConnection.CreateFile("OrderDatabase.db3");
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
                                          "Type INTEGER NOT NULL);";
            query.Add(customersTable);

            const string ordersTable = "CREATE TABLE IF NOT EXISTS Orders (" +
                                       "Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                                       "OrderCode TEXT, " +
                                       "CreatedTime INTEGER NOT NULL, " +
                                       "UpdatedTime INTEGER NOT NULL, " +
                                       "ShipId INTEGER, " +
                                       "TotalPrice TEXT, " +
                                       "IsVerify TEXT, " +
                                       "VerifyBy INTEGER, " +
                                       "OrderFrom TEXT NOT NULL, " +
                                       "Type TEXT NOT NULL, " +
                                       "FOREIGN KEY(VerifyBy) REFERENCES Accounts(Id));";
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
                                         "CreatedBy INTEGER, " +
                                         "FOREIGN KEY(CreatedBy) REFERENCES Accounts(Id));";
            query.Add(productsTable);

            const string orderDetailTable = "CREATE TABLE IF NOT EXISTS OrderDetail (" +
                                            "Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                                            "OrderId INTEGER, " +
                                            "Quantity INTEGER, " +
                                            "DeliverCity TEXT, " +
                                            "DeliverDistrict TEXT, " +
                                            "DeliverAddress TEXT, " +
                                            "ProductId INTEGER, " +
                                            "CustomerId INTEGER, " +
                                            "FOREIGN KEY(OrderId) REFERENCES Orders(Id), " +
                                            "FOREIGN KEY(ProductId) REFERENCES Products(Id), " +
                                            "FOREIGN KEY(CustomerId) REFERENCES Customers(Id));";
            query.Add(orderDetailTable);

            CreateTable(query);
        }

        public void CreateTables2()
        {
            SQLiteConnection.CreateFile("OrderDatabase.db3");
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
                                       "CustomerId INTEGER, "+
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

        public void InitData()
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

        public int ExecuteQueryToGetId(string query)
        {
            CreateConection();
            SQLiteCommand cmd = new SQLiteCommand(query, _con);
            var reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            string a;
            if (dt.Rows.Count == 0)
            {
                a = "0";
            }
            else
            {
                DataRow row = dt.Rows[0];
                a = Convert.ToString(row[0]);
            }

            //while (reader.Read())
            //{
            //    a = reader[0].ToString();
            //}

            CloseConnection();

            return Int32.Parse(a); ;
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