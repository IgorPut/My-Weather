using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace My_Weather.Singleton
{
    public sealed class Geoposition
    {
        //This variable is going to store the Geoposition Instance
        private static Geoposition Instance = null;

        public List<Geolocation.Geo> gp;
        public string latitude;
        public string longitude;
        public bool useMyLocation = true;

        //The following Static Method is going to return the Geoposition Instance
        public static Geoposition GetInstance()
        {
            //If the variable instance is null, then create the Geoposition instance 
            //else return the already created Geoposition instance
            //This version is not thread safe
            if (Instance == null)
            {
                Instance = new Geoposition();
            }
            //Return the Geoposition Instance
            return Instance;
        }

        //Конструктор класса
        private Geoposition()
        {
            //gp[0].GeoPosition.Latitude = 0;
            //gp[0].GeoPosition.Longitude = 0;
        }
    }
}
