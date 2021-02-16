using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using BLApi;
using BO;

namespace WPF_UI
{
    /// <summary>
    /// Interaction logic for AddLineWindow.xaml
    /// </summary>
    public partial class AddLineWindow : Window
    {
        static IBL bl = BlFactory.GetBL();
        static BusLine busLine;
        static ObservableCollection<int> stations = new ObservableCollection<int>();
        public AddLineWindow()
        {
            InitializeComponent();

            cbArea.ItemsSource = Enum.GetValues(typeof(BO.Areas));

            busLine = new BusLine();
            DataContext = busLine;

            cbFirstStop.ItemsSource = bl.GetAllBusStations();
            cbLastStop.ItemsSource = bl.GetAllBusStations();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AllStationsListBox.ItemsSource = from station in bl.GetAllBusStations()
                                             select (" תחנה " + station.BusStationKey + " : " + station.StationName);
            AllStationsListBox.SelectedIndex = 0;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string currentItemText = AllStationsListBox.SelectedValue.ToString();
            if (!LineStationsListBox.Items.Cast<String>().Any(s => s.ToString() == currentItemText))
                LineStationsListBox.Items.Add(currentItemText);
            else
                MessageBox.Show("תחנה זו כבר ברשימה");
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            int currentItemIndex = LineStationsListBox.SelectedIndex;
            LineStationsListBox.Items.RemoveAt(currentItemIndex);
        }
        private void tb_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;
            e.Handled = !(key > 33 && key < 44 || key > 73 && key < 84 || key == 2);
        }

        private void AddLineButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tbLineNumber.Text.Length == 0 || cbArea.SelectedIndex == -1 || cbFirstStop.SelectedIndex == -1 || cbLastStop.SelectedIndex == -1)
                    throw new Exception();

                else
                {
                    busLine = new BusLine();
                    DataContext = busLine;

                    busLine.AllStationsOfLine = from string item in LineStationsListBox.Items
                                                select int.Parse(item.Substring(6, 5));

                    string lbFirst = (string)LineStationsListBox.Items.GetItemAt(0);
                    int lbf = int.Parse(lbFirst.Substring(6, 5));
                    BusStation firstStation = cbFirstStop.SelectedValue as BusStation;
                    if (firstStation.BusStationKey != lbf)
                        busLine.AllStationsOfLine.Append(firstStation.BusStationKey);


                    string lbLast = (string)LineStationsListBox.Items.GetItemAt(LineStationsListBox.Items.Count - 1);
                    int lbl = int.Parse(lbLast.Substring(6, 5));
                    BusStation lastStation = cbLastStop.SelectedValue as BusStation;
                    if (lastStation.BusStationKey != lbl)
                        busLine.AllStationsOfLine.Append(lastStation.BusStationKey);

                    busLine.BusLineNumber = int.Parse(tbLineNumber.Text);
                    busLine.Area = (BO.Areas)cbArea.SelectedItem;
                    busLine.FirstStationKey = firstStation.BusStationKey;
                    busLine.LastStationKey = lastStation.BusStationKey;
                    busLine.TotalDistance = double.Parse(tbTotalDistance.Text);
                    busLine.TotalTime = double.Parse(tbTotalTime.Text);


                    bl.AddBusLine(busLine);
                    LineStationsListBox.Items.Clear();
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error thrown: " + ex);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}

