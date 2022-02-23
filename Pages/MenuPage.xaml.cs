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

namespace My_Weather.Pages
{
    /// <summary>
    /// Логика взаимодействия для MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {

        private readonly Uri uriCurrentForecast = new Uri("/My Weather;component/Pages/CurrentForecastPage.xaml", UriKind.Relative);
        private readonly Uri uriDailyForecast = new Uri("/My Weather;component/Pages/DailyForecastPage.xaml", UriKind.Relative);
        private readonly Uri uriNightForecast = new Uri("/My Weather;component/Pages/NightForecastPage.xaml", UriKind.Relative);
        private readonly Uri uriMap = new Uri("/My Weather;component/Pages/MapPage.xaml", UriKind.Relative);

        public MenuPage()
        {
            InitializeComponent();
        }

        private void ButtonСurrent_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.DefaultPage = "/My Weather;component/Pages/CurrentForecastPage.xaml";
            NavigationService.Navigate(uriCurrentForecast);
        }

        private void ButtonDay_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.DefaultPage = "/My Weather;component/Pages/DailyForecastPage.xaml";
            NavigationService.Navigate(uriDailyForecast);
        }

        private void ButtonNight_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.DefaultPage = "/My Weather;component/Pages/NightForecastPage.xaml";
            NavigationService.Navigate(uriNightForecast);
        }

        private void ButtonMap_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.DefaultPage = "/My Weather;component/Pages/MapPage.xaml";
            NavigationService.Navigate(uriMap);
        }

    }
}
