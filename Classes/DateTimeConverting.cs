using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Weather.Classes
{
    class DateTimeConverting
    {
        public DateTime dt;
        public string dayOfWeek;
        public string dayOfMonth;
        public string dm; //значение даты в формате dd.mm

        public DateTimeConverting (int epochDate)
        {
            dt = UnixTimeStampToDateTime(epochDate);
            dayOfWeek = dt.ToString("dddd");
            dayOfMonth = dt.ToString("M");
            dm = dt.ToString("dd.MM");
        }

        public string timeOfDay ()
        {
            //    DateTime dt = UnixTimeStampToDateTime(epochDate);
            TimeSpan time = dt.TimeOfDay;
            string timeString = Convert.ToString(time);
            return timeString.Substring(0, timeString.Length - 3);
        }

        //public static string dayOfWeek (int epochDate)
        //{
        //    DateTime dt = UnixTimeStampToDateTime(epochDate);
        //    return dt.ToString("dddd");
        //}

        //public static string dayOfMonth (int epochDate)
        //{
        //    DateTime dt = UnixTimeStampToDateTime(epochDate);
        //    return dt.ToString("M");
        //}

        //public static string timeOfDay (int epochDate)
        //{
        //    DateTime dt = UnixTimeStampToDateTime(epochDate);
        //    TimeSpan time = dt.TimeOfDay;
        //    string timeString = Convert.ToString(time);
        //    return timeString.Substring(0, timeString.Length - 3);
        //}


        private static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
