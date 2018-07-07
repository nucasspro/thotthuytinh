using CrystalDecisions.Shared;
using OMS.Model;
using SAPBusinessObjects.WPF.Viewer;
using System;
using System.Data;
using System.Windows.Forms;
using System.Windows.Input;

namespace OMS.ViewModel
{
    public class ReportUCViewModel : BaseViewModel
    {
        #region command

        public ICommand ButtonPreviewCommand { get; set; }
        public ICommand SelectionChangedCommand { get; set; }
        public ICommand ButtonExportCommand { get; set; }

        #endregion command

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

        private int _SelectedIndex { get; set; }

        public int SelectedIndex
        {
            get => _SelectedIndex;
            set { _SelectedIndex = value; OnPropertyChanged(); }
        }

        private DateTime _StartedDate { get; set; }

        public DateTime StartedDate
        {
            get => _StartedDate;
            set { _StartedDate = value; OnPropertyChanged(); }
        }

        private DateTime _EndDate { get; set; }

        public DateTime EndDate
        {
            get => _EndDate;
            set { _EndDate = value; OnPropertyChanged(); }
        }

        private bool _ExportIsEnabled { get; set; }

        public bool ExportIsEnabled
        {
            get => _ExportIsEnabled;
            set { _ExportIsEnabled = value; OnPropertyChanged(); }
        }

        public RevenueReport CrystalReport { get; set; }

        #endregion virable

        #region methods

        public ReportUCViewModel()
        {
            CrystalReport = new RevenueReport();
            var now = DateTime.Now;
            ReportMonthIsEnbled = true;
            ReportYear = now.Year;
            SelectionChangedCommand = new RelayCommand<System.Windows.Controls.ComboBox>(p => true, p =>
            {
                SelectedIndex = p.SelectedIndex;
                if (SelectedIndex == 0)
                {
                    ReportMonthIsEnbled = true;
                }
                else
                    ReportMonthIsEnbled = false;
            });

            ButtonPreviewCommand = new RelayCommand<CrystalReportsViewer>(p => true, p =>
            {
                Orders orders;
                DataTable temp;

                //validate information
                if (ReportYear < 2000 || ReportYear > 3000)
                    System.Windows.MessageBox.Show("Năm trong khoảng (2000; 3000)");

                //set value to StartedDate and EndDate
                if (SelectedIndex == 0)
                {
                    StartedDate = new DateTime(ReportYear, ReportMonth + 1, 1, 0, 0, 0);
                    EndDate = ReturnEndDate(ReportMonth + 1, ReportYear);
                }
                else
                {
                    StartedDate = new DateTime(ReportYear, 1, 1);
                    EndDate = new DateTime(ReportYear, 12, 31);
                }

                orders = new Orders();
                temp = orders.CreateReport(StartedDate, EndDate);
                CrystalReport.Database.Tables["Revenue"].SetDataSource(temp);
                p.ViewerCore.ReportSource = CrystalReport;
                if (CrystalReport != null)
                    ExportIsEnabled = true;
                else
                    ExportIsEnabled = false;
            });

            ButtonExportCommand = new RelayCommand<object>(p => true, p =>
            {
                ExportPDF(CrystalReport);
            });
        }

        public bool CheckYear(int year)
        {
            if (((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0))
                return true;
            return false;
        }

        public DateTime ReturnEndDate(int month, int year)
        {
            DateTime temp;
            int date;
            if (month == 2)
            {
                if (CheckYear(year))
                    date = 29;
                else
                    date = 28;
            }
            if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
                date = 31;
            else
                date = 30;
            temp = new DateTime(year, month, date, 0, 0, 0);
            return temp;
        }

        public void ExportPDF(RevenueReport report)
        {
            var exportOptions = new ExportOptions();
            var diskFile = new DiskFileDestinationOptions();

            var sfd = new SaveFileDialog();
            sfd.Filter = "Pdf Files|*.pdf";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                diskFile.DiskFileName = sfd.FileName;
            }
            exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            exportOptions.ExportDestinationOptions = diskFile;
            exportOptions.ExportFormatOptions = new PdfRtfWordFormatOptions();

            try
            {
                report.Export(exportOptions);
                System.Windows.MessageBox.Show("Xuất File thành công!");
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Có lỗi xảy ra trong quá trình xuất file!");
            }
        }

        #endregion methods
    }
}