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
        public ICommand CloseWindowCommand { get; set; }
        public ICommand ButtonOrderManagementClickCommand { get; set; }
        public ICommand ButtonProductManagementClickCommand { get; set; }
        public ICommand ButtonReportManagementClickCommand { get; set; }
        public ICommand ButtonScheduleManagementClickCommand { get; set; }

        public ICommand ChangeGridCursorCommand { get; set; }
        #endregion

        public bool IsLoaded = false;
        public MainWindow main;
        public MainViewModel()
        {
            //if (!IsLoaded)
            //{
            //    IsLoaded = true;
            //    LoginViewModel lg = new LoginViewModel();
            //    lg.ShowLoginWindow();
            //}
            //Close window event
            CloseWindowCommand = new RelayCommand<MainWindow>(
                (p) => { return p == null? false : true; }, 
                (p) => { p.Close(); }
                );

            // showtab
            ClickButtonShowTabCommand(ButtonOrderManagementClickCommand);
            ClickButtonShowTabCommand(ButtonProductManagementClickCommand);
            ClickButtonShowTabCommand(ButtonReportManagementClickCommand);
            ClickButtonShowTabCommand(ButtonScheduleManagementClickCommand);
        }

        public void ClickButtonShowTabCommand(ICommand command)
        {
            command = new RelayCommand<object>(
                (p) => { return p == null ? false : true; },
                (p) => {
                    int index = Int32.Parse(p.ToString());
                    ChangeGridCursor(index);
                }
                );
        }
        public void ChangeGridCursor(int Uid)
        {
           

        }

    }
}
