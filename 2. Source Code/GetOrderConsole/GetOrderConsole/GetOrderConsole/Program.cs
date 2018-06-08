using System;

namespace GetOrderConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Create Database and Create Orders table
            DbConnect dbConnect = new DbConnect();
            dbConnect.CreateTables2();
            
            
            //Get order data from Zalo and insert list order to database
            //GetFromZalo zalo = new GetFromZalo();
            //zalo.Init();
            //zalo.GetData();

            //Create WooCommerce object and get order
            //GetFromWooCommerce wooCommerce = new GetFromWooCommerce();
            //wooCommerce.Init();
            //wooCommerce.GetData();

            Console.ReadKey();
        }
    }
}