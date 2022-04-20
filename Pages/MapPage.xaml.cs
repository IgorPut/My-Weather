using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Maps.MapControl.WPF;


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

        public MapPage()
        {
            MyDeviceLocation();

            mapcontrol.Center = locgeo;
            mapcontrol.Marker = locgeo;

            InitializeComponent();

            //MapItemsControl myMap = new MapItemsControl();

            //ParentPanel.Children.Add(MyMap);
            DataContext = mapcontrol;
        }

        //private void Page_Loaded(object sender, RoutedEventArgs e)
        //{

        //}

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
                var co = watcher.Position.Location;
                locgeo.Latitude = co.Latitude;
                locgeo.Longitude = co.Longitude;

                mapcontrol.Latitude = co.Latitude;
                mapcontrol.Longitude = co.Longitude;

                watcher.Stop();

            }
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
    }
}
