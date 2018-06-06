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
            dbConnect.CreateTable();

            //Get order data from Zalo and insert list order to database
            GetFromZalo zalo = new GetFromZalo();
            zalo.Init();
            zalo.InsertOrderToDB();

            //GetFromWooCommerce wooCommerce = new GetFromWooCommerce();
            //wooCommerce.Getproduct();

            Console.ReadKey();
        }
    }
}