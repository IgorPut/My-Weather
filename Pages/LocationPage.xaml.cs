using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace My_Weather
{
    /// <summary>
    /// Логика взаимодействия для LocationPage.xaml
    /// </summary>
    public partial class LocationPage : Page
    {
        private string searchResult, searchCity;
        WebResponse response_city;
        public LocationPage()
        {
            InitializeComponent();
        }

        private void SearchText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.IsKeyDown(Key.Enter))
            {
                searchCity = SearchText.Text.Trim();
                GetCity(searchCity);
            }
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            searchCity = SearchText.Text.Trim();
            GetCity(searchCity);
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            Seach_result.Content = "";
            SearchText.Text = "";
            SearchText.Focus();

            //Frame frameInMainWindows = (Frame)Application.Current.MainWindow.FindName("Frame1");
            //frameInMainWindows.Background = new SolidColorBrush(Colors.Yellow);

            //Button buttonInMainWindows = (Button)Application.Current.MainWindow.FindName("ButtonLocation");
            //buttonInMainWindows.BorderBrush = new SolidColorBrush(Colors.Yellow);
            //Border borderInTemplate = (Border)buttonInMainWindows.Template.FindName("border", buttonInMainWindows);
            //borderInTemplate.BorderBrush = new SolidColorBrush(Colors.Yellow);

        }

        static void Delay()
        {
            Thread.Sleep(200);
        }

        private async void GetCity(string searchCity)
        {
            await Task.Run(() => Delay()); // вызов асинхронной операции для нормальной инициализации в потоке переменной
            //string url_geo = $"http://dataservice.accuweather.com/locations/v1/cities/search?apikey=9pbmpNTkGYJTGy8sKGDxiIy8ADvYjqIl&q=%D0%A1%D0%BB%D1%83%D1%86%D0%BA&language=ru&details=false";
            string url_city = $"http://dataservice.accuweather.com/locations/v1/cities/search?apikey=9pbmpNTkGYJTGy8sKGDxiIy8ADvYjqIl&q={searchCity}&language=ru&details=false";

            WebRequest request_city = WebRequest.Create(url_city);
            request_city.Method = "GET";
            request_city.ContentType = "application/x-www-urlencoded";

            try
            {
                response_city = await request_city.GetResponseAsync();

                string answer_city = string.Empty;

                using (Stream s_city = response_city.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(s_city))
                    {
                        answer_city = await reader.ReadToEndAsync();
                    }

                    response_city.Close();

                    if (answer_city == "[]")
                        Seach_result.Content = "Не знойдзена";
                    else
                    {
                        //TextBoxAnswer.Text = answer_city;
                        List<SearchCity.City> cL = JsonConvert.DeserializeObject<List<SearchCity.City>>(answer_city);
                        searchResult = cL[0].LocalizedName + " " + cL[0].Country.LocalizedName;
                        Seach_result.Content = searchResult;
                    }
                }


                ////LabelGeoKey.Content = gL[0].Key;
                //try
                //{
                //    geoKey = gL[0].Key;

                //    localasedContent = gL[0].LocalizedName + " (" + gL[0].Region.LocalizedName + ", " + gL[0].Country.LocalizedName + ", " + gL[0].AdministrativeArea.LocalizedName + ") " + gL[0].AdministrativeArea.CountryID;

                //    CurrentWeather();
                //}
                //catch (ArgumentOutOfRangeException outOfRange)
                //{
                //    geocount++;
                //    if (geocount < 10)
                //        GetKeyLocation();
                //    else
                //        TextBoxAnswer.Visibility = Visibility.Visible;
                //    TextBoxAnswer.Text += "Argument " + outOfRange;
                //}
            }
            catch (WebException e)
            {
                //    //response_geo.Close();
                //    geocount++;
                //    if (geocount < 10)
                //        GetKeyLocation();
                //    else
                //    {
                TextBoxAnswer.Visibility = Visibility.Visible;

                // If you reach this point, an exception has been caught.  
                TextBoxAnswer.Text += "A WebException has been caught. ";

                // Write out the WebException message.  
                //TextBoxAnswer.Text += e.ToString();

                // Get the WebException status code.  
                WebExceptionStatus status = e.Status;
                // If status is WebExceptionStatus.ProtocolError,
                //   there has been a protocol error and a WebResponse
                //   should exist. Display the protocol error.  
                if (status == WebExceptionStatus.ProtocolError)
                {
                    TextBoxAnswer.Text += "The server returned protocol error ";
                    // Get HttpWebResponse so that you can check the HTTP status code.  
                    HttpWebResponse httpResponse = (HttpWebResponse)e.Response;
                    TextBoxAnswer.Text += (int)httpResponse.StatusCode + " - " + httpResponse.StatusCode;
                }
                //    }
            }
        }
    }
}
