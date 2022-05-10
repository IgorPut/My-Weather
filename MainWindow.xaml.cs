using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;

namespace My_Weather
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Uri uriCurrentForecast = new Uri("/My Weather;component/Pages/CurrentForecastPage.xaml", UriKind.Relative);
        private readonly Uri uriDailyForecast = new Uri("/My Weather;component/Pages/DailyForecastPage.xaml", UriKind.Relative);
        private readonly Uri uriNightForecast = new Uri("/My Weather;component/Pages/NightForecastPage.xaml", UriKind.Relative);
        private readonly Uri uriMap = new Uri("/My Weather;component/Pages/MapPage.xaml", UriKind.Relative);
        //private readonly Uri uriMenu = new Uri("/My Weather;component/Pages/MenuPage.xaml", UriKind.Relative);
        private readonly DropShadowEffect myDropShadowEffect = new DropShadowEffect();
        private readonly DropShadowEffect clearDropShadowEffect = null;

        private readonly Uri uriDefault = new Uri(Properties.Settings.Default.DefaultPage, UriKind.Relative);

        //ResourceManager res_man;    // declare Resource manager to access to specific cultureinfo
        //CultureInfo cul;            // declare culture info

        public MainWindow()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo(Properties.Settings.Default.CultureName);
            //System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

            //var culture = new System.Globalization.CultureInfo("be-BE");
            //var day = culture.DateTimeFormat.GetDayName(DateTime.Today.DayOfWeek);

            //cul = CultureInfo.CreateSpecificCulture(Properties.Resources.Name);

            InitializeComponent();
            DataContext = new MainViewModel();

            //ButtonMenu.Effect = myDropShadowEffect;

            // Кнопку развернуть скрываем. В ней нет необходимости в приложении
            ImageMax.Visibility = Visibility.Hidden;

            //            Frame1.Source = uriMenu;

            Frame1.Source = uriDefault;
            //Frame1.Source = uriCurrentForecast;

            //Frame1.Navigate(forecastPage);

            //Tb.Text = CultureInfo.CurrentCulture.Name;

#if DEBUG
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
#endif
        }
        
        // Эффект тени при наведении курсора на объект (иконки Зарыть и Свернуть)
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

        private void ButtonMenu_Click(object sender, RoutedEventArgs e)
        {
            //Properties.Settings.Default.DefaultPage = "/My Weather;component/Pages/MenuPage.xaml";
            //Image_bg.Visibility = Visibility.Hidden;
            //Frame1.Source = uriMenu;
            ////NavigationService.Navigate(uriMenu);
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void ExpanderLang_LostFocus(object sender, RoutedEventArgs e)
        {
            ExpanderLang.IsExpanded = false;

        }

        private void My_App_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void Button_LostFocus(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.CultureName = Properties.Resources.Name;
            Frame1.Refresh();
        }

        private void ButtonСurrent_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.DefaultPage = "/My Weather;component/Pages/CurrentForecastPage.xaml";
            Frame1.Source = uriCurrentForecast;
            //NavigationService.Navigate(uriCurrentForecast);

        }

        private void ButtonDay_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.DefaultPage = "/My Weather;component/Pages/DailyForecastPage.xaml";
            Frame1.Source = uriDailyForecast;
        }

        private void ButtonNight_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.DefaultPage = "/My Weather;component/Pages/NightForecastPage.xaml";
            Frame1.Source = uriNightForecast;
        }

        private void ButtonMap_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.DefaultPage = "/My Weather;component/Pages/MapPage.xaml";
            Frame1.Source = uriMap;

        }
    }
}
