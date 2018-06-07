using System;
using ConsoleGetOrder;

namespace GetOrderConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Create Database and Create Orders table
            DbConnect dbConnect = new DbConnect();
            dbConnect.CreateTables2();
           // dbConnect.ExecuteQueryToGetId(
            //    "select Id from Customers where Customers.Phone = '849632097691' or Customers.Phone is null limit 1;");
            //Get order data from Zalo and insert list order to database
            GetFromZalo zalo = new GetFromZalo();
            zalo.Init();
            zalo.GetOrderAndOrderDetail();
            //try
            //{
            //    zalo.GetOrderAndOrderDetail();
            //    Console.WriteLine("ok");
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    throw;
            //}
            

            //Console.WriteLine(zalo.a());
            //zalo.InsertOrderToDb();

            //GetFromWooCommerce wooCommerce = new GetFromWooCommerce();
            //wooCommerce.Getproduct();

            Console.ReadKey();
        }
    }
}