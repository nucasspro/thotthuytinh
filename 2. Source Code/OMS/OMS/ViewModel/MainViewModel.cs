using OMS.Model;
using System.Windows;
using System.Windows.Input;

namespace OMS.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        #region command

        public ICommand LoadedCommand { get; set; }

        #endregion command

        #region Variable

        public bool IsLoaded = false;

        #endregion Variable

        #region Method

        public MainViewModel()
        {
            LoadedCommand = new RelayCommand<Window>(p => true, p =>
            {
                var dBConnect = new DBConnect();
                if (dBConnect.Init() == false)
                {
                    MessageBox.Show("Không tìm thấy cơ sở dữ liệu.");
                    Application.Current.Shutdown();
                }
                if (p == null) return;
                IsLoaded = true;
                p.Hide();
                var lg = new LoginWindow();
                lg.ShowDialog();
                var LoginVM = lg.DataContext as LoginViewModel;
                if (LoginVM == null) return;
                if (LoginVM.IsLogin) p.Show();
                else p.Close();
            });
        }

        #endregion Method
    }
}