using My_Weather.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        private readonly Singleton.Geoposition gP;
        public List<SearchCity.City> cL;

        private System.Windows.Forms.BindingSource citiesBindingSource;
        private DataSetCities citiesDataSet;
        private DataSetCitiesTableAdapters.CitiesTableAdapter citiesTableAdapter =
            new DataSetCitiesTableAdapters.CitiesTableAdapter();

        public LocationPage()
        {
            gP = Singleton.Geoposition.GetInstance();

            InitializeComponent();

            Seach_result.Content = "";
            Classes.Language.NameLanguage = Properties.Resources.Name;

            //SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FavoriteCities;Integrated Security=True;Pooling=False");
            //connection.Open();
            //SqlCommand addCity = new SqlCommand("insert into Cities (Name, Key) values" +
            //    "(@Name, @Key)", connection);
            //var name = "Slutsk";
            //var key = 29706;
            //addCity.Parameters.AddWithValue("@Name", name);
            //addCity.Parameters.AddWithValue("@Key", key);
            //addCity.ExecuteNonQuery();

            // Create a DataSet for the Cities data.
            citiesDataSet = new DataSetCities();
            citiesDataSet.DataSetName = "citiesDataSet";

            // Create a BindingSource for the Customers data.
            citiesBindingSource = new System.Windows.Forms.BindingSource();
            citiesBindingSource.DataMember = "Cities";
            citiesBindingSource.DataSource = citiesDataSet;

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

        private void Button_SetAsCuurrent_Click(object sender, RoutedEventArgs e)
        {
            //gP = Singleton.Geoposition.GetInstance();
            gP.useMyLocation = false;
            gP.gp[0].GeoPosition.Latitude = cL[0].GeoPosition.Latitude;
            gP.gp[0].GeoPosition.Longitude = cL[0].GeoPosition.Longitude;
            gP.latitude = cL[0].GeoPosition.Latitude.ToString("F3");
            gP.longitude = cL[0].GeoPosition.Longitude.ToString("F3");

            Seach_result.Content += " is set as the current location";
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Fill the Cities table adapter with data.
            citiesTableAdapter.ClearBeforeFill = true;
            citiesTableAdapter.Fill(citiesDataSet.Cities);

            // Assign the BindingSource to
            // the data context of the main grid.
            citiesGrid.DataContext = citiesBindingSource;

            // Assign the BindingSource to
            // the data source of the list box.
            listBox1.ItemsSource = citiesBindingSource;

            // Handle the currency management aspect of the data models.
            // Attach an event handler to detect when the current item
            // changes via the WPF ListBox. This event handler synchronizes
            // the list collection with the BindingSource.
            //

            BindingListCollectionView cv = CollectionViewSource.GetDefaultView(
                citiesBindingSource) as BindingListCollectionView;

            cv.CurrentChanged += new EventHandler(WPF_CurrentChanged);
        }

        // This event handler updates the current item
        // of the data binding.
        void WPF_CurrentChanged(object sender, EventArgs e)
        {
            BindingListCollectionView cv = sender as BindingListCollectionView;
            citiesBindingSource.Position = cv.CurrentPosition;
        }

        private void Button_AddToFavirite_Click(object sender, RoutedEventArgs e)
        {
            citiesTableAdapter.Insert(cL[0].LocalizedName, cL[0].Country.LocalizedName, cL[0].Region.LocalizedName, cL[0].Key);
            Page_Loaded(sender, e);
        }

        private async void GetCity(string searchCity)
        {
            await Task.Run(() => Delay()); // вызов асинхронной операции для нормальной инициализации в потоке переменной

            string translatedCity = "";
            //string myText = searchCity;

            Http http = new Http();

            if (Properties.Resources.Name != "en-US")
            {

                await http.Translate(searchCity, "en");

                using (http.response)
                {
                    string body = await http.response.Content.ReadAsStringAsync();
                    TranslateAPI.Rootobject translateText = JsonConvert.DeserializeObject<TranslateAPI.Rootobject>(body);
                    searchCity = translateText.translated_text.en;
                }
            }

            //string url_city = $"http://dataservice.accuweather.com/locations/v1/cities/search?apikey=9pbmpNTkGYJTGy8sKGDxiIy8ADvYjqIl&q={searchCity}&language={Classes.Language.NameLanguage}&details=false";
            string url_city = $"http://dataservice.accuweather.com/locations/v1/cities/search?apikey=9pbmpNTkGYJTGy8sKGDxiIy8ADvYjqIl&q={searchCity}&language=en-us&details=false";

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
                        cL = JsonConvert.DeserializeObject<List<SearchCity.City>>(answer_city);
                        if (Properties.Resources.Name != "en-US")
                        {
                            translatedCity = string.Join("|", new string[] { cL[0].LocalizedName, cL[0].Country.LocalizedName });

                            //    Http http = new Http();
                            switch (Properties.Resources.Name)
                            {
                                case "be-BE":
                                    await http.Translate(translatedCity, "be", "en");

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
                                    await http.Translate(translatedCity, "ru", "en");

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
