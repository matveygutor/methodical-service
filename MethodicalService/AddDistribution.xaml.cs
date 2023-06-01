using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для AddDistribution.xaml
    /// </summary>
    public partial class AddDistribution : Window
    {
        public DataRow? Data { get; private set; }

        public AddDistribution()
        {
            InitializeComponent();
            SelectItemsSourse();
        }

        public void BuildForAdd(DataRow data)
        {
            Data = data;
            textNumberUPD.Text = Data["Number_the_UPD"].ToString();
            textName.Text = Data["Name_the_UPD"].ToString();
        }

        public void BuildForEdit(DataRow data)
        {
            Data = data;
            ID_Distribution.Text = Data["ID_Distribution"].ToString(); 
            textNote.Text = Data["Note"].ToString();
            textNumberUPD.Text = Data["ID_Receipts"].ToString();
            textName.Text = Data["Name_the_UPD"].ToString();
            textStatus.Text = Data["Instance_status"].ToString();
            comboDevelop.SelectedValue = Data["cn_E"].ToString();
            datePick.Text = Data["Date_receipt"].ToString();
        }

        private void SelectItemsSourse()
        {
            using var connection = new SqlConnection(Properties.Resources.connectionString);
            connection.Open();
            var values = connection.Query<string>("SELECT Surname+' '+Name+' '+FatherName FROM Employee");
            comboDevelop.ItemsSource = values;
        }

        private void ButtonDistribut_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new(Properties.Resources.connectionString);
            using IDbConnection db = new SqlConnection(Properties.Resources.connectionString);
            string sqlExpression = "sp_InsertDistribution";
            int count = GetCountExemplar(connection);
            int exemplar = (int)db.ExecuteScalar($"select Number_instances from Journal_receipt_UPD_35 where Number_the_UPD = {textNumberUPD.Text}");
            if (exemplar > count)
            {
                try
                {
                    //using SqlConnection connection = new(Properties.Resources.connectionString);
                    connection.Open();
                    SqlCommand command = new(sqlExpression, connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    SqlParameter status_Parameter = new() { ParameterName = "@instance_status", Value = textStatus.Text };
                    command.Parameters.Add(status_Parameter);

                    SqlParameter date_Parameter = new() { ParameterName = "@date_receipt", Value = datePick.SelectedDate.Value };
                    command.Parameters.Add(date_Parameter);

                    SqlParameter note_Parameter = new() { ParameterName = "@note", Value = textNote.Text };
                    command.Parameters.Add(note_Parameter);

                    int numberUPD = (int)db.ExecuteScalar($"select id_receipts from Journal_receipt_UPD_35 where Number_the_UPD = {textNumberUPD.Text}");
                    SqlParameter id_receipt_Parameter = new()
                    {
                        ParameterName = "@id_receipts",
                        Value = numberUPD
                    };
                    command.Parameters.Add(id_receipt_Parameter);

                    SqlParameter cn_e_Parameter = new()
                    {
                        ParameterName = "@cn_e",
                        Value = comboDevelop.SelectedIndex + 1
                    };
                    command.Parameters.Add(cn_e_Parameter);

                    SqlParameter nameUPD_Parameter = new()
                    {
                        ParameterName = "@nameUPD",
                        Value = textName.Text
                    };
                    command.Parameters.Add(nameUPD_Parameter);


                    if (command.ExecuteNonQuery() != -1)
                    {
                        MessageBox.Show($"УПД с номером {textNumberUPD.Text} успешно распределена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }

            }
            else
            {
                MessageBox.Show($"Вы распределили все экземпляры для УПД с номером {textNumberUPD.Text}", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private int GetCountExemplar(SqlConnection connection)
        {

            IDbConnection db = new SqlConnection(Properties.Resources.connectionString);
            int count;
            connection.Open();
            string sqlExpression1 = "sp_GetCountExemplar";
            SqlCommand command1 = new(sqlExpression1, connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            int id = (int)db.ExecuteScalar($"select id_receipts from Journal_receipt_UPD_35 where Number_the_UPD = {textNumberUPD.Text}");
            SqlParameter number_Parameter = new()
            {
                ParameterName = "@id",
                Value = id
            };
            command1.Parameters.Add(number_Parameter);
            count = (int)command1.ExecuteScalar();
            connection.Close();
            return count;


        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            IDbConnection db = new SqlConnection(Properties.Resources.connectionString);
            SqlConnection connection = new(Properties.Resources.connectionString);
            string sqlExpression = "sp_UpdateDistribution";
            try
            {
                connection.Open();
                SqlCommand command = new(sqlExpression, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter status_Parameter = new() { ParameterName = "@instance_status", Value = textStatus.Text };
                command.Parameters.Add(status_Parameter);

                SqlParameter date_Parameter = new() { ParameterName = "@date_receipt", Value = datePick.SelectedDate.Value };
                command.Parameters.Add(date_Parameter);

                SqlParameter note_Parameter = new() { ParameterName = "@note", Value = textNote.Text };
                command.Parameters.Add(note_Parameter);

                int numberUPD = (int)db.ExecuteScalar($"select id_receipts from Journal_receipt_UPD_35 where Number_the_UPD = {textNumberUPD.Text}");
                SqlParameter id_receipt_Parameter = new()
                {
                    ParameterName = "@id_receipts",
                    Value = numberUPD
                };
                command.Parameters.Add(id_receipt_Parameter);

                SqlParameter cn_e_Parameter = new()
                {
                    ParameterName = "@cn_e",
                    Value = comboDevelop.SelectedIndex + 1
                };
                command.Parameters.Add(cn_e_Parameter);

                SqlParameter nameUPD_Parameter = new()
                {
                    ParameterName = "@nameUPD",
                    Value = textName.Text
                };
                command.Parameters.Add(nameUPD_Parameter);

                SqlParameter id_Parameter = new()
                {
                    ParameterName = "@id_distribution",
                    Value = ID_Distribution.Text
                };
                command.Parameters.Add(id_Parameter);


                if (command.ExecuteNonQuery() != -1)
                {
                    MessageBox.Show($"Экземпляр с номером {textNumberUPD.Text} успешно изменен", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
    }
}


