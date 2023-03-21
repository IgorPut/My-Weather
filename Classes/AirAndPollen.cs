using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Weather.Classes
{
    class AirAndPollen
    {
        public static string AirQuality(string category, int categoryValue)
        {
            string cul = Properties.Resources.Name;
            if (categoryValue == 1)
            {
                switch (cul)
                {
                    case "be-BE":
                        return " - добрая";
                    case "ru-RU":
                        return " - хорошее";
                    default:
                        return category;
                }
            }
            else return category;
        }

        public static string UV_Category(int categoryValue, string category)
        {
            string cul = Properties.Resources.Name;
            switch (categoryValue)
            {
                case 0:
                    switch (cul)
                    {
                        case "be-BE":
                            return "Добры";
                        default:
                            return category;
                    }
                case 1:
                    switch (cul)
                    {
                        case "be-BE":
                            return "Нiзкi";
                        default:
                            return category;
                    }
                case 2:
                case 3:
                    switch (cul)
                    {
                        case "be-BE":
                            return "Умеран.";
                        default:
                            return category;
                    }
                case 4:
                    switch (cul)
                    {
                        case "be-BE":
                            return "Выс.";
                        default:
                            return category;
                    }
                default:
                    return category;
            }
        }

    }
}
