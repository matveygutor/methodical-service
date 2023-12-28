using Dapper;
using MethodicalService.Service;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
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
using System.Xml.Linq;


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
                using var connection = new SqlConnection(Properties.Resources.connectionString);
                connection.Open();
                string hash = connection.Query<string>($"SELECT hash FROM user_identity WHERE login = '{Login}'").SingleOrDefault(); 
 
                byte[] salt = GenerateSalt();

                // создаем Sha256-хеш
                byte[] sha256Hash = GenerateSha256Hash(Password, salt);
                string sha256HashString = Convert.ToBase64String(sha256Hash);
                
                if (hash.Equals(sha256HashString))
                {
                    string name = connection.Query<string>($"SELECT name + ' ' + fathername FROM user_identity WHERE login = '{Login}'").SingleOrDefault();
                    MessageBox.Show($"Добро пожаловать, {name}", "Вход в систему", MessageBoxButton.OK, MessageBoxImage.Information);

                    string role = connection.Query<string>($"SELECT role_name FROM user_role WHERE id_role = (SELECT user_role FROM user_identity WHERE login = '{Login}')").SingleOrDefault();


                    User user = new(name, role);

                    MainWindow window = new(user);

                    window.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show($"Неверный логин или пароль", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                    ClearTextBoxes();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка при подключении к базе данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        static byte[] GenerateSha256Hash(string password, byte[] salt)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltedPassword = new byte[salt.Length + passwordBytes.Length];

            SHA256CryptoServiceProvider hash = new();

            return hash.ComputeHash(saltedPassword);
        }
        static byte[] GenerateSalt()
        {
            const int SaltLength = 64;
            byte[] salt = new byte[SaltLength];

            var rngRand = new RNGCryptoServiceProvider();
            rngRand.GetBytes(salt);

            return salt;
        }

        public void ClearTextBoxes()
        {
            tbLogin.Text = "";
            tbPassword.Password = "";
            tbLogin.Focus();
        }
    }
}
