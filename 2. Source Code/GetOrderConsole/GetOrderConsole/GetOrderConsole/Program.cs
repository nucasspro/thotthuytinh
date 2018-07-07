using System;

namespace GetOrderConsole
{
    public class Program
    {
        public static readonly DateTime Time1 = DateTime.Today.AddHours(00).AddMinutes(00).ToLocalTime();
        public static readonly DateTime Time2 = DateTime.Today.AddHours(08).AddMinutes(00).ToLocalTime();
        public static readonly DateTime Time3 = DateTime.Today.AddHours(16).AddMinutes(00).ToLocalTime();
        private static long OaId = 0;
        private static string SecretKey = "";
        private static string HostUrl = "";
        private static string ConsumerKey = "";
        private static string ConsumerSecret = "";
        private static string FBPageId = "";
        private static string FBUserName = "";
        private static string FBPassWord = "";
        private static string DatabasePath = "";
        private static void Main(string[] args)
        {
            try
            {
                OaId = long.Parse(args[0]);
                SecretKey = args[1];
                HostUrl = args[2];
                ConsumerKey = args[3];
                ConsumerSecret = args[4];
                FBPageId = args[5];
                FBUserName = args[6];
                FBPassWord = args[7];
                DatabasePath = args[8];
            }
            catch (Exception)
            {
                Console.WriteLine("arguments Not Match");
            }
            Console.WriteLine(DatabasePath);
            DbConnect dbConnect = new DbConnect();
            dbConnect.Init(DatabasePath);
            var time = CheckTime();
            RunApplication(time);
            Console.ReadKey();
        }

        private static void RunApplication(int time)
        {
            try
            {
                Console.WriteLine("Running...");

                Zalo zalo = new Zalo(Time1, Time2, Time3, OaId, SecretKey);
                zalo.GetData(time);
                Console.WriteLine("Zalo END!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Loi " + e);
            }
            try
            {
                WooCommerce wooCommerce = new WooCommerce(Time1, Time2, Time3, HostUrl, ConsumerKey, ConsumerSecret);
                wooCommerce.GetData(time);
                Console.WriteLine("Woo END!");
            }
            catch (Exception e1)
            {
                Console.WriteLine("Loi " + e1);
            }
            try
            {
                Facebook facebook = new Facebook(Time1, Time2, Time3, FBPageId, FBUserName, FBPassWord);
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