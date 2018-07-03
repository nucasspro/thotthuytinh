namespace OMS.Model
{
    public class Accounts
    {
        private DBConnect dBConnect;

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }

        public int CheckAccount(string UserName, string Password)
        {
            dBConnect = new DBConnect();
            string query = "select id from Accounts where Username='" + UserName + "' and Password ='" + Password + "' limit 1;";
            return dBConnect.ExecuteQueryToGetIdAndCount(query);
        }
    }
}