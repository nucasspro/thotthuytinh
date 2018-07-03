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
            // ReSharper disable once ComplexConditionExpression
            LoadedCommand = new RelayCommand<Window>(p => true,
                p =>
                {
                    if (p == null) return;
                    IsLoaded = true;
                    p.Hide();
                    LoginWindow lg = new LoginWindow();
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