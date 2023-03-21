using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Weather.TranslateAPI
{
    public class Rootobject
    {
        public int status { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string original_text { get; set; }
        public Translated_Text translated_text { get; set; }
        public int translated_characters { get; set; }
    }

    public class Translated_Text
    {
        public string be { get; set; }
    }
}
