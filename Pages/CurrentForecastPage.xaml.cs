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
using System.Globalization;

using My_Weather.Classes;
using System.Windows.Documents;
using System.Net.Http;
using System.Windows.Media.Animation;

namespace My_Weather
{
    /// <summary>
    /// Логика взаимодействия для ForecastPage.xaml
    /// </summary>
    public sealed partial class CurrentForecastPage : Page
    {
        private Random rand;

        private string geoKey, localasedContent;
        private double ImageRefreshWidth, ImageRefreshHeight;
        private double EllipseRefreshWidth, EllipseRefreshHeight;
        private int geocount = 0;
        private WebResponse response_geo;
        private SolidColorBrush randomColorBrush;
        private readonly Singleton.Geoposition gP;
        private readonly Singleton.СLocation dL;
        private bool refresh;

        private byte[] GetRandomBytes(int n)
        {
            //  Fill an array of bytes of length "n" with random numbers.
            var randomBytes = new byte[n];
            rand.NextBytes(randomBytes);
            return randomBytes;
        }

        private readonly Duration _openCloseDuration = new Duration(TimeSpan.FromSeconds(0.5));

        public CurrentForecastPage()
        {

            //System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

            InitializeComponent();

            Loaded += Page_Loaded;

            SetColorTextBox();


            Classes.Language.NameLanguage = Properties.Resources.Name;

            gP = Singleton.Geoposition.GetInstance();
            dL = Singleton.СLocation.GetInstance();

            //TextBoxAnswer.Text = dL.deviceLocation;
            //TextBoxAnswer.Text += dL.latitude.ToString();


            Current.Measure(new Size(Current.MaxWidth, Current.MaxHeight));
            DoubleAnimation heightAnimation = new DoubleAnimation(0, 540, _openCloseDuration);
            Current.BeginAnimation(HeightProperty, heightAnimation);

            if (gP.useMyLocation)
                MyDeviceLocation();
            else
                GetKeyLocation();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LabelDT.Content = LabelDateTime.Content = LabelTemp.Content = LabelTempAdd.Content = "";
            LabelLocalased.Content = /*LabelIndex.Content =*/ LabelUVIndex.Content = LabelWind.Content = InfoMessage.Text = "";
            LabelWindGust.Content = LabelHumidity.Content = LabelDewPoint.Content = LabelPressure.Content = "";
            LabelCloudCover.Content = LabelVisibility.Content = LabelCeiling.Content = "";
            LabelIndoorHumidity.Text = ""; TextBoxAnswer.Text = "";

            EllipseRefresh.Visibility = Visibility.Hidden; TextBoxAnswer.Visibility = Visibility.Collapsed;
        }

        private void SetColorTextBox()
        {
            rand = new Random();

            byte[] rgb = GetRandomBytes(3)/*, inv_rgb = GetValue(0,0,0)*/;
            //inv_rgb[0] = 255 - rgb[0];
            //int c = 255;

            //  Create a solid color brush using the three random numbers.
            randomColorBrush = new SolidColorBrush(Color.FromArgb(255, rgb[0], rgb[1], rgb[2]));
            //inverseColorBrush = new SolidColorBrush(Color.FromArgb(255, (byte)c-rgb[0], rgb[1], rgb[2]));

            //  Set both the text color and the text box border to the random color.
            TextBoxAnswer.BorderBrush = randomColorBrush;
            TextBoxAnswer.Foreground = randomColorBrush;

            LabelShortPhrase.Foreground = InfoMessage.Foreground = LabelLocalased.Foreground = randomColorBrush;
            //LabelLocalased.Background = randomColorBrush;
            //randomColorBrush;
            //InfoMessage.Foreground = randomColorBrush;
        }

        private void MyDeviceLocation()
        {
            PrBarConnect.IsIndeterminate = true;
            PrBarConnect.Visibility = Visibility.Visible; refresh = false;

            DeviceLocation devLoc = new DeviceLocation(dL.latitude, dL.longitude);
            //dl.culture проверяет, изменилась ли культура
            if (dL.culture != Properties.Resources.Name | gP.latitude != devLoc.latitude | gP.longitude != devLoc.longitude)
            //if (gP.latitude != devLoc.latitude | gP.longitude != devLoc.longitude)
            {
                dL.culture = Properties.Resources.Name;
                gP.latitude = devLoc.latitude;
                gP.longitude = devLoc.longitude;
                GetKeyLocation();
            }
            else
                DataFromGeoposition();
        }

        private static void Delay()
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

                //List<Geolocation.Geo> gL = JsonConvert.DeserializeObject<List<Geolocation.Geo>>(answer_geo);

                gP.gp = JsonConvert.DeserializeObject<List<Geolocation.Geo>>(answer_geo);

                DataFromGeoposition();

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
                //        TextBoxAnswer.Text += "Argument " + outOfRange;
                //}
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

            CurrentWeather();
        }


        //Текущая погода        
        private async void CurrentWeather()
        {
            //String url = $"http://dataservice.accuweather.com/currentconditions/v1/{geoKey}?apikey=9pbmpNTkGYJTGy8sKGDxiIy8ADvYjqIl&language=ru-ru&details=true";
            string url = $"http://dataservice.accuweather.com/currentconditions/v1/{geoKey}?apikey=9pbmpNTkGYJTGy8sKGDxiIy8ADvYjqIl&language={Classes.Language.NameLanguage}&details=true";
            //string url = $"http://dataservice.accuweather.com/currentconditions/v1/{geoKey}?apikey=9pbmpNTkGYJTGy8sKGDxiIy8ADvYjqIl&language=en&details=true";

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

                    List<CurrentWeather.Class1> cW = JsonConvert.DeserializeObject<List<CurrentWeather.Class1>>(answer);

                    //Вывод даты и дня недели
                    DateTimeConverting myDateTime = new DateTimeConverting(cW[0].EpochTime);
                    LabelDT.Content = (myDateTime.dt.ToString("dddd", CultureInfo.CreateSpecificCulture(Properties.Resources.Name)) + ", "
                        + myDateTime.dt.ToString("M", CultureInfo.CreateSpecificCulture(Properties.Resources.Name))).ToUpper();

                    LabelDateTime.Content = myDateTime.TimeOfDay();

                    //string v = "pack://application:,,,/My Weather;component/Images/Icons/" + cW[0].IconFile;
                    string iconFile = "pack://application:,,,/My Weather;component/Images/Icons/" + IconFile.getIconFile(cW[0].WeatherIcon);
                    Uri uri = new Uri(iconFile, UriKind.Absolute);
                    try
                    {
                        ImageSource imgSource = new BitmapImage(uri);
                        ImageIcon.Source = imgSource;
                    }
                    catch
                    {

                    }

                    LabelTemp.Content = cW[0].Temperature.Metric.Val;
                    LabelTempAdd.Content = cW[0].Temperature.Metric.Unit;

                    TbRealFeel.Text = Properties.Resources.RealFeel + " " + cW[0].RealFeelTemperature.Metric.Val + " ";

                    TbRealFeelShade.Text = Properties.Resources.RealFeelShade + " " + cW[0].RealFeelTemperatureShade.Metric.Val + " ";

                    //Online translate RapidAPI NLM
                    if (Properties.Resources.Name == "be-BE")
                    {
                        string myText = string.Join("|", new string[] { cW[0].WeatherText, localasedContent, cW[0].RealFeelTemperature.Metric.Phrase, cW[0].RealFeelTemperatureShade.Metric.Phrase });
                        //string myText = string.Join("|", new string[] { cW[0].WeatherText, localasedContent, cW[0].RealFeelTemperature.Metric.Phrase, "It's terribly cold" });
                        Http http = new Http();
                        await http.Translate(myText, "be", "ru");

                        using (http.response)
                        {
                            string body = await http.response.Content.ReadAsStringAsync();
                            TranslateAPI.Rootobject translateText = JsonConvert.DeserializeObject<TranslateAPI.Rootobject>(body);
                            string[] phrases = translateText.translated_text.be.Split('|');
                            //TextBoxAnswer.Visibility = Visibility.Visible;
                            //TextBoxAnswer.Text = phrases[0] + " - " + phrases[1] + " - " + phrases[2] + " - " + phrases[3];
                            LabelShortPhrase.Content = phrases[0];
                            LabelLocalased.Content = phrases[1];
                            if (phrases[2] == "Bitterly Cold")
                                TbRealFeel.Inlines.Add(new Run("Жудасна холадна") { Foreground = randomColorBrush });
                            else
                                TbRealFeel.Inlines.Add(new Run(phrases[2]) { Foreground = randomColorBrush });
                            if (3 < phrases.Length) // иногда переводчик возвращает без этого индекса
                                if (phrases[3] == "Bitterly Cold")
                                    TbRealFeelShade.Inlines.Add(new Run("Жудасна холадна") { Foreground = randomColorBrush });
                                else
                                    TbRealFeelShade.Inlines.Add(new Run(phrases[2]) { Foreground = randomColorBrush });
                        }
                    }
                    else
                    {
                        LabelShortPhrase.Content = cW[0].WeatherText;
                        LabelLocalased.Content = localasedContent;
                        TbRealFeel.Inlines.Add(new Run(cW[0].RealFeelTemperature.Metric.Phrase) { Foreground = randomColorBrush });
                        TbRealFeelShade.Inlines.Add(new Run(cW[0].RealFeelTemperatureShade.Metric.Phrase) { Foreground = randomColorBrush });
                    }

                    LabelIndex.Content = Properties.Resources.LabelUVIndex;
                    LabelUVIndex.Content = AirAndPollen.UV_Category(cW[0].UVIndex, cW[0].UVIndexText) + " " + cW[0].UVIndex;

                    LabelWind.Content = Properties.Resources.LabelWind;
                    LabelWindValue.Content = WindSpeed.Power(cW[0].Wind.Speed.Metric.Value) + " " + WindDirection.Wind_Direction(cW[0].Wind.Direction.Degrees, cW[0].Wind.Direction.Localized) + " " +
                        Convert.ToInt16(cW[0].Wind.Speed.Metric.Value) + " " + UnitTypes.UnitName(cW[0].Wind.Speed.Metric.UnitType, cW[0].Wind.Speed.Metric.Unit);
                    LabelWindGust.Content = Properties.Resources.LabelWindGust;
                    LabelWindGustValue.Content = WindSpeed.Power(cW[0].WindGust.Speed.Metric.Value) + " " + Convert.ToInt16(cW[0].WindGust.Speed.Metric.Value) + " " +
                        UnitTypes.UnitName(cW[0].WindGust.Speed.Metric.UnitType, cW[0].WindGust.Speed.Metric.Unit);

                    LabelHumidity.Content = Properties.Resources.LabelHumidity;
                    LabelHumidityValue.Content = cW[0].RelativeHumidity + " %";

                    //Точка росы
                    LabelDewPoint.Content = Properties.Resources.LabelDewPoint;
                    LabelDewPointValue.Content = Convert.ToInt16(cW[0].DewPoint.Metric.Value) + UnitTypes.UnitName(cW[0].DewPoint.Metric.UnitType, cW[0].DewPoint.Metric.Unit);

                    LabelPressure.Content = Properties.Resources.LabelPressure;
                    LabelPressureValue.Content = Classes.Pressure.Tendency(cW[0].PressureTendency.Code) + " " +
                        Pressure.PressureUnitRu(cW[0].Pressure.Metric.UnitType, cW[0].Pressure.Metric.Unit, cW[0].Pressure.Metric.Value);

                    LabelCloudCover.Content = Properties.Resources.LabelCloudCover;
                    LabelCloudCoverValue.Content = cW[0].CloudCover + " %";

                    LabelVisibility.Content = Properties.Resources.LabelVisibility + " " + cW[0].ObstructionsToVisibility;
                    //LabelVisibilityValue.Content = Convert.ToInt16(cW[0].Visibility.Metric.Value) + " " + Classes.Distance.DistanceRu(cW[0].Visibility.Metric.Unit, cW[0].Visibility.Metric.UnitType);
                    LabelVisibilityValue.Content = cW[0].Visibility.Metric.Value + " " + Classes.Distance.DistanceRu(cW[0].Visibility.Metric.Unit, cW[0].Visibility.Metric.UnitType);

                    LabelCeiling.Content = Properties.Resources.LabelCeiling;
                    LabelCeilingValue.Content = Convert.ToInt16(cW[0].Ceiling.Metric.Value / 100) * 100 + " " + Classes.Distance.DistanceRu(cW[0].Ceiling.Metric.Unit, cW[0].Ceiling.Metric.UnitType);

                    LabelIndoorHumidity.Text = Properties.Resources.LabelIndoorHumidity;
                    LabelIndoorHumidityValue.Content = cW[0].IndoorRelativeHumidity + "% " + "(" + IndoorHumidity.GetPhrase(cW[0].IndoorRelativeHumidity) + ")";

                    List<int> numbers = new List<int>() { 6, 24 };
                    int pastHours = numbers.PickRandom();

                    switch (pastHours)
                    {
                        case 6:
                            InfoMessage.Text = LastTemp.LastTempText(pastHours) + "\n" + cW[0].TemperatureSummary.Past6HourRange.Minimum.Metric.Value + " <=> " +
                                cW[0].TemperatureSummary.Past6HourRange.Maximum.Metric.Value + " " +
                                UnitTypes.UnitName(cW[0].TemperatureSummary.Past6HourRange.Maximum.Metric.UnitType, cW[0].TemperatureSummary.Past6HourRange.Maximum.Metric.Unit);
                            break;
                        case 24:
                            InfoMessage.Text = LastTemp.LastTempText(pastHours) + "\n" + cW[0].TemperatureSummary.Past24HourRange.Minimum.Metric.Value + " <=> " +
                                cW[0].TemperatureSummary.Past24HourRange.Maximum.Metric.Value + " " +
                                UnitTypes.UnitName(cW[0].TemperatureSummary.Past24HourRange.Maximum.Metric.UnitType, cW[0].TemperatureSummary.Past24HourRange.Maximum.Metric.Unit);
                            break;
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
            }
            refresh = true;
            ImageRefresh.IsEnabled = IsEnabled;

        }

        private void ImageRefresh_MouseUp(object sender, MouseButtonEventArgs e)
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

        //Нажатие на кнопку Обновить
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
