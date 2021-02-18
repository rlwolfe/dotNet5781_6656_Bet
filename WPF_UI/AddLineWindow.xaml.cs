using BLApi;
using BO;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace WPF_UI
{
	/// <summary>
	/// Interaction logic for AddLineWindow.xaml
	/// </summary>
	public partial class AddLineWindow : Window
	{
		static IBL bl = BlFactory.GetBL();
		static BusLine busLine;
		public AddLineWindow()
		{
			InitializeComponent();

			cbArea.ItemsSource = Enum.GetValues(typeof(BO.Areas));

			busLine = new BusLine();
			DataContext = busLine;
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
			LineStationsListBox.SelectedIndex = 0;
		}

		private void RemoveButton_Click(object sender, RoutedEventArgs e)
		{
			if (LineStationsListBox.Items.IsEmpty)
				return;

			int currentItemIndex = LineStationsListBox.SelectedIndex;

			if (currentItemIndex == -1)
				currentItemIndex += LineStationsListBox.Items.Count;

			LineStationsListBox.Items.RemoveAt(currentItemIndex);
		}

		private void tb_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			int key = (int)e.Key;
			//normal numbers        keypad numbers
			e.Handled = !(key > 33 && key < 44 || key > 73 && key < 84 ||
						//arrows           delete      backspace    Num-Lock
						key == 23 || key == 25 || key == 32 || key == 2 || key == 114);
		}

		private void AddLineButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				string error = "";
				if (tbLineNumber.Text.Length == 0)
					error += "חייב למלא מספר קו\n";

				if (cbArea.SelectedIndex == -1)
					error += "חייב לבחור אזור\n";

				if (LineStationsListBox.Items.Count < 2)
					error += "אין מספיק תחנות בקו\n";

				if (error != "")
				{
					MessageBox.Show(error);
					return;
				}

				busLine = new BusLine();
				DataContext = busLine;

				busLine.AllStationsOfLine = from string item in LineStationsListBox.Items
											select int.Parse(item.Substring(6, 5));

				busLine.BusLineNumber = int.Parse(tbLineNumber.Text);
				busLine.Area = (BO.Areas)cbArea.SelectedItem;
				busLine.FirstStationKey = busLine.AllStationsOfLine.First();
				busLine.LastStationKey = busLine.AllStationsOfLine.Last();

				bl.AddBusLine(busLine);
				LineStationsListBox.Items.Clear();
				Close();
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

