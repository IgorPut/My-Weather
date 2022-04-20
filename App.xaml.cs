using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using My_Weather.Localization;

namespace My_Weather
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {

        //Запуск через SturtupUri в App.xaml
        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    base.OnStartup(e);

        //    LocalizationManager.Instance.LocalizationProvider = new ResxLocalizationProvider();

        //}

        //Запуск через Sturtup в App.xaml. Позволяет манипулировать стартовым окном до его появления. В данном варианте просто эксперимент. Никаких манипуляций с окном не выполняется
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //base.OnStartup(e);

            LocalizationManager.Instance.LocalizationProvider = new ResxLocalizationProvider();

            // Create the startup window
            MainWindow wnd = new MainWindow();
            // Do stuff here, e.g. to the window
            //wnd.Title = "Something else";
            // Show the window
            wnd.Show();
        }

    }
}
