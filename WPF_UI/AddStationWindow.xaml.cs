using BLApi;
using System;
using System.Windows;

namespace WPF_UI
{
	/// <summary>
	/// Interaction logic for OpeningWindow.xaml
	/// </summary>
	public partial class AddStationWindow : Window
	{
		static IBL bl = BlFactory.GetBL();
		public AddStationWindow()
		{
			InitializeComponent();
		}

		private void AddStationButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				int stationCode;
				int.TryParse(busStationKeyTextBox.Text, out stationCode);
				bl.AddStation(new BO.BusStation
				{
					BusStationKey = stationCode,
					Address = addressTextBox.Text,
					StationName = stationNameTextBox.Text
				});
				MessageBox.Show("התחנה נוספה בהצלחה");
				Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(" : ארעה תקלה/שגיאה" + ex);
			}

		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
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
