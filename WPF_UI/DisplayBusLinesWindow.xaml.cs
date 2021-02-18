using BLApi;
using BO;
using System.Linq;
using System.Windows;

namespace WPF_UI
{
	/// <summary>
	/// Interaction logic for DisplayBusLinesWindow.xaml
	/// </summary>
	public partial class DisplayBusLinesWindow : Window
	{
		static IBL bl = BlFactory.GetBL();
		public DisplayBusLinesWindow()
		{
			InitializeComponent();
		}

		private void AddLineButton_Click(object sender, RoutedEventArgs e)
		{
			BusStation Station = (BusStation)DataContext;
		}

		private void DeleteLineButton_Click(object sender, RoutedEventArgs e)
		{
			BusStation Station = (BusStation)DataContext;
			BusLine busLine = (BusLine)lbBusLines.SelectedItem;
			busLine.AllStationsOfLine = busLine.AllStationsOfLine.Where(s => s != Station.BusStationKey);
			bl.UpdateBusLine(busLine);
			MessageBox.Show("Line " + busLine.BusLineNumber + " in the " + busLine.Area + " region, was deleted from " + Station.BusStationKey);
			Close();
		}
	}
}
