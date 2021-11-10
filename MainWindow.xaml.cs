﻿using System;
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

using System.Globalization;
using System.Resources;
using System.Windows.Navigation;

namespace My_Weather
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private readonly object forecastPage = new ForecastPage();
        //private readonly object mapPage = new MapPage();
        private Uri uriCurrentForecast = new Uri("/My Weather;component/Pages/CurrentForecastPage.xaml", UriKind.Relative);
        private Uri uriDailyForecast = new Uri("/My Weather;component/Pages/DailyForecastPage.xaml", UriKind.Relative);
        private Uri uriMap = new Uri("/My Weather;component/MapPage.xaml", UriKind.Relative);
        private Uri uriMenu = new Uri("/My Weather;component/Pages/MenuPage.xaml", UriKind.Relative);
        private DropShadowEffect myDropShadowEffect = new DropShadowEffect();
        private DropShadowEffect clearDropShadowEffect = null;

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

            // Кнопку развернуть скрываем. В ней нет необходимости в приложении
            ImageMax.Visibility = Visibility.Hidden;

            //            Frame1.Source = uriMenu;
            Frame1.Source = uriCurrentForecast;

            //Frame1.Navigate(forecastPage);

            //Tb.Text = CultureInfo.CurrentCulture.Name;

#if DEBUG
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
#endif
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

        private void ButtonMenu_Click(object sender, RoutedEventArgs e)
        {
            Frame1.Source = uriMenu;
            //NavigationService.Navigate(uriMenu);
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void ExpanderLang_LostFocus(object sender, RoutedEventArgs e)
        {
            ExpanderLang.IsExpanded = false;
            TBl1.Text = DateTime.Now.ToString("dddd", CultureInfo.CreateSpecificCulture(Properties.Resources.Name));
            Properties.Settings.Default.CultureName = Properties.Resources.Name;
        }

        private void My_App_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }
}
