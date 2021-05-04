using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Weather.Classes
{
    class DeviceLocation
    {
        public string latitude, longitude;

        public DeviceLocation(string latVal, string longVal)
        {
            latitude = latVal;
            longitude = longVal;
        }
    }
}
