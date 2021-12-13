using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Weather.Classes
{
    class IndoorHumidity
    {
        public static string getPhrase(int val)
        {
            string cul = Properties.Resources.Name;
            if (val < 35)
            {
                switch (cul)
                {
                    case "be-BE":
                        return "Злёгку сухi";
                    case "ru-RU":
                        return "Слегка сухой";
                    default:
                        return "Slightly Dry";
                }
            }
            if (val < 51)
            {
                switch (cul)
                {
                    case "be-BE":
                        return "Ідэальнае";
                    case "ru-RU":
                        return "Идеальный";
                    default:
                        return "Ideal Humidity";
                }
            }
            if (val < 80)
            {
                switch (cul)
                {
                    case "be-BE":
                        return "Трохі вільготны";
                    case "ru-RU":
                        return "Слегка влажный";
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
