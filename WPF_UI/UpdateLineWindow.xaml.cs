using BLApi;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WPF_UI
{
	/// <summary>
	/// Interaction logic for UpdateLineWindow.xaml
	/// </summary>
	public partial class UpdateLineWindow : Window
	{
		static IBL bl = BlFactory.GetBL();
		public BusLine busLine;
		public UpdateLineWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			AllStationsListBox.SelectedIndex = 0;
			LineStationsListBox.SelectedIndex = 0;
			busLine = DataContext as BusLine;

			tbArea.Text = Enum.GetName(typeof(BO.Areas), busLine.Area);
			tbLineNumber.Text = busLine.BusLineNumber.ToString();

			tbTotalDistance.Text = busLine.TotalDistance.ToString();
			tbTotalTime.Text = busLine.TotalTime.ToString();

			List<string> myStationsList = (from key in (DataContext as BusLine).AllStationsOfLine
										   let station = bl.GetBusStation(key)
										   select (" תחנה " + station.BusStationKey + " : " + station.StationName)).ToList();
			foreach (string item in myStationsList)
			{
				LineStationsListBox.Items.Add(item);
			}
			AllStationsListBox.ItemsSource = from station in bl.GetAllBusStations()
											 select (" תחנה " + station.BusStationKey + " : " + station.StationName);
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			string currentItemText = AllStationsListBox.SelectedValue.ToString();
			if (!LineStationsListBox.Items.Cast<String>().Any(s => s.ToString() == currentItemText))
				LineStationsListBox.Items.Add(currentItemText);
			else
				MessageBox.Show("תחנה זו כבר ברשימה");
			LineStationsListBox.SelectedIndex = 0;
		}

		private void RemoveButton_Click(object sender, RoutedEventArgs e)
		{
			int currentItemIndex = LineStationsListBox.SelectedIndex;
			if (LineStationsListBox.Items.IsEmpty)
				return;
			else if (currentItemIndex == -1)
				currentItemIndex += LineStationsListBox.Items.Count;

			LineStationsListBox.Items.RemoveAt(currentItemIndex);
			LineStationsListBox.SelectedIndex = currentItemIndex - 1;

			if (LineStationsListBox.SelectedIndex == -1)
				LineStationsListBox.SelectedIndex = 0;
		}

		private void UpdateLineButton_Click(object sender, RoutedEventArgs e)
		{
			if (LineStationsListBox.Items.Count < 2)
			{
				MessageBox.Show("אין מספיק תחנות בקו");
				return;
			}

			try
			{
				busLine.TotalDistance = double.Parse(tbTotalDistance.Text);
				busLine.TotalTime = double.Parse(tbTotalTime.Text);

				busLine.AllStationsOfLine = from string item in LineStationsListBox.Items
											select int.Parse(item.Substring(6, 5));

				string lbFirst = (string)LineStationsListBox.Items.GetItemAt(0);
				busLine.FirstStationKey = int.Parse(lbFirst.Substring(6, 5));

				string lbLast = (string)LineStationsListBox.Items.GetItemAt(LineStationsListBox.Items.Count - 1);
				busLine.LastStationKey = int.Parse(lbLast.Substring(6, 5));

				bl.UpdateBusLine(busLine);
				DataContext = busLine;
				Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error thrown: " + ex);
			}
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			MainWindow mainWindow = new MainWindow();
			mainWindow.Show();
		}
	}
}
