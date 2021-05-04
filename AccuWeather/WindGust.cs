using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Weather.AccuWeather
{
    class WindGust
    {
        public Speed1 Speed { get; set; }
        public Direction Direction { get; set; }
    }
    public class Speed1
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
        public string UnitRu
        {
            get
            {
                switch (UnitType)
                {
                    case 7:
                        return "км/ч";
                    default:
                        return " ";
                }
            }
        }
        public int UnitType { get; set; }

    }
}
