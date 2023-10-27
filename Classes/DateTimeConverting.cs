using System;

namespace My_Weather.Classes
{
    internal class DateTimeConverting
    {
        public DateTime dt;
        public string dayOfWeek;
        public string dm; //значение даты в формате dd.mm

        public DateTimeConverting (int epochDate)
        {
            dt = UnixTimeStampToDateTime(epochDate); //Преобразуем полученную в запросе дату Epoch Date (тип int) в тип DateTime
            dayOfWeek = dt.ToString("dddd");
            dm = dt.ToString("dd.MM");
        }

        public string TimeOfDay ()
        {
            //    DateTime dt = UnixTimeStampToDateTime(epochDate);
            TimeSpan time = dt.TimeOfDay;
            string timeString = Convert.ToString(time);
            return timeString.Substring(0, timeString.Length - 3);
        }

        private static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
