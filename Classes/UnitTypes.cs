using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Weather.Classes
{
    class UnitTypes
    {
        public static string UnitName(int unitType, string unit)
        {
            string cul = Properties.Resources.Name;

            switch (unitType)
            {
                case 1:
                    switch (cul)
                    {
                        case "be-BE":
                            return "м";
                        case "ru-RU":
                            return "м";
                        default:
                            return unit;
                    }
                case 3:
                    switch (cul)
                    {
                        case "be-BE":
                            return "мм";
                        case "ru-RU":
                            return "мм";
                        default:
                            return unit;
                    }
                case 4:
                    switch (cul)
                    {
                        case "be-BE":
                            return "см";
                        //case "ru-RU":
                        //    return "см";
                        default:
                            return unit;
                    }
                case 7:
                    switch (cul)
                    {
                        case "be-BE":
                            return "км/г";
                        case "ru-RU":
                            return "км/ч";
                        default:
                            return unit;
                    }
                case 17:
                case 18:
                    return "°" + unit;
                default:
                    return unit;
            }
        }

        //public static string Hour ()
        //{
        //    switch (cul)
        //    {
        //        case "be-BE":
        //            return "г";
        //        case "ru-RU":
        //            return "ч";
        //        default:
        //            return "h";
        //    }

        //}
    }
}
