using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading;
using System.Diagnostics;
using BLApi;
using BO;

namespace WPF_UI
{
	/// <summary>
	/// Interaction logic for LineTripWindow.xaml
	/// </summary>
	public partial class LineTripWindow : Window
    {
        static IBL bl = BlFactory.GetBL();
        private Stopwatch stopWatch;
        private bool isTimerRun;
        private Thread timerThread;
        List<List<string>> timeInfo = new List<List<string>>();
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
                                     select " תחנה " + station.BusStationKey + " : " + station.StationName;

            foreach (LineTrip trip in bl.GetAllLineTrips())
            {
                for (TimeSpan iter = TimeSpan.Parse(trip.Departure.ToString(@"hh\:mm"));
                            iter < new TimeSpan(24, 0, 0); iter = iter.Add(new TimeSpan(0, trip.Frequency, 0)))
                {
                    List<string> times = new List<string>();
                    times.Add(trip.LineNumber.ToString());
                    times.Add(iter.ToString(@"hh\:mm"));
                    times.Add(trip.StationKey.ToString());

                    timeInfo.Add(times);
                }
            }
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
                BusStation busStation = bl.GetBusStation(int.Parse(station.Substring(6, 5)));

                List<string> timesList = (from time in timeInfo
                                          where busStation.BusStationKey == int.Parse(time[2])
                                          select "קו מספר: " + time[0] + " מגיע בשעה - " + time[1]).ToList();

                timesList = (from lineId in busStation.LinesThatPass
                             let trip = bl.GetLineTrip(lineId, busStation.BusStationKey)
                             select (lineId + "%" + "קו מספר: " + bl.GetBusLine(lineId).BusLineNumber +
                                        " מגיע בשעה - " + trip.Departure.ToString(@"hh\:mm"))).ToList();

                foreach (string line in timesList)
                {
                    int lineId = int.Parse(line.Split('%')[0]);
                    string toDisplay = line.Split('%')[1];
                    TimeSpan time = TimeSpan.Parse(line.Split('-')[1].Trim());

                    TimeSpan current = TimeSpan.Parse(timerTB.Text);

                    if(current.Hours.Equals(time.Hours))
                        lbBuses.Items.Add(toDisplay);

                    string bus = toDisplay.Split(' ')[2];
                    BusLine busLine = bl.GetBusLine(lineId);
                    DateTime start = Convert.ToDateTime(toDisplay.Split('-')[1].Trim());
                    TimeSpan freq = new TimeSpan(0, bl.GetLineTrip(busLine.Id, busStation.BusStationKey).Frequency, 0);

                    for (TimeSpan iter = TimeSpan.Parse(start.ToShortTimeString()).Add(freq); iter < new TimeSpan(24, 0, 0); iter = iter.Add(freq))
                    {
                        if(iter.Hours.Equals(current.Hours))
                            lbBuses.Items.Add("קו מספר: " + bus + " מגיע בשעה - " + iter.ToString(@"hh\:mm"));
                    }
                }
                //LineTrip trip = from t in bl.GetAllLineTrips()
                //                where t.StationKey == int.Parse(station.Substring(6, 5))
                //                select "מגיע בעוד: " + t.Arrival + "דקות ";
                //lbBuses.ItemsSource = from line in bl.GetAllBusLines()
                //                      where line.AllStationsOfLine.Contains(int.Parse(station.Substring(6, 5)))
                //                      select "קו: " + line.BusLineNumber + " : לכיוון " + bl.GetBusStation(line.LastStationKey).StationName;// + bl.CalculateDistance();
                UpdateLayout();
            }
        }

        private void tb_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;
            e.Handled = !(key > 33 && key < 44 || key > 73 && key < 84 || key == 2);
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
