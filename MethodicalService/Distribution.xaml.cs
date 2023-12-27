using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
    /// Логика взаимодействия для Distribution.xaml
    /// </summary>
    public partial class Distribution : Window
    {
        public SqlDataAdapter? adapter = null;
        public DataTable? table = null;
        public int Id { get; set; }

        public Distribution(int id)
        {
            Id = id;
            InitializeComponent();
        }

        public void SelectDataFromServer(int id)
        {
            string sqlExpression = "sp_GetDistribution";
            using SqlConnection connection = new(Properties.Resources.connectionString);
            try
            {
                connection.Open();
                SqlCommand command = new(sqlExpression, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                SqlParameter id_Parameter = new() { ParameterName = "@id_receipts", Value = id };
                command.Parameters.Add(id_Parameter);

                table = new DataTable();
                adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
                distributionGrid.ItemsSource = table.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection?.Close();
            }
        }

        private void DistributionGrid_Loaded(object sender, RoutedEventArgs e)
        {
            SelectDataFromServer(Id);
        }

        private void MenuItemEdit_Click(object sender, RoutedEventArgs e)
        {
            if (distributionGrid.CurrentItem != null)
            {
                var row = (distributionGrid.CurrentItem as DataRowView).Row;
                AddDistribution add = new();
                add.textNumberUPD.IsReadOnly = true;
                add.buttonDistribut.Visibility = Visibility.Collapsed;
                add.buttonEdit.Visibility = Visibility.Visible;
                add.BuildForEdit(row);
                add.ShowDialog();
                SelectDataFromServer(Id);
            }
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            string sqlExpression = "sp_DeleteDistribution";
            if (distributionGrid.SelectedItems != null)
            {
                if (MessageBox.Show("Вы действительно хотите удалить эту запись?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    for (int i = 0; i < distributionGrid.SelectedItems.Count; i++)
                    {
                        if (distributionGrid.SelectedItems[i] is DataRowView)
                        {
                            var row = ((DataRowView)distributionGrid.CurrentItem).Row;
                            string id = row["ID_Distribution"].ToString();
                            using SqlConnection connection = new(Properties.Resources.connectionString);
                            connection.Open();
                            SqlCommand command = new(sqlExpression, connection)
                            {
                                CommandType = CommandType.StoredProcedure
                            };

                            SqlParameter id_Parameter = new() { ParameterName = "@id", Value = id };
                            command.Parameters.Add(id_Parameter);
                            if (command.ExecuteNonQuery() != -1)
                            {
                                MessageBox.Show($"Экземпляр с номером {row["ID_Receipts"]} успешно удален", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                }
            }
            SelectDataFromServer(Id);
        }

        private void TextSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            textSearch.Clear();
        }

        private void TextSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textSearch.Text == string.Empty)
            {
                textSearch.Text = "Поиск";
            }
        }

        private void TextSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textSearch.Text != string.Empty)
            {
                string sqlExpression = "sp_SearchDistribution";
                SqlConnection connection = new(Properties.Resources.connectionString);
                try
                {
                    connection.Open();
                    SqlCommand command = new(sqlExpression, connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    SqlParameter value_Parameter = new() { ParameterName = "@value", Value = textSearch.Text };
                    command.Parameters.Add(value_Parameter);

                    SqlParameter id_Parameter = new() { ParameterName = "@id", Value = Id };
                    command.Parameters.Add(id_Parameter);

                    table = new DataTable();
                    adapter = new SqlDataAdapter(command);
                    adapter.Fill(table);
                    distributionGrid.ItemsSource = table.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection?.Close();
                }
            }
            else
            {
                SelectDataFromServer(Id);
            }
        }
    }
}
