using Dapper;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace MethodicalService.Forms
{
    /// <summary>
    /// Логика взаимодействия для Distribution_log.xaml
    /// </summary>
    public partial class Distribution_log : UserControl
    {
        public SqlDataAdapter? adapter = null;
        public DataTable? distributionTable = null;

        public Distribution_log()
        {
            InitializeComponent();
        }

        public void SelectDataFromServer()
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
                distributionTable = new DataTable();
                adapter = new SqlDataAdapter(command);
                adapter.Fill(distributionTable);
                distributionGrid.ItemsSource = distributionTable.DefaultView;
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
            SelectDataFromServer();
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
                SelectDataFromServer();
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
            SelectDataFromServer();
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

                    distributionTable = new DataTable();
                    adapter = new SqlDataAdapter(command);
                    adapter.Fill(distributionTable);
                    distributionGrid.ItemsSource = distributionTable.DefaultView;
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
                SelectDataFromServer();
            }
        }
    }
}
