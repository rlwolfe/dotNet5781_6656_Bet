using BLApi;
using BO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF_UI
{
	/// <summary>
	/// Interaction logic for LineTripWindow.xaml
	/// Simulation window for bus stop time throughout the day
	/// each station will show the buses arriving within the hour of when the simulation timer shows
	/// the rate can be change to anytime up to an hour per second (3599 in the textbox + the one second from from real time)
	/// </summary>
	public partial class LineTripWindow : Window
	{
		static IBL bl = BlFactory.GetBL();
		private Stopwatch stopWatch;
		private bool isTimerRun;
		private Thread timerThread;
		private TimeSpan start = TimeSpan.Zero;
		private int rate = 60;
		public LineTripWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			stopWatch = new Stopwatch();
			timerTB.Text = stopWatch.Elapsed.ToString();
			tbRunningRate.Text = rate.ToString();

			lbStations.ItemsSource = from station in bl.GetAllBusStations()
									 select "תחנה " + station.BusStationKey + " : " + station.StationName;
		}

		private void StartClock()
		{
			if (!isTimerRun)
			{
				stopWatch.Restart();
				isTimerRun = true;

				timerThread = new Thread(RunTimer);
				timerThread.Start();
			}
		}

		private void setButton_Click(object sender, RoutedEventArgs e)
		{
			string rateStr = tbRunningRate.Text;
			if (rateStr != "")
			{
				if (rate < 3600)
					rate = int.Parse(rateStr);
				else
					MessageBox.Show("!הקבצ מהיר מידי");
			}
			else
				MessageBox.Show("השדה ריק");
		}

		private void RunTimer()
		{
			TimeSpan sim = TimeSpan.Zero;
			while (isTimerRun)
			{
				sim = sim.Add(new TimeSpan(0, 0, rate));
				TimeSpan timer = sim.Add(start.Add(stopWatch.Elapsed));

				if (!timer.Days.Equals(0))
					timer = timer.Subtract(new TimeSpan(timer.Days, 0, 0, 0));

				string timerText = timer.ToString();
				timerText = timerText.Substring(0, 8);

				SetTimer(timerText);
				Thread.Sleep(1000);
			}
		}

		private void SetText(string text)
		{
			timerTB.Text = text;
		}

		private void SetTimer(string timerText)
		{
			if (!CheckAccess())
			{
				Action<string> d = SetText;
				Dispatcher.BeginInvoke(d, timerText);
			}
			else
			{
				timerTB.Text = timerText;
			}
		}

		private void startSimulationButton_Click(object sender, RoutedEventArgs e)
		{
			timerTB.Text = start.ToString();
			start = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
			StartClock();
		}

		private void lbStations_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			lbBuses.Items.Clear();
			ListBox lb = sender as ListBox;

			if (lb.SelectedIndex != -1)
			{
				string station = lb.SelectedItem as string;
				BusStation busStation = bl.GetBusStation(int.Parse(station.Substring(5, 5)));

				List<string> timesList = (from lineId in busStation.LinesThatPass
										  let trip = bl.GetLineTrip(lineId, busStation.BusStationKey)
										  select (lineId + "%" + "קו מספר: " + bl.GetBusLine(lineId).BusLineNumber
										  + " מגיע בשעה - " + trip.Departure.ToString(@"hh\:mm"))).ToList();

				foreach (string line in timesList)
				{
					int lineId = int.Parse(line.Split('%')[0]);
					string toDisplay = line.Split('%')[1];

					TimeSpan time = TimeSpan.Parse(line.Split('-')[1].Trim());
					TimeSpan current = TimeSpan.Parse(timerTB.Text);

					if (current.Hours.Equals(time.Hours))
						lbBuses.Items.Add(toDisplay);

					string bus = toDisplay.Split(' ')[2];
					BusLine busLine = bl.GetBusLine(lineId);

					DateTime start = Convert.ToDateTime(toDisplay.Split('-')[1].Trim());
					TimeSpan freq = new TimeSpan(0, bl.GetLineTrip(busLine.Id, busStation.BusStationKey).Frequency, 0);

					for (TimeSpan iter = start.TimeOfDay.Add(freq); iter < new TimeSpan(24, 0, 0); iter = iter.Add(freq))
					{
						if (iter.Hours.Equals(current.Hours))
							lbBuses.Items.Add("קו מספר: " + bus + " מגיע בשעה - " + iter.ToString(@"hh\:mm"));
					}
				}
				UpdateLayout();
			}
		}

		private void tb_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			int key = (int)e.Key;
			//normal numbers        keypad numbers
			e.Handled = !(key > 33 && key < 44 || key > 73 && key < 84 ||
						//arrows           delete      backspace    Num-Lock
						key == 23 || key == 25 || key == 32 || key == 2 || key == 114);
		}
		private void endButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Environment.Exit(Environment.ExitCode);
		}

	}
}
