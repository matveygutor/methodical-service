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


namespace MethodicalService
{
    /// <summary>
    /// Логика взаимодействия для Authentication.xaml
    /// </summary>
    public partial class Authentication : Window
    {
        public Authentication()
        {
            InitializeComponent();
            btLogin.IsEnabled = false;
            tbLogin.Focus();
        }
        public string? Login { get; set; }
        public string? Password { get; set; }

        private void tbLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            Login = tbLogin.Text;
            if (String.IsNullOrWhiteSpace(Login) || String.IsNullOrWhiteSpace(Password))
                btLogin.IsEnabled = false;
            else
                btLogin.IsEnabled = true;
        }

        private void tbPassword_TextChanged(object sender, RoutedEventArgs e)
        {
            Login = tbLogin.Text;
            Password = tbPassword.Password;
            if (String.IsNullOrWhiteSpace(Login) || String.IsNullOrWhiteSpace(Password))
                btLogin.IsEnabled = false;
            else
                btLogin.IsEnabled = true;
        }
        private void btLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Login = "iga";
                MainWindow window = new MainWindow();
                
                window.Show();
                this.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка при подключении к базе данных!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ClearTextBoxes()
        {
            tbLogin.Text = "";
            tbPassword.Password = "";
            tbLogin.Focus();
        }
    }
}
