using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
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
    /// 
    internal sealed class GlList : List<Geolocation.Geo>
    {
    }

    public sealed partial class NightForecastPage : Page
    {

        Random rand;

        //private readonly DeviceLocation devLoc = new DeviceLocation(0, 0);
        private string geoKey, localasedContent;
        private double ImageRefreshWidth, ImageRefreshHeight;
        private double EllipseRefreshWidth, EllipseRefreshHeight;
        private int geocount = 0;
        private readonly Singleton.Geoposition gP;
        private readonly Singleton.СLocation dL;
        private bool refresh;
        private SolidColorBrush randomColorBrush;

        private byte[] GetRandomBytes(int n)
        {
            //  Fill an array of bytes of length "n" with random numbers.
            var randomBytes = new byte[n];
            rand.NextBytes(randomBytes);
            return randomBytes;
        }

        private readonly Duration _openCloseDuration = new Duration(TimeSpan.FromSeconds(0.5));

        public NightForecastPage()
        {
            //Loaded += Page_Loaded;

            InitializeComponent();

            SetColorTextBox();

            LabelDT.Content = LabelDateTime.Content = "";
            LabelTempAdd_Copy.Content = "";
            LabelTempMin.Content = ""; LabelTempMinAdd.Content = "";
            //LabelRealFeelMin.Content = "";
            LabelLocalased.Content = localasedContent = "";
            LabelWindValue.Content = LabelWindGustValue.Content = "";
            Text.Text = ""; LabelErrors.Content = "";
            EllipseRefresh.Visibility = Visibility.Hidden; TextBoxAnswer.Visibility = Visibility.Collapsed;

            Classes.Language.NameLanguage = Properties.Resources.Name;

            gP = Singleton.Geoposition.GetInstance();
            dL = Singleton.СLocation.GetInstance();

            //TextBoxAnswer.Text = dL.deviceLocation;
            //TextBoxAnswer.Text += dL.latitude.ToString();

            Night.Measure(new Size(Night.MaxWidth, Night.MaxHeight));
            DoubleAnimation heightAnimation = new DoubleAnimation(0, 600, _openCloseDuration);
            Night.BeginAnimation(HeightProperty, heightAnimation);

            MyDeviceLocation();

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

            //LabelLocalased.Foreground = randomColorBrush;
            TbPhrase.Foreground = randomColorBrush;
        }

        private void MyDeviceLocation()
        {
            PrBarConnect.IsIndeterminate = true;
            PrBarConnect.Visibility = Visibility.Visible;
            DeviceLocation devLoc = new DeviceLocation(dL.latitude, dL.longitude);
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

        static void Delay()
        {
            Thread.Sleep(300);
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

                    //TextBoxAnswer.Text = answer_geo; 
                }

                //Первоначальное объявление переменной gL
                //List<Geolocation.Class1> gL = JsonConvert.DeserializeObject<List<Geolocation.Class1>>(answer_geo);

                // Объявление переменной gL по другому
                //GlList gL = JsonConvert.DeserializeObject<GlList>(answer_geo);
                // Класс GlList, производный от List<Geolocation.Class1>, объявлен в начале namespace для упрощения кода (см. первоначальное определение переменной gl)
                //Можно использовать вместо объявления класса директиву
                //using GlList = System.Collections.Generic.List<System.Geolocation.Class1>
                //поместив строку в начале файла. Более корректно. Не пробовал.

                gP.gp = JsonConvert.DeserializeObject<GlList>(answer_geo);
                //gP.gp = JsonConvert.DeserializeObject<List<Geolocation.Geo>>(answer_geo);

                DataFromGeoposition();

            }
            catch (WebException e)
            {
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
            try
            {
                geoKey = gP.gp[0].Key;

                localasedContent = gP.gp[0].LocalizedName + " (" + gP.gp[0].Region.LocalizedName + ", " + gP.gp[0].Country.LocalizedName + ", " + gP.gp[0].AdministrativeArea.LocalizedName + ") "
                    + gP.gp[0].AdministrativeArea.CountryID;

                ForecastDay();
            }
            catch (ArgumentOutOfRangeException outOfRange)
            {
                geocount++;
                if (geocount < 10)
                    GetKeyLocation();
                else
                    LabelErrors.Content = "Argument " + outOfRange;
            }
        }


        //        Прогноз на день
        private async void ForecastDay()
        {
            string url = $"http://dataservice.accuweather.com/forecasts/v1/daily/1day/{geoKey}?apikey=9pbmpNTkGYJTGy8sKGDxiIy8ADvYjqIl&language={Classes.Language.NameLanguage}&details=true&metric=true";
            //LabelErrors.Content = geoKey;
            //Основной запрос
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/x-www-urlencoded";

            WebResponse response = await request.GetResponseAsync();

            string answer = string.Empty;


            using (Stream s = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(s))
                {
                    answer = await reader.ReadToEndAsync();
                }

                response.Close();

                //TextBoxAnswer.Text = answer;

                DailyWeather.Rootobject dW = JsonConvert.DeserializeObject<DailyWeather.Rootobject>(answer);

                //Вывод даты и дня недели
                DateTimeConverting myDateTime = new DateTimeConverting(dW.DailyForecasts[0].EpochDate);
                //LabelDT.Content = (myDateTime.dayOfWeek + ", " + myDateTime.dayOfMonth).ToUpper();
                LabelDT.Content = (myDateTime.dt.ToString("dddd", CultureInfo.CreateSpecificCulture(Properties.Resources.Name)) + ", "
                    + myDateTime.dt.ToString("M", CultureInfo.CreateSpecificCulture(Properties.Resources.Name))).ToUpper();

                LabelDateTime.Content = myDateTime.dm;

                //string v = "pack://application:,,,/My Weather;component/Images/Icons/" + aW.DailyForecasts[0].Day.iconFile;
                string iconFile = "pack://application:,,,/My Weather;component/Images/Icons/" + IconFile.getIconFile(dW.DailyForecasts[0].Night.Icon);
                Uri uri = new Uri(iconFile, UriKind.Absolute);
                try
                {
                    ImageSource imgSource = new BitmapImage(uri);
                    ImageIcon.Source = imgSource;
                }
                catch
                {

                }

                //LabelTempMin.Content = Math.Round(dW.DailyForecasts[0].Temperature.Minimum.Value) + "°";
                LabelTempMin.Content = string.Format("{0:0}", dW.DailyForecasts[0].Temperature.Minimum.Value) + "°";
                LabelTempMinAdd.Content = Properties.Resources.LabelTempMin;
                //LabelTempAdd.Content = "C"; LabelTempAdd_Copy.Content = "C";

                //LabelRealFeelMin.Content = Properties.Resources.RealFeel + " " + string.Format("{0:0}", dW.DailyForecasts[0].RealFeelTemperature.Minimum.Value) + "°";
                TbRealFeel.Text = Properties.Resources.RealFeel + " " + string.Format("{0:0}", dW.DailyForecasts[0].RealFeelTemperature.Minimum.Value) + "° ";
                


                //LabelShortPhrase.Content = dW.DailyForecasts[0].Night.ShortPhrase;                //Текст рисунка
                //LabelPhrase.Content = dW.DailyForecasts[0].RealFeelTemperature.Minimum.Phrase;  //Текст ощущений

                LabelWindValue.Content = Classes.WindDirection.Wind_Direction(dW.DailyForecasts[0].Night.Wind.Direction.Degrees, dW.DailyForecasts[0].Night.Wind.Direction.Localized) + " " + Convert.ToInt16(dW.DailyForecasts[0].Night.Wind.Speed.Value) + " " +
                    Classes.UnitTypes.UnitName(dW.DailyForecasts[0].Day.Wind.Speed.UnitType, dW.DailyForecasts[0].Day.Wind.Speed.Unit);

                //Порывы ветра
                LabelWindGustValue.Content = Convert.ToInt16(dW.DailyForecasts[0].Night.WindGust.Speed.Value) + " " +
                    UnitTypes.UnitName(dW.DailyForecasts[0].Night.WindGust.Speed.UnitType, dW.DailyForecasts[0].Night.WindGust.Speed.Unit);

                LabelPrecipitationProbabilityVal.Content = dW.DailyForecasts[0].Night.PrecipitationProbability + " %";

                LabelThunderstormProbabilityVal.Content = dW.DailyForecasts[0].Night.ThunderstormProbability + " %";

                string liquidKind = ""/*, liquidVals = ""*/;
                if (dW.DailyForecasts[0].Night.TotalLiquid.Value > 0)
                {
                    Liquid liquid = new Liquid(dW.DailyForecasts[0].Night.Rain.Value, dW.DailyForecasts[0].Night.Rain.Unit, dW.DailyForecasts[0].Night.Snow.Value,
                        dW.DailyForecasts[0].Night.Snow.Unit, dW.DailyForecasts[0].Night.Ice.Value, dW.DailyForecasts[0].Night.Ice.Unit);
                    liquidKind = "(" + string.Join("/", liquid.liquidNames) + ")";
                    //liquidVals = "(" + string.Join("/", liquid.liquidVals) + ")";
                }
                LabelPrecipitation.Content = Properties.Resources.LabelPrecipitation + " " + liquidKind;
                LabelTotalPrecipitationVal.Content = dW.DailyForecasts[0].Night.TotalLiquid.Value + " " +
                    UnitTypes.UnitName(dW.DailyForecasts[0].Night.TotalLiquid.UnitType, dW.DailyForecasts[0].Night.TotalLiquid.Unit);

                LabelHoursPrecipitationVal.Content = dW.DailyForecasts[0].Night.HoursOfPrecipitation;

                LabelCloudCover.Content = Properties.Resources.LabelCloudCover;
                LabelCloudCoverValue.Content = dW.DailyForecasts[0].Night.CloudCover + " %";

                //Text.Text = dW.Headline.Text;

                //Online translate RapidAPI NLM
                if (Properties.Resources.Name == "be-BE")
                {
                    string myText = string.Join("|", new string[] { localasedContent, dW.DailyForecasts[0].RealFeelTemperature.Minimum.Phrase, dW.DailyForecasts[0].Night.LongPhrase, dW.Headline.Text });

                    Http http = new Http();
                    await http.Translate(myText, "be", "en");

                    using (http.response)
                    {
                        string body = await http.response.Content.ReadAsStringAsync();
                        TranslateAPI.Rootobject translateText = JsonConvert.DeserializeObject<TranslateAPI.Rootobject>(body);
                        string[] phrases = translateText.translated_text.be.Split('|');
                        LabelLocalased.Content = phrases[0];
                        TbRealFeel.Inlines.Add(new Run(phrases[1]) { Foreground = randomColorBrush });
                        TbPhrase.Text = phrases[2];
                        Text.Text = phrases[3];
                    }
                }
                else
                {
                    //LabelShortPhrase.Content = dW.DailyForecasts[0].Night.ShortPhrase;
                    LabelLocalased.Content = localasedContent;
                    TbRealFeel.Inlines.Add(new Run(dW.DailyForecasts[0].RealFeelTemperature.Minimum.Phrase) { Foreground = randomColorBrush });
                    TbPhrase.Text = dW.DailyForecasts[0].Night.LongPhrase;
                    Text.Text = dW.Headline.Text;
                }
            }
            refresh = true;
            ImageRefresh.IsEnabled = IsEnabled;
            PrBarConnect.IsIndeterminate = false;
            PrBarConnect.Visibility = Visibility.Collapsed;
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ImageRefresh.Width = ImageRefreshWidth;
            ImageRefresh.Height = ImageRefreshHeight;
            EllipseRefresh.Width = EllipseRefreshWidth;
            EllipseRefresh.Height = EllipseRefreshHeight;

            if (refresh == false)
                ImageRefresh.IsEnabled = false;
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
            
            MyDeviceLocation();
        }
    }
}
