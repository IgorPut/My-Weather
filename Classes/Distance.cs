using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Weather.Classes
{
    class Distance
    {
        public static string DistanceRu(string unit, int unitType)
        {
            switch (unitType)
            {
                case 5:
                    return "м";
                case 6:
                    return "км";
                default:
                    return unit;
            }

        }
    }
}
