using System;

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
        }

        private static void RunApplication(int time)
        {
            try
            {
                Console.WriteLine("Running...");

                Zalo zalo = new Zalo(Time1, Time2, Time3);
                zalo.GetData(time);
                Console.WriteLine("Zalo END!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Loi " + e);
            }
            try
            {
                WooCommerce wooCommerce = new WooCommerce(Time1, Time2, Time3);
                wooCommerce.GetData(time);
                Console.WriteLine("Woo END!");
            }
            catch (Exception e1)
            {
                Console.WriteLine("Loi " + e1);
            }
            try
            {
                Facebook facebook = new Facebook(Time1, Time2, Time3);
                facebook.GetData(time);
                Console.WriteLine("Face END!");

                Console.WriteLine("END!");
            }
            catch (Exception e2)
            {
                Console.WriteLine("Loi " + e2);
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