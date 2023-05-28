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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SE104___WPF
{
    /// <summary>
    /// Interaction logic for CC_MClass.xaml
    /// </summary>
    public partial class CC_MClass : UserControl
    {
        public CC_MClass()
        {
            InitializeComponent();
            loadThongTin();
        }
        string bang = "Data Source=LAPTOP-9Q9UCI39;Initial Catalog=SE104;Integrated Security=True";
        SqlConnection sqlCon;
        SqlCommand sqlCom;
        SqlDataReader read;
        SqlDataAdapter sqlDa;
        DataTable dtbClass;

        private void loadThongTin()
        {
            using (sqlCon = new SqlConnection(bang))
            {
                sqlCon.Open();
                sqlDa = new SqlDataAdapter("SELECT * FROM LOP ", sqlCon);
                dtbClass = new DataTable();
                sqlDa.Fill(dtbClass);
                dataGridViewClass.ItemsSource = dtbClass.DefaultView;
                sqlCon.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddStudentToClass addStudentToClass = new AddStudentToClass();
            addStudentToClass.Show();
        }
    }
}
