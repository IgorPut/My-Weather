using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Weather.Classes
{
    class IndoorHumidity
    {
        public static string GetPhrase(int val)
        {
            string cul = Properties.Resources.Name;
            if (val < 30)
            {
                switch (cul)
                {
                    case "be-BE":
                        return "Вельмі суха";
                    case "ru-RU":
                        return "Очень сухо";
                    default:
                        return "Very Dry";
                }
            }

            if (val < 35)
            {
                switch (cul)
                {
                    case "be-BE":
                        return "Злёгку суха";
                    case "ru-RU":
                        return "Слегка сухо";
                    default:
                        return "Slightly Dry";
                }
            }

            if (val < 39)
            {
                switch (cul)
                {
                    case "be-BE":
                        return "Сухо";
                    case "ru-RU":
                        return "Сухо";
                    default:
                        return "Dry";
                }
            }
            if (val < 51)
            {
                switch (cul)
                {
                    case "be-BE":
                        return "Ідэальна";
                    case "ru-RU":
                        return "Идеально";
                    default:
                        return "Ideal Humidity";
                }
            }
            if (val < 80)
            {
                switch (cul)
                {
                    case "be-BE":
                        return "Трохі вільготна";
                    case "ru-RU":
                        return "Слегка влажно";
                    default:
                        return "Slightly Humid";
                }
            }
            switch (cul)
            {
                case "be-BE":
                    return "Высокая вільг.";
                case "ru-RU":
                    return "Высокая влажн.";
                default:
                    return "High Humidity";
            }
        }

    }
    }
