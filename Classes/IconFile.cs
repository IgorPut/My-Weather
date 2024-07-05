using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        public static ImageSource iconSource(int Icon)
        {
            string iconFile = "pack://application:,,,/My Weather;component/Images/Icons/" + IconFile.getIconFile(Icon);
            Uri uri = new Uri(iconFile, UriKind.Absolute);
            return new BitmapImage(uri);
        }
    }
}
