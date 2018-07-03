using OMS.Model;
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

        #region Variable

        public bool IsLogin;
        public int isVeryfy;
        private string _UserName;
        public string UserName { get => _UserName; set { _UserName = value; OnPropertyChanged(); } }

        private string _Password;
        public string Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }

        public Accounts Accounts;

        #endregion Variable

        #region Method

        public LoginViewModel()
        {
            Accounts = new Accounts();
            PasswordChangedCommand = new RelayCommand<PasswordBox>(p => true, p => { Password = p.Password; });

            HitLoginButtonCommand = new RelayCommand<Window>(p => true, p => { IsLogin = false; Login(p); });
        }

        private void Login(Window p)
        {
            if (p == null)
                return;
            if (UserName == null || Password == null)
            {
                MessageBox.Show("Hãy điền đầy đủ cả trường Username và Password!");
            }
            else
            {
                if (Accounts.CheckAccount(UserName, Password) == 0)
                {
                    MessageBox.Show("Tài khoản không tồn tại!");
                }
                else
                {
                    IsLogin = true;
                    p.Close();
                    isVeryfy = Accounts.CheckAccount(UserName, Password);
                }
            }
        }

        #endregion Method
    }
}