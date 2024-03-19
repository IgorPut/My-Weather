using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace My_Weather.Pages
{
    /// <summary>
    /// Логика взаимодействия для PeriodPage.xaml
    /// </summary>
    /// 
    internal sealed class GlList : List<Geolocation.Geo>
    {
    }
    public partial class PeriodPage : Page
    {
        Random rand;

        private string geoKey, localasedContent;
        private double ImageRefreshWidth, ImageRefreshHeight;
        private double EllipseRefreshWidth, EllipseRefreshHeight;
        private int geocount = 0;
        private readonly Singleton.Geoposition gP;
        private readonly Singleton.СLocation dL;
        private bool refresh;
        private SolidColorBrush randomColorBrush;

        private byte[] GetRandomBytes(int n)
        {
            //  Fill an array of bytes of length "n" with random numbers.
            var randomBytes = new byte[n];
            rand.NextBytes(randomBytes);
            return randomBytes;
        }

        private readonly Duration _openCloseDuration = new Duration(TimeSpan.FromSeconds(0.5));

        public PeriodPage()
        {
            InitializeComponent();

            SetColorTextBox();

            LabelDT.Content = "";
            LabelLocalased.Content = localasedContent = "";
            TextBoxAnswer.Visibility = Visibility.Collapsed;

            Classes.Language.NameLanguage = Properties.Resources.Name;

            gP = Singleton.Geoposition.GetInstance();
            dL = Singleton.СLocation.GetInstance();

            //TextBoxAnswer.Text = dL.deviceLocation;
            //TextBoxAnswer.Text += dL.latitude.ToString();

            Period.Measure(new Size(Period.MaxWidth, Period.MaxHeight));
            DoubleAnimation heightAnimation = new DoubleAnimation(0, 600, _openCloseDuration);
            Period.BeginAnimation(HeightProperty, heightAnimation);

            //if (gP.useMyLocation)
            //    MyDeviceLocation();
            //else
            //    GetKeyLocation();
        }

        private void SetColorTextBox()
        {
            rand = new Random();

            byte[] rgb = GetRandomBytes(3);

            //  Create a solid color brush using the three random numbers.
            randomColorBrush = new SolidColorBrush(Color.FromArgb(255, rgb[0], rgb[1], rgb[2]));

            //  Set both the text color and the text box border to the random color.
            TextBoxAnswer.BorderBrush = randomColorBrush;
            TextBoxAnswer.Foreground = randomColorBrush;

            //TbPhrase.Foreground = randomColorBrush;
        }
    }
}
