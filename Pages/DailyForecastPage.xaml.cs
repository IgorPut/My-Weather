﻿using System;
using System.Collections.Generic;
using System.Linq;
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

using System.Net.Http;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Device.Location;

using My_Weather.Classes;

namespace My_Weather
{
    /// <summary>
    /// Логика взаимодействия для ForecastPage.xaml
    /// </summary>
    public sealed partial class DailyForecastPage : Page
    {

        Random rand;

        private GeoCoordinateWatcher watcher;
        private Classes.DeviceLocation devLoc = new Classes.DeviceLocation("0", "0");
        private string geoKey;
        private double ImageRefreshWidth, ImageRefreshHeight;
        private double EllipseRefreshWidth, EllipseRefreshHeight;
//        private readonly string nameLanguage = Classes.Language.NameLanguage();



        private byte[] GetRandomBytes(int n)
        {
            //  Fill an array of bytes of length "n" with random numbers.
            var randomBytes = new byte[n];
            rand.NextBytes(randomBytes);
            return randomBytes;
        }

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
            LabelLocalased.Content = ""; 
            LabelIndex.Content = LabelUVIndex.Content = LabelWind.Content = LabelWindValue.Content = LabelWindGust.Content = LabelWindGustValue.Content = "";
            LabelPrecipitationProbability.Content = LabelThunderstormProbability.Content = "";
            LabelPrecipitation.Content = LabelHoursPrecipitation.Content = LabelCloudCover.Content = "";
            Text.Text = ""; LabelErrors.Content = "";
            EllipseRefresh.Visibility= Visibility.Hidden; TextBoxAnswer.Visibility = Visibility.Collapsed;

            MyDeviceLocation();

//            GetKeyLocation();

            //Grid_Loaded_1();

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void SetColorTextBox ()
        {
            rand = new Random();

            byte[] rgb = GetRandomBytes(3);

            //  Create a solid color brush using the three random numbers.
            var randomColorBrush = new SolidColorBrush(Color.FromArgb(255, rgb[0], rgb[1], rgb[2]));

            //  Set both the text color and the text box border to the random color.
            TextBoxAnswer.BorderBrush = randomColorBrush;
            TextBoxAnswer.Foreground = randomColorBrush;

            LabelLocalased.Foreground = randomColorBrush;
            //LabelLocalased.Background = randomColorBrush;
        }

        private void MyDeviceLocation ()
        {
            //Координаты
            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);

            // Use MovementThreshold to ignore noise in the signal.
            watcher.StatusChanged += GeoCoordinateWatcherStatusChanged;

            bool started = watcher.TryStart(false, TimeSpan.FromMilliseconds(200));
            if (!started)
            {
                //LabelErrors.Content = "GeoCoordinateWatcher timed out on start.";
                MyDeviceLocation();
            }
            else
                GetKeyLocation();

        }


        private void GeoCoordinateWatcherStatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            if (e.Status == GeoPositionStatus.Ready)
            {
                var co = watcher.Position.Location;
                devLoc.latitude = co.Latitude.ToString("0.000");
                devLoc.longitude = co.Longitude.ToString("0.000");

                watcher.Stop();

                //LabelLat.Content = latitude + "/" + longitude;

            }
        }

        static void Delay()
        {
            Thread.Sleep(300);
        }

        //Запрос geo
        private async void GetKeyLocation()
        {
            await Task.Run(() => Delay()); // вызов асинхронной операции для нормальной инициализации в потоке переменной

            string url_geo = $"http://dataservice.accuweather.com/locations/v1/geoposition/search.json?q={devLoc.latitude},{devLoc.longitude}&apikey=9pbmpNTkGYJTGy8sKGDxiIy8ADvYjqIl&language={Classes.Language.nameLanguage}";

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

                List<Geolocation.Class1> gL = JsonConvert.DeserializeObject<List<Geolocation.Class1>>(answer_geo);

                try
                {
                    geoKey = gL[0].Key;

                    LabelLocalased.Content = gL[0].LocalizedName + " (" + gL[0].Region.LocalizedName + ", " + gL[0].Country.LocalizedName + ", " + gL[0].AdministrativeArea.LocalizedName + ") " + gL[0].AdministrativeArea.CountryID;

                    ForecastDay();

                    //CurrentWeather();
                }
                catch (ArgumentOutOfRangeException outOfRange)
                {
                    LabelErrors.Content = "Argument " + outOfRange;
                    MyDeviceLocation();
                }
            }
            catch(WebException e)
            {
                LabelErrors.Content = "WebException " + e;
            }
        }

        //        Прогноз на день
        private async void ForecastDay()
        {
            //            String url = $"http://dataservice.accuweather.com/forecasts/v1/daily/1day/{geoKey}?apikey=9pbmpNTkGYJTGy8sKGDxiIy8ADvYjqIl&language=ru-ru&details=true&metric=true";
            string url = $"http://dataservice.accuweather.com/forecasts/v1/daily/1day/{geoKey}?apikey=9pbmpNTkGYJTGy8sKGDxiIy8ADvYjqIl&language={Classes.Language.nameLanguage}&details=true&metric=true";
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

                TextBoxAnswer.Text = answer;

                DailyWeather.Rootobject dW = JsonConvert.DeserializeObject<DailyWeather.Rootobject>(answer);



                //Вывод даты и дня недели
                DateTimeConverting myDateTime = new DateTimeConverting(dW.DailyForecasts[0].EpochDate);
                LabelDT.Content = (myDateTime.dayOfWeek + ", " + myDateTime.dayOfMonth).ToUpper();

                LabelHeadingPage.Content = Properties.Resources.LabelHeadingPageDailyForecast;

                LabelDateTime.Content = myDateTime.dm;

                //string v = "pack://application:,,,/My Weather;component/Images/Icons/" + aW.DailyForecasts[0].Day.iconFile;
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

                LabelRealFeel.Content = Properties.Resources.LabelRealFeel + " " + Convert.ToInt16(dW.DailyForecasts[0].RealFeelTemperature.Maximum.Value) + "°";
                LabelRealFeelShade.Content = Properties.Resources.LabelRealFeelShade + " " + Convert.ToInt16(dW.DailyForecasts[0].RealFeelTemperatureShade.Maximum.Value) + "°";
                LabelRealFeelMin.Content = Properties.Resources.LabelRealFeel + " " + Convert.ToInt16(dW.DailyForecasts[0].RealFeelTemperature.Minimum.Value) + "°";

                LabelShortPhrase.Content = dW.DailyForecasts[0].Day.ShortPhrase;

                LabelIndex.Content = Properties.Resources.LabelUVIndex;
                LabelUVIndex.Content = dW.DailyForecasts[0].AirAndPollen[5].Value + " "
                    + Classes.UvIndex.UV_Category(dW.DailyForecasts[0].AirAndPollen[5].CategoryValue, dW.DailyForecasts[0].AirAndPollen[5].Category);

                LabelWind.Content = Properties.Resources.LabelWind;
                LabelWindValue.Content = Classes.WindDirection.Wind_Direction(dW.DailyForecasts[0].Day.Wind.Direction.Degrees, dW.DailyForecasts[0].Day.Wind.Direction.Localized) + " " + Convert.ToInt16(dW.DailyForecasts[0].Day.Wind.Speed.Value) + " " +
                    Classes.UnitTypes.UnitName(dW.DailyForecasts[0].Day.Wind.Speed.UnitType, dW.DailyForecasts[0].Day.Wind.Speed.Unit);

                LabelWindGust.Content = Properties.Resources.LabelWindGust;
                LabelWindGustValue.Content = Convert.ToInt16(dW.DailyForecasts[0].Day.WindGust.Speed.Value) + " " + 
                    Classes.UnitTypes.UnitName(dW.DailyForecasts[0].Day.WindGust.Speed.UnitType, dW.DailyForecasts[0].Day.WindGust.Speed.Unit);

                LabelPrecipitationProbability.Content = Properties.Resources.LabelPrecipitationProbability;
                LabelPrecipitationProbabilityVal.Content = dW.DailyForecasts[0].Day.PrecipitationProbability + " %";

                LabelThunderstormProbability.Content = Properties.Resources.LabelThunderstormProbability;
                LabelThunderstormProbabilityVal.Content = dW.DailyForecasts[0].Day.ThunderstormProbability + " %";

                string liquidKind = "";
                if (dW.DailyForecasts[0].Day.TotalLiquid.Value > 0)
                {
                    liquidKind = "(" + Liquid.LiquidKind(dW.DailyForecasts[0].Day.Rain.Value, dW.DailyForecasts[0].Day.Snow.Value, dW.DailyForecasts[0].Day.Ice.Value) + ")";
                }
                LabelPrecipitation.Content = Properties.Resources.LabelPrecipitation + " " + liquidKind;
                LabelTotalPrecipitationVal.Content = Convert.ToInt16(dW.DailyForecasts[0].Day.TotalLiquid.Value) + " " +
                    Classes.UnitTypes.UnitName(dW.DailyForecasts[0].Day.TotalLiquid.UnitType, dW.DailyForecasts[0].Day.TotalLiquid.Unit);

                LabelHoursPrecipitation.Content = Properties.Resources.LabelHoursPrecipitation;
                LabelHoursPrecipitationVal.Content = dW.DailyForecasts[0].Day.HoursOfPrecipitation + UnitTypes.Hour();

                LabelCloudCover.Content = Properties.Resources.LabelCloudCover;
                LabelCloudCoverValue.Content = dW.DailyForecasts[0].Day.CloudCover + " %";

                Text.Text = dW.Headline.Text;
            }
        }

        //Текущая погода        
        //private async void CurrentWeather()
        //{
        //    LabelHeading.Content = "ТЕКУЩАЯ ПОГОДА";

        //    String url = $"http://dataservice.accuweather.com/currentconditions/v1/{geoKey}?apikey=9pbmpNTkGYJTGy8sKGDxiIy8ADvYjqIl&language=ru-ru&details=true";
        //    //String url = $"http://dataservice.accuweather.com/currentconditions/v1/{geoKey}?apikey=9pbmpNTkGYJTGy8sKGDxiIy8ADvYjqIl&details=true";

        //    WebRequest request = WebRequest.Create(url);
        //    request.Method = "GET";
        //    request.ContentType = "application/x-www-urlencoded";

        //    WebResponse response = await request.GetResponseAsync();

        //    string answer = string.Empty;

        //    using (Stream s = response.GetResponseStream())
        //    {
        //        using (StreamReader reader = new StreamReader(s))
        //        {
        //            answer = await reader.ReadToEndAsync();
        //        }

        //        response.Close();

        //        //TextBoxAnswer.Text = answer;

        //        List<CurrentWeather.Class1> cW = JsonConvert.DeserializeObject<List<CurrentWeather.Class1>>(answer);

        //        //Вывод даты и дня недели
        //        //LabelDT.Content = (cW[0].dt.ToString("dddd") + ", " + cW[0].dt.ToString("M")).ToUpper();

        //        //LabelDateTime.Content = cW[0].timeOfDay;

        //        string v = "pack://application:,,,/My Weather;component/Images/Icons/" + cW[0].iconFile;
        //        Uri uri = new Uri(v, UriKind.Absolute);
        //        try
        //        {
        //            ImageSource imgSource = new BitmapImage(uri);
        //            ImageIcon.Source = imgSource;
        //        }
        //        catch
        //        {

        //        }

        //        LabelTemp.Content = cW[0].Temperature.Metric.value;
        //        LabelTempAdd.Content = cW[0].Temperature.Metric.Unit;

        //        LabelRealFeel.Content = "RealFeel® " + cW[0].RealFeelTemperature.Metric.value;
        //        LabelRealFeelShade.Content = "RealFeel Shade™ " + cW[0].RealFeelTemperatureShade.Metric.value;

        //        LabelShortPhrase.Content = cW[0].WeatherText;

        //        LabelUVIndex.Content = cW[0].UVIndexText + " " + cW[0].UVIndex;
        //        LabelWind.Content = Classes.WindSpeed.Power(cW[0].Wind.Speed.Metric.Value) + " " + cW[0].Wind.Direction.Localized + " " + Convert.ToInt16(cW[0].Wind.Speed.Metric.Value) + " " + Classes.WindSpeed.WindSpeedRu(cW[0].Wind.Speed.Metric.UnitType, cW[0].Wind.Speed.Metric.Unit);
        //        LabelWindGust.Content = Classes.WindSpeed.Power(cW[0].WindGust.Speed.Metric.Value) + " " + Convert.ToInt16(cW[0].WindGust.Speed.Metric.Value) + " " + Classes.WindSpeed.WindSpeedRu(cW[0].WindGust.Speed.Metric.UnitType, cW[0].WindGust.Speed.Metric.Unit);
        //        LabelHumidity.Content = cW[0].RelativeHumidity + " %";
        //        LabelDewPoint.Content = Convert.ToInt16(cW[0].DewPoint.Metric.Value) + "°" + cW[0].DewPoint.Metric.unit;

        //        LabelPressure.Content = Classes.Pressure.Tendency(cW[0].PressureTendency.Code) + " " + Classes.Pressure.PressureUnitRu(cW[0].Pressure.Metric.UnitType, cW[0].Pressure.Metric.Unit, cW[0].Pressure.Metric.Value);
        //        LabelCloudCover.Content = cW[0].CloudCover + " %";
        //        LabelVisibility.Content = Convert.ToInt16(cW[0].Visibility.Metric.Value) + " " + Classes.Distance.DistanceRu(cW[0].Visibility.Metric.Unit, cW[0].Visibility.Metric.UnitType);
        //        LabelCeiling.Content = Convert.ToInt16(cW[0].Ceiling.Metric.Value / 100) * 100 + " " + Classes.Distance.DistanceRu(cW[0].Ceiling.Metric.Unit, cW[0].Ceiling.Metric.UnitType);

        //    }

        //}

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
