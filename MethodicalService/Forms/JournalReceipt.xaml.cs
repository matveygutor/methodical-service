using Dapper;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MethodicalService.Forms
{
    public partial class JournalReceipt : UserControl
    {
        SqlDataAdapter? adapter = null;
        DataTable? table = null;

        public JournalReceipt()
        {
            InitializeComponent();

            try
            {
                using var connection = new SqlConnection(Properties.Resources.connectionString);
                connection.Open();
                List<string> values = (List<string>)connection.Query<string>("SELECT Name FROM Specialty");
                values.Add("Все");
                specialityComboBox.ItemsSource = values;
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            SelectDataFromServer("sp_GetReceipt");
        }

        public void SelectDataFromServer(string sqlExpression)
        {
            SqlConnection connection = new(Properties.Resources.connectionString);
            try
            {
                connection.Open();
                SqlCommand command = new(sqlExpression, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                table = new DataTable();
                adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
                receiptGrid.ItemsSource = table.DefaultView;
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

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddReceipt addReceipt = new();
            addReceipt.buttonEdit.Visibility = Visibility.Hidden;
            DataRow row = table.NewRow();
            addReceipt.Build(row);
            if (addReceipt.ShowDialog() == true)
                table.Rows.Add(row);
            SelectDataFromServer("sp_GetReceipt");
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (receiptGrid.CurrentItem != null)
            {
                var row = (receiptGrid.CurrentItem as DataRowView).Row;
                AddReceipt add = new();
                add.buttonRegistr.Visibility = Visibility.Hidden;
                add.Build(row);
                add.ShowDialog();
                SelectDataFromServer("sp_GetReceipt");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            string sqlExpression = "sp_DeleteReceipt";
            if (receiptGrid.SelectedItems != null)
            {
                if (MessageBox.Show("При удалении УПД будут удалены все распределенные экземпляры\nВы действительно хотите удалить эту запись?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    for (int i = 0; i < receiptGrid.SelectedItems.Count; i++)
                    {
                        if (receiptGrid.SelectedItems[i] is DataRowView)
                        {

                            var row = (receiptGrid.CurrentItem as DataRowView).Row;
                            string id = row["ID_Receipts"].ToString();
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
                                MessageBox.Show($"УПД с номером {id} успешно удалена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                }
            }
            SelectDataFromServer("sp_GetReceipt");
        }

        private void Filters_Click(object sender, RoutedEventArgs e)
        {
            
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
            SelectDataFromServer("sp_GetReceipt");
        }

        private void TextSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textSearch.Text != string.Empty)
            {
                string sqlExpression = "sp_SearchReceipts";
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

                    table = new DataTable();
                    adapter = new SqlDataAdapter(command);
                    adapter.Fill(table);
                    receiptGrid.ItemsSource = table.DefaultView;
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
                SelectDataFromServer("sp_GetReceipt");
            }
        }

        private void Distribution_Click(object sender, RoutedEventArgs e)
        {
            AddDistribution addDistribution = new();
            addDistribution.textNumberUPD.IsReadOnly = true;
            addDistribution.textName.IsReadOnly = true;
            try
            {
                if (receiptGrid.SelectedItem != null)
                {
                    DataRow row = (receiptGrid.CurrentItem as DataRowView).Row;
                    addDistribution.Show();
                    addDistribution.buttonEdit.Visibility = Visibility.Collapsed;
                    addDistribution.BuildForAdd(row);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source);
            }
        }

        private void MenuAddEditDelete_Opened(object sender, RoutedEventArgs e)
        {

            if (receiptGrid.SelectedIndex == -1)
            {
                MenuItemDelete.Visibility = Visibility.Collapsed;
                MenuItemEdit.Visibility = Visibility.Collapsed;
                MenuItemRegistr.Visibility = Visibility.Collapsed;
                separator.Visibility = Visibility.Collapsed;
            }
            if (receiptGrid.SelectedIndex != -1)
            {
                MenuItemAdd.Visibility = Visibility.Collapsed;
                MenuItemRegistr.Visibility = Visibility.Visible;
                MenuItemDelete.Visibility = Visibility.Visible;
                MenuItemEdit.Visibility = Visibility.Visible;
                separator.Visibility = Visibility.Visible;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SelectDataFromServer("sp_GetIDAnOverdueUPD");
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SelectDataFromServer("sp_GetReceipt");
        }

        private void receiptGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (receiptGrid.SelectedItem != null)
            {
                var row = (receiptGrid.CurrentItem as DataRowView).Row;
                int id = (int)row["Number_the_UPD"];

                Distribution distribution = new(id);
                distribution.Show();
            }
        }

        private void specialityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((string)specialityComboBox.SelectedItem == "Все")
            {
                SelectDataFromServer("sp_GetReceipt");
            }
            else
            {
                string sqlExpression = "sp_GetDataOnSpeciality";
                try
                {
                    using SqlConnection sqlConnection1 = new(Properties.Resources.connectionString);
                    sqlConnection1.Open();
                    SqlCommand command = new(sqlExpression, sqlConnection1)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    SqlParameter speciality_Parameter = new() { ParameterName = "@speciality", Value = specialityComboBox.SelectedItem.ToString() };
                    command.Parameters.Add(speciality_Parameter);

                    table = new DataTable();
                    adapter = new SqlDataAdapter(command);
                    adapter.Fill(table);
                    receiptGrid.ItemsSource = table.DefaultView;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}

