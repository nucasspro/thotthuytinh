using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.ViewModel
{
    public class LoginViewModel:BaseViewModel
    {
        public void ShowLoginWindow()
        {
            LoginWindow lg = new LoginWindow();
            lg.ShowDialog();
        }
    }
}
