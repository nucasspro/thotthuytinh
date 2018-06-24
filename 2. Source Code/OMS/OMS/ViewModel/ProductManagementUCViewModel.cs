using Microsoft.Win32;
using OMS.Model;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Controls;
using System.Windows.Input;

namespace OMS.ViewModel
{
    public class ProductManagementUCViewModel : BaseViewModel
    {
        #region MyRegion

        public ICommand CreateCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand AddImage1Command { get; set; }
        public ICommand AddImage2Command { get; set; }
        public ICommand AddImage3Command { get; set; }

        #endregion MyRegion

        #region Variable

        public ObservableCollection<Products> ListProduct { get; set; }

        private Products _SelectedItem { get; set; }

        public Products SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem == null)
                    return;
                ProductId = SelectedItem.Id;
                ProductName = SelectedItem.Name;
                ProductPrice = SelectedItem.Price;
                ProductWidth = SelectedItem.Width;
                ProductWeight = SelectedItem.Weight;
                ProductHeight = SelectedItem.Height;
                ProductLength = SelectedItem.Length;
                ProductImage1 = SelectedItem.Image1;
                ProductImage2 = SelectedItem.Image2;
                ProductImage3 = SelectedItem.Image3;
                ProductQuantity = Convert.ToInt32(SelectedItem.Quantity);
            }
        }

        private string _ProductId { get; set; }

        public string ProductId
        {
            get => _ProductId;
            set { _ProductId = value; OnPropertyChanged(); }
        }

        private string _ProductName { get; set; }

        public string ProductName
        {
            get => _ProductName;
            set { _ProductName = value; OnPropertyChanged(); }
        }

        private string _ProductPrice { get; set; }

        public string ProductPrice
        {
            get => _ProductPrice;
            set { _ProductPrice = value; OnPropertyChanged(); }
        }

        private string _ProductWidth { get; set; }

        public string ProductWidth
        {
            get => _ProductWidth;
            set { _ProductWidth = value; OnPropertyChanged(); }
        }

        private string _ProductHeight { get; set; }

        public string ProductHeight
        {
            get => _ProductHeight;
            set { _ProductHeight = value; OnPropertyChanged(); }
        }

        private string _ProductWeight { get; set; }

        public string ProductWeight
        {
            get => _ProductWeight;
            set { _ProductWeight = value; OnPropertyChanged(); }
        }

        private string _ProductLength { get; set; }

        public string ProductLength
        {
            get => _ProductLength;
            set { _ProductLength = value; OnPropertyChanged(); }
        }

        private string _ProductImage1 { get; set; }

        public string ProductImage1
        {
            get => _ProductImage1;
            set { _ProductImage1 = value; OnPropertyChanged(); }
        }

        private string _ProductImage2 { get; set; }

        public string ProductImage2
        {
            get => _ProductImage2;
            set { _ProductImage2 = value; OnPropertyChanged(); }
        }

        private string _ProductImage3 { get; set; }

        public string ProductImage3
        {
            get => _ProductImage3;
            set { _ProductImage3 = value; OnPropertyChanged(); }
        }

        private int _ProductQuantity { get; set; }

        public int ProductQuantity
        {
            get => _ProductQuantity;
            set { _ProductQuantity = Convert.ToInt32(value); OnPropertyChanged(); }
        }

        #endregion Variable

        #region Method

        public ProductManagementUCViewModel()
        {
            ListProduct = new ObservableCollection<Products>();
            LoadProduct();
            CreateCommand = new RelayCommand<Button>(p => true, p => { CreateProduct(); LoadProduct(); });
            UpdateCommand = new RelayCommand<Button>(p => true, p => { UpdateProduct(); LoadProduct(); });
            DeleteCommand = new RelayCommand<Button>(p => true, p => UpdateProduct());
            AddImage1Command = new RelayCommand<Button>(p => true, p => ProductImage1 = FindFilePath());
            AddImage2Command = new RelayCommand<Button>(p => true, p => ProductImage2 = FindFilePath());
            AddImage3Command = new RelayCommand<Button>(p => true, p => ProductImage3 = FindFilePath());
        }

        public void LoadProduct()
        {
            DBConnect dbConnect = new DBConnect();
            const string query = @"select * from Products;";
            DataTable dataTable = dbConnect.SelectQuery(query);
            foreach (var row in dataTable.Rows)
            {
                Products product = new Products
                {
                    Id = (string)((DataRow)row).ItemArray[0],
                    Name = (string)((DataRow)row).ItemArray[1],
                    Weight = (string)((DataRow)row).ItemArray[2],
                    Width = (string)((DataRow)row).ItemArray[3],
                    Height = (string)((DataRow)row).ItemArray[4],
                    Length = (string)((DataRow)row).ItemArray[5],
                    Price = (string)((DataRow)row).ItemArray[6],
                    Image1 = (string)((DataRow)row).ItemArray[7],
                    Image2 = (string)((DataRow)row).ItemArray[8],
                    Image3 = (string)((DataRow)row).ItemArray[9],
                    Quantity = Convert.ToInt32(((DataRow)row).ItemArray[10]),
                    CreatedBy = new Accounts { Id = Convert.ToInt32(((DataRow)row).ItemArray[11]) }
                };
                ListProduct.Add(product);
            }
        }

        public void CreateProduct()
        {
            DBConnect dbConnect = new DBConnect();
            string query = $"insert into Products(Id, Name, Weight, Width, Height, Length, Price, Image1, Image2, Image3, Quantity, CreatedBy) " +
                           $"values ('{ProductId}', '{ProductName}', '{ProductWeight}', '{ProductWidth}', '{ProductHeight}', '{ProductLength}', '{ProductPrice}', '','','','{ProductQuantity}', 1)";
            dbConnect.ExecuteQuery(query);
            ListProduct.Clear();
        }

        public void UpdateProduct()
        {
            DBConnect dbConnect = new DBConnect();
            string query = $"Update Products " +
                           $"Set Name = '{ProductName}', Weight = '{ProductWeight}', Width = '{ProductWidth}', Height = '{ProductHeight}', Length = '{ProductLength}', Price = '{ProductPrice}', Image1 = '{ProductImage1}', Image2 = '{ProductImage2}', Image3 = '{ProductImage3}', Quantity = {ProductQuantity} " +
                           $"where Id = '{ProductId}';";
            ListProduct.Clear();
            dbConnect.ExecuteQuery(query);
        }

        public string FindFilePath()
        {
            FileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
            return fileDialog.FileName;
        }

        #endregion Method
    }
}