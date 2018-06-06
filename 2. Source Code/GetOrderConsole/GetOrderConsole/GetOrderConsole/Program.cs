using ConsoleGetOrder;
using System;

namespace GetOrderConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Create Database and Create Orders table
            DbConnect dbConnect = new DbConnect();
            dbConnect.CreateTables();

            //Get order data from Zalo and insert list order to database
            //GetFromZalo zalo = new GetFromZalo();
            //zalo.Init();
            //Console.WriteLine(zalo.a();
            //zalo.InsertOrderToDb();

            //GetFromWooCommerce wooCommerce = new GetFromWooCommerce();
            //wooCommerce.Getproduct();

            Console.ReadKey();
        }
    }
}