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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace My_Weather.Pages
{
    /// <summary>
    /// Логика взаимодействия для PeriodPage.xaml
    /// </summary>
    /// 
    internal sealed class GlList : List<Geolocation.Geo>
    {
    }
    public partial class PeriodPage : Page
    {
        Random rand;

        private string geoKey, localasedContent;
        private int geocount = 0;
        private readonly Singleton.Geoposition gP;
        private readonly Singleton.СLocation dL;
        private SolidColorBrush randomColorBrush;
        private WebResponse response_geo;


        private byte[] GetRandomBytes(int n)
        {
            //  Fill an array of bytes of length "n" with random numbers.
            var randomBytes = new byte[n];
            rand.NextBytes(randomBytes);
            return randomBytes;
        }

        private readonly Duration _openCloseDuration = new Duration(TimeSpan.FromSeconds(0.5));

        public PeriodPage()
        {
            InitializeComponent();

            SetColorTextBox();

            LabelLocalased.Content = localasedContent = "";
            TextBoxAnswer.Visibility = Visibility.Collapsed;

            Classes.Language.NameLanguage = Properties.Resources.Name;

            gP = Singleton.Geoposition.GetInstance();
            dL = Singleton.СLocation.GetInstance();

            //TextBoxAnswer.Text = dL.deviceLocation;
            //TextBoxAnswer.Text += dL.latitude.ToString();

            Period.Measure(new Size(Period.MaxWidth, Period.MaxHeight));
            DoubleAnimation heightAnimation = new DoubleAnimation(0, 600, _openCloseDuration);
            Period.BeginAnimation(HeightProperty, heightAnimation);

            if (gP.useMyLocation)
                MyDeviceLocation();
            else
                GetKeyLocation();
        }

        private void SetColorTextBox()
        {
            rand = new Random();

            byte[] rgb = GetRandomBytes(3);

            //  Create a solid color brush using the three random numbers.
            randomColorBrush = new SolidColorBrush(Color.FromArgb(255, rgb[0], rgb[1], rgb[2]));

            //  Set both the text color and the text box border to the random color.
            TextBoxAnswer.BorderBrush = randomColorBrush;
            TextBoxAnswer.Foreground = randomColorBrush;
        }

        private void MyDeviceLocation()
        {
            PrBarConnect.IsIndeterminate = true;
            PrBarConnect.Visibility = Visibility.Visible;

            DeviceLocation devLoc = new DeviceLocation(dL.latitude, dL.longitude);
            //dl.culture проверяет, изменилась ли культура
            if (dL.culture != Properties.Resources.Name | gP.latitude != devLoc.latitude | gP.longitude != devLoc.longitude)
            {
                dL.culture = Properties.Resources.Name;
                gP.latitude = devLoc.latitude;
                gP.longitude = devLoc.longitude;
                GetKeyLocation();
            }
            else
                DataFromGeoposition();
        }

        private async void GetKeyLocation()
        {
            await Task.Run(() => Delay()); // вызов асинхронной операции для нормальной инициализации в потоке переменной

            string url_geo = $"http://dataservice.accuweather.com/locations/v1/geoposition/search.json?q={gP.latitude},{gP.longitude}&apikey=9pbmpNTkGYJTGy8sKGDxiIy8ADvYjqIl&language={Classes.Language.NameLanguage}&details=false&metric=true";

            WebRequest request_geo = WebRequest.Create(url_geo);
            request_geo.Method = "GET";
            request_geo.ContentType = "application/x-www-urlencoded";

            try
            {
                response_geo = await request_geo.GetResponseAsync();

                string answer_geo = string.Empty;

                using (Stream s_geo = response_geo.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(s_geo))
                    {
                        answer_geo = await reader.ReadToEndAsync();
                    }

                    response_geo.Close();

                    //TextBoxAnswer.Text = answer_geo;
                }

                gP.gp = JsonConvert.DeserializeObject<List<Geolocation.Geo>>(answer_geo);

                DataFromGeoposition();
            }
            catch (WebException e)
            {
                //response_geo.Close();
                geocount++;
                if (geocount < 10)
                    GetKeyLocation();
                else
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
                }
            }
        }
        private void DataFromGeoposition()
        {
            geoKey = gP.gp[0].Key;

            localasedContent = gP.gp[0].LocalizedName + " (" + gP.gp[0].Region.LocalizedName + ", " + gP.gp[0].Country.LocalizedName + ", " + gP.gp[0].AdministrativeArea.LocalizedName + ") "
                + gP.gp[0].AdministrativeArea.CountryID;

            Five_DaysWeather();
        }

        private async void Five_DaysWeather()
        {
            string url = $"http://dataservice.accuweather.com/forecasts/v1/daily/5day/{geoKey}?apikey=9pbmpNTkGYJTGy8sKGDxiIy8ADvYjqIl&language={Classes.Language.NameLanguage}&details=false&metric=true";

            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/x-www-urlencoded";

            try
            {
                WebResponse response = await request.GetResponseAsync();

                string answer = string.Empty;

                PrBarConnect.IsIndeterminate = false;
                PrBarConnect.Visibility = Visibility.Collapsed;

                using (Stream s = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(s))
                    {
                        answer = await reader.ReadToEndAsync();
                    }

                    response.Close();

                    //TextBoxAnswer.Text = answer;

                    FiveDaysWeather.Rootobject five_days = JsonConvert.DeserializeObject<FiveDaysWeather.Rootobject>(answer);

                    LabelDate1.Content = five_days.DailyForecasts[0].Date.ToString();
                    LabelDate2.Content = five_days.DailyForecasts[1].Date.ToString();
                    LabelDate3.Content = five_days.DailyForecasts[2].Date.ToString();
                    LabelDate4.Content = five_days.DailyForecasts[3].Date.ToString();
                    LabelDate5.Content = five_days.DailyForecasts[4].Date.ToString();

                    LabelTmax1.Content = five_days.DailyForecasts[0].Temperature.Maximum.Value;
                    LabelTmax2.Content = five_days.DailyForecasts[1].Temperature.Maximum.Value;
                    LabelTmax3.Content = five_days.DailyForecasts[2].Temperature.Maximum.Value;
                    LabelTmax4.Content = five_days.DailyForecasts[3].Temperature.Maximum.Value;
                    LabelTmax5.Content = five_days.DailyForecasts[4].Temperature.Maximum.Value;

                    LabelTmin1.Content = five_days.DailyForecasts[0].Temperature.Minimum.Value;
                    LabelTmin2.Content = five_days.DailyForecasts[1].Temperature.Minimum.Value;
                    LabelTmin3.Content = five_days.DailyForecasts[2].Temperature.Minimum.Value;
                    LabelTmin4.Content = five_days.DailyForecasts[3].Temperature.Minimum.Value;
                    LabelTmin5.Content = five_days.DailyForecasts[4].Temperature.Minimum.Value;

                    ImageDay1.Source = IconFile.iconSource(five_days.DailyForecasts[0].Day.Icon);
                    ImageDay2.Source = IconFile.iconSource(five_days.DailyForecasts[1].Day.Icon);
                    ImageDay3.Source = IconFile.iconSource(five_days.DailyForecasts[2].Day.Icon);
                    ImageDay4.Source = IconFile.iconSource(five_days.DailyForecasts[3].Day.Icon);
                    ImageDay5.Source = IconFile.iconSource(five_days.DailyForecasts[4].Day.Icon);

                    ImageNight1.Source = IconFile.iconSource(five_days.DailyForecasts[0].Night.Icon);
                    ImageNight2.Source = IconFile.iconSource(five_days.DailyForecasts[1].Night.Icon);
                    ImageNight3.Source = IconFile.iconSource(five_days.DailyForecasts[2].Night.Icon);
                    ImageNight4.Source = IconFile.iconSource(five_days.DailyForecasts[3].Night.Icon);
                    ImageNight5.Source = IconFile.iconSource(five_days.DailyForecasts[4].Night.Icon);

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
            }
        }

        private static void Delay()
        {
            Thread.Sleep(300);
        }

    }
}
