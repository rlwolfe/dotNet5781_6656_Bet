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
using BO;
using BLApi;

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
