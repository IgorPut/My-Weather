using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Weather.Classes
{
    class UnitTypes
    {
        public static string cul = Properties.Resources.Name;

        public static string UnitName(int unitType, string unit)
        {
            switch (unitType)
            {
                case 7:
                    switch (cul)
                    {
                        case "be":
                            return "км/г";
                        case "ru":
                            return "км/ч";
                        default:
                            return unit;
                    }
                case 3:
                    switch (cul)
                    {
                        case "be":
                            return "мм";
                        case "ru":
                            return "мм";
                        default:
                            return unit;
                    }
                default:
                    return unit;
            }
        }

        public static string Hour ()
        {
            switch (cul)
            {
                case "be":
                    return "г";
                case "ru":
                    return "ч";
                default:
                    return "h";
            }

        }
    }
}
