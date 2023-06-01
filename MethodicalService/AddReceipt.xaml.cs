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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MethodicalService
{
    /// <summary>
    /// Логика взаимодействия для AddReceipt.xaml
    /// </summary>
    public partial class AddReceipt : Window
    {
        public DataRow? Data { get; private set; }
        public string NumberUPD { get; set; }
        public AddReceipt()
        {
            InitializeComponent();
            ComboItemsSource();
        }

        private string GetIntValueFromString(string a)
        {
            int value;
            int.TryParse(string.Join("", a.Where(c => char.IsDigit(c))), out value);
            return value.ToString();
        }

        public void Build(DataRow data)
        {
            Data = data;
            string value = GetIntValueFromString(Data["Duration_training"].ToString());
            ID_Receipts.Text = Data["ID_Receipts"].ToString();
            textNumberUPD.Text = Data["Number_the_UPD"].ToString();
            textCount.Text = Data["Number_instances"].ToString();
            textName.Text = Data["Name_the_UPD"].ToString();
            textNote.Text = Data["Note"].ToString();
            textNumber.Text = Data["Meeting_number_YO"].ToString();
            datePick.Text = Data["Date_approval"].ToString();

            NumberUPD = Data["Number_the_UPD"].ToString();
            comboYear.SelectedValue = int.Parse(value[0].ToString());
            if (value.Length == 2)
            {
                comboMonth.SelectedValue = int.Parse(value[1].ToString());
            }
            if (value.Length == 3)
            {
                comboMonth.SelectedValue = int.Parse(value[1].ToString()) + int.Parse(value[2].ToString());
            }
            comboDevelop.SelectedValue = Data["cn_E"].ToString();
            comboSpeciality.SelectedValue = Data["Specialty_cipher"].ToString();
            comboType.SelectedValue = Data["Type_UPD"].ToString();
        }
        //private void UpdateValue()
        //{
        //    string duration_training = $"{comboYear.SelectedValue}г. {comboMonth.SelectedValue}м.";
        //    Data["ID_Receipts"] = ID_Receipts.Text;
        //    Data["Number_instances"] = textCount.Text;
        //    Data["Name_the_UPD"] = textName.Text;
        //    Data["Note"] = textNote.Text;
        //    Data["Meeting_number_YO"] = textNumber.Text;
        //    Data["cn_E"] = comboDevelop.SelectedValue;
        //    Data["Specialty_cipher"] = comboSpeciality.SelectedValue;
        //    Data["Type_UPD"] = comboType.SelectedValue;
        //    Data["Number_the_UPD"] = textNumberUPD.Text;
        //    Data["Duration_training"] = duration_training;
        //}

        private void ComboItemsSource()
        {
            comboDevelop.Items.Clear();
            using var connection = new SqlConnection(Properties.Resources.connectionString);
            connection.Open();
            var values = connection.Query<string>("SELECT Surname+' '+Name+' '+FatherName FROM Employee");
            comboDevelop.ItemsSource = values;
            values = connection.Query<string>("SELECT Name FROM Specialty");
            comboSpeciality.ItemsSource = values;
            values = connection.Query<string>("SELECT Full_name FROM Type_UPD");
            comboType.ItemsSource = values;
        }

        // Событие на выбор типа УПД
        // В зависимости от типа скрывается или показывается выпадающий список предметов
        private void comboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void buttonRegistr_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new(Properties.Resources.connectionString);
            string sqlExpression = "sp_InsertReceipt";
            using IDbConnection db = new SqlConnection(Properties.Resources.connectionString);
            int count = CheckNumberOfUPD(connection);

            if (count == 0)
            {
                try
                {
                    string duration_training = $"{comboYear.SelectedValue}г. {comboMonth.SelectedValue}мес.";

                    using SqlConnection sqlConnection1 = new(Properties.Resources.connectionString);
                    sqlConnection1.Open();
                    SqlCommand command = new(sqlExpression, sqlConnection1)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    //
                    // Name UPD
                    //
                    SqlParameter nameUPD_Parameter = new() { ParameterName = "@nameUPD", Value = textName.Text };
                    command.Parameters.Add(nameUPD_Parameter);
                    //
                    // Type UPD
                    //
                    int type = int.Parse(db.ExecuteScalar($"select ID_UPD FROM Type_UPD where Full_name like '{comboType.SelectedValue}'").ToString());
                    SqlParameter typeUPD_Parameter = new() { ParameterName = "@typeUPD", Value = type.ToString() };
                    command.Parameters.Add(typeUPD_Parameter);
                    //
                    // Number UPD
                    //
                    SqlParameter numberUPD_Parameter = new() { ParameterName = "@numberUPD", Value = int.Parse(textNumberUPD.Text) };
                    command.Parameters.Add(numberUPD_Parameter);
                    //
                    // Number Instances UPD
                    //
                    SqlParameter numberInstancesUPD_Parameter = new()
                    {
                        ParameterName = "@numberInstances",
                        Value = int.Parse(textCount.Text)
                    };
                    command.Parameters.Add(numberInstancesUPD_Parameter);
                    //
                    // Date approval
                    //
                    SqlParameter dateApproval_Parameter = new()
                    {
                        ParameterName = "@dateApproval",
                        Value = datePick.SelectedDate.Value
                    };
                    command.Parameters.Add(dateApproval_Parameter);
                    //
                    // Speciality 
                    //
                    SqlParameter speciality_Parameter = new()
                    {
                        ParameterName = "@speciality",
                        Value = comboSpeciality.SelectedValue
                    };
                    command.Parameters.Add(speciality_Parameter);
                    //
                    // Duration
                    //
                    SqlParameter duration_Parameter = new()
                    {
                        ParameterName = "@duration",
                        Value = duration_training
                    };
                    command.Parameters.Add(duration_Parameter);
                    //
                    // Note
                    //
                    SqlParameter note_Parameter = new()
                    {
                        ParameterName = "@note",
                        Value = textNote.Text
                    };
                    command.Parameters.Add(note_Parameter);
                    //
                    // Developer
                    //
                    SqlParameter developer_Parameter = new()
                    {
                        ParameterName = "@cn_E",
                        Value = comboDevelop.SelectedIndex + 1
                    };
                    command.Parameters.Add(developer_Parameter);
                    //
                    // Meeting number
                    //
                    SqlParameter meetingNumber_Parameter = new()
                    {
                        ParameterName = "@meetingNumber",
                        Value = int.Parse(textNumber.Text)
                    };
                    command.Parameters.Add(meetingNumber_Parameter);
                    //SqlCommand cmd = new()
                    //{
                    //    CommandType = CommandType.Text,
                    //    CommandText = "INSERT Journal_receipt_UPD_35 " +
                    //                                                "(Name_the_UPD, " +
                    //                                                 "Type_UPD, " +
                    //                                                 "Number_the_UPD, " +
                    //                                                 "Number_instances, " +
                    //                                                 "Date_approval, " +
                    //                                                 "Specialty_cipher, " +
                    //                                                 "Duration_training, " +
                    //                                                 "Note, " +
                    //                                                 "cn_E, " +
                    //                                                 "Meeting_number_YO) " +
                    //                                                 "VALUES " +
                    //                                                 $"(N'{textName.Text}', " +
                    //                                                 $"(select ID_UPD FROM Type_UPD where Full_name like '{comboType.SelectedValue}'), " +
                    //                                                 $"{textNumberUPD.Text}, " +
                    //                                                 $"{textCount.Text}, " +
                    //                                                 $"'{datePick.SelectedDate.Value.ToShortDateString()}', " +
                    //                                                 $"N'{comboSpeciality.SelectedValue}', " +
                    //                                                 $"N'{duration_training}', " +
                    //                                                 $"N'{textNote.Text}', " +
                    //                                                 $"{comboDevelop.SelectedIndex}, " +
                    //                                                 $"{textNumber.Text})",
                    //    Connection = sqlConnection1
                    //};
                    //cmd.ExecuteNonQuery();

                    if (command.ExecuteNonQuery() != -1)
                    {
                        MessageBox.Show($"УПД с номером {textNumberUPD.Text} успешно зарегистрирована", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
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
                MessageBox.Show($"УПД номером {textNumberUPD.Text} уже зарегистрирована в системе", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sqlExpression = "sp_UpdateReceipt";
                using IDbConnection db = new SqlConnection(Properties.Resources.connectionString);
                SqlConnection connection = new(Properties.Resources.connectionString);
                int count = CheckNumberOfUPD(connection);
                if (textNumberUPD.Text == NumberUPD)
                {
                    if (count == 1)
                    {
                        try
                        {
                            string duration_training = $"{comboYear.SelectedValue}г. {comboMonth.SelectedValue}мес.";

                            using SqlConnection sqlConnection1 = new(Properties.Resources.connectionString);
                            sqlConnection1.Open();
                            SqlCommand command = new(sqlExpression, sqlConnection1)
                            {
                                CommandType = CommandType.StoredProcedure
                            };
                            //
                            // Id
                            //
                            SqlParameter id_Parameter = new() { ParameterName = "@id", Value = ID_Receipts.Text };
                            command.Parameters.Add(id_Parameter);
                            //
                            // Name UPD
                            //
                            SqlParameter nameUPD_Parameter = new() { ParameterName = "@nameUPD", Value = textName.Text };
                            command.Parameters.Add(nameUPD_Parameter);
                            //
                            // Type UPD
                            //
                            int type = int.Parse(db.ExecuteScalar($"select ID_UPD FROM Type_UPD where Full_name like '{comboType.SelectedValue}'").ToString());
                            SqlParameter typeUPD_Parameter = new() { ParameterName = "@typeUPD", Value = type.ToString() };
                            command.Parameters.Add(typeUPD_Parameter);
                            //
                            // Number UPD
                            //
                            SqlParameter numberUPD_Parameter = new() { ParameterName = "@numberUPD", Value = int.Parse(textNumberUPD.Text) };
                            command.Parameters.Add(numberUPD_Parameter);
                            //
                            // Number Instances UPD
                            //
                            SqlParameter numberInstancesUPD_Parameter = new()
                            {
                                ParameterName = "@numberInstances",
                                Value = int.Parse(textCount.Text)
                            };
                            command.Parameters.Add(numberInstancesUPD_Parameter);
                            //
                            // Date approval
                            //
                            SqlParameter dateApproval_Parameter = new()
                            {
                                ParameterName = "@dateApproval",
                                Value = datePick.SelectedDate.Value
                            };
                            command.Parameters.Add(dateApproval_Parameter);
                            //
                            // Speciality 
                            //
                            SqlParameter speciality_Parameter = new()
                            {
                                ParameterName = "@speciality",
                                Value = comboSpeciality.SelectedValue
                            };
                            command.Parameters.Add(speciality_Parameter);
                            //
                            // Duration
                            //
                            SqlParameter duration_Parameter = new()
                            {
                                ParameterName = "@duration",
                                Value = duration_training
                            };
                            command.Parameters.Add(duration_Parameter);
                            //
                            // Note
                            //
                            SqlParameter note_Parameter = new()
                            {
                                ParameterName = "@note",
                                Value = textNote.Text
                            };
                            command.Parameters.Add(note_Parameter);
                            //
                            // Developer
                            //
                            SqlParameter developer_Parameter = new()
                            {
                                ParameterName = "@cn_E",
                                Value = comboDevelop.SelectedIndex + 1
                            };
                            command.Parameters.Add(developer_Parameter);
                            //
                            // Meeting number
                            //
                            SqlParameter meetingNumber_Parameter = new()
                            {
                                ParameterName = "@meetingNumber",
                                Value = int.Parse(textNumber.Text)
                            };
                            command.Parameters.Add(meetingNumber_Parameter);
                            //SqlCommand cmd = new()
                            //{
                            //    CommandType = CommandType.Text,
                            //    CommandText = "UPDATE Journal_receipt_UPD_35 SET " +
                            //                                                 "Name_the_UPD = " + $"N'{textName.Text}', " +
                            //                                                 "Type_UPD = " + $"(select ID_UPD from Type_UPD where Full_name like '{comboType.SelectedValue}'), " +
                            //                                                 "Number_the_UPD = " + $"{int.Parse(textNumberUPD.Text)}, " +
                            //                                                 "Number_instances = " + $"{textCount.Text}, " +
                            //                                                 "Date_approval = " + $"'{datePick.SelectedDate.Value}', " +
                            //                                                 "Specialty_cipher = " + $"N'{comboSpeciality.SelectedValue}', " +
                            //                                                 "Duration_training = " + $"N'{duration_training}', " +
                            //                                                 "Note = " + $"N'{textNote.Text}', " +
                            //                                                 "cn_E = " + $"{comboDevelop.SelectedIndex}, " +
                            //                                                 "Meeting_number_YO = " + $"{textNumber.Text}" +
                            //                                                 $"WHERE ID_Receipts = {ID_Receipts.Text}",
                            //    Connection = sqlConnection1
                            //};
                            //sqlConnection1.Open();
                            //cmd.ExecuteNonQuery();
                            //sqlConnection1.Close();
                            if (command.ExecuteNonQuery() != -1)
                            {
                                MessageBox.Show($"УПД с номером {textNumberUPD.Text} успешно изменена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message + ex.StackTrace, "Error");
                        }
                    }
                    if (count == 0)
                    {
                        if (MessageBox.Show($"УПД с номером {textNumberUPD.Text} не зарегистрирована в системе \nВы хотите зарегистрировать новую УПД", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                        {
                            buttonEdit.Visibility = Visibility.Collapsed;
                            buttonRegistr.Visibility = Visibility.Visible;
                        }
                    }
                    if (count > 1)
                    {
                        MessageBox.Show($"УПД с номером {textNumberUPD.Text} уже зарегистрирована в системе", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    count = CheckNumberOfUPD(connection);
                    if (count == 0)
                    {
                        if (MessageBox.Show($"УПД с номером {textNumberUPD.Text} не зарегистрирована в системе \nВы хотите зарегистрировать новую УПД?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                        {
                            buttonEdit.Visibility = Visibility.Collapsed;
                            buttonRegistr.Visibility = Visibility.Visible;
                        }
                    }
                    if (count > 0)
                    {
                        MessageBox.Show($"УПД с номером {textNumberUPD.Text} уже зарегистрирована в системе", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private int CheckNumberOfUPD(SqlConnection connection)
        {
            int count;
            connection.Open();
            string sqlExpression1 = "sp_CheckNumberOfUPD";
            SqlCommand command1 = new(sqlExpression1, connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            SqlParameter number_Parameter = new()
            {
                ParameterName = "@number",
                Value = textNumberUPD.Text
            };
            command1.Parameters.Add(number_Parameter);
            count = (int)command1.ExecuteScalar();
            connection.Close();
            return count;
        }
    }
}