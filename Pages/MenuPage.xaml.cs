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

        private Uri uriCurrentForecast = new Uri("/My Weather;component/Pages/CurrentForecastPage.xaml", UriKind.Relative);
        private Uri uriMap = new Uri("/My Weather;component/Pages/MapPage.xaml", UriKind.Relative);

        public MenuPage()
        {
            InitializeComponent();
        }

        private void ButtonForecast_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(uriCurrentForecast);
        }

        private void ButtonMap_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(uriMap);
        }
    }
}
