﻿using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Maps.MapControl.WPF;
using Newtonsoft.Json;

namespace My_Weather
{
    /// <summary>
    /// Логика взаимодействия для MapPage.xaml
    /// </summary>
    //public partial class MapPage : Page
    public sealed partial class MapPage : Page//, INotifyPropertyChanged
    {
        // TODO WTS: Set your preferred default zoom level
        private const double DefaultZoomLevel = 10;

        //        public event PropertyChangedEventHandler PropertyChanged;

        private GeoCoordinateWatcher watcher;
        private readonly MyMapControl mapcontrol = new MyMapControl { ZoomLevel = DefaultZoomLevel };
        private static readonly Location locgeo = new Location(); //static, чтобы не сбрасывались координаты

        private int geocount = 0;

        private string messageBoxText = "";

        public MapPage()
        {
            Classes.Language.nameLanguage = Properties.Resources.Name;

            MyDeviceLocation();

            mapcontrol.Center = locgeo;
            mapcontrol.Marker = locgeo;

            InitializeComponent();

            //mapcontrol.Mode = new RoadMode();
            //myMap.Mode = new RoadMode();
            //mapcontrol.Mode = new AerialMode(true);

            //MapItemsControl myMap = new MapItemsControl();

            //ParentPanel.Children.Add(MyMap);
            DataContext = mapcontrol;
        }

        private void MyDeviceLocation()
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
            //else
                //GetKeyLocation();

        }

        private void GeoCoordinateWatcherStatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            if (e.Status == GeoPositionStatus.Ready)
            {
                GeoCoordinate co = watcher.Position.Location;
                locgeo.Latitude = co.Latitude;
                locgeo.Longitude = co.Longitude;
                //locgeo.Altitude = co.Altitude;

                GetKeyLocation();
                //LabelGeo.Content = co;

                mapcontrol.Latitude = co.Latitude;
                mapcontrol.Longitude = co.Longitude;

                watcher.Stop();

            }
        }

        //Запрос geo
        private async void GetKeyLocation()
        {
            await Task.Run(() => Thread.Sleep(200)); // вызов асинхронной операции для нормальной инициализации в потоке переменной

            string url_geo = $"http://dataservice.accuweather.com/locations/v1/geoposition/search.json?q={locgeo.Latitude},{locgeo.Longitude}&apikey=9pbmpNTkGYJTGy8sKGDxiIy8ADvYjqIl&language={Classes.Language.nameLanguage}";

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

                messageBoxText = 
                    gL[0].LocalizedName + " (" + gL[0].Region.LocalizedName + ", " + gL[0].Country.LocalizedName + ", " + gL[0].AdministrativeArea.LocalizedName + ") " + gL[0].AdministrativeArea.CountryID + "\n" + "\n" +
                    Properties.Resources.Altitude +": " + gL[0].GeoPosition.Elevation.Metric.Value.ToString() + " " + Classes.UnitTypes.UnitName(1, gL[0].GeoPosition.Elevation.Metric.Unit) + "\n" + 
                    Properties.Resources.Latitude + ": " + mapcontrol.Latitude.ToString("F3") + "°" + "\n" +
                    Properties.Resources.Longitude + ": " + mapcontrol.Longitude.ToString("F3") + "°" + "\n";
            }
            catch (WebException e)
            {
                geocount++;
                if (geocount < 10)
                    GetKeyLocation();
                else
                {
                    //TextBoxAnswer.Visibility = Visibility.Visible;

                    // If you reach this point, an exception has been caught.  
                    //TextBoxAnswer.Text += "A WebException has been caught. ";

                    // Write out the WebException message.  
                    //TextBoxAnswer.Text += e.ToString();

                    // Get the WebException status code.  
                    WebExceptionStatus status = e.Status;
                    // If status is WebExceptionStatus.ProtocolError,
                    //   there has been a protocol error and a WebResponse
                    //   should exist. Display the protocol error.  
                    if (status == WebExceptionStatus.ProtocolError)
                    {
                        //TextBoxAnswer.Text += "The server returned protocol error ";
                        // Get HttpWebResponse so that you can check the HTTP status code.  
                        HttpWebResponse httpResponse = (HttpWebResponse)e.Response;
                        //TextBoxAnswer.Text += (int)httpResponse.StatusCode + " - " + httpResponse.StatusCode;
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (myMap.Mode.ToString() == "Microsoft.Maps.MapControl.WPF.RoadMode")
            {
                //Set the map mode to Aerial with labels
                myMap.Mode = new AerialMode(true);
            }


            else if (myMap.Mode.ToString() == "Microsoft.Maps.MapControl.WPF.AerialMode")
            {
                //Set the map mode to RoadMode
                myMap.Mode = new RoadMode();
            }

        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(messageBoxText);
        }
    }

    public class MyMapControl
    {
        private double zoomLevel;

        public double ZoomLevel
        {
            get { return zoomLevel; }

            set
            {
                if (value != zoomLevel)
                {
                    zoomLevel = value;
                }
            }
        }

        private double latitude;

        public double Latitude
        {
            get { return latitude; }

            set
            {
                if (value != latitude)
                {
                    latitude = value;
                }
            }
        }

        private double longitude;

        public double Longitude
        {
            get { return longitude; }

            set
            {
                if (value != longitude)
                {
                    longitude = value;
                }
            }
        }

        private Location center;
        public Location Center
        {
            get { return center; }
            set
            {
                if (value != center)
                {
                    center = value;
                }
            }
        }

        private Location marker;
        public Location Marker
        {
            get { return marker; }
            set
            {
                if (value != marker)
                {
                    marker = value;
                }
            }
        }

        //private MapMode mode;

        //public MapMode Mode
        //{
        //    get { return mode; }
        //    set
        //    {
        //        if (value != mode)
        //        {
        //            mode = value;
        //        }
        //    }
        //}
    }
}
