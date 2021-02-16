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
using System.Windows.Shapes;

namespace WPF_UI
{
    /// <summary>
    /// Interaction logic for OpeningWindow.xaml
    /// </summary>
    public partial class OpeningWindow : Window
    {
        public OpeningWindow()
        {
            InitializeComponent();
        }

        private void rideButton_Click(object sender, RoutedEventArgs e)
        {
            //UserWindow userWindow = new UserWindow();
            //userWindow.Show();
            LineTripWindow lineTripWindow = new LineTripWindow();
            lineTripWindow.Show();
            Close();
        }

        private void manageButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
