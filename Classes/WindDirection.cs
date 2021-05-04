using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Weather.Classes
{
    class WindDirection
    {
        public static string Wind_Direction(int degrees, string localized)
        {
            string cul = Properties.Resources.Name;
            if (degrees < 236.25)
            {
                switch (cul)
                {
                    case "be":
                        return "ПдЗ";
                    default:
                        return localized;
                }
            }
            else if (degrees < 281.25)
            {
                switch (cul)
                {
                    case "be":
                        return "З";
                    //case "en":
                    //    return "WNW";
                    //case "ru":
                    //    return "ЗСЗ";
                    default:
                        return localized;
                }
            }
            else if (degrees < 303.75)
            {
                switch (cul)
                {
                    case "be":
                        return "ЗПнЗ";
                    //case "en":
                    //    return "WNW";
                    //case "ru":
                    //    return "ЗСЗ";
                    default:
                        return localized;
                }
            }

            else
                return localized;
        }

    }
}
