using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OMS.ViewModel
{
    public class MainViewModel:BaseViewModel
    {
        #region command
        public ICommand LoadedCommand { get; set; }
        #endregion
        public bool IsLoaded = false;
        public MainViewModel()
        {
            //LoadedCommand = new RelayCommand<Window>(
            //    (p) => { return true; },
            //    (p) =>
            //    {
            //        if (p == null)
            //            return;
            //        IsLoaded = true;
            //        p.Hide();
            //        LoginWindow lg = new LoginWindow();
            //        lg.ShowDialog();
            //        var LoginVM = lg.DataContext as LoginViewModel;
            //        if (LoginVM == null)
            //            return;
            //        if (LoginVM.IsLogin)
            //        {
            //            p.Show();
            //        }
            //        else
            //        {
            //            p.Close();
            //        }
            //    }
            //    );

        }


    }
}
