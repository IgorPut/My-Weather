namespace My_Weather.Classes
{
    class DeviceLocation
    {
        public string latitude, longitude;

        public DeviceLocation(double latVal, double longVal)
        {
            latitude = latVal.ToString("0.000");
            longitude = longVal.ToString("0.000");
        }
    }
}
