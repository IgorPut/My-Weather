﻿using System;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

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
        private readonly Uri uriLocation = new Uri("/My Weather;component/Pages/LocationPage.xaml", UriKind.Relative);
        private readonly Uri uriPeriod = new Uri("/My Weather;component/Pages/PeriodPage.xaml", UriKind.Relative);

        private readonly DropShadowEffect myDropShadowEffect = new DropShadowEffect();
        private readonly DropShadowEffect clearDropShadowEffect = null;

        private readonly Uri uriDefault = new Uri(Properties.Settings.Default.DefaultPage, UriKind.Relative);

        private Singleton.СLocation dL;
        private Classes.Background bgP = new Classes.Background();

        private BitmapImage img = new BitmapImage(new Uri(Properties.Settings.Default.FrameBackground));

        //Border borderInTemplate = null;

        public MainWindow()
        {
            dL = Singleton.СLocation.GetInstance();
            //gP = Singleton.Geoposition.GetInstance();

            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo(Properties.Settings.Default.CultureName);

            //var culture = new System.Globalization.CultureInfo("be-BE");
            //var day = culture.DateTimeFormat.GetDayName(DateTime.Today.DayOfWeek);

            //cul = CultureInfo.CreateSpecificCulture(Properties.Resources.Name);

            InitializeComponent();
            DataContext = new MainViewModel();

            try
            {
                Rect bounds = Properties.Settings.Default.WindowPosition;
                My_App.Top = bounds.Top;
                My_App.Left = bounds.Left;
                // Восстановить размеры, только если они
                // устанавливались для окна вручную
                //if (My_App.SizeToContent == SizeToContent.Manual)
                //{
                //    My_App.Width = bounds.Width;
                //    My_App.Height = bounds.Height;
                //}
            }
            catch
            {
                MessageBox.Show("Нет сохраненных параметров");
            }

            ImageBrushNight.ImageSource = img;

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

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void ExpanderLang_LostFocus(object sender, RoutedEventArgs e)
        {
            ExpanderLang.IsExpanded = false;
            //makeActiveButton();
        }

        private void My_App_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void Button_LostFocus(object sender, RoutedEventArgs e)
        {
            //ButtonMap.Focus();
            //FocusManager.SetFocusedElement(MainGrid, ButtonMap);
            //IInputElement focusedElement = FocusManager.GetFocusedElement(MainGrid);
            //Focus.Text = focusedElement.ToString();

            Properties.Settings.Default.CultureName = Properties.Resources.Name;
            Frame1.Refresh();
            //makeActiveButton();

            //borderInTemplate = (Border)ButtonCurrent.Template.FindName("Border", ButtonCurrent);
            ////borderInTemplate.Background = new SolidColorBrush(Color.FromArgb(255, 10, 10, 10));
            //borderInTemplate.Background = new SolidColorBrush(Colors.Yellow);
            //Keyboard.Focus(ButtonDay);
            //Focus.Text = v.ToString();

        }

        private void ButtonСurrent_Click(object sender, RoutedEventArgs e)
        {
            SystemSounds.Beep.Play();
            Properties.Settings.Default.DefaultPage = "/My Weather;component/Pages/CurrentForecastPage.xaml";
            Frame1.Source = uriCurrentForecast;
            Properties.Settings.Default.ActiveButton = 1;
            //img = new BitmapImage(new Uri("pack://application:,,,/My Weather;component/Images/Background/DaySpring1.jpg"));
            //Properties.Settings.Default.FrameBackground = "pack://application:,,,/My Weather;component/Images/Background/DaySpring1.jpg";
            img = new BitmapImage(new Uri("pack://application:,,,/My Weather;component/Images/Background/" + bgP.background_pictures[0]));
            Properties.Settings.Default.FrameBackground = "pack://application:,,,/My Weather;component/Images/Background/" + bgP.background_pictures[0];
            ImageBrushNight.ImageSource = img;
        }

        private void ButtonDay_Click(object sender, RoutedEventArgs e)
        {
            SystemSounds.Beep.Play();
            Properties.Settings.Default.DefaultPage = "/My Weather;component/Pages/DailyForecastPage.xaml";
            Frame1.Source = uriDailyForecast;
            Properties.Settings.Default.ActiveButton = 2;
            //img = new BitmapImage(new Uri("pack://application:,,,/My Weather;component/Images/Background/DaySpring.jpg"));
            //Properties.Settings.Default.FrameBackground = "pack://application:,,,/My Weather;component/Images/Background/DaySpring.jpg";
            img = new BitmapImage(new Uri("pack://application:,,,/My Weather;component/Images/Background/" + bgP.background_pictures[1]));
            Properties.Settings.Default.FrameBackground = "pack://application:,,,/My Weather;component/Images/Background/" + bgP.background_pictures[1];
            ImageBrushNight.ImageSource = img;
        }

        private void ButtonNight_Click(object sender, RoutedEventArgs e)
        {
            SystemSounds.Beep.Play();
            Properties.Settings.Default.DefaultPage = "/My Weather;component/Pages/NightForecastPage.xaml";
            Properties.Settings.Default.ActiveButton = 3;
            Frame1.Source = uriNightForecast;
            //img = new BitmapImage(new Uri("pack://application:,,,/My Weather;component/Images/Background/NightSpring.jpg"));
            //Properties.Settings.Default.FrameBackground = "pack://application:,,,/My Weather;component/Images/Background/NightSpring.jpg";
            img = new BitmapImage(new Uri("pack://application:,,,/My Weather;component/Images/Background/" + bgP.background_pictures[2]));
            Properties.Settings.Default.FrameBackground = "pack://application:,,,/My Weather;component/Images/Background/" + bgP.background_pictures[2];
            ImageBrushNight.ImageSource = img;
        }

        private void ButtonPeriod_Click(object sender, RoutedEventArgs e)
        {
            SystemSounds.Beep.Play();
            Properties.Settings.Default.DefaultPage = "/My Weather;component/Pages/PeriodPage.xaml";
            Frame1.Source = uriPeriod;
            Properties.Settings.Default.ActiveButton = 4;
        }

        private void ButtonMap_Click(object sender, RoutedEventArgs e)
        {
            SystemSounds.Beep.Play();
            Properties.Settings.Default.DefaultPage = "/My Weather;component/Pages/MapPage.xaml";
            Frame1.Source = uriMap;
            Properties.Settings.Default.ActiveButton = 6;
        }

        private void ButtonLocation_Click(object sender, RoutedEventArgs e)
        {
            SystemSounds.Beep.Play();
            Properties.Settings.Default.DefaultPage = "/My Weather;component/Pages/LocationPage.xaml";
            Frame1.Source = uriLocation;
            Properties.Settings.Default.ActiveButton = 5;
            //ButtonMap.Background = new SolidColorBrush(Colors.Yellow);

            //// Finding the border that is generated by the ControlTemplate of the Button
            //Border borderInTemplate = (Border)ButtonMap.Template.FindName("border", ButtonMap);
            //borderInTemplate.Background = new SolidColorBrush(Colors.Yellow);
        }

        private void MakeActiveButton()
        {
            switch (Properties.Settings.Default.ActiveButton)
            {
                case 1:
                    ButtonCurrent.Focus();
                    break;
                case 2:
                    ButtonDay.Focus();
                    break;
                case 3:
                    ButtonNight.Focus();
                    break;
                case 4:
                    ButtonPeriod.Focus();
                    break;
                case 5:
                    ButtonLocation.Focus();
                    break;
                default:
                    ButtonMap.Focus();
                    break;
            }
        }

        private void Frame1_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            MakeActiveButton();
        }

        private void My_App_LocationChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.WindowPosition = My_App.RestoreBounds;
        }
    }
}
