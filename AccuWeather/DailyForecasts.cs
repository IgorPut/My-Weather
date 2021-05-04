using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Weather.AccuWeather
{
    class DailyForecasts
    {
		//=======EpochDate из массива DailyForecast json================================
		public double EpochDate; 

		public DateTime dt
		{
			get
			{
				return UnixTimeStampToDateTime(EpochDate);
			}
		}

		public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
		{
			// Unix timestamp is seconds past epoch
			System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
			return dtDateTime;
		}

  //      public DayOfWeek DayOfWeek
  //      {
  //          get
  //          {
  //              return dt.DayOfWeek;
  //          }
  //      }

		//public string DayMonth //получаем значение даты в формате dd.mm 
		//{
  //          get 
		//	{
		//		string _dm = dt.ToString("d"); //Дата в строку
		//		return _dm.Substring(0, _dm.Length - 5); //Обрезаем в конце строки год и точку перед ним
		//	}
		//}

		//======Другие объекты из DailyForecast json=================================
		//public Temperature Temperature { get; set; } 

		public Day Day { get; set; }

        //public Realfeeltemperature RealFeelTemperature { get; set; }
        //public Realfeeltemperatureshade RealFeelTemperatureShade { get; set; }
		public Airandpollen[] AirAndPollen { get; set; }


	}


}
