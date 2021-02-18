using BLApi;
using System.Windows;

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
			if (tbUser.Text == "" || tbPass.Text == "")
				MessageBox.Show("תמלא את השדות");
			else
			{
				bl.AddNewUser(tbUser.Text, tbPass.Text, WorkerCheckBox.IsChecked.Value);
				Close();
			}
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			UserWindow userWindow = new UserWindow();
			userWindow.Show();
		}
	}
}
