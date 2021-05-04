using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Weather.AccuWeather
{
    class Realfeeltemperatureshade
    {
        public Maximum2 Maximum { get; set; }
    }

    public class Maximum2
    {
        public int _Value;
        public float Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = Convert.ToInt16(value);
            }
        }

        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

}
