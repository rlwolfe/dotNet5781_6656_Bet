using BLApi;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF_UI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		static IBL bl = BlFactory.GetBL();
		public MainWindow()
		{
			InitializeComponent();
			BusStationsDg.ItemsSource = bl.GetAllBusStations();
			BusLinesDg.ItemsSource = bl.GetAllBusLines();
		}

		private void BusStationsDg_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			BusStation selectedStation = BusStationsDg.SelectedItem as BusStation;

			IEnumerable<string> LinesInStation = from lineId in selectedStation.LinesThatPass
												 let line = bl.GetBusLine(lineId)
												 select (" קו " + line.BusLineNumber + " : לכיוון " + bl.GetBusStation(line.LastStationKey).StationName);

			LinesPassListBox.ItemsSource = LinesInStation;
		}

		private void Delete_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				BusStation selectedStation = BusStationsDg.SelectedItem as BusStation;
				bl.DeleteStation(selectedStation.BusStationKey);
				MessageBox.Show("התחנה נמחקה בהצלחה");
				Refresh();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void Update_Click(object sender, RoutedEventArgs e)
		{
			UpdateStationWindow updateStationWindow = new UpdateStationWindow { DataContext = BusStationsDg.SelectedItem };
			updateStationWindow.Show();
			Close();
		}

		private void ViewArrivals_Click(object sender, RoutedEventArgs e)
		{
			ArrivalsWindow arrivalsWindow = new ArrivalsWindow { DataContext = BusStationsDg.SelectedItem };
			arrivalsWindow.Show();
			Close();
		}

		private void AddStationButton_Click(object sender, RoutedEventArgs e)
		{
			AddStationWindow newWindow = new AddStationWindow();
			newWindow.Show();
			Close();
		}

		private void AddLineButton_Click(object sender, RoutedEventArgs e)
		{
			AddLineWindow newWindow = new AddLineWindow();
			newWindow.Show();
			Close();
		}

		private void BusList_Click(object sender, RoutedEventArgs e)
		{
			DataGrid tempDG = (DataGrid)sender;
			BusStation tempS = (BusStation)tempDG.SelectedItem;
			int key = tempS.BusStationKey;

			DisplayBusLinesWindow newWindow = new DisplayBusLinesWindow { DataContext = bl.GetBusStation(tempS.BusStationKey) };

			List<BusLine> lines = new List<BusLine>();
			foreach (BusLine line in bl.GetAllBusLines())
				foreach (int stop in line.AllStationsOfLine)
					if (stop == key)
					{
						lines.Add(line);
						break;
					}

			newWindow.lbBusLines.ItemsSource = lines;
			newWindow.Show();
		}

		private void Refresh()
		{
			MainWindow newWindow = new MainWindow();
			newWindow.Show();
			Close();
		}

		private void BusLinesDg_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			BusLine selectedStation = BusLinesDg.SelectedItem as BusLine;
			List<string> StationsInLine = new List<string>();
			foreach (int item in selectedStation.AllStationsOfLine)
				StationsInLine.Add("תחנה" + item + " : " + bl.GetBusStation(item).StationName);
			StationsListBox.ItemsSource = StationsInLine;
		}

		private void DeleteLine_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				BusLine selectedStation = BusLinesDg.SelectedItem as BusLine;
				bl.DeleteBusLine(selectedStation);
				MessageBox.Show("הקו נמחק בהצלחה");
				Refresh();
			}
			catch (Exception ex)
			{
				MessageBox.Show("An error occured " + ex);
			}
		}

		private void UpdateLine_Click(object sender, RoutedEventArgs e)
		{
			UpdateLineWindow updateLineWindow = new UpdateLineWindow { DataContext = BusLinesDg.SelectedItem };
			updateLineWindow.Show();
			Close();
		}

		private void simButton_Click(object sender, RoutedEventArgs e)
		{
			LineTripWindow tripWindow = new LineTripWindow();
			tripWindow.Show();
			Close();
		}
		private void tb_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			int key = (int)e.Key;
			//normal numbers        keypad numbers
			e.Handled = !(key > 33 && key < 44 || key > 73 && key < 84 ||
						//arrows           delete      backspace    Num-Lock
						key == 23 || key == 25 || key == 32 || key == 2 || key == 114);
		}
		private void searchButton_Click(object sender, RoutedEventArgs e)
		{
			if (tabs.SelectedItem == buses)
				SearchBusList();

			else if (tabs.SelectedItem == stops)
				SearchStopList();
		}
		private void SearchBusList()
		{
			var filteredView = bl.GetAllBusLines().Where(x => x.FirstStationKey.Equals(int.Parse(searchBox.Text))
														  || x.LastStationKey.Equals(int.Parse(searchBox.Text)));

			BusLinesDg.ItemsSource = filteredView;
		}
		private void SearchStopList()
		{
			var filteredView = bl.GetAllBusStations().Where(x => x.BusStationKey.Equals(int.Parse(searchBox.Text)));

			BusStationsDg.ItemsSource = filteredView;
		}
		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			BusStationsDg.ItemsSource = bl.GetAllBusStations();
			BusLinesDg.ItemsSource = bl.GetAllBusLines();
		}
	}
}
