using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OMS.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        #region command

        public ICommand HitLoginButtonCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }

        #endregion command

        public bool IsLogin;
        public Model.DBConnect dBConnect;
        private String _UserName;
        public string UserName { get => _UserName; set { _UserName = value; OnPropertyChanged(); } }
        private String _Password;
        public string Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }

        public LoginViewModel()
        {
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; },
               (p =>
               {
                   Password = p.Password;
               }));

            HitLoginButtonCommand = new RelayCommand<Window>((p) => { return true; },
                (p =>
                {
                    IsLogin = false;
                    Login(p);
                }));
        }

        private void Login(Window p)
        {
            dBConnect = new Model.DBConnect();
            String query;
            if (p == null)
                return;
            if (UserName == null || Password == null)
            {
                MessageBox.Show("Hãy điền đầy đủ cả trường Username và Password!");
            }
            else
            {
                query = "select id from Accounts where Username='" + UserName + "' and Password ='" + Password + "' limit 1;";
                if (dBConnect.ExecuteQueryToGetIdAndCount(query) == 0)
                {
                    MessageBox.Show("Tài khoản không tồn tại!");
                }
                else
                {
                    IsLogin = true;
                    p.Close();
                }
                
            }
        }
    }
}