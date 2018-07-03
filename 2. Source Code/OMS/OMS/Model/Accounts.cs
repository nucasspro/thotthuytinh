namespace OMS.Model
{
    public class Accounts
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }

        #region method

        public int CheckAccount(string UserName, string Password)
        {
            DBConnect dBConnect = new DBConnect();
            string query = "select id from Accounts where Username='" + UserName + "' and Password ='" + Password + "' limit 1;";
            return dBConnect.ExecuteQueryToGetIdAndCount(query);
        }

        #endregion method
    }
}