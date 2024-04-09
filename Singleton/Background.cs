using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Weather.Singleton
{
    class Background
    {
        //private static Background Instance = null;

        List<string> spring = new List<string>() { "MorningSpring.jpg", "DaySpring.jpg", "NightSpring.jpg" };
        List<string> summer = new List<string>() { "MorningSummer.jpg", "DaySummer.jpg", "NightSummer.jpg" };
        List<string> autumn = new List<string>() { "MorningAutumn.jpg.jpg", "DayAutumn.jpg", "NightAutumn.jpg" };
        List<string> winter = new List<string>() { "MorningWinter.jpg", "DayWinter.jpg", "NightWinter.jpg" };

        public List<string> background_pictures;

        //public static Background GetInstance()
        //{
        //    if (Instance == null)
        //    {
        //        Instance = new Background();
        //    }
        //    return Instance;
        //}

        public Background()
        {
            int month = DateTime.Now.Month;

            switch (month)
            {
                case 3: case 4: case 5:
                    background_pictures = spring;
                    break;
                case 6:
                case 7:
                case 8:
                    background_pictures = summer;
                    break;
                case 9:
                case 10:
                case 11:
                    background_pictures = autumn;
                    break;
                case 12:
                case 1:
                case 2:
                    background_pictures = winter;
                    break;
            }
        }
    }
}
