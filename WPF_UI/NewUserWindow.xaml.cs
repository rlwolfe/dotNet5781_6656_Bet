using System.Windows;
using BLApi;

namespace WPF_UI
{
	/// <summary>
	/// Interaction logic for NewUserWindow.xaml
	/// </summary>
	public partial class NewUserWindow : Window
	{
		static IBL bl = BlFactory.GetBL();
		public NewUserWindow()
		{
			InitializeComponent();
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			bl.AddNewUser(tbUser.Text, tbPass.Text, WorkerCheckBox.IsChecked.Value);
			Close();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			UserWindow userWindow = new UserWindow();
			userWindow.Show();
		}
	}
}
