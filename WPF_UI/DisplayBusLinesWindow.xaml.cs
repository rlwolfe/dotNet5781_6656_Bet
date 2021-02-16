using System;
using System.Collections.Generic;
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
