using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Weather.AccuWeather
{
    class Day
    {
        
        public string Icon { get; set; }
        public string iconFile
        {
            get
            {
                if (Icon.Length == 1)
                    return "0" + Icon + "-s.png";
                else
                    return Icon + "-s.png";
            }
        }

        public string IconPhrase { get; set; }
        public string ShortPhrase { get; set; }

        public int PrecipitationProbability { get; set; }
        public int ThunderstormProbability { get; set; }

        public Wind Wind { get; set; }

        public WindGust WindGust { get; set; }

        public Rain Rain { get; set; }

        public int CloudCover { get; set; }
    }
}
