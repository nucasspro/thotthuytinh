﻿using Facebook;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OMS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using xNet;

namespace OMS.ViewModel
{
    public class SchedulePostUCViewModel : BaseViewModel
    {
        #region Command

        public ICommand LoadCommand { get; set; }
        public ICommand CreateCommand { get; set; }
        public ICommand CreateWithAllProductsCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        #endregion Command

        #region Variable

        private string _Description { get; set; }

        public string Description
        {
            get => _Description;
            set { _Description = value; OnPropertyChanged(); }
        }

        private DateTime _DateTimePickerDate { get; set; }

        public DateTime DateTimePickerDate
        {
            get => _DateTimePickerDate;
            set { _DateTimePickerDate = value; OnPropertyChanged(); }
        }

        private DateTime _DateTimePickerTime { get; set; }

        public DateTime DateTimePickerTime
        {
            get => _DateTimePickerTime;
            set { _DateTimePickerTime = value; OnPropertyChanged(); }
        }

        private string _ComboboxProductListSelectedValue { get; set; }

        public string ComboboxProductListSelectedValue
        {
            get => _ComboboxProductListSelectedValue;
            set { _ComboboxProductListSelectedValue = value; OnPropertyChanged(); }
        }

        private Products _ComboboxProductListSelectedItem { get; set; }

        public Products ComboboxProductListSelectedItem
        {
            get => _ComboboxProductListSelectedItem;
            set { _ComboboxProductListSelectedItem = value; OnPropertyChanged(); }
        }

        private Posts _ListSchedulePostSelectedItem { get; set; }

        public Posts ListSchedulePostSelectedItem
        {
            get => _ListSchedulePostSelectedItem;
            set
            {
                _ListSchedulePostSelectedItem = value;
                OnPropertyChanged();
                if (ListSchedulePostSelectedItem == null)
                    return;
                Description = ListSchedulePostSelectedItem.Message;
                DateTimePickerDate = UnixTimestampToDateTime(Convert.ToDouble(ListSchedulePostSelectedItem.CreatedTime));
                DateTimePickerTime = UnixTimestampToDateTime(Convert.ToDouble(ListSchedulePostSelectedItem.CreatedTime));
            }
        }

        public ObservableCollection<Products> ListProduct { get; set; }
        public ObservableCollection<Posts> ListSchedulePost { get; set; }

        private const string GraphUrl = @"https://graph.facebook.com/v3.0/";

        //thotthuytinh
        //private const string PageId = "1790501110988348";
        //raplyrics
        private const string PageId = "722487931126157";

        private string _pageAccessToken;

        #endregion Variable

        #region Method

        public SchedulePostUCViewModel()
        {
            GetPageAccessToken();
            ListProduct = new ObservableCollection<Products>();
            ListSchedulePost = new ObservableCollection<Posts>();
            LoadProduct();

            LoadCommand = new RelayCommand<Button>(p => true, p =>
            {
                LoadSchedulePost();
            });

            CreateCommand = new RelayCommand<Button>(p => true, p =>
            {
                CreateASchedulePost();
                LoadSchedulePost();
            });

            CreateWithAllProductsCommand = new RelayCommand<Button>(p => true, p =>
            {
                CreateWithAllProducts();
                LoadSchedulePost();
            });

            // ReSharper disable once ComplexConditionExpression
            DeleteCommand = new RelayCommand<Button>(p => true, p =>
            {
                if (ListSchedulePostSelectedItem == null)
                    return;
                try
                {
                    DeleteASchedulePost(ListSchedulePostSelectedItem.Id);
                    ListSchedulePost.Clear();
                    LoadSchedulePost();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Xóa thất bại!" + e);
                }
            });
        }

        private string GetAccessToken()
        {
            const string username = "nucasspronewrap@gmail.com";
            const string password = "Nucass2189401222";

            const string address = "https://nghia.org/public/api/v1/buildLogin.php";
            const string data = "u=" + username + "&p=" + password;
            WebClient client = new WebClient
            {
                Headers = { [HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded" }
            };
            string str4 = client.UploadString(address, data);
            string accessToken = Convert.ToString(JObject.Parse(new WebClient().DownloadString(str4))["access_token"]);
            return accessToken;
        }

        private void GetPageAccessToken()
        {
            HttpRequest httpRequest = new HttpRequest();
            string accessToken = GetAccessToken();
            string newaddress = $"{GraphUrl}{PageId}?fields=access_token&access_token={accessToken}";
            var json = JsonConvert.DeserializeObject(httpRequest.Get(newaddress).ToString());
            JToken jToken = JToken.FromObject(json);
            _pageAccessToken = jToken["access_token"].ToString();
        }

        private List<string> GetListPhotoId()
        {
            if (ComboboxProductListSelectedValue == null)
            {
                MessageBox.Show("Các trường * không được bỏ trống!");
                return null;
            }
            else
            {
                List<string> listPhotoId = new List<string>
                {
                    UploadAPhoto(ComboboxProductListSelectedItem.Image1),
                    UploadAPhoto(ComboboxProductListSelectedItem.Image2),
                    UploadAPhoto(ComboboxProductListSelectedItem.Image3)
                };
                return listPhotoId;
            }
        }

        private string UploadAPhoto(string photoPath)
        {
            FacebookClient facebook = new FacebookClient(_pageAccessToken);
            var imageStream = File.OpenRead(photoPath);
            //var imageId = facebook.Post($"/{PageId}/photos", new
            //{
            //    message = "",
            //    published = "False",
            //    scheduled_publish_time = "1530253379",
            //    file =
            //        new FacebookMediaStream { ContentType = "image/png", FileName = "test upload image4" }.SetValue(
            //            imageStream)
            //});
            var imageId = facebook.Post($"/{PageId}/photos", new
            {
                published = "False",
                scheduled_publish_time = "1530253379",
                file =
                    new FacebookMediaStream { ContentType = "image/png", FileName = "test upload image4" }.SetValue(
                        imageStream)
            });
            return imageId.ToString().Replace("{\"id\":\"", "").Replace("\"}", "");
        }

        private void CreateASchedulePost()
        {
            var list = GetListPhotoId();
            if (list == null)
                return;
            string published = "False";
            string dateTime = DateTimePickerDate.ToString("MM/dd/yyyy") + " " + DateTimePickerTime.ToString("HH:mm:ss");
            string scheduledPublishTime = ConvertToTimeSpan(dateTime);
            string fields = $"/feed?message={Description}&published={published}&scheduled_publish_time={scheduledPublishTime}&access_token={_pageAccessToken}";
            for (int i = 0; i < list.Count; i++)
            {
                fields += $"&attached_media[{i}]={{\"media_fbid\":\"{list[i]}\"}}";
            }
            string newAddress = GraphUrl + PageId + fields;
            HttpRequest httpRequest = new HttpRequest();
            httpRequest.Post(newAddress);
        }

        private void CreateWithAllProducts()
        {
            string published = "False";
            string dateTime = DateTimePickerDate.ToString("MM/dd/yyyy") + " " + DateTimePickerTime.ToString("HH:mm:ss");
            string scheduledPublishTime = ConvertToTimeSpan(dateTime);
            string fields = $"/feed?message={Description}&published={published}&scheduled_publish_time={scheduledPublishTime}&access_token={_pageAccessToken}";

            foreach (var item in ListProduct)
            {
                List<string> list = new List<string>
                {
                    UploadAPhoto(item.Image1),
                    UploadAPhoto(item.Image2),
                    UploadAPhoto(item.Image3)
                };
                string newFields = fields;
                for (int i = 0; i < list.Count; i++)
                {
                    newFields += $"&attached_media[{i}]={{\"media_fbid\":\"{list[i]}\"}}";
                }
                string newAddress = GraphUrl + PageId + newFields;
                HttpRequest httpRequest = new HttpRequest();
                httpRequest.Post(newAddress);
            }
            MessageBox.Show("Tạo xong!");
        }

        /// <summary>
        /// postId = "722487931126157_2044843528890584";
        /// </summary>
        /// <param name="postId"></param>
        private void DeleteASchedulePost(string postId)
        {
            try
            {
                FacebookClient facebook = new FacebookClient(_pageAccessToken);
                facebook.Delete($"/{postId}");
                MessageBox.Show("xong");
            }
            catch (Exception e)
            {
                MessageBox.Show("loi" + e);
            }
        }

        private void LoadProduct()
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
                    Description = (string)((DataRow)row).ItemArray[2],
                    Weight = (string)((DataRow)row).ItemArray[3],
                    Width = (string)((DataRow)row).ItemArray[4],
                    Height = (string)((DataRow)row).ItemArray[5],
                    Length = (string)((DataRow)row).ItemArray[6],
                    Price = (string)((DataRow)row).ItemArray[7],
                    Image1 = (string)((DataRow)row).ItemArray[8],
                    Image2 = (string)((DataRow)row).ItemArray[9],
                    Image3 = (string)((DataRow)row).ItemArray[10],
                    Quantity = Convert.ToInt32(((DataRow)row).ItemArray[11]),
                    CreatedBy = new Accounts { Id = Convert.ToInt32(((DataRow)row).ItemArray[12]) }
                };
                ListProduct.Add(product);
            }
        }

        private void LoadSchedulePost()
        {
            try
            {
                string newAddress = $"{PageId}/promotable_posts?is_published=false";
                FacebookClient facebook = new FacebookClient(_pageAccessToken);
                var json = JsonConvert.DeserializeObject(facebook.Get($"/{newAddress}").ToString());
                JToken jToken = JToken.FromObject(json);
                ListSchedulePost.Clear();
                foreach (var item in jToken["data"])
                {
                    Posts post = new Posts
                    {
                        Id = item["id"].ToString(),
                        Message = item["message"].ToString(),
                        CreatedTime = item["scheduled_publish_time"].ToString()
                    };
                    ListSchedulePost.Add(post);
                }
            }
            catch (Exception e)
            {
            }
        }

        private string ConvertToTimeSpan(string time)
        {
            DateTime dateTime = DateTime.Parse(time).ToLocalTime();
            var dateTimeOffset = new DateTimeOffset(dateTime);
            return dateTimeOffset.ToUnixTimeSeconds().ToString();
        }

        private DateTime UnixTimestampToDateTime(double unixTime)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dateTime = dateTime.AddSeconds(unixTime).ToLocalTime();
            return dateTime;
        }

        #endregion Method
    }
}