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
            string cul = Properties.Resources.Name;
            int pressureInt = Convert.ToInt32(pressure);
            string pressureString = pressureInt + " " + unit;
            if (unitType == 14)
            {
                switch (cul)
                {
                    case "be-BE":
                        pressureString = Convert.ToInt32(pressure * 0.75) + " мм рт сл";
                        break;
                    case "en-US":
                        pressureString = Convert.ToInt32(pressure * 0.75) + " mmHg";
                        break;
                    case "ru-RU":
                        pressureString = Convert.ToInt32(pressure * 0.75) + " мм рт ст";
                        break;
                }

                //pressureString = Convert.ToInt32(pressure * 0.75) + " мм рт ст";
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
