using BLApi;
using System.Windows;

namespace WPF_UI
{
	/// <summary>
	/// Interaction logic for UserWindow.xaml
	/// </summary>
	public partial class UserWindow : Window
	{
		static IBL bl = BlFactory.GetBL();
		public UserWindow()
		{
			InitializeComponent();
		}

		private void EnterButton_Click(object sender, RoutedEventArgs e)
		{
			if (bl.UserVerified(tbUser.Text, tbPass.Password))
			{
				OpeningWindow openingWindow = new OpeningWindow() { DataContext = bl.UserAdmin(tbUser.Text) };
				openingWindow.Show();
				Close();
			}
			else
				MessageBox.Show("שם משתמש או סיסמה שגוי");
		}

		private void NewUserButton_Click(object sender, RoutedEventArgs e)
		{
			NewUserWindow newUserWindow = new NewUserWindow();
			newUserWindow.Show();
			Close();
		}

		private void ChangePassButton_Click(object sender, RoutedEventArgs e)
		{
			//ChangePassWindow changePassWindow = new ChangePassWindow();
			//changePassWindow.Show();
			//Close();
		}
	}
}
