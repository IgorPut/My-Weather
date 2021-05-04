using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Weather.Classes
{
    class Pressure
    {
        //Перевод из мбар в мм рт ст
        public static string PressureUnitRu(int unitType, string unit, float pressure)
        {
            int pressureInt = Convert.ToInt32(pressure);
            string pressureString = pressureInt + " " + unit;
            if (unitType == 14)
            {
                pressureString = Convert.ToInt32(pressure * 0.75) + " мм рт ст";
            }
            return pressureString;
        }
        
        //Отображение тенденции давления
        public static string Tendency(string code)
        {
            switch (code)
            {
                case "F":
                    return "↓";
                case "R":
                    return "↑";
                default:
                    return "↔";
            }
        }

    }
}
