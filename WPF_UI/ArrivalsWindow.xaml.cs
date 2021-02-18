using BLApi;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WPF_UI
{
	/// <summary>
	/// Interaction logic for ArrivalsWindow.xaml
	/// </summary>
	public partial class ArrivalsWindow : Window
	{
		static IBL bl = BlFactory.GetBL();
		public ArrivalsWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			BusStation busStation = DataContext as BusStation;
			tbStation.Text = busStation.BusStationKey.ToString();

			List<string> timesList = (from lineId in busStation.LinesThatPass
									  let trip = bl.GetLineTrip(lineId, busStation.BusStationKey)
									  select (lineId + "%" + "קו מספר: " + bl.GetBusLine(lineId).BusLineNumber + " מגיע בשעה - " + trip.Departure.ToString(@"hh\:mm"))).ToList();

			foreach (string line in timesList)
			{
				int lineId = int.Parse(line.Split('%')[0]);
				string toDisplay = line.Split('%')[1];

				lbTimeTable.Items.Add(toDisplay);

				string bus = toDisplay.Split(' ')[2];
				BusLine busLine = bl.GetBusLine(lineId);
				DateTime start = Convert.ToDateTime(toDisplay.Split('-')[1].Trim());
				TimeSpan freq = new TimeSpan(0, bl.GetLineTrip(busLine.Id, busStation.BusStationKey).Frequency, 0);

				for (TimeSpan iter = start.TimeOfDay.Add(freq); iter < new TimeSpan(24, 0, 0); iter = iter.Add(freq))
				{
					lbTimeTable.Items.Add("קו מספר: " + bus + " מגיע בשעה - " + iter.ToString(@"hh\:mm"));
				}
			}
		}

		private void backButton_Click(object sender, RoutedEventArgs e)
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
