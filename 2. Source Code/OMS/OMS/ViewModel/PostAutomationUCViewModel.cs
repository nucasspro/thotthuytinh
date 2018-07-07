using Facebook;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OMS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using xNet;

namespace OMS.ViewModel
{
    public class PostAutomationUCViewModel : BaseViewModel
    {
        #region Command

        public ICommand LoadedCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand GetTokenCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        public ICommand CreateCommand { get; set; }
        public ICommand CreateWithAllProductsCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        #endregion Command

        #region Variable

        private string _FbPageId { get; set; }

        public string FbPageId
        {
            get => _FbPageId;
            set { _FbPageId = value; OnPropertyChanged(); }
        }

        private string _FbUsername { get; set; }

        public string FbUsername
        {
            get => _FbUsername;
            set { _FbUsername = value; OnPropertyChanged(); }
        }

        private string _FbPassword { get; set; }

        public string FbPassword
        {
            get => _FbPassword;
            set { _FbPassword = value; OnPropertyChanged(); }
        }

        private string _PageAccessToken { get; set; }

        public string PageAccessToken
        {
            get => _PageAccessToken;
            set { _PageAccessToken = value; OnPropertyChanged(); }
        }

        private bool _ShowActionButton { get; set; }
        public bool ShowActionButton
        {
            get => _ShowActionButton;
            set { _ShowActionButton = value; OnPropertyChanged(); }
        }

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
        private string _AccessToken;
        public Products Product;

        #endregion Variable

        #region Method

        public PostAutomationUCViewModel()
        {
            DateTimePickerDate = DateTime.Today;

            ListProduct = new ObservableCollection<Products>();
            ListSchedulePost = new ObservableCollection<Posts>();
            Product = new Products();

            LoadedCommand = new RelayCommand<Window>(p => true, p =>
            {
                ShowActionButton = false;
                ListProduct.Clear();
                foreach (var item in Product.LoadProduct())
                {
                    ListProduct.Add(item);
                }
            });

            PasswordChangedCommand = new RelayCommand<PasswordBox>(p => true, p => { _FbPassword = p.Password; });

            GetTokenCommand = new RelayCommand<PasswordBox>(p => true, p =>
            {
                string token = GetPageAccessToken();
                if (token != "")
                {
                    _PageAccessToken = token;
                    ShowActionButton = true;
                }
                else
                {
                    _PageAccessToken = "Không lấy được token.";
                }
            });

            LoadCommand = new RelayCommand<Button>(p => true, p => { LoadSchedulePost(); });

            CreateCommand = new RelayCommand<Button>(p => true, p => { CreateASchedulePost(); LoadSchedulePost(); });

            CreateWithAllProductsCommand = new RelayCommand<Button>(p => true, p => { CreateWithAllProducts(); LoadSchedulePost(); });

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

        private void GetAccessToken()
        {
            const string address = "https://nghia.org/public/api/v1/buildLogin.php";
            string data = "u=" + FbUsername + "&p=" + FbPassword;
            try
            {
                var client = new WebClient
                {
                    Headers = { [HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded" }
                };
                string str4 = client.UploadString(address, data);
                _AccessToken = Convert.ToString(JObject.Parse(new WebClient().DownloadString(str4))["access_token"]);
            }
            catch (Exception)
            {
                MessageBox.Show("Lấy Access Token thất bại, kiểm tra lại thông tin tài khoản!");
            }
        }

        private string GetPageAccessToken()
        {
            GetAccessToken();
            string newaddress = $"{GraphUrl}{FbPageId}?fields=access_token&access_token={_AccessToken}";
            try
            {
                var httpRequest = new HttpRequest();
                var json = JsonConvert.DeserializeObject(httpRequest.Get(newaddress).ToString());
                var jToken = JToken.FromObject(json);
                return jToken["access_token"].ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Lấy Page Access Token thất bại, kiểm tra lại thông tin Page!");
                return "";
            }
        }

        private List<string> GetListPhotoId()
        {
            if (ComboboxProductListSelectedValue == null)
            {
                MessageBox.Show("Các trường * không được bỏ trống!");
                return null;
            }

            List<string> listPhotoId = new List<string>
            {
                UploadAPhoto(ComboboxProductListSelectedItem.Image1),
                UploadAPhoto(ComboboxProductListSelectedItem.Image2),
                UploadAPhoto(ComboboxProductListSelectedItem.Image3)
            };
            return listPhotoId;
        }

        private string UploadAPhoto(string photoPath)
        {
            var facebook = new FacebookClient(_PageAccessToken);
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
            var imageId = facebook.Post($"/{FbPageId}/photos", new
            {
                published = "False",
                //scheduled_publish_time = "1530253379",
                file = new FacebookMediaStream { ContentType = "image/png", FileName = "test upload image4" }.SetValue(imageStream)
            }
            );
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
            string fields = $"/feed?message={Description}&published={published}&scheduled_publish_time={scheduledPublishTime}&access_token={_PageAccessToken}";
            for (int i = 0; i < list.Count; i++)
            {
                fields += $"&attached_media[{i}]={{\"media_fbid\":\"{list[i]}\"}}";
            }
            string newAddress = GraphUrl + FbPageId + fields;
            var httpRequest = new HttpRequest();
            httpRequest.Post(newAddress);
        }

        private void CreateWithAllProducts()
        {
            string published = "False";
            string dateTime = DateTimePickerDate.ToString("MM/dd/yyyy") + " " + DateTimePickerTime.ToString("HH:mm:ss");
            string scheduledPublishTime = ConvertToTimeSpan(dateTime);
            //string fields = $"/feed?message={Description}&published={published}&scheduled_publish_time={scheduledPublishTime}&access_token={_pageAccessToken}";

            for (int i = 0; i < ListProduct.Count; i++)
            {
                string newScheduledPublishTime = (Convert.ToInt32(scheduledPublishTime) + 3600 * 24 * (i + 1)).ToString();
                string fields = $"/feed?message={ListProduct[i].Description}&published={published}&scheduled_publish_time={newScheduledPublishTime}&access_token={_PageAccessToken}";
                List<string> list = new List<string>
                {
                    UploadAPhoto(ListProduct[i].Image1),
                    UploadAPhoto(ListProduct[i].Image2),
                    UploadAPhoto(ListProduct[i].Image3)
                };
                string newFields = fields;
                for (int j = 0; j < list.Count; j++)
                {
                    newFields += $"&attached_media[{j}]={{\"media_fbid\":\"{list[j]}\"}}";
                }
                string newAddress = GraphUrl + FbPageId + newFields;
                var httpRequest = new HttpRequest();
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
                var facebook = new FacebookClient(_PageAccessToken);
                facebook.Delete($"/{postId}");
                MessageBox.Show("xong");
            }
            catch (Exception e)
            {
                MessageBox.Show("loi" + e);
            }
        }

        private void LoadSchedulePost()
        {
            try
            {
                string newAddress = $"{FbPageId}/promotable_posts?is_published=false";
                var facebook = new FacebookClient(_PageAccessToken);
                var json = JsonConvert.DeserializeObject(facebook.Get($"/{newAddress}").ToString());
                var jToken = JToken.FromObject(json);
                ListSchedulePost.Clear();
                foreach (var item in jToken["data"])
                {
                    var post = new Posts
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
            var dateTime = DateTime.Parse(time);
            var dateTimeOffset = new DateTimeOffset(dateTime);
            return dateTimeOffset.ToUnixTimeSeconds().ToString();
        }

        private DateTime UnixTimestampToDateTime(double unixTime)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dateTime = dateTime.AddSeconds(unixTime).ToLocalTime();
            return dateTime;
        }

        #endregion Method
    }
}