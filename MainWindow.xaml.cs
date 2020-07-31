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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace My_Weather
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private readonly object forecastPage = new ForecastPage();
        //private readonly object mapPage = new MapPage();
        private Uri uriForecast = new Uri("/My Weather;component/ForecastPage.xaml", UriKind.Relative);
        private Uri uriMap = new Uri("/My Weather;component/MapPage.xaml", UriKind.Relative);
        private DropShadowEffect myDropShadowEffect = new DropShadowEffect();
        private DropShadowEffect clearDropShadowEffect = null;

        public MainWindow()
        {
            InitializeComponent();

            //Frame1.Navigate(forecastPage);

#if DEBUG
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
#endif
        }

        private void ShowMapButton_Click(object sender, RoutedEventArgs e)
        {
            //Frame1.Navigate(mapPage);
            //Frame1.Source = uriMap;
        }

        private void ShowForecastPage_Click(object sender, RoutedEventArgs e)
        {
            //Frame1.Navigate(forecastPage);
            //Frame1.Source = uriForecast;
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Image)sender).Effect = myDropShadowEffect;
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Image)sender).Effect = clearDropShadowEffect;
        }

        private void ImageMin_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ImageMax_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void ImageClose_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

    }
}
