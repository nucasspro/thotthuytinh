using CrystalDecisions.CrystalReports.Engine;
using OMS.Model;
using SAPBusinessObjects.WPF.Viewer;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OMS.ViewModel
{
    internal class ReportUCViewModel : BaseViewModel
    {
        #region command

        public ICommand ButtonPreviewCommand { get; set; }
        public ICommand SelectionChangedCommand { get; set; }

        #endregion commandC:\Users\SiMenPC\Documents\GitHub\thotthuytinh\2. Source Code\OMS\OMS\ViewModel\ReportUCViewModel.cs

        #region virable

        private bool _ReportMonthIsEnbled { get; set; }

        public bool ReportMonthIsEnbled
        {
            get => _ReportMonthIsEnbled;
            set { _ReportMonthIsEnbled = value; OnPropertyChanged(); }
        }

        private int _ReportYear { get; set; }

        public int ReportYear
        {
            get => _ReportYear;
            set { _ReportYear = value; OnPropertyChanged(); }
        }

        private int _ReportMonth { get; set; }

        public int ReportMonth
        {
            get => _ReportMonth;
            set { _ReportMonth = value; OnPropertyChanged(); }
        }

        public RevenueReport CrystalReport { get; set; }


        #endregion virable

        #region methods

        public ReportUCViewModel()
        {
            CrystalReport = new RevenueReport();
            DateTime now = DateTime.Now;
            ReportMonthIsEnbled = true;
            ReportYear = now.Year;
            SelectionChangedCommand = new RelayCommand<ComboBox>(p => true, p =>
            {
                int temp = p.SelectedIndex;
                if (temp == 0)
                    ReportMonthIsEnbled = true;
                else
                    ReportMonthIsEnbled = false;
            });

            ButtonPreviewCommand = new RelayCommand<CrystalReportsViewer>(p => true, p =>
            {
                p.ViewerCore.ReportSource= CrystalReport;
                if (ReportYear < 2000 || ReportYear > 3000)
                    MessageBox.Show("Năm trong khoảng (2000; 3000)");
                MessageBox.Show((ReportMonth + 1).ToString() + ReportYear.ToString());
            });
        }

        private void CreateReport()
        {
            DBConnect dB = new DBConnect();
            DataTable dataTable = new DataTable();
            string test = "select OrderFrom, Id, UpdatedTime, GrandPrice "+
                            "from Orders";
            dataTable = dB.SelectQuery(test);
            
            
        }

        #endregion methods
    }
}