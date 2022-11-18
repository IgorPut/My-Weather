using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Weather.Classes
{
    class LastTemp
    {
        public static string LastTempText()
        {
            string cul = Properties.Resources.Name;
            switch (cul)
            {
                case "be-BE":
                    return "Дыяпазон змены тэмпературы за апошнія 24 гадзіны: ";
                case "ru-RU":
                    return "Диапазон изменения температуры за последние 24 часа: ";
                default:
                    return "Temperature range for the last 24 hours: ";
            }
        }
    }
}
