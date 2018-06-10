using System;

namespace GetOrderConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            DbConnect dbConnect = new DbConnect();
            dbConnect.Init();

            try
            {
                GetFromZalo zalo = new GetFromZalo();
                zalo.Init();
                zalo.GetData();

                GetFromWooCommerce wooCommerce = new GetFromWooCommerce();
                wooCommerce.Init();
                wooCommerce.GetData();
                Console.WriteLine("END!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Loi " + e);
                throw;
            }

            //GetOrderFromFacebook facebook = new GetOrderFromFacebook();
            //facebook.Init();
            //Console.WriteLine(facebook.ConvertToDateTime("2018-06-08T01:08:52+0000"));
            Console.ReadKey();
        }
    }
}