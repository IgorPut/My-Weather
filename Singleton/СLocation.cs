using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;

namespace My_Weather.Singleton
{
    public sealed class СLocation
    {
        private static СLocation Instance = null;
        GeoCoordinateWatcher watcher;
        
        public double latitude;
        public double longitude;
        //public bool location_changed = false;
        public string deviceLocation = "ggggg";

        public static СLocation GetInstance()
        {
            if (Instance == null)
                Instance = new СLocation();
            return Instance;
        }

        private СLocation()
        {
            GetLocationEvent();
        }

        private void GetLocationEvent()
        {
            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(PositionChanged);
            //bool started = watcher.TryStart(false, TimeSpan.FromMilliseconds(2000));
            //if (!started)
            //{
            //    deviceLocation = "GeoCoordinateWatcher timed out on start.";
            //}
            //else
            //    deviceLocation = "DeviceLocation";

            watcher.Start();

        }

        private void PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            latitude = e.Position.Location.Latitude;
            longitude = e.Position.Location.Longitude;

            //location_changed = true;

            deviceLocation = "PositionChanged";

            //PrintPosition(e.Position.Location.Latitude, e.Position.Location.Longitude);
        }

        //void PrintPosition(double Latitude, double Longitude)
        //{
        //    Console.WriteLine("Latitude: {0}, Longitude {1}", Latitude, Longitude);
        //}
    }
}
/*
 * The Permission, Status, and Position properties support INotifyPropertyChanged 
 * (https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8.1),
 * so that an application can data-bind to these properties.
*/
