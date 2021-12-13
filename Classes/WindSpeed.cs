using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Weather.Classes
{
    class WindSpeed
    {
        public static string Power(float speed)
        {
            string cul = Properties.Resources.Name;
            if (speed < 2)
                switch (cul)
                {
                    case "be-BE":
                        return "Штыль";
                    case "ru-RU":
                        return "Штиль";
                    default:
                        return "Calm";
                }
            if (speed < 5)
                switch (cul)
                {
                    case "be-BE":
                        return "Ціхі";
                    case "ru-RU":
                        return "Тихий";
                    default:
                        return "Quiet";
                }
            if (speed < 11)
                switch (cul)
                {
                    case "be-BE":
                        return "Лёгкі";
                    case "ru-RU":
                        return "Лёгкий";
                    default:
                        return "Light breeze";
                }
            if (speed < 19)
                switch (cul)
                {
                    case "be-BE":
                        return "Слабы";
                    case "ru-RU":
                        return "Слабый";
                    default:
                        return "Weak";
                }
            if (speed < 28)
                switch (cul)
                {
                    case "be-BE":
                        return "Умераны";
                    case "ru-RU":
                        return "Умеренный";
                    default:
                        return "Moderate";
                }
            if (speed < 38)
                switch (cul)
                {
                    case "be-BE":
                        return "Свежы";
                    case "ru-RU":
                        return "Свежий";
                    default:
                        return "Fresh";
                }
            if (speed < 49)
                switch (cul)
                {
                    case "be-BE":
                        return "Моцны";
                    case "ru-RU":
                        return "Сильный";
                    default:
                        return "Strong";
                }
            if (speed < 61)
                switch (cul)
                {
                    case "be-BE":
                        return "Дужы";
                    case "ru-RU":
                        return "Крепкий";
                    default:
                        return "High";
                }
            if (speed < 74)
                return "Очень крепкий";
            if (speed < 88)
                return "Шторм";
            if (speed < 102)
                return "Сильный шторм";
            if (speed < 117)
                return "Жестокий шторм";

            return "Ураган";
        }


    }
}
