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
    /// Логика взаимодействия для LocationPage.xaml
    /// </summary>
    public partial class LocationPage : Page
    {
        private string input_text;
        public LocationPage()
        {
            InitializeComponent();
        }

        private void SearchText_KeyDown(object sender, KeyEventArgs e)
        {
            //input_text = ;
            if (e.KeyboardDevice.IsKeyDown(Key.Enter))
            {
                Seach_result.Content = SearchText.Text.Trim();
            }
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            Seach_result.Content = SearchText.Text.Trim();
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            Seach_result.Content = "";
            SearchText.Text = "";
        }
    }
}
