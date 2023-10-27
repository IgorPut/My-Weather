using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Maps.MapControl.WPF;
using My_Weather.Classes;
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
        private Location pinLocation;

        private string messageBoxText = "";

        private Singleton.Geoposition gP;

        public MapPage()
        {
            Classes.Language.NameLanguage = Properties.Resources.Name;

            gP = Singleton.Geoposition.GetInstance();

            MyDeviceLocation();

            mapcontrol.Center = locgeo;
            mapcontrol.Marker = locgeo;
            mapcontrol.Mode = Properties.Settings.Default.MapMode == "ROAD" ? new RoadMode() : (MapMode)new AerialMode(true);

            InitializeComponent();

            DataContext = mapcontrol;
        }

        private void MyDeviceLocation()
        {
            //Координаты
            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High)
            {
                // Use MovementThreshold to ignore noise in the signal.
                MovementThreshold = 20 // 20 meters
            };

            //watcher.StatusChanged += GeoCoordinateWatcherStatusChanged;
            watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(GeoCoordinateWatcherPositionChanged);


            bool started = watcher.TryStart(false, TimeSpan.FromMilliseconds(200));
            if (!started)
            {
                //LabelErrors.Content = "GeoCoordinateWatcher timed out on start.";
                //MyDeviceLocation();
            }
            //else
                //GetKeyLocation();

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
            locgeo.Latitude = co.Latitude;
            locgeo.Longitude = co.Longitude;
            mapcontrol.Latitude = co.Latitude;
            mapcontrol.Longitude = co.Longitude;

            if (gP.latitude != devLoc.latitude | gP.longitude != devLoc.longitude)
            {
                gP.latitude = devLoc.latitude;
                gP.longitude = devLoc.longitude;
                GetKeyLocation();
                //LabelGeo.Content = "GetKeyLocation";
            }
            else
            {
                DataFromGeoposition();
                //LabelGeo.Content = "Else";
            }
            //watcher.Stop();
        }

        private void DataFromGeoposition()
        {
            // Текст, отображаемый при нажатии на Info
            messageBoxText =
                gP.gp[0].LocalizedName + " (" + gP.gp[0].Region.LocalizedName + ", " + gP.gp[0].Country.LocalizedName + ", " + gP.gp[0].AdministrativeArea.LocalizedName + ") "
                + gP.gp[0].AdministrativeArea.CountryID + "\n" + "\n" +
                Properties.Resources.Altitude + ": " + gP.gp[0].GeoPosition.Elevation.Metric.Value.ToString() + " " + Classes.UnitTypes.UnitName(1, gP.gp[0].GeoPosition.Elevation.Metric.Unit) + "\n"
                //+ Properties.Resources.Latitude + ": " + mapcontrol.Latitude.ToString("F3") + "°" + "\n" +
                //Properties.Resources.Longitude + ": " + mapcontrol.Longitude.ToString("F3") + "°" + "\n";
                +Properties.Resources.Latitude + ": " + gP.latitude + "°" + "\n" +
                Properties.Resources.Longitude + ": " + gP.longitude + "°" + "\n";
            TextInfo.Text = messageBoxText;
        }

        //private void GeoCoordinateWatcherStatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        //{
        //if (e.Status == GeoPositionStatus.Ready)
        //{
        //    GeoCoordinate co = watcher.Position.Location;
        //    locgeo.Latitude = co.Latitude;
        //    locgeo.Longitude = co.Longitude;

        //    GetKeyLocation();
        //    //LabelGeo.Content = co;

        //    mapcontrol.Latitude = co.Latitude;
        //    mapcontrol.Longitude = co.Longitude;

        //    watcher.Stop();

        //}
        //}

        //Запрос geo
        private async void GetKeyLocation()
        {
            await Task.Run(() => Thread.Sleep(300)); // вызов асинхронной операции для нормальной инициализации в потоке переменной

            //string url_geo = $"https://dataservice.accuweather.com/locations/v1/geoposition/search.json?q={locgeo.Latitude},{locgeo.Longitude}&apikey=9pbmpNTkGYJTGy8sKGDxiIy8ADvYjqIl&language={Classes.Language.NameLanguage}";
            string url_geo = $"https://dataservice.accuweather.com/locations/v1/geoposition/search.json?q={gP.latitude},{gP.longitude}&apikey=9pbmpNTkGYJTGy8sKGDxiIy8ADvYjqIl&language={Classes.Language.NameLanguage}";

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

                gP.gp = JsonConvert.DeserializeObject<List<Geolocation.Geo>>(answer_geo);

                DataFromGeoposition();


                //List<Geolocation.Geo> gL = JsonConvert.DeserializeObject<List<Geolocation.Geo>>(answer_geo);

                //// Текст, отображаемый при нажатии на Info
                //messageBoxText = 
                //    gL[0].LocalizedName + " (" + gL[0].Region.LocalizedName + ", " + gL[0].Country.LocalizedName + ", " + gL[0].AdministrativeArea.LocalizedName + ") "
                //    + gL[0].AdministrativeArea.CountryID + "\n" + "\n" +
                //    Properties.Resources.Altitude +": " + gL[0].GeoPosition.Elevation.Metric.Value.ToString() + " " + Classes.UnitTypes.UnitName(1, gL[0].GeoPosition.Elevation.Metric.Unit) + "\n"
                //    + Properties.Resources.Latitude + ": " + mapcontrol.Latitude.ToString("F3") + "°" + "\n" +
                //    Properties.Resources.Longitude + ": " + mapcontrol.Longitude.ToString("F3") + "°" + "\n";
                //TextInfo.Text = messageBoxText;
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
            var button = (Button)sender;
            switch (button.Name)
            {
                case "MapMode":
                    MapMode_Click();
                    break;
                case "Info":
                    Info_Click();
                    break;
            }
        }

        //private void MapMode_Click(object sender, RoutedEventArgs e)
        private void MapMode_Click()
        {
            if (myMap.Mode.ToString() == "Microsoft.Maps.MapControl.WPF.RoadMode")
            {
                //Set the map mode to Aerial with labels
                myMap.Mode = new AerialMode(true);
                Properties.Settings.Default.MapMode = "AERIALWITHLABELS";
            }


            else if (myMap.Mode.ToString() == "Microsoft.Maps.MapControl.WPF.AerialMode")
            {
                //Set the map mode to RoadMode
                myMap.Mode = new RoadMode();
                Properties.Settings.Default.MapMode = "ROAD";
            }

        }

        //private void Info_Click(object sender, RoutedEventArgs e)
        private void Info_Click()
        {
            //_ = MessageBox.Show(messageBoxText, "", MessageBoxButton.OK, MessageBoxImage.Information);
            TextInfo.Text = messageBoxText;
            Info_panel.Visibility = Visibility.Visible;

        }

        private void HideInfo_Click(object sender, RoutedEventArgs e)
        {
            Info_panel.Visibility = Visibility.Collapsed;
        }

        private void MapWithPushpins_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Disables the default mouse double-click action.
            e.Handled = true;

            // Determin the location to place the pushpin at on the map.

            //Get the mouse click coordinates
            Point mousePosition = e.GetPosition(this);
            //Convert the mouse coordinates to a locatoin on the map
            //Location pinLocation = myMap.ViewportPointToLocation(mousePosition);

            // The pushpin to add to the map.
            Pushpin pin = new Pushpin();
            pin.Location = myMap.ViewportPointToLocation(mousePosition);

            //pin.Content = pinLocation.ToString();
            pin.MouseLeftButtonDown += pushpinClick;

            // Adds the pushpin to the map.
            myMap.Children.Add(pin);
        }

        private void pushpinClick(object sender, RoutedEventArgs e)
        {
            Pushpin p = sender as Pushpin;
            //InfoText.Text = (string)p.Content;
            //InfoTextLat.Text = p.Location.Latitude.ToString();
            //InfoTextLong.Text = p.Location.Longitude.ToString();
            pinLocation = MapLayer.GetPosition(p);
            Infobox.Visibility = Visibility.Visible;
            MapLayer.SetPosition(Infobox, pinLocation);
            //InfoButtonDel.MouseLeftButtonDown += buttonClick; 
        }

        private void InfoButtonDel_Click(object sender, RoutedEventArgs e) //3 Варианта удаления pin
        {
            //1-й
            myMap.Children.OfType<Pushpin>().ToList().ForEach(pin =>
            {
                if (pin.Location == pinLocation)
                {
                    myMap.Children.Remove(pin);
                    Infobox.Visibility = Visibility.Collapsed;
                }
            });

            //2-й
            //List<Pushpin> Pins = myMap.Children.OfType<Pushpin>().ToList();
            //foreach (Pushpin pin in Pins)
            //{
            //    if (pin.Location == pinLocation)
            //    {
            //        myMap.Children.Remove(pin);
            //        Infobox.Visibility = Visibility.Collapsed;
            //        break;
            //    }
            //}

            //3-й
            //Pushpin pin = myMap.Children.OfType<Pushpin>().ToList().Find(
            //    delegate (Pushpin pn)
            //    {
            //        return pn.Location == pinLocation;
            //    }
            //    );

            //if (pin != null)
            //{
            //    myMap.Children.Remove(pin);
            //    Infobox.Visibility = Visibility.Collapsed;
            //}

        }

        private void InfoButtonCW_Click(object sender, RoutedEventArgs e)
        {
            myMap.Children.OfType<Pushpin>().ToList().ForEach(pin =>
            {
                if (pin.Location == pinLocation)
                {
                    gP.latitude = pinLocation.Latitude.ToString("F3");
                    gP.longitude = pinLocation.Longitude.ToString("F3");
                    pin.Background = Brushes.Blue;
                    GetKeyLocation();
                    Infobox.Visibility = Visibility.Collapsed;

                }
            });
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

        private MapMode mode;

        public MapMode Mode
        {
            get { return mode; }
            set
            {
                if (value != mode)
                {
                    mode = value;
                }
            }
        }
    }
}
