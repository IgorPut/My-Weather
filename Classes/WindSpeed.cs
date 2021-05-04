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
                    case "be":
                        return "Штыль";
                    case "ru":
                        return "Штиль";
                    default:
                        return "Calm";
                }
            if (speed < 5)
                switch (cul)
                {
                    case "be":
                        return "Ціхі";
                    case "ru":
                        return "Тихий";
                    default:
                        return "Quiet";
                }
            if (speed < 11)
                switch (cul)
                {
                    case "be":
                        return "Лёгкі";
                    case "ru":
                        return "Лёгкий";
                    default:
                        return "Light breeze";
                }
            if (speed < 19)
                switch (cul)
                {
                    case "be":
                        return "Слабы";
                    case "ru":
                        return "Слабый";
                    default:
                        return "Weak";
                }
            if (speed < 28)
                switch (cul)
                {
                    case "be":
                        return "Умераны";
                    case "ru":
                        return "Умеренный";
                    default:
                        return "Moderate";
                }
            if (speed < 38)
                return "Свежий";
            if (speed < 49)
                return "Сильный";
            if (speed < 61)
                return "Крепкий";
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
