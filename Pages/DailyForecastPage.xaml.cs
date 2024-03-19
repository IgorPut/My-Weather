using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Device.Location;

using My_Weather.Classes;
using System.Globalization;
using System.Windows.Media.Animation;

namespace My_Weather
{
    /// <summary>
    /// Логика взаимодействия для ForecastPage.xaml
    /// </summary>
    public sealed partial class DailyForecastPage : Page
    {

        Random rand;

        private GeoCoordinateWatcher watcher;
        //private readonly DeviceLocation devLoc = new DeviceLocation(0,0);
        private string geoKey, localasedContent;
        private double ImageRefreshWidth, ImageRefreshHeight;
        private double EllipseRefreshWidth, EllipseRefreshHeight;
        private int geocount = 0;
        private SolidColorBrush randomColorBrush;
        private readonly Singleton.Geoposition gP;
        //private static int Counter = 0;


        private byte[] GetRandomBytes(int n)
        {
            //  Fill an array of bytes of length "n" with random numbers.
            var randomBytes = new byte[n];
            rand.NextBytes(randomBytes);
            return randomBytes;
        }

        private readonly Duration _openCloseDuration = new Duration(TimeSpan.FromSeconds(0.5));

        public DailyForecastPage()
        {
            //Loaded += Page_Loaded;

            InitializeComponent();

            SetColorTextBox();

            LabelDT.Content = LabelHeadingPage.Content = LabelDateTime.Content = "";
            LabelTempAdd.Content = LabelTempAdd_Copy.Content = "";
            LabelTempMax.Content = ""; LabelTempMaxAdd.Content = "";
            LabelTempMin.Content = ""; LabelTempMinAdd.Content = "";
            LabelRealFeel.Content = LabelRealFeelShade.Content = LabelRealFeelMin.Content = "";
            LabelLocalased.Content = localasedContent = "";
            LabelIndex.Content = LabelUVIndex.Content = LabelWind.Content = LabelWindValue.Content = LabelWindGust.Content = LabelWindGustValue.Content = "";
            LabelPrecipitationProbability.Content = LabelThunderstormProbability.Content = "";
            LabelPrecipitation.Content = LabelHoursPrecipitation.Content = LabelCloudCover.Content = "";
            Text.Text = AirQuality.Text = ""; LabelErrors.Content = "";
            EllipseRefresh.Visibility = Visibility.Hidden; TextBoxAnswer.Visibility = Visibility.Collapsed;

            Classes.Language.NameLanguage = Properties.Resources.Name;

            gP = Singleton.Geoposition.GetInstance();

            Daily.Measure(new Size(Daily.MaxWidth, Daily.MaxHeight));
            DoubleAnimation heightAnimation = new DoubleAnimation(0, 540, _openCloseDuration);
            Daily.BeginAnimation(HeightProperty, heightAnimation);

            PrBarConnect.IsIndeterminate = true;
            PrBarConnect.Visibility = Visibility.Visible;
            if (gP.useMyLocation)
                MyDeviceLocation();
            else
                GetKeyLocation();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

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
            //TextBoxAnswer.Text = rgb[0].ToString() + " " + rgb[1].ToString() + " " + rgb[2].ToString();
            LabelLocalased.Foreground = AirQuality.Foreground = LabelShortPhrase.Foreground = LabelPhrase.Foreground = Text.Foreground = randomColorBrush;
        }

        private void MyDeviceLocation()
        {
            //Координаты
            //watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default)
            {
                //Default означает «Оптимизировать по мощности, производительности и другим соображениям стоимости»,
                //а GeoPositionAccuracy.High означает «Предоставить максимально точный отчет».

                MovementThreshold = 20 // 20 meters
            };
            //A simple way to think about it is that the lower the MovementThreshold, the more often your app will be pinged with location data.
            //Microsoft’s Location Programming Best Practices for Windows Phone recommends a minimum setting of 20 meters.

            // Use MovementThreshold to ignore noise in the signal.
            watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(GeoCoordinateWatcherStatusChanged);
            watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(GeoCoordinateWatcherPositionChanged);


            bool started = watcher.TryStart(false, TimeSpan.FromMilliseconds(200));
            if (!started)
            {
                //LabelErrors.Content = "GeoCoordinateWatcher timed out on start.";
                //MyDeviceLocation();
            }
            else
            {
                //if (gL.gp == null)
                //{
                //    GetKeyLocation();
                //}
                //else
                //    DataFromGeoposition();
            }
        }


        private void GeoCoordinateWatcherStatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            //if (e.Status == GeoPositionStatus.Disabled)

            //    MessageBox.Show("The location service is currently turned off.");

            //else if (e.Status == GeoPositionStatus.NoData)

            //    MessageBox.Show("No location data is currently available. Try again later.");
            
            //else if (e.Status == GeoPositionStatus.Ready)
            //{

            //    MessageBox.Show("The location service is currently turned on.");

            //}
        }

        private void GeoCoordinateWatcherPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            GeoCoordinate co = watcher.Position.Location;
            DeviceLocation devLoc = new DeviceLocation(co.Latitude, co.Longitude);
            //devLoc.latitude = co.Latitude.ToString("0.000");
            //devLoc.longitude = co.Longitude.ToString("0.000");
            if (gP.latitude != devLoc.latitude | gP.longitude != devLoc.longitude)
            {
                gP.latitude = devLoc.latitude;
                gP.longitude = devLoc.longitude;
                GetKeyLocation();
            }
            else
            {
                DataFromGeoposition();
            }
        }

        static void Delay()
        {
            Thread.Sleep(200);
        }

        //Запрос geo
        private async void GetKeyLocation()
        {
            await Task.Run(() => Delay()); // вызов асинхронной операции для нормальной инициализации в потоке переменной

            string url_geo = $"http://dataservice.accuweather.com/locations/v1/geoposition/search.json?q={gP.latitude},{gP.longitude}&apikey=9pbmpNTkGYJTGy8sKGDxiIy8ADvYjqIl&language={Classes.Language.NameLanguage}";

            WebRequest request_geo = WebRequest.Create(url_geo);
            request_geo.Method = "GET";
            request_geo.ContentType = "application/x-www-urlencoded";

            try
            {

                WebResponse response_geo = await request_geo.GetResponseAsync();

                string answer_geo = string.Empty;

                using (Stream s_geo = response_geo.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(s_geo))
                    {
                        answer_geo = await reader.ReadToEndAsync();
                    }

                    response_geo.Close();
                }

                //Counter++;
                //LabelErrors.Content += Counter.ToString();

                //gL.latitude = devLoc.latitude;
                //gL.longitude = devLoc.longitude;
                //LabelErrors.Content += gL.latitude + "/" + gL.longitude + " ";


                gP.gp = JsonConvert.DeserializeObject<List<Geolocation.Geo>>(answer_geo);

                DataFromGeoposition();
            }

            catch (WebException e)
            {
                geocount++;
                if (geocount < 20)
                {
                    TextBoxAnswer.Visibility = Visibility.Visible;
                    TextBoxAnswer.Text += "geocount=" + geocount;
                    GetKeyLocation();
                }
                else
                    //response_geo.Close();
                    geocount++;
                if (geocount < 10)
                    GetKeyLocation();
                else
                {
                    TextBoxAnswer.Visibility = Visibility.Visible;
                    TextBoxAnswer.Text += "A WebException has been caught. ";

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

            ForecastDay();
        }

        //        Прогноз на день
        private async void ForecastDay()
        {
            await Task.Run(() => Delay()); // вызов асинхронной операции для нормальной инициализации в потоке переменной
            string url = $"http://dataservice.accuweather.com/forecasts/v1/daily/1day/{geoKey}?apikey=9pbmpNTkGYJTGy8sKGDxiIy8ADvYjqIl&language={Classes.Language.NameLanguage}&details=true&metric=true";
            //LabelErrors.Content = geoKey;
            //Основной запрос
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET"; //Метод HTTP. Представляет метод протокола HTTP GET.
            request.ContentType = "application/x-www-urlencoded";

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
            }

            DailyWeather.Rootobject dW = JsonConvert.DeserializeObject<DailyWeather.Rootobject>(answer);

            //Online translate RapidAPI NLM
            if (Properties.Resources.Name == "be-BE")
            {
                string myText = string.Join("|", new string[] { dW.DailyForecasts[0].Day.ShortPhrase, localasedContent, dW.DailyForecasts[0].RealFeelTemperature.Maximum.Phrase, dW.Headline.Text });
                //TextBoxAnswer.Text += myText;

                Http http = new Http();
                await http.Translate(myText, "be", "ru");

                using (http.response)
                {
                    string body = await http.response.Content.ReadAsStringAsync();
                    TranslateAPI.Rootobject translateText = JsonConvert.DeserializeObject<TranslateAPI.Rootobject>(body);
                    string[] phrases = translateText.translated_text.be.Split('|');
                    LabelShortPhrase.Content = phrases[0];
                    LabelLocalased.Content = phrases[1];
                    LabelPhrase.Content = phrases[2];
                    Text.Text = phrases[3];
                }
            }
            else
            {
                LabelShortPhrase.Content = dW.DailyForecasts[0].Day.ShortPhrase;
                LabelLocalased.Content = localasedContent;
                LabelPhrase.Content = dW.DailyForecasts[0].RealFeelTemperature.Maximum.Phrase;
                Text.Text = dW.Headline.Text;
            }

            //Вывод даты и дня недели
            DateTimeConverting myDateTime = new DateTimeConverting(dW.DailyForecasts[0].EpochDate);
            //LabelDT.Content = (myDateTime.dayOfWeek + ", " + myDateTime.dayOfMonth).ToUpper();
            LabelDT.Content = (myDateTime.dt.ToString("dddd", CultureInfo.CreateSpecificCulture(Properties.Resources.Name)) + ", "
                + myDateTime.dt.ToString("M", CultureInfo.CreateSpecificCulture(Properties.Resources.Name))).ToUpper();

            LabelHeadingPage.Content = Properties.Resources.LabelHeadingPageDailyForecast;

            LabelDateTime.Content = myDateTime.dm;

            //Иконки погоды
            string iconFile = "pack://application:,,,/My Weather;component/Images/Icons/" + IconFile.getIconFile(dW.DailyForecasts[0].Day.Icon);
            Uri uri = new Uri(iconFile, UriKind.Absolute);
            try
            {
                ImageSource imgSource = new BitmapImage(uri);
                ImageIcon.Source = imgSource;
            }
            catch
            {

            }

            LabelTempMax.Content = Convert.ToInt16(dW.DailyForecasts[0].Temperature.Maximum.Value) + "°";
            LabelTempMaxAdd.Content = Properties.Resources.LabelTempMax;
            LabelTempMin.Content = Convert.ToInt16(dW.DailyForecasts[0].Temperature.Minimum.Value) + "°";
            LabelTempMinAdd.Content = Properties.Resources.LabelTempMin;
            LabelTempAdd.Content = "C"; LabelTempAdd_Copy.Content = "C";

            LabelRealFeel.Content = Properties.Resources.RealFeel + " " + Convert.ToInt16(dW.DailyForecasts[0].RealFeelTemperature.Maximum.Value) + "°";
            LabelRealFeelShade.Content = Properties.Resources.RealFeelShade + " " + Convert.ToInt16(dW.DailyForecasts[0].RealFeelTemperatureShade.Maximum.Value) + "°";
            LabelRealFeelMin.Content = Properties.Resources.RealFeel + " " + Convert.ToInt16(dW.DailyForecasts[0].RealFeelTemperature.Minimum.Value) + "°";

            //LabelShortPhrase.Content = dW.DailyForecasts[0].Day.ShortPhrase;                //Текст рисунка
            //LabelPhrase.Content = dW.DailyForecasts[0].RealFeelTemperature.Maximum.Phrase;  //Текст ощущений

            LabelIndex.Content = Properties.Resources.LabelUVIndex;
            //LabelUVIndex.Content = dW.DailyForecasts[0].AirAndPollen[5].Value + " "
            //    + AirAndPollen.UV_Category(dW.DailyForecasts[0].AirAndPollen[5].Value, dW.DailyForecasts[0].AirAndPollen[5].Category).Split(new char[] { ' ' })[0];
            LabelUVIndex.Content = dW.DailyForecasts[0].AirAndPollen[5].Value + " "
                + AirAndPollen.UV_Category(dW.DailyForecasts[0].AirAndPollen[5].Value, dW.DailyForecasts[0].AirAndPollen[5].Category);

            LabelWind.Content = Properties.Resources.LabelWind;
            LabelWindValue.Content = WindDirection.Wind_Direction(dW.DailyForecasts[0].Day.Wind.Direction.Degrees, dW.DailyForecasts[0].Day.Wind.Direction.Localized) + " " +
                Convert.ToInt16(dW.DailyForecasts[0].Day.Wind.Speed.Value) + " " + UnitTypes.UnitName(dW.DailyForecasts[0].Day.Wind.Speed.UnitType, dW.DailyForecasts[0].Day.Wind.Speed.Unit);

            //Порывы ветра
            LabelWindGust.Content = Properties.Resources.LabelWindGust;
            LabelWindGustValue.Content = Convert.ToInt16(dW.DailyForecasts[0].Day.WindGust.Speed.Value) + " " +
                UnitTypes.UnitName(dW.DailyForecasts[0].Day.WindGust.Speed.UnitType, dW.DailyForecasts[0].Day.WindGust.Speed.Unit);

            LabelPrecipitationProbability.Content = Properties.Resources.LabelPrecipitationProbability;
            LabelPrecipitationProbabilityVal.Content = dW.DailyForecasts[0].Day.PrecipitationProbability + " %";

            LabelThunderstormProbability.Content = Properties.Resources.LabelThunderstormProbability;
            LabelThunderstormProbabilityVal.Content = dW.DailyForecasts[0].Day.ThunderstormProbability + " %";

            string liquidKind = "";
            if (dW.DailyForecasts[0].Day.TotalLiquid.Value > 0)
            {
                Liquid liquid = new Liquid(dW.DailyForecasts[0].Day.HoursOfRain, dW.DailyForecasts[0].Day.Rain.Unit, dW.DailyForecasts[0].Day.HoursOfSnow,
                    dW.DailyForecasts[0].Day.Snow.Unit, dW.DailyForecasts[0].Day.HoursOfIce, dW.DailyForecasts[0].Day.Ice.Unit);
                liquidKind = "(" + string.Join("/", liquid.liquidNames) + ")";
                //liquidVals = "(" + string.Join("/", liquid.liquidVals) + ")";
                //LabelTotalPrecipitationVal.Content = liquidVals;
            }
            LabelPrecipitation.Content = Properties.Resources.LabelPrecipitation + " " + liquidKind;
            //LabelTotalPrecipitationVal.Content = Convert.ToInt16(dW.DailyForecasts[0].Day.TotalLiquid.Value) + " " +
            //    Classes.UnitTypes.UnitName(dW.DailyForecasts[0].Day.TotalLiquid.UnitType, dW.DailyForecasts[0].Day.TotalLiquid.Unit);
            LabelTotalPrecipitationVal.Content = dW.DailyForecasts[0].Day.TotalLiquid.Value + " " +
                UnitTypes.UnitName(dW.DailyForecasts[0].Day.TotalLiquid.UnitType, dW.DailyForecasts[0].Day.TotalLiquid.Unit);

            LabelHoursPrecipitation.Content = Properties.Resources.LabelHoursPrecipitation;
            LabelHoursPrecipitationVal.Content = dW.DailyForecasts[0].Day.HoursOfPrecipitation;

            LabelCloudCover.Content = Properties.Resources.LabelCloudCover;
            LabelCloudCoverValue.Content = dW.DailyForecasts[0].Day.CloudCover + " %";

            //Text.Text = dW.Headline.Text;

            AirQuality.Text = Properties.Resources.AirQuality + AirAndPollen.AirQuality(dW.DailyForecasts[0].AirAndPollen[0].Category, dW.DailyForecasts[0].AirAndPollen[0].CategoryValue);

        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ImageRefresh.Width = ImageRefreshWidth;
            ImageRefresh.Height = ImageRefreshHeight;
            EllipseRefresh.Width = EllipseRefreshWidth;
            EllipseRefresh.Height = EllipseRefreshHeight;

            MyDeviceLocation();
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            EllipseRefresh.Visibility = Visibility.Visible;
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            EllipseRefresh.Visibility = Visibility.Hidden;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ImageRefreshWidth = ImageRefresh.Width;
            ImageRefreshHeight = ImageRefresh.Height;
            EllipseRefreshWidth = EllipseRefresh.Width;
            EllipseRefreshHeight = EllipseRefresh.Height;

            ImageRefresh.Width = 30;
            ImageRefresh.Height = 30;

            EllipseRefresh.Width = 34;
            EllipseRefresh.Height = 34;
        }
    }
}
