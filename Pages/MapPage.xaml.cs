using System;
using System.Collections.Generic;
using System.ComponentModel;
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

using Microsoft.Maps.MapControl;



namespace My_Weather
{
    /// <summary>
    /// Логика взаимодействия для MapPage.xaml
    /// </summary>
    //public partial class MapPage : Page
    public sealed partial class MapPage : Page//, INotifyPropertyChanged
    {
        // TODO WTS: Set your preferred default zoom level
        private const double DefaultZoomLevel = 17;

        //public event PropertyChangedEventHandler PropertyChanged;

        //public MapPage()
        //{
        //    InitializeComponent();
        //}
    }
}
