using My_Weather.Classes;
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

            Seach_result.Content = "";
            Classes.Language.NameLanguage = Properties.Resources.Name;
            //LabelCulture.Content = Classes.Language.NameLanguage;
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

            string q = "";
            string myText = searchCity;

            Http http = new Http();

            await http.Translate(myText, "en");

            using (http.response)
            {
                string body = await http.response.Content.ReadAsStringAsync();
                TranslateAPI.Rootobject translateText = JsonConvert.DeserializeObject<TranslateAPI.Rootobject>(body);
                q = translateText.translated_text.en;
            }

            //string url_city = $"http://dataservice.accuweather.com/locations/v1/cities/search?apikey=9pbmpNTkGYJTGy8sKGDxiIy8ADvYjqIl&q={searchCity}&language={Classes.Language.NameLanguage}&details=false";
            string url_city = $"http://dataservice.accuweather.com/locations/v1/cities/search?apikey=9pbmpNTkGYJTGy8sKGDxiIy8ADvYjqIl&q={q}&language=en-us&details=false";

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
                    { 
                        Seach_result.Content = "Не знойдзена";
                        ControlLocation.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        //TextBoxAnswer.Text = answer_city;
                        List<SearchCity.City> cL = JsonConvert.DeserializeObject<List<SearchCity.City>>(answer_city);
                        if (Properties.Resources.Name != "en-US")
                        {
                            myText = string.Join("|", new string[] { cL[0].LocalizedName, cL[0].Country.LocalizedName });

                            //    Http http = new Http();
                            switch (Properties.Resources.Name)
                            {
                                case "be-BE":
                                    await http.Translate(myText, "be", "en");

                                    using (http.response)
                                    {
                                        string body = await http.response.Content.ReadAsStringAsync();
                                        TranslateAPI.Rootobject translateText = JsonConvert.DeserializeObject<TranslateAPI.Rootobject>(body);
                                        string[] phrases = translateText.translated_text.be.Split('|');
                                        cL[0].LocalizedName = phrases[0];
                                        cL[0].Country.LocalizedName = phrases[1];
                                    }
                                    break;
                                case "ru-RU":
                                    await http.Translate(myText, "ru", "en");

                                    using (http.response)
                                    {
                                        string body = await http.response.Content.ReadAsStringAsync();
                                        TranslateAPI.Rootobject translateText = JsonConvert.DeserializeObject<TranslateAPI.Rootobject>(body);
                                        string[] phrases = translateText.translated_text.ru.Split('|');
                                        cL[0].LocalizedName = phrases[0];
                                        cL[0].Country.LocalizedName = phrases[1];
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }

                        searchResult = cL[0].LocalizedName + ", " + cL[0].Country.LocalizedName;
                        Seach_result.Content = searchResult;
                        ControlLocation.Visibility = Visibility.Visible;
                    }
                }

            }
            catch (WebException e)
            {
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
