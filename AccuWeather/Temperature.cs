using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Weather.AccuWeather
{
    class Temperature
    {
        public Maximum Maximum { get; set; }
    }

    class Maximum
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
    }

}
