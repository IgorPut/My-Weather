using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Weather.AccuWeather
{

	//{
	//"Headline":
	//	{
	//	"EffectiveDate":"2020-08-08T08:00:00-04:00",
	//	"EffectiveEpochDate":1596888000,
	//	"Severity":5,
	//	"Text":"Expect showers Saturday morning",
	//	"Category":"rain",
	//	"EndDate":"2020-08-08T14:00:00-04:00",
	//	"EndEpochDate":1596909600,
	//	"MobileLink":"http://m.accuweather.com/en/us/boston-ma/02108/extended-weather-forecast/348735?lang=en-us",
	//	"Link":"http://www.accuweather.com/en/us/boston-ma/02108/daily-weather-forecast/348735?lang=en-us"
	//	},
	//"DailyForecasts":[
	//	{
	//	"Date":"2020-08-05T07:00:00-04:00",
	//	"EpochDate":1596625200,
	//	"Temperature":
	//		{
	//		"Minimum":
	//			{
	//			"Value":68.0,"Unit":"F",
	//			"UnitType":18
	//			},
	//		"Maximum":
	//			{
	//			"Value":90.0,"Unit":"F","UnitType":18
	//			}
	//		},
	//	"Day":
	//		{
	//		"Icon":4,
	//		"IconPhrase":"Intermittent clouds",
	//		"HasPrecipitation":false
	//		},
	//	"Night":
	//		{
	//		"Icon":34,
	//		"IconPhrase":"Mostly clear",
	//		"HasPrecipitation":false
	//		},
	//	"Sources":["AccuWeather"],
	//	"MobileLink":"http://m.accuweather.com/en/us/boston-ma/02108/daily-weather-forecast/348735?day=1&lang=en-us",
	//	"Link":"http://www.accuweather.com/en/us/boston-ma/02108/daily-weather-forecast/348735?day=1&lang=en-us"
	//	}]
	//}

    class AccuWeather
    {
		public DailyForecasts[] DailyForecasts;
    }
}
