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
            if (degrees < 11.25)
            {
                switch (cul)
                {
                    case "be-BE":
                        return "Пн";
                    default:
                        return localized;
                }
            }
            if (degrees < 33.75)
            {
                switch (cul)
                {
                    case "be-BE":
                        return "ПнПнЎ";
                    default:
                        return localized;
                }
            }
            if (degrees < 56.25)
            {
                switch (cul)
                {
                    case "be-BE":
                        return "ПнЎ";
                    default:
                        return localized;
                }
            }
            if (degrees < 78.75)
            {
                switch (cul)
                {
                    case "be-BE":
                        return "ЎПнЎ";
                    default:
                        return localized;
                }
            }
            if (degrees < 101.25)
            {
                switch (cul)
                {
                    case "be-BE":
                        return "Ў";
                    default:
                        return localized;
                }
            }
            if (degrees < 123.75)
            {
                switch (cul)
                {
                    case "be-BE":
                        return "ЎПдЎ";
                    default:
                        return localized;
                }
            }
            if (degrees < 146.25)
            {
                switch (cul)
                {
                    case "be-BE":
                        return "ПдЎ";
                    default:
                        return localized;
                }
            }
            if (degrees < 168.75)
            {
                switch (cul)
                {
                    case "be-BE":
                        return "ПдПдЎ";
                    default:
                        return localized;
                }
            }
            if (degrees < 236.25)
            {
                switch (cul)
                {
                    case "be-BE":
                        return "ПдЗ";
                    default:
                        return localized;
                        //return cul;
                }
            }
            if (degrees < 281.25)
            {
                switch (cul)
                {
                    case "be-BE":
                        return "З";
                    default:
                        return localized;
                }
            }
            if (degrees < 303.75)
            {
                switch (cul)
                {
                    case "be-BE":
                        return "ЗПнЗ";
                    default:
                        return localized;
                }
            }
            if (degrees <= 360)
            {
                switch (cul)
                {
                    case "be-BE":
                        return "Пн";
                    default:
                        return localized;
                }
            }

            else
                return localized;
        }

    }
}
