using System.Windows;

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

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			rideButton.IsEnabled = !bool.Parse(DataContext.ToString());
			manageButton.IsEnabled = bool.Parse(DataContext.ToString());
		}

		private void rideButton_Click(object sender, RoutedEventArgs e)
		{
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
			UserWindow userWindow = new UserWindow();
			userWindow.Show();
			Close();
		}
	}
}
