using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Weather.DailyWeather
{

    public class Rootobject
    {
        public Headline Headline { get; set; }
        public Dailyforecast[] DailyForecasts { get; set; }
    }

    public class Headline
    {
        public DateTime EffectiveDate { get; set; }
        public int EffectiveEpochDate { get; set; }
        public int Severity { get; set; }
        public string Text { get; set; }
        public string Category { get; set; }
        public DateTime EndDate { get; set; }
        public int EndEpochDate { get; set; }
        public string MobileLink { get; set; }
        public string Link { get; set; }
    }

    public class Dailyforecast
    {
        public DateTime Date { get; set; }
        public int EpochDate { get; set; }
        public Sun Sun { get; set; }
        public Moon Moon { get; set; }
        public Temperature Temperature { get; set; }
        public Realfeeltemperature RealFeelTemperature { get; set; }
        public Realfeeltemperatureshade RealFeelTemperatureShade { get; set; }
        public float HoursOfSun { get; set; }
        public Degreedaysummary DegreeDaySummary { get; set; }
        public Airandpollen[] AirAndPollen { get; set; }
        public Day Day { get; set; }
        public Night Night { get; set; }
        public string[] Sources { get; set; }
        public string MobileLink { get; set; }
        public string Link { get; set; }
    }

    public class Sun
    {
        public DateTime Rise { get; set; }
        public int EpochRise { get; set; }
        public DateTime Set { get; set; }
        public int EpochSet { get; set; }
    }

    public class Moon
    {
        public DateTime Rise { get; set; }
        public int EpochRise { get; set; }
        public object Set { get; set; }
        public object EpochSet { get; set; }
        public string Phase { get; set; }
        public int Age { get; set; }
    }

    public class Temperature
    {
        public Minimum Minimum { get; set; }
        public Maximum Maximum { get; set; }
    }

    public class Minimum
    {
        public float Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

    public class Maximum
    {
        public float Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

    public class Realfeeltemperature
    {
        public Minimum1 Minimum { get; set; }
        public Maximum1 Maximum { get; set; }
    }

    public class Minimum1
    {
        public float Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

    public class Maximum1
    {
        public float Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

    public class Realfeeltemperatureshade
    {
        public Minimum2 Minimum { get; set; }
        public Maximum2 Maximum { get; set; }
    }

    public class Minimum2
    {
        public float Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

    public class Maximum2
    {
        public float Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

    public class Degreedaysummary
    {
        public Heating Heating { get; set; }
        public Cooling Cooling { get; set; }
    }

    public class Heating
    {
        public float Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

    public class Cooling
    {
        public float Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

    public class Day
    {
        public int Icon { get; set; }
        public string IconPhrase { get; set; }
        public bool HasPrecipitation { get; set; }
        public string ShortPhrase { get; set; }
        public string LongPhrase { get; set; }
        public int PrecipitationProbability { get; set; }
        public int ThunderstormProbability { get; set; }
        public int RainProbability { get; set; }
        public int SnowProbability { get; set; }
        public int IceProbability { get; set; }
        public Wind Wind { get; set; }
        public Windgust WindGust { get; set; }
        public Totalliquid TotalLiquid { get; set; }
        public Rain Rain { get; set; }
        public Snow Snow { get; set; }
        public Ice Ice { get; set; }
        public float HoursOfPrecipitation { get; set; }
        public float HoursOfRain { get; set; }
        public float HoursOfSnow { get; set; }
        public float HoursOfIce { get; set; }
        public int CloudCover { get; set; }
    }

    public class Wind
    {
        public Speed Speed { get; set; }
        public Direction Direction { get; set; }
    }

    public class Speed
    {
        public float Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

    public class Direction
    {
        public int Degrees { get; set; }
        public string Localized { get; set; }
        public string English { get; set; }
    }

    public class Windgust
    {
        public Speed1 Speed { get; set; }
        public Direction1 Direction { get; set; }
    }

    public class Speed1
    {
        public float Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

    public class Direction1
    {
        public int Degrees { get; set; }
        public string Localized { get; set; }
        public string English { get; set; }
    }

    public class Totalliquid
    {
        public float Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

    public class Rain
    {
        public float Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

    public class Snow
    {
        public float Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

    public class Ice
    {
        public float Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

    public class Night
    {
        public int Icon { get; set; }
        public string IconPhrase { get; set; }
        public bool HasPrecipitation { get; set; }
        public string ShortPhrase { get; set; }
        public string LongPhrase { get; set; }
        public int PrecipitationProbability { get; set; }
        public int ThunderstormProbability { get; set; }
        public int RainProbability { get; set; }
        public int SnowProbability { get; set; }
        public int IceProbability { get; set; }
        public Wind1 Wind { get; set; }
        public Windgust1 WindGust { get; set; }
        public Totalliquid1 TotalLiquid { get; set; }
        public Rain1 Rain { get; set; }
        public Snow1 Snow { get; set; }
        public Ice1 Ice { get; set; }
        public float HoursOfPrecipitation { get; set; }
        public float HoursOfRain { get; set; }
        public float HoursOfSnow { get; set; }
        public float HoursOfIce { get; set; }
        public int CloudCover { get; set; }
    }

    public class Wind1
    {
        public Speed2 Speed { get; set; }
        public Direction2 Direction { get; set; }
    }

    //Значение, ед. измерения, тип единицы измерения скорости ветра ночью
    public class Speed2
    {
        public float Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

    public class Direction2
    {
        public int Degrees { get; set; }
        public string Localized { get; set; }
        public string English { get; set; }
    }

    public class Windgust1
    {
        public Speed3 Speed { get; set; }
        public Direction3 Direction { get; set; }
    }

    public class Speed3
    {
        public float Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

    public class Direction3
    {
        public int Degrees { get; set; }
        public string Localized { get; set; }
        public string English { get; set; }
    }

    public class Totalliquid1
    {
        public float Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

    public class Rain1
    {
        public float Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

    public class Snow1
    {
        public float Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

    public class Ice1
    {
        public float Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

    public class Airandpollen
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public string Category { get; set; }
        public int CategoryValue { get; set; }
        public string Type { get; set; }
    }

}
