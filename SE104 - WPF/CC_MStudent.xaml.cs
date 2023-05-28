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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SE104___WPF.DBClass;

using System.Data;

using System.Data.SqlClient;

namespace SE104___WPF
{
    /// <summary>
    /// Interaction logic for CC_MStudent.xaml
    /// </summary>
    public partial class CC_MStudent : UserControl
    {
        clsHOCSINH hs = new clsHOCSINH();
        DBConnect dBConnect = new DBConnect();
        public CC_MStudent()
        {
            InitializeComponent();
            loadThongTin();
        }
        string bang = "Data Source=LAPTOP-9Q9UCI39;Initial Catalog=SE104;Integrated Security=True";
        SqlConnection sqlCon;
        SqlCommand sqlCom;
        SqlDataReader read;
        SqlDataAdapter sqlDa;
        DataTable dtbStudent;

        private void loadThongTin()
        {
            string sql = "SELECT * FROM HOCSINH";
            using (sqlCon = new SqlConnection(bang))
            {
                sqlCon.Open();
                sqlDa = new SqlDataAdapter(sql, sqlCon);
                dtbStudent = new DataTable();
                sqlDa.Fill(dtbStudent);
                dataGridViewStudent.ItemsSource = dtbStudent.DefaultView;
                sqlCon.Close();
            }
            

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddNewStudent addNewStudent = new AddNewStudent();
            addNewStudent.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ProfileStudent profileStudent = new ProfileStudent();
            profileStudent.Show();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
          
        }



        
    }
}
