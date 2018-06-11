using System;
using System.Globalization;

namespace GetOrderConsole
{
    public class Program
    {
        public static readonly DateTime Time1 = DateTime.Today.AddHours(00).AddMinutes(00).ToLocalTime();
        public static readonly DateTime Time2 = DateTime.Today.AddHours(08).AddMinutes(00).ToLocalTime();
        public static readonly DateTime Time3 = DateTime.Today.AddHours(16).AddMinutes(00).ToLocalTime();

        private static void Main(string[] args)
        {
            DbConnect dbConnect = new DbConnect();
            dbConnect.Init();

            var time = CheckTime();
            RunApplication(time);

            Console.ReadKey();
        }

        private static void RunApplication(int time)
        {
            try
            {
                Console.WriteLine("Running...");
                Zalo zalo = new Zalo();
                zalo.Init(Time1, Time2, Time3);
                zalo.GetData(time);

                WooCommerce wooCommerce = new WooCommerce();
                wooCommerce.Init(Time1, Time2, Time3);
                wooCommerce.GetData(time);
                
                //GetOrderFromFacebook facebook = new GetOrderFromFacebook();
                //facebook.Init();
                //Console.WriteLine(facebook.ConvertToDateTime("2018-06-08T01:08:52+0000"));

                Console.WriteLine("END!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Loi " + e);
                throw;
            }
        }

        private static int CheckTime()
        {
            DateTime dateTime = DateTime.Now;

            if (dateTime > Time3)
                return 3;
            return dateTime > Time2 ? 2 : 1;
        }
    }
}