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
                                          "Address TEXT, " +
                                          "Type TEXT NOT NULL);";
            query.Add(customersTable);

            const string ordersTable = "CREATE TABLE IF NOT EXISTS Orders (" +
                                       "Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                                       "OrderCode TEXT, " +
                                       "CreatedTime TEXT, " +
                                       "UpdatedTime TEXT, " +
                                       "SubTotal TEXT, " +
                                       "GrandPrice TEXT, " +
                                       "CustomerId INTEGER, " +
                                       "Status TEXT, " +
                                       "VerifyBy INTEGER, " +
                                       "OrderFrom TEXT NOT NULL, " +
                                       "Type TEXT NOT NULL, " +
                                       "ShippingAddress TEXT, " +
                                       "BillingAddress TEXT, " +
                                       "CallShip TEXT, " +
                                       "ShipPrice TEXT, " +
                                       "PackageWidth TEXT, " +
                                       "PackageHeight TEXT, " +
                                       "PackageLenght TEXT);";
            query.Add(ordersTable);

            const string productsTable = "CREATE TABLE IF NOT EXISTS Products (" +
                                         "Id TEXT NOT NULL PRIMARY KEY, " +
                                         "Name TEXT NOT NULL, " +
                                         "Description TEXT, " +
                                         "Weight TEXT, " +
                                         "Width TEXT, " +
                                         "Height TEXT, " +
                                         "Length TEXT, " +
                                         "Price TEXT NOT NULL, " +
                                         "Image1 TEXT, " +
                                         "Image2 TEXT, " +
                                         "Image3 TEXT, " +
                                         "Quantity INTEGER, " +
                                         "CreatedBy INTEGER, " +
                                         "Status TEXT);";
            query.Add(productsTable);

            const string orderDetailTable = "CREATE TABLE IF NOT EXISTS OrderDetail (" +
                                            "Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                                            "OrderId INTEGER, " +
                                            "ProductId TEXT, " +
                                            "Quantity INTEGER);";
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
            try
            {
                ExecuteQuery(@"insert into Accounts(Username, Password, Type) values ('admin', 'admin', 'admin');");
                ExecuteQuery(@"insert into Accounts(Username, Password, Type) values ('user', 'user', 'user');");
                ExecuteQuery(@"insert into Products(Id, Name, Description, Weight, Width, Height, Length, Price, Image1, Image2, Image3, Quantity, CreatedBy, Status) values ('TKCL001', 'Thớt thủy tinh kính cường lực  3D tròn (TKCL001)', 'Thớt thủy tinh kính cường lực  3D tròn (TKCL001)', '3.2', '35', '1.2', '35', '350000', 'http', 'http', 'http', '10', '1', 'Chưa xóa');");
                ExecuteQuery(@"insert into Products(Id, Name, Description, Weight, Width, Height, Length, Price, Image1, Image2, Image3, Quantity, CreatedBy, Status) values ('TKCL002', 'Thớt thủy tinh kính cường lực  3D tròn (TKCL002)', 'Thớt thủy tinh kính cường lực  3D tròn (TKCL002)', '3.2', '35', '1.2', '35', '350000', 'http', 'http', 'http', '10', '1', 'Chưa xóa');");
                ExecuteQuery(@"insert into Products(Id, Name, Description, Weight, Width, Height, Length, Price, Image1, Image2, Image3, Quantity, CreatedBy, Status) values ('TKCL003', 'Thớt thủy tinh kính cường lực  3D tròn (TKCL003)', 'Thớt thủy tinh kính cường lực  3D tròn (TKCL003)', '3.2', '35', '1.2', '35', '350000', 'http', 'http', 'http', '10', '1', 'Chưa xóa');");
                ExecuteQuery(@"insert into Products(Id, Name, Description, Weight, Width, Height, Length, Price, Image1, Image2, Image3, Quantity, CreatedBy, Status) values ('TKCL004', 'Thớt thủy tinh kính cường lực  3D tròn (TKCL004)', 'Thớt thủy tinh kính cường lực  3D tròn (TKCL004)', '3.2', '35', '1.2', '35', '350000', 'http', 'http', 'http', '10', '1', 'Chưa xóa');");
            }
            catch (Exception e)
            {
                Console.WriteLine("tao data mau that bai" + e);
                throw;
            }
        }

        public void ExecuteQuery(string query)
        {
            CreateConection();
            SQLiteCommand cmd = new SQLiteCommand(query, _con);
            cmd.ExecuteNonQuery();
            CloseConnection();
        }

        public int GetIdAndCountId(string query)
        {
            CreateConection();
            SQLiteCommand command = new SQLiteCommand(query, _con);
            var reader = command.ExecuteReader();
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

        public string GetData(string query)
        {
            CreateConection();
            SQLiteCommand command = new SQLiteCommand(query, _con);
            var reader = command.ExecuteReader();
            string a;
            using (DataTable dt = new DataTable())
            {
                dt.Load(reader);
                DataRow row = dt.Rows[0];
                a = row[0].ToString();
            }
            CloseConnection();
            return a;
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