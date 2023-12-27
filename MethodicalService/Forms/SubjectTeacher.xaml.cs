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

namespace MethodicalService.Forms
{
    /// <summary>
    /// Логика взаимодействия для SubjectTeacher.xaml
    /// </summary>
    public partial class SubjectTeacher : UserControl
    {
        SqlDataAdapter? adapter = null;
        DataTable? table = null;

        public SubjectTeacher()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            SelectDataFromServer("sp_GetSubjectTeacher");
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
                subjectGrid.ItemsSource = table.DefaultView;
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
    }
}
