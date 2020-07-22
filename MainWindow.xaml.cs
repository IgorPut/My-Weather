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
    }
}
