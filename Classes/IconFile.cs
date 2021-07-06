using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Weather.Classes
{
    class IconFile
    {
        public static string getIconFile (int Icon)
        {
            //string iconStr = Icon.ToString();
            if (Icon.ToString().Length == 1)
                return "0" + Icon + "-s.png";
            else
                return Icon + "-s.png";
        }
    }
}
