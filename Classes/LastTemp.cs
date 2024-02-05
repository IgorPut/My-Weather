using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Weather.Classes
{
    class LastTemp
    {
        public static string LastTempText(int hours)
        {
            string cul = Properties.Resources.Name, hoursStr;

            switch (cul)
            {
                case "be-BE":
                    if (hours == 6)
                        hoursStr = "гадзін";
                    else 
                        hoursStr = "гадзіны";
                    break;
                case "ru-RU":
                    if (hours == 6)
                        hoursStr = "часов";
                    else
                        hoursStr = "часа";
                    break;
                default:
                    hoursStr = "hours";
                    break;
            }

            switch (cul)
            {
                case "be-BE":
                    return "Дыяпазон змены тэмпературы за апошнія " + hours.ToString() + " " + hoursStr + ": ";
                case "ru-RU":
                    return "Диапазон изменения температуры за последние " + hours.ToString() + " " + hoursStr + ": ";
                default:
                    return "Temperature range for the last " + hours.ToString() + " " + hoursStr + ": ";
            }
        }
    }
}
