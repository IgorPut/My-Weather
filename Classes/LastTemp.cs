namespace My_Weather.Classes
{
    class LastTemp
    {
        public static string LastTempText(int hours)
        {
            string cul = Properties.Resources.Name, hoursStr;

            switch (cul)
            {
                case "be-BE":
                    if (hours == 24)
                        hoursStr = "гадзіны";
                    else 
                        hoursStr = "гадзін";
                    break;
                case "ru-RU":
                    if (hours == 24)
                        hoursStr = "часа";
                    else
                        hoursStr = "часов";
                    break;
                default:
                    hoursStr = "hours";
                    break;
            }

            switch (cul)
            {
                case "be-BE":
                    return "Дыяпазон змены тэмпературы за апошнія " + hours.ToString() + " " + hoursStr + ": ";
                case "ru-RU":
                    return "Диапазон изменения температуры за последние " + hours.ToString() + " " + hoursStr + ": ";
                default:
                    return "Temperature range for the last " + hours.ToString() + " " + hoursStr + ": ";
            }
        }
    }
}
